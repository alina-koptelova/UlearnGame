using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Menu
{
    private Texture2D menuBackground;
    private Texture2D exitButton;
    private Texture2D startButton;
    private bool isActive;
    
    public Menu(ContentManager content)
    {
        menuBackground = content.Load<Texture2D>("menuback");
        exitButton = content.Load<Texture2D>("exit");
        startButton = content.Load<Texture2D>("start");
        isActive = true;
    }

    public void Update(GraphicsDeviceManager _graphics)
    {
        var exitButtonRect = new Rectangle((int)(_graphics.PreferredBackBufferWidth * 0.339), 
            (int)(_graphics.PreferredBackBufferHeight * 0.666), (int)(exitButton.Width * 0.65), 
            (int)(exitButton.Height * 0.666));
        var startButtonRect = new Rectangle((int)(_graphics.PreferredBackBufferWidth * 0.339), 
            (int)(_graphics.PreferredBackBufferHeight * 0.46), (int)(startButton.Width * 0.65), 
            (int)(startButton.Height * 0.46));

        if (isActive && Mouse.GetState().LeftButton == ButtonState.Pressed && 
            exitButtonRect.Contains(Mouse.GetState().Position))
        {
            Environment.Exit(0);
        }

        if (isActive && Mouse.GetState().LeftButton == ButtonState.Pressed &&
            startButtonRect.Contains(Mouse.GetState().Position))
        {
            isActive = false;
        }
    }
    
    public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics, float scale)
    {
        if (isActive)
        {
            _spriteBatch.Draw(menuBackground, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(startButton, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.339), (int)(_graphics.PreferredBackBufferHeight * 0.46)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(exitButton, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.339), (int)(_graphics.PreferredBackBufferHeight * 0.666)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}