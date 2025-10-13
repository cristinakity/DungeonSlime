# Setting up Game Controllers in WSL2

Game controllers don't automatically work in WSL2 containers because USB devices need to be explicitly shared from Windows to WSL2.

## Option 1: Use USBIPD (Recommended)

1. **Install USBIPD on Windows:**
   ```powershell
   winget install --interactive --exact dorssel.usbipd-win
   ```

2. **List USB devices from Windows PowerShell (as Administrator):**
   ```powershell
   usbipd list
   ```

3. **Find your controller and note the BUSID (e.g., 1-4)**

4. **Bind the controller to WSL:**
   ```powershell
   usbipd bind --busid 1-4
   ```

5. **Attach the controller to WSL2:**
   ```powershell
   usbipd attach --wsl --busid 1-4
   ```

6. **Verify in WSL2:**
   ```bash
   ls /dev/input/
   jstest /dev/input/js0
   ```

## Option 2: Test without Physical Controller

You can test the game using keyboard controls:
- WASD or Arrow Keys for movement
- Space for speed boost
- M to toggle mute
- +/- for volume control

## Option 3: Use Virtual Controller

Install a virtual gamepad software on Windows that can simulate controller input via keyboard, such as:
- vJoy + FreePIE
- x360ce
- AntiMicro

## Rebuilding Container

After setting up USB passthrough, you may need to rebuild the dev container:
1. Press `Ctrl+Shift+P`
2. Type "Dev Containers: Rebuild Container"
3. Select it and wait for rebuild

## Troubleshooting

If controllers still don't work:
1. Check if the device appears: `ls /dev/input/`
2. Test with jstest: `jstest /dev/input/js0`
3. Check permissions: `ls -la /dev/input/`
4. Verify the container has access to input group