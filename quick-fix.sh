#!/bin/bash

echo "=== MonoGame Dev Container Quick Fix ==="
echo

# Set DISPLAY
export DISPLAY=:1
echo "✓ DISPLAY set to: $DISPLAY"

# Test X11 connection
echo
echo "Testing X11 connection..."
if [ -S "/tmp/.X11-unix/X1" ]; then
    echo "✓ X11 socket found at /tmp/.X11-unix/X1"
else
    echo "✗ X11 socket not found"
    echo "  Available sockets:"
    ls -la /tmp/.X11-unix/
fi

# Test gamepad
echo
echo "Testing gamepad devices..."
if ls /dev/input/js* &>/dev/null; then
    echo "✓ Gamepad devices found:"
    ls -la /dev/input/js*
else
    echo "✗ No gamepad devices found"
fi

echo
echo "=== IMPORTANT: Run this on your Bazzite HOST (not in container) ==="
echo "xhost +local:"
echo
echo "Then try running your game with:"
echo "export DISPLAY=:1"
echo "cd DungeonSlime && dotnet run"
