#!/bin/bash

echo "=== Host Controller Detection Guide ==="
echo
echo "This guide helps you find controllers on your HOST system (not the container)"
echo

echo "=== For Linux Host ==="
echo "1. List input devices:"
echo "   ls /dev/input/"
echo "   # Look for js0, js1, etc. (joystick devices)"
echo
echo "2. Check connected USB devices:"
echo "   lsusb | grep -i 'game\\|joy\\|controller\\|xbox\\|playstation'"
echo
echo "3. Use evtest to see all input devices:"
echo "   sudo evtest"
echo "   # Shows all input devices with their names"
echo
echo "4. Test specific gamepad:"
echo "   sudo evtest /dev/input/event<X>"
echo "   # Replace <X> with the event number for your controller"
echo
echo "5. Check kernel messages for new devices:"
echo "   dmesg | tail -20"
echo "   # Connect/disconnect controller and run this"
echo

echo "=== For Windows Host with WSL ==="
echo "1. In Windows PowerShell (as Administrator):"
echo "   Get-PnpDevice | Where-Object {\\$_.FriendlyName -like '*game*' -or \\$_.FriendlyName -like '*controller*' -or \\$_.FriendlyName -like '*xbox*'}"
echo
echo "2. Check Device Manager:"
echo "   - Open Device Manager (devmgmt.msc)"
echo "   - Look under 'Human Interface Devices' or 'Gaming devices'"
echo
echo "3. Windows Game Controller Settings:"
echo "   - Press Win+R, type 'joy.cpl' and press Enter"
echo "   - This opens Game Controllers panel"
echo

echo "=== For macOS Host ==="
echo "1. System Information:"
echo "   - Hold Option key and click Apple menu > System Information"
echo "   - Look under Hardware > USB for connected controllers"
echo
echo "2. Terminal command:"
echo "   system_profiler SPUSBDataType | grep -A 10 -B 5 -i 'game\\|controller\\|xbox\\|playstation'"
echo

echo "=== Common Controller Types ==="
echo "- Xbox controllers: Usually show as 'Xbox' or 'Microsoft Controller'"
echo "- PlayStation controllers: Show as 'Sony' or 'Wireless Controller'"
echo "- Nintendo controllers: Show as 'Nintendo' or 'Pro Controller'"
echo "- Generic USB gamepads: Various names, often with 'USB Gamepad' or 'HID'"
echo

echo "=== Quick Test Commands for Linux Host ==="
echo "Run these commands on your HOST system (outside the container):"
echo

echo "# Check if any joystick devices exist"
echo "ls -la /dev/input/js*"
echo

echo "# List all input devices with details"
echo "cat /proc/bus/input/devices | grep -A 5 -B 5 -i game"
echo

echo "# Check USB devices for controllers"
echo "lsusb | grep -i -E 'game|joy|controller|xbox|playstation|nintendo'"
echo

echo "=== Testing Controller Input ==="
echo "If you find a controller device (like /dev/input/js0), test it with:"
echo "sudo apt install joystick  # Install testing tools"
echo "jstest /dev/input/js0      # Test the first controller"
echo

echo "=== Troubleshooting ==="
echo "1. Make sure controller is properly connected"
echo "2. Try unplugging and reconnecting"
echo "3. Check if controller works in other applications"
echo "4. Some wireless controllers need to be paired first"
echo "5. Some controllers may need specific drivers"