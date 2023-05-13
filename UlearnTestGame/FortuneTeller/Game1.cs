using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace FortuneTeller;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D background;
    private Texture2D door;
    private Texture2D cardsDeck;
    private Texture2D book;
    private Texture2D cup;
    private float scale;
    private SpriteFont font;
    private int rating = 15;
    Rectangle clientRect;
    private Song song;
    private Menu menu;
    private Texture2D cursor;
    private Owl owl;
    private DialogBox dialogBox;
    private SpriteFont dialogFont;
    private Book openedBook;
    private Client[] clients;
    private bool isVisibleOldRating;
    private bool isVisibleNewRating;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Window.Title = "Симулятор гадалки таро";
        Content.RootDirectory = "Content";
        _graphics.IsFullScreen = true;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        
        _graphics.ApplyChanges();

        // Вычисление масштабного коэффициента
        scale = _graphics.PreferredBackBufferWidth / 1920f;
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        isVisibleOldRating = true;
        isVisibleNewRating = false;
        cursor = Content.Load<Texture2D>("cursor");
        Mouse.SetCursor(MouseCursor.FromTexture2D(cursor, 0, 0));
        background = Content.Load<Texture2D>("room");
        owl = new Owl(Content);
        door = Content.Load<Texture2D>("door");
        cardsDeck = Content.Load<Texture2D>("cardsDeck");
        book = Content.Load<Texture2D>("book");
        cup = Content.Load<Texture2D>("cup");
        font = Content.Load<SpriteFont>("File");
        song = Content.Load<Song>("melody");
        openedBook = new Book(Content, book);
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.01f;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        dialogFont = Content.Load<SpriteFont>("dialogbox");
        dialogBox = new DialogBox(dialogFont,
            "Ты - гадалка на таро. Принимай клиентов, нажимая на дверь. " +
            "Затем гадай им по колоде карт на столе, используя книгу с подсказками", GraphicsDevice);
        menu = new Menu(Content);
        clients = new[]
        {
            new Client(Content, _graphics, GraphicsDevice, door, "client1", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client2", dialogFont,
            "I have problem", new[]
            {
                "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
            },
            1, rating,
            cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client3", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client4", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client5", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client6", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
            new Client(Content, _graphics, GraphicsDevice, door, "client7", dialogFont,
                "I have problem", new[]
                {
                    "Answer 1 aaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaa",
                    "Answer 2 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaa"
                },
                1, rating,
                cardsDeck, "cardexample", "cardexample", new[] { "you're so cool", "fuck you" }),
        };
    }
    
    void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
    {
        MediaPlayer.Volume -= 0f;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        owl.Update(gameTime);
        menu.Update(_graphics);
        dialogBox.Update();
        openedBook.Update();
        // client1.Update(gameTime);
        // if (client1.ClientIsGone())
        //     client2.Update(gameTime);
        clients[0].Update(gameTime);
        for (var i = 1; i < clients.Length; i++)
        {
            if (clients[i - 1].ClientIsGone())
                clients[i].Update(gameTime);
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        menu.Draw(_spriteBatch, _graphics, scale);
        if (!menu.IsActive())
        {
            _spriteBatch.Draw(background, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            owl.Draw(_spriteBatch, scale);
            _spriteBatch.Draw(door, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.896), (int)(_graphics.PreferredBackBufferHeight * 0.205)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(book, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.625), 
                    (int)(_graphics.PreferredBackBufferHeight * 0.741)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(cardsDeck, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.448), 
                    (int)(_graphics.PreferredBackBufferHeight * 0.798)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            
            
            _spriteBatch.Draw(cup, new Vector2(15, 10), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            dialogBox.Draw(_spriteBatch, GraphicsDevice);
            if (isVisibleOldRating && isVisibleNewRating == false)
                _spriteBatch.DrawString(font, rating.ToString(), new Vector2(90, 30), Color.Black);
            
            clients[0].Draw(_spriteBatch, _graphics, GraphicsDevice, scale);
            for (var i = 1; i < clients.Length; i++)
            {
                if (clients[i - 1].ClientIsGone())
                {
                    var newRat = clients[i - 1].UpdateRating();
                    isVisibleOldRating = false;
                    isVisibleNewRating = true;
                    if (isVisibleNewRating && isVisibleOldRating == false)
                        _spriteBatch.DrawString(font, newRat.ToString(), new Vector2(90, 30), Color.Black);
                    clients[i].Draw(_spriteBatch, _graphics, GraphicsDevice, scale);
                }
            }
            
            var bookClickPosition = new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.625),
                (int)(_graphics.PreferredBackBufferHeight * 0.741));
            openedBook.Draw(_spriteBatch, _graphics, scale, bookClickPosition);
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}