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

    public bool CollidesWith(GameObject other)
    {
        return Position.X + Width / 2 > other.Position.X - other.Width / 2 &&
               Position.X - Width / 2 < other.Position.X + other.Width / 2 &&
               Position.Y + Height / 2 > other.Position.Y - other.Height / 2 &&
               Position.Y - Height / 2 < other.Position.Y + other.Height / 2;
    }
    
    public abstract void Update(GameTime deltaTime);
    public abstract void Draw();
}