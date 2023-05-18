using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Client
{
    private Rectangle doorRect;
    private bool isDoorLocked;
    private readonly Texture2D client;
    private bool clientIsVisible;
    private readonly DialogBox dialogBox;
    private readonly AnswerBox answerBox;
    private readonly MessageBox messageBox1;
    private readonly MessageBox messageBox2;
    private readonly Card cards1;
    private readonly Card cards2;
    private bool clientIsGone;
    private readonly Book openedBook;

    public Client(ContentManager content, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, 
        Texture2D door, string clientTextureName, SpriteFont font, string problem, string[] options, 
        int correctAnswerIndex, Texture2D cardsDeck, string cardTexture1Name, string cardTexture2Name,
        string[] messages, Book openedBook)
    {
        clientIsGone = false;
        client = content.Load<Texture2D>(clientTextureName);
        doorRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.896), 
            (int)(graphics.PreferredBackBufferHeight * 0.205), (int)(door.Width * 0.896), door.Height);
        dialogBox = new DialogBox(font, problem, graphicsDevice);
        cards1 = new Card(content, cardsDeck, cardTexture1Name);
        cards2 = new Card(content, cardsDeck, cardTexture2Name);
        answerBox = new AnswerBox(font, options, correctAnswerIndex, graphicsDevice);
        messageBox1 = new MessageBox(messages[0], graphicsDevice, content);
        messageBox2 = new MessageBox(messages[1], graphicsDevice, content);
        this.openedBook = openedBook;
    }

    public void Update(GameTime gameTime)
    {
        if (!isDoorLocked && Mouse.GetState().LeftButton == ButtonState.Pressed 
                          && doorRect.Contains(Mouse.GetState().Position))
        {
            clientIsVisible = true;
            isDoorLocked = true;
        }
        if (clientIsVisible)
        {
            dialogBox.Update();
        }
        if (dialogBox.IsVisible())
        {
            cards1.DeactivateClick();
            cards2.DeactivateClick();
            openedBook.DeactivateClick();
        }
        if (!dialogBox.IsVisible())
        {
            ActivateClick();
        }
        if (!dialogBox.IsVisible() && !answerBox.IsAnswerSelected())
        {
            cards1.Update(gameTime);
            cards2.Update(gameTime);
        }
        if (cards1.IsFlipped() && cards2.IsFlipped() && !cards1.BookIsOpened() && !cards2.BookIsOpened())
        {
            answerBox.Update();
        }

        if (!answerBox.IsVisible())
        {
            clientIsVisible = false;
            clientIsGone = true;
        }
        if (clientIsGone)
        {
            if (answerBox.IsRightAnswer())
            {
                messageBox1.Update(gameTime);
                isDoorLocked = false;
            }
            else if (!answerBox.IsRightAnswer())
            {
                messageBox2.Update(gameTime);
                isDoorLocked = false;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, 
        GraphicsDevice graphicsDevice, float scale)
    {
        var card1Position = new Vector2((int)(graphics.PreferredBackBufferWidth * 0.339), 
               (int) (graphics.PreferredBackBufferHeight * 0.25));
        var card2Position = new Vector2((int)(graphics.PreferredBackBufferWidth * 0.519), 
                 (int)(graphics.PreferredBackBufferHeight * 0.25));
        if (clientIsVisible)
        {
            spriteBatch.Draw(client, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            dialogBox.Draw(spriteBatch, graphicsDevice);
        }
        if (!dialogBox.IsVisible() && !answerBox.IsAnswerSelected())
        {
            cards1.Draw(spriteBatch, graphics, scale, card1Position);
            cards2.Draw(spriteBatch, graphics, scale, card2Position);
        }
        if (cards1.IsFlipped() && cards2.IsFlipped() && !cards1.BookIsOpened() && !cards2.BookIsOpened())
        {
            answerBox.Draw(spriteBatch, graphicsDevice);
        }
        if (clientIsGone)
        {
            if (answerBox.IsRightAnswer())
            {
                messageBox1.Draw(spriteBatch, graphicsDevice);
            }
            if (!answerBox.IsRightAnswer())
            {
                messageBox2.Draw(spriteBatch, graphicsDevice);
            }
        }
    }

    public int GetRating(int rating)
    {
        if (answerBox.IsAnswerSelected())
        {
            if (answerBox.IsRightAnswer())
                rating -= 1;
            if (!answerBox.IsRightAnswer())
                rating += 1;
        }

        return rating;
    }

    public bool IsMessageBoxNotVisible()
    {
        return answerBox.IsAnswerSelected() && (!messageBox1.IsVisible() || !messageBox2.IsVisible());
    }

    public bool ClientIsGone()
    {
        return clientIsGone;
    }
    
    private void ActivateClick()
    {
        var timer = new Timer(500);
        
        timer.Elapsed += (s, e) =>
        {
            cards1.ActivateClick();
            cards2.ActivateClick();
            openedBook.ActivateClick();
            timer.Dispose();
        };
        
        timer.Start();
    }
}