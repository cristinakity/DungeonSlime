#!/bin/bash

# MonoGame .NET 8 Windows/WSL2 Post-Create Setup
echo "Setting up MonoGame development environment for Windows/WSL2..."

# Install MonoGame templates
dotnet new install MonoGame.Templates.CSharp

# Set up multiarch for wine32 (needed for MGCB editor)
sudo dpkg --add-architecture i386

# Update package lists
sudo apt-get update

# Install Xvfb (X Virtual Framebuffer) and required system packages + gamepad tools
sudo apt-get install -y xvfb curl p7zip-full wine64 wine32:i386 joystick jstest-gtk evtest

# Install audio packages for WSL2/Linux containers
sudo apt-get install -y pulseaudio pulseaudio-utils alsa-utils libasound2-dev

# Configure ALSA to use PulseAudio as the default device
sudo tee /etc/asound.conf > /dev/null << EOF
pcm.!default {
    type pulse
}
ctl.!default {
    type pulse
}
EOF

# Create user-specific ALSA configuration
mkdir -p ~/.config/pulse
tee ~/.asoundrc > /dev/null << EOF
pcm.!default {
    type pulse
}
ctl.!default {
    type pulse
}
EOF

# Install GTK libraries (essential for MGCB editor)
sudo apt-get install -y libgtk-3-0 libgtk-3-dev libcanberra-gtk-module
sudo apt-get install -y fonts-cantarell libgtk-3-0 libgtk-3-dev libcanberra-gtk-module gtk-sharp3
sudo apt-get install -y fonts-cantarell

# Install image viewer and file associations
sudo apt-get install -y eog
sudo apt-get install -y xdg-utils
xdg-mime default eog.desktop image/png
xdg-mime default eog.desktop image/jpeg image/jpg image/bmp image/gif image/tiff image/webp

# Set up XDG_RUNTIME_DIR (use host if available, fallback to temp)
if [ -d "/run/user/1000" ]; then
    export XDG_RUNTIME_DIR="/run/user/1000"
else
    export XDG_RUNTIME_DIR="/tmp/runtime-vscode"
    mkdir -p $XDG_RUNTIME_DIR
    chmod 700 $XDG_RUNTIME_DIR
fi

# Install MonoGame content pipeline tools (CRITICAL for MGCB editor)
echo "Installing MonoGame content pipeline tools..."
wget -qO- https://monogame.net/downloads/net8_mgfxc_wine_setup.sh | bash

# Add environment variables to bashrc for persistent sessions
echo "export XDG_RUNTIME_DIR=\"${XDG_RUNTIME_DIR}\"" >> ~/.bashrc
echo '# DISPLAY will be inherited from host environment' >> ~/.bashrc

# Set up audio environment variables
echo 'export PULSE_SERVER="unix:${XDG_RUNTIME_DIR}/pulse/native"' >> ~/.bashrc
echo 'export ALSOFT_DRIVERS=pulse,alsa' >> ~/.bashrc

# Set up gamepad/joystick permissions (minimal setup)
sudo groupadd -f input || true
sudo usermod -a -G input vscode || true

echo "MonoGame Windows/WSL2 setup complete!"
echo ""
echo "Audio: Configured for PulseAudio via WSLg"
echo "Controllers: Will be detected at runtime (use USBIPD for physical controllers)"
echo "Graphics: Configured for X11 via WSLg"
echo "MGCB Editor: Wine-based content pipeline tools installed"
echo ""
echo "To test your setup:"
echo "  dotnet run"