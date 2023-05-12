using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Client
{
    private Rectangle doorRect;
    private bool isDoorLocked = false;
    private string clientTextureName;
    private Texture2D client;
    private string problem;
    private string[] options;
    private string[] messages;
    private int correctAnswerIndex;
    private bool clientIsVisible = false;
    private DialogBox dialogBox;
    private DialogBox dialogBoxThanks;
    private AnswerBox answerBox;
    private MessageBox messageBox1;
    private MessageBox messageBox2;
    private Card cards1;
    private Card cards2;
    private SpriteFont font;
    private int rat;

    public Client(ContentManager content, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, 
        Texture2D door, string clientTextureName, SpriteFont font, string problem, string[] options, 
        int correctAnswerIndex, int rating, Texture2D cardsDeck, string cardTexture1Name, string cardTexture2Name,
        string[] messages)
    {
        client = content.Load<Texture2D>(clientTextureName);
        doorRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.896), 
            (int)(graphics.PreferredBackBufferHeight * 0.205), (int)(door.Width * 0.896), door.Height);
        dialogBox = new DialogBox(font, problem, graphicsDevice);
        cards1 = new Card(content, cardsDeck, cardTexture1Name);
        cards2 = new Card(content, cardsDeck, cardTexture2Name);
        answerBox = new AnswerBox(font, options, correctAnswerIndex, graphicsDevice, rating);
        dialogBoxThanks = new DialogBox(font, "Thanks", graphicsDevice);
        messageBox1 = new MessageBox(font, messages[0], graphicsDevice, content);
        messageBox2 = new MessageBox(font, messages[1], graphicsDevice, content);
        rat = rating;
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
            if (!dialogBox.IsVisible() && !answerBox.IsAnswerSelected())
            {
                cards1.Update(gameTime);
                cards2.Update(gameTime);
            }
            if (cards1.IsFlipped() && cards2.IsFlipped() && !cards1.BookIsOpened() && !cards2.BookIsOpened())
            {
                answerBox.Update();
            }
            if (answerBox.IsAnswerSelected() && clientIsVisible)
            {
                dialogBoxThanks.Update();
            }
            if (!dialogBoxThanks.IsVisible())
            {
                clientIsVisible = false;
            }
        }
        if (answerBox.IsRightAnswer() && !clientIsVisible)
        {
            messageBox1.Update(gameTime);
            isDoorLocked = false;
        }
        else if (!answerBox.IsRightAnswer() && !clientIsVisible)
        {
            messageBox2.Update(gameTime);
            isDoorLocked = false;
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
            
            if (!dialogBox.IsVisible() && !answerBox.IsAnswerSelected())
            {
                cards1.Draw(spriteBatch, graphics, scale, card1Position);
                cards2.Draw(spriteBatch, graphics, scale, card2Position);
            }
            if (cards1.IsFlipped() && cards2.IsFlipped() && !cards1.BookIsOpened() && !cards2.BookIsOpened())
            {
                answerBox.Draw(spriteBatch, graphicsDevice);
            }
            if (answerBox.IsAnswerSelected() && clientIsVisible)
            {
                dialogBoxThanks.Draw(spriteBatch, graphicsDevice);
            }
            if (!dialogBoxThanks.IsVisible())
                clientIsVisible = false;
        }
        if (!dialogBoxThanks.IsVisible() && !clientIsVisible)
        {
            if (answerBox.IsRightAnswer())
            {
                messageBox1.Draw(spriteBatch, graphicsDevice);
            }
            else if (!answerBox.IsRightAnswer())
            {
                messageBox2.Draw(spriteBatch, graphicsDevice);
            }
        }
    }

    public int UpdateRating()
    {
        if (!messageBox1.IsVisible()) 
            return answerBox.GetRating();
        if (!messageBox2.IsVisible())
            return answerBox.GetRating();
        return rat;
    }
    
    public bool IsDoorLocked()
    {
        return isDoorLocked;
    }

    public bool IsVisible()
    {
        return clientIsVisible;
    }
}