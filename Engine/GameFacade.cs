using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZenith;

public class GameFacade
{
    // Members
    private Color _backgroundColor;
    private (int, int) _screenDimensions;
    private string _windowTitle;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly ContentManager _content;

    // Properties
    public Color BackgroundColor => _backgroundColor;
    public int ScreenWidth => _screenDimensions.Item1;
    public int ScreenHeight => _screenDimensions.Item2;
    public string WindowTitle => _windowTitle;

    public GameFacade(SpriteBatch s, GraphicsDeviceManager g, ContentManager content)
    {
        _backgroundColor = new Color(0, 0, 0);
        _spriteBatch = s;
        _graphicsDeviceManager = g;
        _screenDimensions = (300, 300);
        _windowTitle = "MonoZenith";
        _content = content;
    }
    
    public void SetBackgroundColor(Color c)
    {
        _backgroundColor = c;
    }

    public void SetScreenSize(int w, int h)
    {
        _screenDimensions = (w, h);
    }
    
    /// <summary>
    /// Set the window title.
    /// </summary>
    /// <param name="t">The window title.</param>
    public void SetWindowTitle(string t)
    {
        _windowTitle = t;
    }
    
    public bool GetKeyDown(Keys key)
    {
        KeyboardState state = Keyboard.GetState();
        return state.IsKeyDown(key);
    }
    
    public bool GetMouseButtonDown(MouseButtons button)
    {
        var mouseState = Mouse.GetState();
        return button switch
        {
            MouseButtons.Left => mouseState.LeftButton == ButtonState.Pressed,
            MouseButtons.Middle => mouseState.MiddleButton == ButtonState.Pressed,
            MouseButtons.Right => mouseState.RightButton == ButtonState.Pressed,
            _ => false
        };
    }
    
    public Point GetMousePosition()
    {
        var mouseState = Mouse.GetState();
        return mouseState.Position;
    }
    
    public int GetMouseWheelValue()
    {
        var mouseSate = Mouse.GetState();
        return mouseSate.ScrollWheelValue;
    }
    
    public SpriteFont LoadFont(string font)
    {
        return _content.Load<SpriteFont>($"Fonts/{font}");
    }
    
    public void DrawText(string content, Vector2 pos, SpriteFont font, Color c, float scale=1, float angle=0)
    {
        Vector2 origin = font.MeasureString(content) / 2;
        float rotationAngle = MathHelper.ToRadians(angle);
        _spriteBatch.DrawString(font, content, pos, c, rotationAngle, origin, scale, SpriteEffects.None, 0);
    }
    
    /* Source: https://community.monogame.net/t/loading-png-jpg-etc-directly/7403 */
    public Texture2D LoadImage(string filepath)
    {
        FileStream fs = new FileStream($"Content/{filepath}", FileMode.Open);
        Texture2D spriteAtlas = Texture2D.FromStream(_graphicsDeviceManager.GraphicsDevice, fs);
        fs.Dispose();
        return spriteAtlas;
    }
    
    /* Source: https://www.industrian.net/tutorials/texture2d-and-drawing-sprites/ */
    public void DrawImage(Texture2D texture, Vector2 pos, float scale=1, float angle=0, bool flipped=false)
    {
        float rotationAngle = MathHelper.ToRadians(angle);
    
        // ReSharper disable PossibleLossOfFraction
        Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
    
        // Flip image if needed
        var effect = SpriteEffects.None;
        if (flipped)
            effect = SpriteEffects.FlipHorizontally;
    
        _spriteBatch.Draw(texture, pos, null, Color.White, rotationAngle, origin, scale, effect, 0);
    }
    
    public void DrawRectangle(Color color, Vector2 pos, int width, int height)
    {
        Texture2D pixel = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
        Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        _spriteBatch.Draw(pixel, rect, color);
    }

    public SoundEffectInstance LoadAudio(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        var soundEffect = SoundEffect.FromStream(stream);
        return soundEffect.CreateInstance();
    }
}
