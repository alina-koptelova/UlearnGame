using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Card
{
    private Texture2D cardTexture;
    private Texture2D animationSheet;
    private Rectangle[] frames;
    private int currentFrameIndex;
    private float timer;
    private float animationSpeed = 0.05f;
    private bool isAnimating;
    private bool isFlipped = false;
    private Rectangle cardRect;
    private Vector2 position;
    private Texture2D cardsDeck;
    private Rectangle cardsDeckRect;
    private bool isVisible;
    private float opacity;
    private const float speed = 0.01f;
    private string cardName;
    public Card(ContentManager content, Texture2D cardsDeck, string cardName)
    {
        cardTexture = content.Load<Texture2D>(cardName); // потом поменять чтобы разное делать
        animationSheet = content.Load<Texture2D>("cardsAnimation");
        this.cardsDeck = cardsDeck;
        frames = new Rectangle[14];
        
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
        if (Mouse.GetState().LeftButton == ButtonState.Pressed
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
        
        if (isVisible && opacity < 1f) // если карта должна быть видна и еще не полностью появилась
        {
            opacity += speed; // постепенно увеличиваем прозрачность
            
            if (opacity > 1f) 
                opacity = 1f; // не даем прозрачности превышать 1
        }
        
        if (isAnimating && !isFlipped)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timer > animationSpeed)
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
        }
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }
}