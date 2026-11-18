using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MediaAudioController
{
    internal static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            bool silent = args.Any(arg => arg.Equals("-silent", StringComparison.OrdinalIgnoreCase));
            try
            {
                var action = ParseArguments(args, silent);
                var settings = AudioSettings.Load();
                if (silent)
                {
                    MediaController.ExecuteAsync(action!.Value, settings).GetAwaiter().GetResult();
                }
                else
                {
                    ApplicationConfiguration.Initialize();
                    Application.Run(new MainForm(settings, action));
                }
                return 0;
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or IOException
                or UnauthorizedAccessException or JsonException or COMException or Win32Exception or NotSupportedException)
            {
                if (silent)
                {
                    Console.Error.WriteLine(ex.Message);
                    try
                    {
                        File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "MediaAudioController.error.log"),
                            $"{DateTimeOffset.Now:O} {ex}{Environment.NewLine}");
                    }
                    catch (IOException logError)
                    {
                        Console.Error.WriteLine($"Unable to write error log: {logError.Message}");
                    }
                    catch (UnauthorizedAccessException logError)
                    {
                        Console.Error.WriteLine($"Unable to write error log: {logError.Message}");
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message, "Media Audio Controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return 1;
            }
        }

        private static MediaAction? ParseArguments(string[] args, bool silent)
        {
            MediaAction? action = null;
            bool sawSilent = false;
            foreach (string arg in args)
            {
                if (arg.Equals("-silent", StringComparison.OrdinalIgnoreCase))
                {
                    if (sawSilent)
                        throw new ArgumentException("The -silent parameter may only be specified once.");
                    sawSilent = true;
                    continue;
                }

                MediaAction parsedAction = arg.ToLowerInvariant() switch
                {
                    "-play" => MediaAction.Play,
                    "-pause" => MediaAction.Pause,
                    "-playpause" => MediaAction.PlayPause,
                    _ => throw new ArgumentException($"Unknown parameter: {arg}")
                };
                if (action.HasValue)
                    throw new ArgumentException("Specify only one of -play, -pause, or -playpause.");
                action = parsedAction;
            }

            if (silent && !action.HasValue)
                throw new ArgumentException("-silent requires -play, -pause, or -playpause.");
            return action;
        }
    }
}