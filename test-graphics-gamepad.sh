#!/bin/bash

echo "=== Graphics and Input Device Test ==="
echo

# Test graphics
echo "Testing graphics setup:"
echo "DISPLAY: $DISPLAY"
echo "LIBGL_ALWAYS_INDIRECT: $LIBGL_ALWAYS_INDIRECT"
echo

# Test OpenGL
echo "Testing OpenGL:"
glxinfo | grep "OpenGL" | head -5 2>/dev/null || echo "glxinfo not available or OpenGL not working"
echo

# Test gamepad devices
echo "Testing gamepad devices:"
ls -la /dev/input/js* 2>/dev/null || echo "No joystick devices found"
echo

# Test if X11 forwarding works
echo "Testing X11 (this should show a simple window if graphics work):"
echo "Running xclock for 3 seconds..."
timeout 3s xclock 2>/dev/null || echo "X11 forwarding not working or xclock not available"
echo

echo "=== MonoGame Requirements Check ==="
echo "Checking if required libraries are available:"

# Check for OpenGL libraries
echo -n "libGL.so.1: "
ldconfig -p | grep -q "libGL.so.1" && echo "✓ Found" || echo "✗ Missing"

echo -n "libGLU.so.1: "
ldconfig -p | grep -q "libGLU.so.1" && echo "✓ Found" || echo "✗ Missing"

echo -n "libglut.so: "
ldconfig -p | grep -q "libglut.so" && echo "✓ Found" || echo "✗ Missing"

echo
echo "=== Recommendations ==="
echo "If graphics test fails:"
echo "1. Ensure X11 forwarding is enabled on your host"
echo "2. On Bazzite, make sure you have: xhost +local: (run this on host)"
echo "3. Try running: export DISPLAY=:0 (in container)"
echo "4. For hardware acceleration, ensure /dev/dri is properly mapped"
echo
echo "If gamepad test fails:"
echo "1. Ensure your Xbox controller is connected to the host"
echo "2. Rebuild the dev container to apply device mappings"
echo "3. Check if controller shows up in host with: ls /dev/input/js*"