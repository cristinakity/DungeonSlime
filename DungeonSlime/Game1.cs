using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime;

public class Game1 : Core
{
    private AnimatedSprite _slime;
    private AnimatedSprite _bat;
    private Vector2 _slimePosition;
    private const float MOVEMENT_SPEED = 5.0f;
    private Vector2 _batPosition;
    private Vector2 _batVelocity;
    private Tilemap _tilemap;
    private Rectangle _roomBounds;
    private SoundEffect _bounceSoundEffect;
    private SoundEffect _collectSoundEffect;
    private Song _themeSong;
    private SpriteFont _font;
    private int _score;
    private Vector2 _scoreTextPosition;
    private Vector2 _scoreTextOrigin;
    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();

        Rectangle screenBounds = GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new Rectangle(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeight,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeight * 2
        );

        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;

        _slimePosition = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);

        _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

        AssignRandomBatVelocity();

        Audio.PlaySong(_themeSong);

        _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        _slime = atlas.CreateAnimatedSprite("slime-animation");
        _slime.Scale = new Vector2(4.0f, 4.0f);

        _bat = atlas.CreateAnimatedSprite("bat-animation");
        _bat.Scale = new Vector2(4.0f, 4.0f);

        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        _tilemap.Scale = new Vector2(4.0f, 4.0f);

        _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        _themeSong = Content.Load<Song>("audio/theme");

        _font = Content.Load<SpriteFont>("fonts/04B_30");
    }

    protected override void Update(GameTime gameTime)
    {
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        CheckKeyboardInput();

        CheckGamePadInput();

        var slimeBounds = new Circle(
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Width * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        if (slimeBounds.Left < _roomBounds.Left)
        {
            _slimePosition.X = _roomBounds.Left;
        }
        else if (slimeBounds.Right > _roomBounds.Right)
        {
            _slimePosition.X = _roomBounds.Right - _slime.Width;
        }

        if (slimeBounds.Top < _roomBounds.Top)
        {
            _slimePosition.Y = _roomBounds.Top;
        }
        else if (slimeBounds.Bottom > _roomBounds.Bottom)
        {
            _slimePosition.Y = _roomBounds.Bottom - _slime.Height;
        }

        var newBatPosition = _batPosition + _batVelocity;

        var batBounds = new Circle(
            (int)(newBatPosition.X + (_bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (_bat.Width * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );

        var normal = Vector2.Zero;


        if (batBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Left;
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Right - _bat.Width;
        }

        if (batBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Top;
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Bottom - _bat.Height;
        }

        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _batVelocity = Vector2.Reflect(_batVelocity, normal);

            Audio.PlaySoundEffect(_bounceSoundEffect);
        }

        _batPosition = newBatPosition;

        if (slimeBounds.Intersects(batBounds))
        {
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            int colum = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            _batPosition = new Vector2(colum * _bat.Width, row * _bat.Height);

            AssignRandomBatVelocity();

            Audio.PlaySoundEffect(_collectSoundEffect);

            _score += 100;
        }

        base.Update(gameTime);
    }

    private void AssignRandomBatVelocity()
    {
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        var direction = new Vector2(x, y);

        _batVelocity = direction * MOVEMENT_SPEED;
    }

    private StringBuilder GetPressedKeys()
    {
        var teclas = new StringBuilder();
        teclas.AppendLine("Teclas presionadas:");

        var pressedKeys = Input.Keyboard.CurrentState.GetPressedKeys();

        if (pressedKeys.Length > 0)
        {
            foreach (var key in pressedKeys)
            {
                teclas.AppendLine($"- {key}");
            }
        }
        return teclas;
    }

    private void CheckKeyboardInput()
    {
        //Console.WriteLine(GetPressedKeys());
        float speed = MOVEMENT_SPEED;
        if (Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= speed;
        }

        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += speed;
        }

        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= speed;
        }

        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += speed;
        }

        if (Input.Keyboard.WasKeyJustPressed(Keys.M))
        {
            Audio.ToggleMute();
        }

        if (Input.Keyboard.WasKeyJustReleased(Keys.OemPlus))
        {
            Audio.SongVolumen += 0.1f;
            Audio.SoundEffectVolume += 0.1f;
        }

        if (Input.Keyboard.WasKeyJustReleased(Keys.OemMinus))
        {
            Audio.SongVolumen -= 0.1f;
            Audio.SoundEffectVolume -= 0.1f;
        }
    }

    private void CheckGamePadInput()
    {
        var gamePadState = Input.GamePads[(int)PlayerIndex.One];

        float speed = MOVEMENT_SPEED;
        if (gamePadState.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        // if (gamePadState.IsButtonDown(Buttons.B))
        // {
        //     speed *= 1.5f;
        //     GamePad.SetVibration(PlayerIndex.One, 1.0f, 0.0f);
        // }
        // else
        // {
        //     GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        // }


        if (gamePadState.LeftThumbStick != Vector2.Zero)
        {
            _slimePosition.X += gamePadState.LeftThumbStick.X * speed;
            _slimePosition.Y -= gamePadState.LeftThumbStick.Y * speed;
        }
        else
        {
            if (gamePadState.IsButtonDown(Buttons.DPadUp))
            {
                _slimePosition.Y -= speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadDown))
            {
                _slimePosition.Y += speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                _slimePosition.X -= speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                _slimePosition.X += speed;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // int colorIndex = (int)((gameTime.TotalGameTime.TotalSeconds / 2) % rainbowColors.Length);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // _slime.Draw(SpriteBatch, Vector2.Zero, Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 0.0f);
        // _bat.Draw(SpriteBatch, new Vector2(_slime.Width * 4.0f + 10, 0), Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 1.0f);
        // _slime.Draw(SpriteBatch, new Vector2((_slime.Width * 4.0f) * 2 + 10, 0), Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 0.0f);

        _tilemap.Draw(SpriteBatch);

        _slime.Draw(SpriteBatch, _slimePosition);

        _bat.Draw(SpriteBatch, _batPosition);

        SpriteBatch.DrawString(
            _font,
            $"Score: {_score}",
            _scoreTextPosition,
            Color.White,
            0.0f,
            _scoreTextOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
