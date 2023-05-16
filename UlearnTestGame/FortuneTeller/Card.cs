using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Card
{
    private readonly Texture2D cardTexture;
    private readonly Texture2D animationSheet;
    private readonly Rectangle[] frames;
    private int currentFrameIndex;
    private float timer;
    private const float AnimationSpeed = 0.05f;
    private bool isAnimating;
    private bool isFlipped;
    private Rectangle cardRect;
    private readonly Texture2D cardsDeck;
    private Rectangle cardsDeckRect;
    private bool isVisible;
    private float opacity;
    private const float Speed = 0.01f;
    private readonly Texture2D openBookButton;
    private readonly Book openedBook;
    private bool canClick;
    
    public Card(ContentManager content, Texture2D cardsDeck, string cardTextureName)
    {
        cardTexture = content.Load<Texture2D>(cardTextureName);
        animationSheet = content.Load<Texture2D>("cardsAnimation");
        this.cardsDeck = cardsDeck;
        frames = new Rectangle[14];
        openBookButton = content.Load<Texture2D>("openBookButton");
        openedBook = new Book(content, openBookButton);
        canClick = true;
        
        for (var i = 0; i < 5; i++) 
        {
            frames[i] = new Rectangle(i * cardTexture.Width, 0, cardTexture.Width, 
                cardTexture.Height);
        }
        for (var i = 0; i < 5; i++)
        {
            frames[i + 5] = new Rectangle(i * cardTexture.Width, cardTexture.Height, 
                cardTexture.Width, cardTexture.Height);
        }
        for (var i = 0; i < 4; i++)
        {
            frames[i + 10] = new Rectangle(i * cardTexture.Width, cardTexture.Height * 2, 
                cardTexture.Width, cardTexture.Height);
        }
    }

    public void Update(GameTime gameTime)
    {
        openedBook.Update();
        
        if (canClick && Mouse.GetState().LeftButton == ButtonState.Pressed
            && cardsDeckRect.Contains(Mouse.GetState().Position))
        {
            isVisible = true;
        }
        if (isVisible && Mouse.GetState().LeftButton == ButtonState.Pressed 
                      && cardRect.Contains(Mouse.GetState().Position))
        {
            currentFrameIndex = 0;
            isAnimating = true;
            timer = 0f;
        }
        if (isVisible && opacity < 1f)
        {
            opacity += Speed;
            
            if (opacity > 1f) 
                opacity = 1f;
        }
        if (isAnimating && !isFlipped)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timer > AnimationSpeed)
            {
                timer = 0;
                currentFrameIndex++;
                
                if (currentFrameIndex >= frames.Length)
                {
                    isAnimating = false;
                    isFlipped = true;
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float scale, Vector2 position)
    {
        cardsDeckRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.3),
            (int)(graphics.PreferredBackBufferHeight * 0.798), cardsDeck.Width, cardsDeck.Height);
        cardRect = new Rectangle((int)position.X, (int)position.Y, 
            (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        var openBookButtonPosition = new Vector2((int)(graphics.PreferredBackBufferWidth * 0.722),
            (int)(graphics.PreferredBackBufferHeight * 0.591));

        if (isVisible)
        {
            if (isFlipped)
            {
                spriteBatch.Draw(cardTexture, cardRect, Color.White * opacity);
            }
            else
            {
                spriteBatch.Draw(animationSheet, cardRect, frames[currentFrameIndex], 
                    Color.White * opacity);
            }

            spriteBatch.Draw(openBookButton, openBookButtonPosition, null, Color.White, 
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            openedBook.Draw(spriteBatch, graphics, scale, openBookButtonPosition);
        }
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }

    public bool BookIsOpened()
    {
        return openedBook.IsVisible();
    }

    public void DeactivateClick()
    {
        canClick = false;
    }

    public void ActivateClick()
    {
        canClick = true;
    }
}