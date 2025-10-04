#!/bin/bash

echo "=== Gamepad/Controller Detection Test ==="
echo

# Check if any input devices are available
echo "Available input devices:"
ls -la /dev/input/ 2>/dev/null || echo "No input devices found"
echo

# Check for joystick devices specifically
echo "Joystick devices:"
ls -la /dev/input/js* 2>/dev/null || echo "No joystick devices found"
echo

# Check for event devices
echo "Event devices:"
ls -la /dev/input/event* 2>/dev/null || echo "No event devices found"
echo

# Test with jstest if available
if command -v jstest &> /dev/null; then
    echo "Testing gamepad with jstest (if available):"
    if ls /dev/input/js* 1> /dev/null 2>&1; then
        echo "Found joystick devices. Run 'jstest /dev/input/js0' to test the first controller."
        echo "Press Ctrl+C to exit the test when done."
        echo
        echo "Quick test of first controller (5 seconds):"
        timeout 5s jstest --show /dev/input/js0 2>/dev/null || echo "No controller input detected in 5 seconds"
    else
        echo "No joystick devices found to test"
    fi
else
    echo "jstest not available - install with: sudo apt install joystick"
fi

echo
echo "=== Instructions ==="
echo "1. Make sure your controller is connected to the host system"
echo "2. Rebuild the dev container to apply the new device mappings"
echo "3. Run your MonoGame application - gamepad input should work"
echo "4. Use 'jstest /dev/input/js0' to test controller input manually"