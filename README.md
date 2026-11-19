# Media Audio Controller

<img src="Assets/logo.svg" alt="Media Audio Controller logo" width="128" height="128">

Media Audio Controller is a Windows desktop application that controls the current Windows media session and changes the system's default audio output when playback starts or pauses.

## Why this tool?

Imagine listening to music through your speakers when a meeting call comes in. Before joining, you may need to pause the music and switch the Windows audio output to your headset. After the call, you want to switch back to your speakers and resume listening. Media Audio Controller combines each of those steps into a single action:

1. Configure **Switch on Pause to** as your headset and **Switch on Play to** as your speakers.
2. Before joining the call, click **Pause**: the tool pauses the current media session and then switches the Windows default audio output to your headset.
3. After the call, click **Play**: the tool switches the default audio output back to your speakers and then resumes playback.

If Windows has no current media session, the Play and Pause actions still switch to their configured audio output and skip the playback command. In that case, **Play/Pause** cannot determine the current playback state, so it switches to the Play output without issuing a playback command.

You can also configure keyboard macro keys to launch the application with parameters. For example, set one key to run `MediaAudioController.exe -silent -pause` and another to run `MediaAudioController.exe -silent -play`. Once the devices are configured in the UI, these commands perform the same actions without showing a window and then exit. Use the full path to the executable in your keyboard software; see [Command-line parameters](#command-line-parameters) for more examples.

The tool controls the current Windows media session and changes the system default output; it does not answer or end calls. A meeting application using its own selected output may not follow the Windows default.

The application icon is embedded in the executable. The SVG logo is included separately for the README.

## Features

- Select separate audio output devices for Play and Pause.
- Populate both lists with active Windows playback devices at startup or with **Refresh**.
- Save selections by device ID and restore them on the next launch.
- Control playback from the window or through command-line parameters, including a windowless mode for shortcuts and automation.

## Requirements

### Running a published application

- Windows 10 version 2004 (build 19041) or later, matching the architecture of the published application (for example, `win-x64` for Windows x64). This is the project's configured minimum; older versions have not been verified. See Microsoft's [supported Windows versions for .NET](https://learn.microsoft.com/en-us/dotnet/core/install/windows#supported-versions) for current support status.
- At least one active audio output device and a Windows multimedia default output device.
- A compatible Windows media session is optional: without one, the configured audio output is still switched, but playback cannot be controlled.
- A writable application directory to save configuration and, in silent mode, error logs.
- **Self-contained publish:** no separate .NET runtime or SDK installation is required; the runtime is included.
- **Framework-dependent publish:** the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) for the application architecture must be installed. Run `dotnet --list-runtimes` and look for `Microsoft.WindowsDesktop.App` 10.x for that architecture.

Neither publish mode requires the full Windows SDK on the target machine. Keep all files in the published distribution together. The Windows API projection and WinRT runtime are required for media controls; they may be included in the executable in a single-file publish rather than appearing as separate DLLs.

The application changes the Windows default device for the Console, Multimedia, and Communications roles. It uses an undocumented Windows audio policy interface to do so; future Windows versions may change this behavior. An application configured to use a specific output device rather than the Windows default may not follow the change.

### Building or publishing from source

- Windows and the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), or a later SDK that supports targeting .NET 10. Check installed SDKs with `dotnet --list-sdks`. The project has no `global.json` SDK pin.
- Access to a NuGet source containing NAudio.Wasapi and any missing .NET/Windows reference or runtime packs. The project targets Windows Forms and the Windows 10 version 2004 SDK API surface; the .NET SDK resolves the necessary reference packs during restore.
- If using Visual Studio, a version supporting .NET 10 and the **.NET desktop development** workload. Open `MediaAudioController.slnx`, restore NuGet packages, and build the solution. A standalone Windows SDK installation may be required by your Visual Studio setup.
- To run and test the result, the runtime requirements above, including an active device and a compatible media session. Building alone does not verify playback or device switching.

## Build and run

From the project directory in PowerShell, restore, build, and run:

