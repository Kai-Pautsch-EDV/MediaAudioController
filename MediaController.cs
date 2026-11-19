using Windows.Media.Control;

namespace MediaAudioController
{
    internal enum MediaAction
    {
        Play,
        Pause,
        PlayPause
    }

    internal static class MediaController
    {
        public static async Task ExecuteAsync(MediaAction action, AudioSettings settings)
        {
            var manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var session = manager.GetCurrentSession();
            var status = session?.GetPlaybackInfo().PlaybackStatus;

            if (action == MediaAction.PlayPause)
            {
                action = status switch
                {
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing => MediaAction.Pause,
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused or
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped => MediaAction.Play,
                    null => MediaAction.Play,
                    _ => throw new InvalidOperationException($"Cannot toggle media playback in state {status}.")
                };
            }

            var devices = AudioDevices.GetActiveDevices();
            string? configuredId = action == MediaAction.Play ? settings.PlayDeviceId : settings.PauseDeviceId;
            var device = devices.Resolve(configuredId)
                ?? throw new InvalidOperationException("No active default audio output device is available.");

            if (action == MediaAction.Pause)
            {
                if (session is not null &&
                    status != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused &&
                    status != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                    await PauseAsync(session);

                AudioDevices.SetDefault(device.Id);
            }
            else
            {
                AudioDevices.SetDefault(device.Id);
                if (session is not null &&
                    status != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing &&
                    !await session.TryPlayAsync())
                    throw new InvalidOperationException("The audio output was changed, but the media session could not be started.");
            }
        }

        private static async Task PauseAsync(GlobalSystemMediaTransportControlsSession session)
        {
            var paused = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            void CheckStatus()
            {
                var status = session.GetPlaybackInfo().PlaybackStatus;
                if (status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused ||
                    status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                    paused.TrySetResult();
            }
            void OnPlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args) => CheckStatus();

            session.PlaybackInfoChanged += OnPlaybackInfoChanged;
            try
            {
                if (!await session.TryPauseAsync())
                    throw new InvalidOperationException("The media session could not be paused; the audio output was not changed.");
                CheckStatus();
                try
                {
                    await paused.Task.WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (TimeoutException ex)
                {
                    throw new InvalidOperationException("The media session did not confirm that playback paused; the audio output was not changed.", ex);
                }
            }
            finally
            {
                session.PlaybackInfoChanged -= OnPlaybackInfoChanged;
            }
        }
    }
}
