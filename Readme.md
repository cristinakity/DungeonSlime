

On the host, run the following command to allow local X11 connections:

```sh
xhost +local:$(id -un)
```