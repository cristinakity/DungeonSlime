using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Auto-exit after 3 seconds in headless mode for testing (disabled for VNC)
        // if (_headlessMode)
        // {
        //     _totalTime += gameTime.ElapsedGameTime.TotalSeconds;
        //     if (_totalTime >= 3.0)
        //     {
        //         Exit();
        //     }
        // }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Console.WriteLine("gameTime.TotalGameTime.TotalSeconds: " + gameTime.TotalGameTime.TotalSeconds);

        // Cambia el color de la pantalla cada 2 segundos, tipo arcoiris
        Color[] rainbowColors = new Color[]
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.Indigo,
            Color.Violet
        };

        int colorIndex = (int)((gameTime.TotalGameTime.TotalSeconds / 2) % rainbowColors.Length);
        GraphicsDevice.Clear(rainbowColors[colorIndex]);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
