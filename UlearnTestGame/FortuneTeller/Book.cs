using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Book
{
    private readonly Texture2D openedBook;
    private readonly Texture2D cross;
    private Rectangle crossRect;
    private readonly Texture2D textureToOpen;
    private Rectangle textureToOpenRect;
    private bool isVisible;
    private bool canClick;

    public Book(ContentManager content, Texture2D textureToOpen)
    {
        openedBook = content.Load<Texture2D>("openbook");
        cross = content.Load<Texture2D>("cross");
        this.textureToOpen = textureToOpen;
        canClick = true;
    }

    public void Update()
    {
        if (canClick)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed
                && textureToOpenRect.Contains(Mouse.GetState().Position))
            {
                isVisible = true;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed
                && crossRect.Contains(Mouse.GetState().Position))
            {
                isVisible = false;
            }
        }

    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float scale, Vector2 textureToOpenPosition)
    {
        crossRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.935),
            (int)(graphics.PreferredBackBufferHeight * 0.039), cross.Width, cross.Height);
        textureToOpenRect = new Rectangle((int)textureToOpenPosition.X, (int)textureToOpenPosition.Y, textureToOpen.Width,
            textureToOpen.Height);
        
        if (isVisible)
        {
            spriteBatch.Draw(openedBook, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.052), 
                    (int)(graphics.PreferredBackBufferHeight * 0.046)), null, Color.White, 
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cross, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.935), 
                    (int)(graphics.PreferredBackBufferHeight * 0.039)), null, Color.White, 
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public bool IsVisible()
    {
        return isVisible;
    }

    public void ActivateClick()
    {
        canClick = true;
    }
    
    public void DeactivateClick()
    {
        canClick = false;
    }
}