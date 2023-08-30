using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.GameObject;

public class Backdrop : GameObject
{
    private readonly Texture2D _texture;
    private const float ScrollSpeed = 0.5f;
    
    public Backdrop(Game g, Vector2 pos, int w, int h, Texture2D texture) : base(g, pos, w, h)
    {
        _texture = texture;
    }

    public void Move()
    {
        Position.X -= ScrollSpeed;

        if (Position.X < -Width / 2)
        {
            Position.X = 288 + Width / 2;
        }
    }
    
    public override void Update(GameTime deltaTime)
    {
        Move();
    }
    
    public override void Draw()
    {
        Game.DrawImage(_texture, Position);
    }
}