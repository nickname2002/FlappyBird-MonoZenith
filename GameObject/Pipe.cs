using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.GameObject;

public class Pipe : GameObject
{
    private readonly Texture2D _texture;
    private const float ScrollSpeed = 2f;
    private readonly Random _random;
    public Pipe LinkedPipe;

    public Pipe(Game g, Vector2 pos, int w, int h, Texture2D texture) : base(g, pos, w, h)
    {
        _texture = texture;
        _random = new Random();
    }

    public void ResetVerticalPosition()
    {
        if (LinkedPipe == null)
        {
            return;
        }
        
        Position.Y = _random.Next(1, 180);
        const int gap = 120 + 320;
        LinkedPipe.Position.Y = Position.Y + gap;
    }
    
    private void Move()
    {
        if (Position.X < -Width)
        {
            Position.X = Game.ScreenWidth + Width * 1.5f;
            ResetVerticalPosition();
        }

        Position.X -= ScrollSpeed;
    }
    
    public override void Update(GameTime deltaTime)
    {
        Move();
    }

    public override void Draw()
    {
        Game.DrawImage(_texture, Position, 1);
    }
}