```powershell
dotnet restore .\MediaAudioController.csproj
dotnet build .\MediaAudioController.csproj -c Release
dotnet run --project .\MediaAudioController.csproj -c Release
```

`dotnet build` and `dotnet run` also restore automatically unless `--no-restore` is specified. The SDK and dependencies are needed for the build; the .NET 10 Desktop Runtime is needed to run a framework-dependent build on a target machine. Debug builds also include symbols for debugging; publish output is recommended for distribution. Existing output directories can contain files left over from older builds or user-created configuration and logs; use a fresh publish directory when preparing a release.

### Publish for distribution

The Visual Studio `FolderProfile` produces a framework-dependent publish and requires the .NET 10 Desktop Runtime on the target machine. The application icon is embedded in the executable; the SVG logo is published separately for the README. The README, license notices, and debugging symbols remain separate files. Use a fresh publish directory to avoid distributing files left over from earlier publishes. For a smaller download, package the publish directory as a ZIP archive.

For a framework-dependent Windows x64 distribution without single-file bundling:

```powershell
dotnet publish .\MediaAudioController.csproj -c Release -r win-x64 --self-contained false -o .\publish
.\publish\MediaAudioController.exe
```

Keep the published files together and place them in a writable directory. Install the matching architecture of the .NET 10 Desktop Runtime on the target machine. Choose the appropriate Windows runtime identifier, such as `win-arm64`, for other target architectures.

The full Windows SDK and the build-time `Microsoft.Windows.SDK.NET.Ref` reference pack are **not** required on the target machine. In this non-bundled publish, `Microsoft.Windows.SDK.NET.dll` and `WinRT.Runtime.dll` are required application dependencies; do not remove them. A framework-dependent publish keeps the .NET runtime out of the distribution, but still includes application-specific dependencies.

Alternatively, publish a self-contained x64 distribution that includes the .NET runtime:

```powershell
dotnet publish .\MediaAudioController.csproj -c Release -r win-x64 --self-contained true -o .\publish-self-contained
.\publish-self-contained\MediaAudioController.exe
```

