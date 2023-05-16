using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Menu
{
    private readonly Texture2D menuBackground;
    private readonly Texture2D exitButton;
    private readonly Texture2D startButton;
    private bool isActive;

    public Menu(ContentManager content)
    {
        menuBackground = content.Load<Texture2D>("menuback");
        exitButton = content.Load<Texture2D>("exit");
        startButton = content.Load<Texture2D>("start");
        isActive = true;
    }

    public void Update(GraphicsDeviceManager graphics)
    {
        var exitButtonRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.339), 
            (int)(graphics.PreferredBackBufferHeight * 0.666), (int)(exitButton.Width * 0.65), 
            (int)(exitButton.Height * 0.666));
        var startButtonRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.339), 
            (int)(graphics.PreferredBackBufferHeight * 0.46), (int)(startButton.Width * 0.65), 
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
    
    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float scale)
    {
        if (isActive)
        {
            spriteBatch.Draw(menuBackground, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(startButton, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.339), 
                    (int)(graphics.PreferredBackBufferHeight * 0.46)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(exitButton, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.339), 
                    (int)(graphics.PreferredBackBufferHeight * 0.666)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}