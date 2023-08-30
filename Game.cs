using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.GameObject;

namespace MonoZenith;

public partial class Game
{
    // Textures
    private Texture2D _backdropTexture;
    private Texture2D _topPipeTexture;
    private Texture2D _bottomPipeTexture;
    
    // Game objects
    private List<Backdrop> _backdrops;
    private List<(Pipe, Pipe)> _pipeCouples;
    private Bird _bird;

    private void InitializeBackdrops()
    {
        const int backdropAmount = 3;
        
        for (int i = 0; i < backdropAmount; i++)
        {
            int backdropX = i * 288 + ScreenWidth / 2;
            _backdrops.Add(new Backdrop(this, new Vector2(backdropX, 256), 288, 512, _backdropTexture));
        }
    }

    private void InitializePipes()
    {
        // Create three pairs of pipes, with a gap size of 100 pixels
        const int pipeGap = 120;
        const int pipeAmount = 2;
        int distanceBetweenPipes = 220;
        
        for (int i = 0; i < pipeAmount; i++)
        {
            int pipeX = ScreenWidth + i * distanceBetweenPipes;
            
            Pipe topPipe = new Pipe(this, new Vector2(pipeX, 0), 52, 320, _topPipeTexture);
            topPipe.ResetVerticalPosition();
            Pipe bottomPipe = new Pipe(this, new Vector2(pipeX, topPipe.Position.Y + pipeGap + 320), 52, 320, _bottomPipeTexture);
            topPipe.LinkedPipe = bottomPipe;
            _pipeCouples.Add((topPipe, bottomPipe));
        }
    }
    
    /* Initialize game vars and load assets. */
    public void Init()
    {
        // Window properties
        SetScreenSize(288, 512);
        SetWindowTitle("Flappy Bird (MonoZenith)");
        
        // Backdrops
        _backdropTexture = LoadImage("Textures/background-day.png");
        _backdrops = new List<Backdrop>();
        InitializeBackdrops();
        
        // Pipes
        _topPipeTexture = LoadImage("Textures/pipe-green-rotated.png");
        _bottomPipeTexture = LoadImage("Textures/pipe-green.png");
        _pipeCouples = new List<(Pipe, Pipe)>();
        InitializePipes();
        
        // Bird
        _bird = new Bird(this, new Vector2(ScreenWidth / 2, 256), 34, 24);
    }

    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        // Update backdrops
        foreach (Backdrop backdrop in _backdrops)
        {
            backdrop.Update(deltaTime);
        }
        
        // Update pipes
        foreach ((Pipe topPipe, Pipe bottomPipe) in _pipeCouples)
        {
            topPipe.Update(deltaTime);
            bottomPipe.Update(deltaTime);
        }
        
        _bird.Update(deltaTime);
    }
    
    /* Draw objects/backdrop. */
    public void Draw()
    {
        // Draw backdrops
        foreach (Backdrop backdrop in _backdrops)
        {
            backdrop.Draw();
        }
        
        // Draw pipes
        foreach ((Pipe topPipe, Pipe bottomPipe) in _pipeCouples)
        {
            topPipe.Draw();
            bottomPipe.Draw();
        }
        
        _bird.Draw();
    }
}