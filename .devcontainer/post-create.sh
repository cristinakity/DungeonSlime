#!/bin/bash

# Install MonoGame templates
dotnet new install MonoGame.Templates.CSharp

# Set up multiarch for wine32
sudo dpkg --add-architecture i386

# Install Xvfb (X Virtual Framebuffer) and required system packages
sudo apt-get update && sudo apt-get install -y xvfb curl p7zip-full wine64 wine32:i386

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

