using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGameLibrary;
using System.Runtime.InteropServices;

namespace DungeonSlime;

public class Game1 : Core
{
    private Texture2D _logo;
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
        _logo = Content.Load<Texture2D>("images/logo");
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

        // // Cambia el color de la pantalla cada 2 segundos, tipo arcoiris
        // Color[] rainbowColors = new Color[]
        // {
        //     Color.Red,
        //     Color.Orange,
        //     Color.Yellow,
        //     Color.Green,
        //     Color.Blue,
        //     Color.Indigo,
        //     Color.Violet
        // };

        // int colorIndex = (int)((gameTime.TotalGameTime.TotalSeconds / 2) % rainbowColors.Length);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Rectangle iconSourceRect = new Rectangle(0, 0, 128, 128);
        Rectangle wordmarkSourceRect = new Rectangle(150, 34, 458, 58);

        // TODO: Add your drawing code here
        SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

        // var logoPosition = new Vector2(
        //     (Window.ClientBounds.Width * 0.5f) - (_logo.Width * 0.5f),
        //     (Window.ClientBounds.Height * 0.5f) - (_logo.Height * 0.5f)
        // );
        SpriteBatch.Draw(
            _logo,
            new Vector2(
                Window.ClientBounds.Width,
                Window.ClientBounds.Height
            ) * 0.5f,
            iconSourceRect,
            Color.White,
            0.0f,
            new Vector2(
                iconSourceRect.Width,
                iconSourceRect.Height
            ) * 0.5f,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        SpriteBatch.Draw(
            _logo,
            new Vector2(
                Window.ClientBounds.Width,
                Window.ClientBounds.Height
            ) * 0.5f,
            wordmarkSourceRect,
            Color.White,
            0.0f,
            new Vector2(
                wordmarkSourceRect.Width,
                wordmarkSourceRect.Height
            ) * 0.5f,
            1.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
