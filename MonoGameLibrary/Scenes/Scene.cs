using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    protected ContentManager Content { get; }
    public bool IsDisposed { get; private set; }

    public Scene()
    {
        Content = new ContentManager(Core.Content.ServiceProvider);

        Content.RootDirectory = Core.Content.RootDirectory;

    }

    public virtual void Initialize()
    {
        LoadContent();
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime) { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disponsing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disponsing)
        {
            UnloadContent();
            Content.Dispose();
        }
    }

}