Self-contained output is larger and must be republished to pick up runtime security updates. Publishing may need to download runtime packs. Both publish modes include `LICENSE` and `THIRD-PARTY-NOTICES.txt`; distribute these files with the application and preserve applicable notices if you repackage it. See [Dependencies](#dependencies) and [License](#license).

## Using the application

1. Start Media Audio Controller. Both device lists are refreshed automatically.
2. Select an output under **Switch on Pause to** and another under **Switch on Play to**. Each selection is saved immediately.
3. Use **Play**, **Pause**, or **Play/Pause** to control the current Windows media session.
4. Use **Refresh** after connecting, disconnecting, or enabling an audio device.

**Play** switches the default output first, then requests playback. If the session is already playing, it only switches the output. **Pause** requests a pause first and waits up to five seconds for the session to report that it is paused or stopped before switching the output. If it is already paused or stopped, the output is switched directly. If pause is rejected or not confirmed, the output is not switched. If playback fails after a Play request, the output may already have changed.

When Windows has no current media session, Play and Pause still switch their configured output and skip the playback command. In that case, Play/Pause cannot infer whether playback should start or pause; it switches to the Play output and does not send a playback command. With a media session, **Play/Pause** reads its status: playing triggers Pause; paused or stopped triggers Play. Other states cannot be toggled and produce an error.

## Command-line parameters

```text
MediaAudioController.exe [-silent] [-play | -pause | -playpause]
```

| Parameter | Description |
| --- | --- |
| `-play` | Switch to the Play output and start playback. |
| `-pause` | Pause playback and switch to the Pause output. |
| `-playpause` | Toggle according to the current media session's playback status. |
| `-silent` | Run one specified action without showing the application window, then exit. |

Parameters are case-insensitive and can appear in either order. Specify at most one action. `-silent` must be accompanied by exactly one action; unknown or duplicate parameters are errors. With no parameters, the window opens without triggering a playback action. Without `-silent`, an action parameter opens the window, refreshes the devices, performs the action, and leaves the window open.

Examples, using PowerShell from the executable's directory:

```powershell
# Open the application
.\MediaAudioController.exe

# Open the application and start playback
.\MediaAudioController.exe -play

# Pause without opening the window
.\MediaAudioController.exe -silent -pause

# Toggle playback without opening the window
.\MediaAudioController.exe -playpause -silent
```

For automation that waits for the GUI-subsystem executable and checks the exit code:

```powershell
$process = Start-Process -FilePath '.\MediaAudioController.exe' `
    -ArgumentList '-silent', '-play' -Wait -PassThru
if ($process.ExitCode -ne 0) {
    Write-Error 'Media Audio Controller failed; check MediaAudioController.error.log.'
}
```

In silent mode, exit code `0` indicates success and `1` indicates a handled error. The application attempts to append error details to `MediaAudioController.error.log` next to the executable and writes the message to standard error. As this is a Windows GUI application, standard error may not be visible when launched without an attached or redirected console. If the application directory is not writable, the log cannot be created. In windowed mode, errors are shown in a dialog; errors during a windowed action do not necessarily result in a nonzero process exit code.

## Configuration

Selections are stored in `MediaAudioController.config.json` in the application directory, not the current working directory. The file is created or updated when the device lists are refreshed or a selection changes. For example:

```json
{
  "PlayDeviceId": "<Windows playback device ID>",
  "PauseDeviceId": "<Windows playback device ID>"
}
```

IDs are assigned by Windows; select devices in the application rather than copying the example values. Refresh preserves each saved selection when its device is still active. If an ID is missing or unavailable, the current Windows Multimedia default is selected and saved in its place. If no active devices are found, the selections are cleared and saved. Silent actions also fall back to the current Multimedia default when a saved device is unavailable, but do not update the configuration. If no usable default is available, the action fails.

The application directory must be writable: otherwise configuration changes cannot be saved and the window displays an error. Invalid JSON also prevents the configuration from loading.

## Troubleshooting

- **No media session:** Audio switching still works. Play and Pause switch to their configured outputs but cannot control playback; Play/Pause switches to the Play output because there is no session status to toggle. Start a compatible media player if you want playback control. The application controls the session Windows reports as current; it does not select a player by name.
- **No devices or the wrong device:** Check Windows sound settings, enable the output, and press **Refresh**. Only active playback devices are listed. Confirm the selections before retrying.
- **Pause times out or Play fails:** The player may not support the command or may not report its playback state promptly. Pause requires confirmation before the device is switched.
- **Configuration or log cannot be written:** Move the application to a directory where your user account has write permission.
- **Silent command fails:** Check the exit code and `MediaAudioController.error.log` in the application directory. Silent mode does not display an error dialog.

## Dependencies

The project references [NAudio.Wasapi 2.2.1](https://www.nuget.org/packages/NAudio.Wasapi/2.2.1) directly for Windows audio endpoint enumeration. It brings in `NAudio.Core` 2.2.1 as a transitive dependency. Other NAudio modules are not included.

The NAudio packages are MIT-licensed. The NAudio license and copyright notice are reproduced in [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt), which is copied into build and publish outputs. Keep that file with distributed copies or substantial portions of NAudio. The notice includes the 2020 copyright from the packaged license and the 2023 copyright from package metadata. See the [NAudio repository](https://github.com/naudio/NAudio) and the resolved NuGet packages for source and license information.

The application also uses .NET Windows Forms and Windows APIs, including the Windows SDK media session APIs and an undocumented audio policy interface. The Windows SDK reference pack is resolved by the .NET SDK rather than declared as a direct package dependency. The .NET runtime and Windows components have their own licensing terms; the project's MIT license does not replace third-party terms.

To inspect the dependencies actually resolved on your machine after restore:

```powershell
dotnet list .\MediaAudioController.csproj package --include-transitive
```

## License

This project is licensed under the [MIT License](LICENSE). Third-party dependencies remain subject to their own licenses and notices.
