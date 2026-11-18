using System.Text.Json;

namespace MediaAudioController
{
    internal sealed class AudioSettings
    {
        private static readonly string SettingsPath = Path.Combine(AppContext.BaseDirectory, "MediaAudioController.config.json");

        public string? PlayDeviceId { get; set; }
        public string? PauseDeviceId { get; set; }

        public static AudioSettings Load()
        {
            if (!File.Exists(SettingsPath))
                return new AudioSettings();

            return JsonSerializer.Deserialize<AudioSettings>(File.ReadAllText(SettingsPath))
                ?? throw new JsonException("The audio configuration is empty.");
        }

        public void Save()
        {
            string temporaryPath = SettingsPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllText(temporaryPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
                File.Move(temporaryPath, SettingsPath, true);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }
    }
}
