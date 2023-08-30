
using Microsoft.Xna.Framework;

namespace MonoZenith.Components;

public abstract class Component
{
    protected Game Game;
    protected Vector2 Position;
    protected int Width;
    protected int Height;
    
    protected Component(Game g, Vector2 pos, int width, int height)
    {
        Game = g;
        Position = pos;
        Width = width;
        Height = height;
    }
    
    public abstract void Update(GameTime deltaTime);
    public abstract void Draw();
}