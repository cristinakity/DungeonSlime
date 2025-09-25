# DungeonSlime

MonoGame (.NET 8) project configured for running with host X11 display inside a Dev Container.

## Dev Container Setup
The repository includes a `.devcontainer` folder. Open the folder in VS Code and choose "Reopen in Container". The container installs:
- .NET 8 SDK
- MonoGame templates & tools (`dotnet new install MonoGame.Templates.CSharp`)
- Wine (for the MGCB editor if needed)
- X11 / DBus integration (reuses host display rather than headless/Xvfb)

### Host Requirements
Make sure your host session allows the container user to connect to X11.

Run on the host (NOT inside the container):
```bash
# Narrow permission (recommended):
xhost +SI:localuser:$(id -un)
# Alternative targeted form accepted by some setups:
xhost +local:$(id -un)
# Broader (less strict – allows all local users):
xhost +local:
```
If you use Flatpak VS Code, ensure these Flatpak permissions are enabled (they usually appear in the Flatseal UI):
- X11 windowing system
- Fallback to X11 windowing system
- D-Bus session bus
- Inter-process communications
- Device access to `/dev/dri`

### Environment / Mounts
Key parts of `.devcontainer/devcontainer.json`:
- Mounts `/tmp/.X11-unix` for X11 socket
- Binds your host `~/.Xauthority` for authentication
- Passes through: `DISPLAY`, `DBUS_SESSION_BUS_ADDRESS`, `XAUTHORITY`

### First Run
After the container finishes building:
```bash
cd DungeonSlime
dotnet run
```
A MonoGame window should appear on your host display (cornflower blue background by default).

## Content Pipeline
Content is defined in `DungeonSlime/Content/Content.mgcb`.
Build automatically happens when you build the project (the `dotnet-mgcb` tool is restored). To manually build:
```bash
cd DungeonSlime/Content
mgcb /@Content.mgcb /platform:DesktopGL /outputDir:bin/DesktopGL /intermediateDir:obj/DesktopGL
```
(Usually not necessary unless scripting CI.)

Add new assets by editing `Content.mgcb` (or using the MGCB Editor if GTK dependencies are present). Example entry for a texture:
```
#begin images/hero.png
/importer:TextureImporter
/processor:TextureProcessor
/build:images/hero.png
```
Place the file under `DungeonSlime/Content/images/hero.png`.

Load in code (after adding to mgcb and building):
```csharp
var logo = Content.Load<Texture2D>("images/logo");
```

## Troubleshooting
| Problem | Fix |
|---------|-----|
| `Authorization required, but no authorization protocol specified` | Ensure `~/.Xauthority` is mounted and `xhost +SI:localuser:$(id -un)` run on host. |
| `NoSuitableGraphicsDeviceException` | Confirm `/dev/dri` is passed and host has 3D drivers; remove any forced `DISPLAY=:99`. |
| GTK / Eto errors when launching MGCB Editor | Install missing GTK packages: `sudo apt-get update && sudo apt-get install -y libgtk-3-0 libgtk-3-dev` (optional). |
| Window does not appear | Check `echo $DISPLAY` inside container matches host (like `:0` or `:1`). Run `glxinfo | grep OpenGL` (if `mesa-utils` installed) to test GL. |

## Optional: MGCB Editor (GUI)
If you want the graphical content pipeline editor:
```bash
sudo apt-get update && sudo apt-get install -y libgtk-3-0 libcanberra-gtk-module
mgcb-editor
```

## Windows Host Variant
If you open this repository on a Windows machine (Docker Desktop / WSL2) and do not need native Linux host X11 passthrough, a simplified variant exists at:
```
.devcontainer/windows/devcontainer.json
```
VS Code will prompt you to pick a container definition when multiple are present. Choose the Windows Host variant for a leaner setup. For graphics on Windows you normally just run the game window (DesktopGL) without extra DISPLAY/X11 configuration.

If you use WSLg (Windows 11), GUI apps already appear on the Windows desktop automatically; no `xhost` steps required.

## License
(Add your license information here.)