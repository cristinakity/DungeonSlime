#!/bin/bash

# Install MonoGame templates
dotnet new install MonoGame.Templates.CSharp

# Set up multiarch for wine32
sudo dpkg --add-architecture i386

# Install Xvfb (X Virtual Framebuffer) and required system packages + gamepad tools
sudo apt-get update && sudo apt-get install -y xvfb curl p7zip-full wine64 wine32:i386 joystick jstest-gtk evtest

sudo apt-get update && sudo apt-get install -y libgtk-3-0 libgtk-3-dev libcanberra-gtk-module
sudo apt-get update && sudo apt-get install -y fonts-cantarell libgtk-3-0 libgtk-3-dev libcanberra-gtk-module gtk-sharp3
sudo apt-get install -y fonts-cantarell
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

# Install MonoGame content pipeline tools
wget -qO- https://monogame.net/downloads/net8_mgfxc_wine_setup.sh | bash

# Add environment variables to bashrc for persistent sessions
echo "export XDG_RUNTIME_DIR=\"${XDG_RUNTIME_DIR}\"" >> ~/.bashrc
echo '# DISPLAY will be inherited from host environment' >> ~/.bashrc

# Set up gamepad/joystick permissions (minimal setup)
sudo groupadd -f input || true
sudo usermod -a -G input vscode || true

