using Microsoft.Xna.Framework;

namespace MonoZenith.GameObject;

public abstract class GameObject
{
    protected Game Game;
    public Vector2 Position;
    protected int Width;
    protected int Height;

    protected GameObject(Game g, Vector2 pos, int w, int h)
    {
        Game = g;
        Position = pos;
        Width = w;
        Height = h;
    }

    public abstract void Update(GameTime deltaTime);
    public abstract void Draw();
}