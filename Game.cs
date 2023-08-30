using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components;
using MonoZenith.GameObject;

namespace MonoZenith;

public partial class Game
{
    // Textures
    private Texture2D _backdropTexture;
    private Texture2D _topPipeTexture;
    private Texture2D _bottomPipeTexture;
    private Texture2D _startLogoTexture;
    private Texture2D _gameOverLogoTexture;
    
    // Game objects
    private List<Backdrop> _backdrops;
    private List<(Pipe, Pipe)> _pipeCouples;
    private Bird _bird;
    
    // UI Components
    Button _startButton;
    Button _gameOverButton;
    
    // Game state
    private SpriteFont _pixelFont;
    private int _score = 0;
    private GameStates _gameState;
    private enum GameStates
    {
        Start,
        Playing,
        GameOver
    }
    
    // Audio
    private SoundEffectInstance _pointSound;

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
        _pipeCouples = new List<(Pipe, Pipe)>();
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
        _pixelFont = LoadFont("pixel");
        
        // Backdrops
        _backdropTexture = LoadImage("Textures/background-day.png");
        _backdrops = new List<Backdrop>();
        InitializeBackdrops();
        
        // Pipes
        _topPipeTexture = LoadImage("Textures/pipe-green-rotated.png");
        _bottomPipeTexture = LoadImage("Textures/pipe-green.png");
        InitializePipes();
        
        // Bird
        _bird = new Bird(this, new Vector2(ScreenWidth / 2, 256), 34, 24);
        
        // Start screen
        _startLogoTexture = LoadImage("Textures/message.png");
        _gameState = GameStates.Start;
        _startButton = new Button(
            this,
            new Vector2(45, 512 / 2 + 100),
            200, 100,
            "START", 3, Color.White,
            Color.Black, 0, Color.Black);
        _startButton.SetOnClickAction(StartGameEvent);
        
        // Game over screen
        _gameOverLogoTexture = LoadImage("Textures/gameover.png");
        _gameOverButton = new Button(
            this,
            new Vector2(45, 512 / 2 + 100),
            200, 100,
            "RESTART", 2, Color.White,
            Color.DarkOrange, 3, Color.OrangeRed);
        _gameOverButton.SetOnClickAction(GoToStartScreenEvent);
        
        // Load audio
        _pointSound = LoadAudio("Content/Audio/point.wav");
    }

    public void HandleCollision()
    {
        if (_bird.IsDead)
            return;
        
        foreach ((Pipe topPipe, Pipe bottomPipe) in _pipeCouples)
        {
            if (_bird.CollidesWith(topPipe) || _bird.CollidesWith(bottomPipe))
            {
                _bird.HitSound.Play();
                _bird.IsDead = true;
            }
        }
    }

    public void UpdateScore()
    {
        // Add 1 to score when the bird passes a pipe
        foreach ((Pipe topPipe, _) in _pipeCouples)
        {
            if (Math.Abs(_bird.Position.X - topPipe.Position.X) < 1)
            {
                _pointSound.Play();
                _score++;
            }
        }
    }
    
    public void StartGameEvent()
    {
        InitializePipes();
        _bird = new Bird(this, new Vector2(ScreenWidth / 2, 256), 34, 24);
        _gameState = GameStates.Playing;
    }

    public void GoToStartScreenEvent()
    {
        _gameState = GameStates.Start;
        _score = 0;
    }
    
    public void DisplayStartScreen()
    {
        DrawImage(_backdropTexture, new Vector2(288 / 2, 512 / 2), 1);
        DrawImage(_startLogoTexture, new Vector2(288 / 2, 512 / 2), 1);
    }

    public void UpdateGame(GameTime deltaTime)
    {
        if (_bird.IsGameOver)
        {
            _gameState = GameStates.GameOver;
        }
        
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
        UpdateScore();
        HandleCollision();
    }
    
    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        switch (_gameState)
        {
            case GameStates.Start:
                _startButton.Update(deltaTime);
                break;
            
            case GameStates.Playing:
                UpdateGame(deltaTime);
                break;
            
            case GameStates.GameOver:
                _gameOverButton.Update(deltaTime);
                break;
            
            default:
                _startButton.Update(deltaTime);
                break;
        }
    }
    
    public void DisplayGame()
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
        DrawText($"{_score.ToString()}", new Vector2(ScreenWidth / 2, 50), _pixelFont, Color.White, 2);
    }

    public void DisplayGameOverScreen()
    {
        DrawImage(_backdropTexture, new Vector2(288 / 2, 512 / 2), 1);
        DrawImage(_gameOverLogoTexture, new Vector2(288 / 2, 512 / 2 - 100), 1);
        DrawText($"{_score.ToString()}", new Vector2(ScreenWidth / 2, 50), _pixelFont, Color.White, 2);
        _gameOverButton.Draw();
    }
    
    /* Draw objects/backdrop. */
    public void Draw()
    {
        switch (_gameState)
        {
            case GameStates.Start:
                DisplayStartScreen();
                break;
            
            case GameStates.Playing:
                DisplayGame();
                break;
            
            case GameStates.GameOver:
                DisplayGameOverScreen();
                break;
            
            default:
                DisplayStartScreen();
                break;
        }
    }
}