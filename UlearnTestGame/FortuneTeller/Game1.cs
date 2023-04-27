using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D background;
    private Texture2D door;
    private Texture2D cardsDeck;
    private Texture2D book;
    private float scale;

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
        door = Content.Load<Texture2D>("door");
        cardsDeck = Content.Load<Texture2D>("cards");
        book = Content.Load<Texture2D>("book");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _spriteBatch.Draw(background, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        _spriteBatch.Draw(door, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        _spriteBatch.Draw(book, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        _spriteBatch.Draw(cardsDeck, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}