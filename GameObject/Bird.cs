using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZenith.GameObject;

internal enum BirdStates
{
    UpFlap,
    MidFlap,
    DownFlap
}

public class Bird : GameObject
{
    private const float Gravity = 0.5f;
    private const float MaxFallSpeed = 10f;
    private const float JumpSpeed = 5f;
    private Vector2 _velocity;
    private List<Texture2D> _textures;
    private BirdStates _state;
    private float _rotation;
    private bool _isDead;
    
    // Animation delay
    private readonly float _animationDelay;
    private float _animationTimer;

    public Bird(Game g, Vector2 pos, int w, int h) : base(g, pos, w, h)
    {
        _velocity = Vector2.Zero;
        _textures = new List<Texture2D>
        {
            Game.LoadImage("Textures/yellowbird-upflap.png"),
            Game.LoadImage("Textures/yellowbird-midflap.png"),
            Game.LoadImage("Textures/yellowbird-downflap.png"),
        };
        _state = BirdStates.UpFlap;
        _rotation = -25f;
        _isDead = false;
        
        // Set animation delay
        _animationDelay = 100f;
    }

    private void ApplyGravity()
    {
        if (_velocity.Y < MaxFallSpeed)
        {
            _velocity.Y += Gravity;
        }
    }

    private void UpdateState(GameTime deltaTime)
    {
        // Update animation timer
        _animationTimer += deltaTime.ElapsedGameTime.Milliseconds;
        if (!(_animationTimer >= _animationDelay)) 
            return;
        
        _state = (BirdStates)(((int)_state + 1) % 3);
        _animationTimer = 0f;
    }
    
    private void Move()
    {
        // Check if bird is dead
        if (Position.Y > Game.ScreenHeight)
        {
            _isDead = true;
            return;
        }

        // Check if bird is jumping
        if (Game.GetKeyDown(Keys.Space))
        {
            _rotation = -25f;
            _velocity.Y = -JumpSpeed;
        }
        else
        {
            _rotation += 2f;
        }

        ApplyGravity();
        Position += _velocity;
    }
    
    public override void Update(GameTime deltaTime)
    {
        if (_isDead)
        {
            return;
        }
        
        UpdateState(deltaTime);
        Move();
    }

    public override void Draw()
    {
        Game.DrawImage(_textures[(int)_state], Position, 1, _rotation);
    }
}