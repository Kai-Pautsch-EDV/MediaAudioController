using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;

namespace MediaAudioController
{
    internal sealed record AudioOutputDevice(string Id, string Name)
    {
        public override string ToString() => Name;
    }

    internal sealed record AudioDeviceList(List<AudioOutputDevice> Devices, string? DefaultDeviceId)
    {
        public AudioOutputDevice? Resolve(string? id) =>
            Devices.FirstOrDefault(device => device.Id == id)
            ?? Devices.FirstOrDefault(device => device.Id == DefaultDeviceId);
    }

    internal static class AudioDevices
    {
        public static AudioDeviceList GetActiveDevices()
        {
            using var enumerator = new MMDeviceEnumerator();
            var devices = new List<AudioOutputDevice>();
            foreach (var device in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                using (device)
                    devices.Add(new AudioOutputDevice(device.ID, device.FriendlyName));
            }

            string? defaultDeviceId = null;
            if (devices.Count > 0)
            {
                using var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                defaultDeviceId = defaultDevice.ID;
            }
            return new AudioDeviceList(devices, defaultDeviceId);
        }

        public static void SetDefault(string deviceId)
        {
            var policy = (IPolicyConfig)new PolicyConfigClient();
            try
            {
                foreach (Role role in new[] { Role.Console, Role.Multimedia, Role.Communications })
                    Marshal.ThrowExceptionForHR(policy.SetDefaultEndpoint(deviceId, role));
            }
            finally
            {
                Marshal.ReleaseComObject(policy);
            }
        }

        [ComImport]
        [Guid("870AF99C-171D-4F9E-AF0D-E63DF40C2BC9")]
        private class PolicyConfigClient { }

        // The PolicyConfig interface is not part of the public Windows Core Audio API.
        [ComImport]
        [Guid("F8679F50-850A-41CF-9C72-430F290290C8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPolicyConfig
        {
            [PreserveSig] int GetMixFormat();
            [PreserveSig] int GetDeviceFormat();
            [PreserveSig] int ResetDeviceFormat();
            [PreserveSig] int SetDeviceFormat();
            [PreserveSig] int GetProcessingPeriod();
            [PreserveSig] int SetProcessingPeriod();
            [PreserveSig] int GetShareMode();
            [PreserveSig] int SetShareMode();
            [PreserveSig] int GetPropertyValue();
            [PreserveSig] int SetPropertyValue();
            [PreserveSig] int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string deviceId, Role role);
            [PreserveSig] int SetEndpointVisibility();
        }
    }
}
