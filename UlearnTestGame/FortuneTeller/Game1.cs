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
    private Texture2D doorTexture;
    private Texture2D clientTexture;
    private Texture2D cardsDeck;
    private Texture2D book;
    private Texture2D cup;
    private float scale;
    private SpriteFont font;
    private int rating = 15;
    Rectangle clientRect;
    private Song song;
    private Menu menu;
    
   private DialogBox dialogBox;
   private SpriteFont dialogFont;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1200;
        _graphics.PreferredBackBufferHeight = 675;
        _graphics.ApplyChanges();

        // Вычисление масштабного коэффициента
        scale = _graphics.PreferredBackBufferWidth / 1920f;
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        background = Content.Load<Texture2D>("room");
        doorTexture = Content.Load<Texture2D>("door2");
        cardsDeck = Content.Load<Texture2D>("cards");
        book = Content.Load<Texture2D>("book");
        cup = Content.Load<Texture2D>("cup");
        font = Content.Load<SpriteFont>("File");
        clientTexture = Content.Load<Texture2D>("owl");
        song = Content.Load<Song>("melody");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        dialogFont = Content.Load<SpriteFont>("dialogbox");
        dialogBox = new DialogBox(dialogFont,
            "Ты - гадалка на таро. Принимай клиентов, нажимая на дверь. " +
            "Затем гадай им по колоде карт на столе, используя книгу с подсказками", GraphicsDevice);
        menu = new Menu(Content);
    }
    
    void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
    {
        MediaPlayer.Volume -= 0.1f;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        menu.Update(_graphics);
        dialogBox.Update();
        
        var doorRect = new Rectangle((int)(_graphics.PreferredBackBufferWidth * 0.896), 
            (int)(_graphics.PreferredBackBufferHeight * 0.205), doorTexture.Width, doorTexture.Height);
        
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && doorRect.Contains(Mouse.GetState().Position))
        {
            clientRect = background.Bounds;
            rating -= 1;
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
            _spriteBatch.Draw(doorTexture, new Vector2((int)(_graphics.PreferredBackBufferWidth * 0.896), (int)(_graphics.PreferredBackBufferHeight * 0.205)), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(clientTexture, Vector2.Zero, clientRect, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(book, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(cardsDeck, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(cup, new Vector2(15, 10), null, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, rating.ToString(), new Vector2(90, 30), Color.Black);
        
            dialogBox.Draw(_spriteBatch, GraphicsDevice);
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}