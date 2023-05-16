using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace FortuneTeller;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private float scale;
    private const int Rating = 8;
    private SpriteFont font;
    private SpriteFont dialogFont;
    private Song song;
    private Texture2D cursor;
    private Texture2D background;
    private Texture2D door;
    private Texture2D cardsDeck;
    private Texture2D book;
    private Texture2D cup;
    private Menu menu;
    private Owl owl;
    private DialogBox dialogBox;
    private Book openedBook;
    private Client[] clients;
    private Ending ending;
    private Texture2D exitButton;
    private Rectangle exitButtonRect;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Window.Title = "Симулятор гадалки таро";
        Content.RootDirectory = "Content";
        graphics.IsFullScreen = true;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.ApplyChanges();
        scale = graphics.PreferredBackBufferWidth / 1920f;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        song = Content.Load<Song>("melody");
        cursor = Content.Load<Texture2D>("cursor");
        Mouse.SetCursor(MouseCursor.FromTexture2D(cursor, 0, 0));
        font = Content.Load<SpriteFont>("File");
        dialogFont = Content.Load<SpriteFont>("dialogbox");
        background = Content.Load<Texture2D>("room");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.05f;
        MediaPlayer.IsRepeating = true;
        owl = new Owl(Content);
        door = Content.Load<Texture2D>("door");
        cardsDeck = Content.Load<Texture2D>("cardsDeck");
        book = Content.Load<Texture2D>("book");
        cup = Content.Load<Texture2D>("cup");
        openedBook = new Book(Content, book);
        dialogBox = new DialogBox(dialogFont,
            "Ты - гадалка на таро. Принимай клиентов, нажимая на дверь. " +
            "Затем гадай им по колоде карт на столе, используя книгу с подсказками", GraphicsDevice);
        menu = new Menu(Content);
        ending = new Ending(Content);
        exitButton = Content.Load<Texture2D>("exitButton");
        exitButtonRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.001), 
            (int)(graphics.PreferredBackBufferHeight * 0.89), (int)(exitButton.Width * 0.61), 
            (int)(exitButton.Height * 0.72));
        clients = new[]
        { 
            new Client(Content, graphics, GraphicsDevice, door, "client1", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1,
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client2", dialogFont,
            "I have problem", new[]
            {
                "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
            },
            1, 
            cardsDeck, "cardexample", "cardexample", 
            new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client3", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, 
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client4", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, 
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client5", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, 
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client6", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, 
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client7", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, 
                cardsDeck, "cardexample", "cardexample", 
                new[] { "you're so cool", "fuck you" }, openedBook),
        };
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        owl.Update(gameTime);
        menu.Update(graphics);

        if (Mouse.GetState().LeftButton == ButtonState.Pressed && 
            exitButtonRect.Contains(Mouse.GetState().Position))
        {
            Exit();
        }
        
        openedBook.Update();
        dialogBox.Update();
        
        clients[0].Update(gameTime);
        
        for (var i = 1; i < clients.Length; i++)
        {
            if (clients[i - 1].ClientIsGone())
            {
                clients[i].Update(gameTime);
            }
        }
        
        ending.Update();
        base.Update(gameTime);
    }

    protected override async void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        menu.Draw(spriteBatch, graphics, scale);
        
        if (!menu.IsActive())
        {
            spriteBatch.Draw(background, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            owl.Draw(spriteBatch, scale);
            spriteBatch.Draw(door, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.896), 
                    (int)(graphics.PreferredBackBufferHeight * 0.205)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(book, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.625), 
                    (int)(graphics.PreferredBackBufferHeight * 0.741)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cardsDeck, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.448), 
                    (int)(graphics.PreferredBackBufferHeight * 0.798)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cup, new Vector2(15, 10), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            dialogBox.Draw(spriteBatch, GraphicsDevice);
            spriteBatch.DrawString(font, clients[0].GetRating(Rating).ToString(), 
                new Vector2(90, 30), Color.Black);
            clients[0].Draw(spriteBatch, graphics, GraphicsDevice, scale);
            var newRating = clients[0].GetRating(Rating);
            var lastClientIndex = clients.Length - 1;

            for (var i = 1; i < clients.Length; i++)
            {
               
                if (clients[i - 1].ClientIsGone() && clients[i - 1].IsMessageBoxNotVisible())
                {
                    spriteBatch.DrawString(font, newRating.ToString(), new Vector2(90, 30), 
                        new Color(128, 55, 128, 0));
                    clients[i].Draw(spriteBatch, graphics, GraphicsDevice, scale);
                    newRating = clients[i].GetRating(newRating);
                    spriteBatch.DrawString(font, newRating.ToString(), new Vector2(90, 30), Color.Black);
                }
                if (i == lastClientIndex)
                {
                    if (clients[i].IsMessageBoxNotVisible())
                    {
                        if (newRating <= 3)
                        {
                            ending.Draw(spriteBatch, scale, false);
                            spriteBatch.Draw(exitButton, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.01), 
                                    (int)(graphics.PreferredBackBufferHeight * 0.907)), null, Color.White,
                                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            ending.Draw(spriteBatch, scale, true);
                            spriteBatch.Draw(exitButton, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.01), 
                                    (int)(graphics.PreferredBackBufferHeight * 0.907)), null, Color.White,
                                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                    }
                }
            }

            var bookClickPosition = new Vector2((int)(graphics.PreferredBackBufferWidth * 0.625),
                (int)(graphics.PreferredBackBufferHeight * 0.741));
            openedBook.Draw(spriteBatch, graphics, scale, bookClickPosition);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
    
    
}