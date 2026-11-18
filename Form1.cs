using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MediaAudioController
{
    public partial class MainForm : Form
    {
        private readonly AudioSettings settings;
        private readonly MediaAction? startupAction;
        private bool updatingDevices;

        public MainForm() : this(AudioSettings.Load(), null) { }

        internal MainForm(AudioSettings settings, MediaAction? startupAction)
        {
            this.settings = settings;
            this.startupAction = startupAction;
            InitializeComponent();
            var assembly = typeof(MainForm).Assembly;
            var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? assembly.GetName().Name;
            var author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            Text = $"{title} v{assembly.GetName().Version}";
            if (!string.IsNullOrWhiteSpace(author))
                Text += $" - {author}";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            comboBoxOnPauseOutput.SelectedIndexChanged += OutputSelectionChanged;
            comboBoxOnPlayOutput.SelectedIndexChanged += OutputSelectionChanged;
            Shown += Form1_Shown;
        }

        private async void Form1_Shown(object? sender, EventArgs e)
        {
            if (RefreshDevices() && startupAction.HasValue)
                await ExecuteActionAsync(startupAction.Value);
        }

        private void buttonRefresh_Click(object sender, EventArgs e) => RefreshDevices();

        private bool RefreshDevices()
        {
            try
            {
                var devices = AudioDevices.GetActiveDevices();
                updatingDevices = true;
                try
                {
                    RefreshOutputComboBox(comboBoxOnPauseOutput, devices, settings.PauseDeviceId);
                    RefreshOutputComboBox(comboBoxOnPlayOutput, devices, settings.PlayDeviceId);
                }
                finally
                {
                    updatingDevices = false;
                }
                SaveSelections();
                return true;
            }
            catch (Exception ex) when (ex is COMException or IOException or UnauthorizedAccessException)
            {
                ShowError(ex);
                return false;
            }
        }

        private static void RefreshOutputComboBox(ComboBox comboBox, AudioDeviceList devices, string? selectedId)
        {
            var selectedDevice = devices.Resolve(selectedId);
            comboBox.BeginUpdate();
            try
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(devices.Devices.ToArray());
                comboBox.SelectedItem = selectedDevice;
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }

        private void OutputSelectionChanged(object? sender, EventArgs e)
        {
            if (updatingDevices)
                return;

            try
            {
                SaveSelections();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                ShowError(ex);
            }
        }

        private void SaveSelections()
        {
            settings.PauseDeviceId = (comboBoxOnPauseOutput.SelectedItem as AudioOutputDevice)?.Id;
            settings.PlayDeviceId = (comboBoxOnPlayOutput.SelectedItem as AudioOutputDevice)?.Id;
            settings.Save();
        }

        private async Task ExecuteActionAsync(MediaAction action)
        {
            groupBoxMediaControls.Enabled = false;
            groupBoxMediaOutput.Enabled = false;
            try
            {
                await MediaController.ExecuteAsync(action, settings);
            }
            catch (Exception ex) when (ex is COMException or InvalidOperationException or Win32Exception
                or UnauthorizedAccessException or IOException or NotSupportedException)
            {
                ShowError(ex);
            }
            finally
            {
                groupBoxMediaOutput.Enabled = true;
                groupBoxMediaControls.Enabled = true;
            }
        }

        private void ShowError(Exception ex) =>
            MessageBox.Show(this, ex.Message, "Media Audio Controller", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private async void buttonPlay_Click(object sender, EventArgs e) => await ExecuteActionAsync(MediaAction.Play);
        private async void buttonPause_Click(object sender, EventArgs e) => await ExecuteActionAsync(MediaAction.Pause);
        private async void buttonPlayPause_Click(object sender, EventArgs e) => await ExecuteActionAsync(MediaAction.PlayPause);
    }
}
