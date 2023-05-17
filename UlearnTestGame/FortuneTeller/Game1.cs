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
    private bool isExitButtonVisible;

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
        isExitButtonVisible = false;
        exitButtonRect = new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.001), 
            (int)(graphics.PreferredBackBufferHeight * 0.89), (int)(exitButton.Width * 0.61), 
            (int)(exitButton.Height * 0.72));
        clients = new[]
        { 
            new Client(Content, graphics, GraphicsDevice, door, "client1", dialogFont,
                "Я конечно не очень доверяю вашим картам...Но мне очень нравится одна девушка..." +
                "Чувствует ли она ко мне то же, что и я к ней?", 
                new[]
                {
                    "Девушка относится к вам как к другу, и, по ее мыслям, у вас не может быть более глубоких отношений." +
                    "Не стоит признаваться ей в любви, так вы только отстранитесь",
                    "Карты говорят, что девушка чувствует к вам то же самое. Она влюблена, и скоро может " +
                    "проявить свои чувства. Будьте терпеливы"
                },
                1,
                cardsDeck, "lovers", "hermit", 
                new[] { "Я решил довериться вашему раскладу и признаться в чувствах. Теперь мы с ней встречаемся! " +
                        "     Вы суперская     гадалка!", 
                    "Так и знал, что не стоит верить всем этим таро... Я признался ей, и оказалось, что наши чувства взаимны! " +
                    "     А вы что      говорили? А?" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client2", dialogFont,
            "Я всю жизнь мечтаю стать магом, но научиться так сложно...Может мне стоит стать как вы? :)", 
            new[]
            {
                "Карты указывают на то, что ваш путь к освоению таро окажется полон неприятностей и сложностей, и лучше" +
                " не начинать это дело вообще.",
                "Возможностей для изучения таро будет много, но не все из них будут успешными. Важно быть настойчивым и" +
                " не сдаваться на первых удачных попытках. "
            },
            1, 
            cardsDeck, "magician", "fortune", 
            new[] { "Я начал изучать таро после вашего расклада. Вы были правы, у меня неплохо получается! " +
                    "   Смотрите, как бы    я не увел у вас клиентов :)", 
                "Наверное, вы специально так сказали! Боитесь, что уведу у вас клиентов? У меня уже очень хорошо " +
                "получается гадать, ха-ха!" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client3", dialogFont,
                "У меня скоро такоой трудный экзамен по векторке! Что мне нужно сделать, чтобы сдать этот " +
                "экзамен??",
                new[]
                {
                    "Пересмотрите свой подход к учебе. Удача сейчас на вашей стороне, и если вы будете усердно" +
                    " стараться, то экзамен пройдет успешно",
                    "Вам стоит бросить университет, найти богатого мужчину и забыть наконец об учебе!"
                },
                0, 
                cardsDeck, "tower", "fortune", 
                new[] { "Ваша трактовка помогла мне поверить в себя и найти энтузиазм для учебы. " +
                        "Я сдала экзамен!!! Огромное спасибо!", 
                    "Ваш расклад был крайне странным...Но вместо богатого мужчины я нашла умников, которые помогли" +
                    " мне подготовиться к экзамену..."}, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client4", dialogFont,
                "Я не плохой человек, но в моей жизни нет близких друзей. Я чувствую себя одиноко, но " +
                "стесняюсь обращаться к людям и легко заводить новые знакомства. Что же мне делать?", 
                new[]
                {
                    "Карты указывают, что у тебя есть способности, которые могут помочь тебе найти друзей. Тебе стоит" +
                    " использовать опыт и умение прислушиваться, чтобы установить связь с людьми",
                    "Твоя социальная неприязнь - следствие твоих недостатков и привычек. Карты указывают, что" +
                    " нужно меняться, чтобы обрести друзей, которые будут тебя понимать и полюбят тебя"
                },
                0, 
                cardsDeck, "star", "knightofcups", 
                new[] { "Я начинаю понимать, что мой опыт может быть ключом к тому, чтобы находить" +
                        " общие темы с людьми. Я постараюсь   открыться для    новых знакомств!", 
                    "Я ожидала от вас конкретных советов, а не банальные слова о том, что мне нужно приобретать новых" +
                    " друзей. Это было полное   разочарование!" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client5", dialogFont,
                "Я сейчас в сложной ситуации, потому что не могу определиться между двумя парнями... " +
                "Один из них мой давний друг, а другой - новый знакомый, с которым меня связывают сильные чувства", 
                new[]
                {
                    "Ты должна выбрать стабильность и безопасность, а не основывать жизнь на эмоциях. Не рискуй своим " +
                    "будущим," + " найди уверенность в том, что ты сделала правильный выбор",
                    "Важно не позволять прошлому мешать тебе в принятии решения и следовать своим эмоциям и интуиции. " +
                    "Помни, что тебе нужно слушать своё сердце, чтобы принять правильное решение"
                },
                1, 
                cardsDeck, "5cups", "knightofswords", 
                new[] { "Твоя трактовка была очень мудрой! Я следовала своим чувствам и выбрала того, " +
                        "кого я действительно     люблю. Спасибо       за твои советы!", 
                    "Я поступила так, как вы сказали и бросила своего парня, а оказалось у моего друга нет чувств ко мне!" +
                    " Вы навсегда испортили все      мои отношения!" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client6", dialogFont,
                "Я такая неуклюжая и беспорядочная! У меня сильный стресс из-за своей неряшливости, но я никак " +
                "не могу организоваться и держать свой дом в чистоте, помогите мне...", 
                new[]
                {
                    "Вы должны научиться быть терпеливой. Начните прививать себе привычку " +
                    "поддерживать свой дом в чистоте и порядке. Со временем станет проще, и вы будете получать удовольствие",
                    "Вы должны забыть об этом и просто расслабиться. Перестаньте беспокоиться о том, что дом не в " +
                    "идеальном состоянии, находите удовольствие в жизни, и все проблемы сами решатся"
                },
                0, 
                cardsDeck, "strength", "hermit", 
                new[] { "Я осталась очень довольна трактовкой! Я стараюсь организовывать свое время, " +
                        "и, кажется, жизнь начинает улучшаться.    Спасибочки!", 
                    "Ожидала, что ваши советы мне помогут, но по итогу в моей жизни ничего не изменилось. " +
                    "Зря потраченные     деньги!" }, openedBook),
            new Client(Content, graphics, GraphicsDevice, door, "client7", dialogFont,
                "Я постоянно трачу много денег и остаюсь без них до конца месяца, мне не хватает стипендии " +
                "на все расходы. Как мне выжить? :(", 
                new[]
                {
                    "Тебе необходимо более осознано планировать свой бюджет и экономить деньги на важные расходы. " +
                    "Имей терпение, устроение финансовой ситуации требует времени",
                    "Тебе нужно найти работу, чтобы выйти из финансовых проблем. Смирись, только это и будет решением" +
                    " всех твоих проблем"
                },
                0, 
                cardsDeck, "10pentacles", "fortune", 
                new[] { "Ваш расклад открыл мне глаза! Теперь я планирую бюджет и не умру с голода в общаге!!", 
                    "Ваша трактовка абсолютно бесполезна! Я слетел со стипы, я не могу совмещать учебу и работу!" }, openedBook),
        };
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        owl.Update(gameTime);
        menu.Update(graphics);

        if (isExitButtonVisible && Mouse.GetState().LeftButton == ButtonState.Pressed && 
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

    protected override void Draw(GameTime gameTime)
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
                            isExitButtonVisible = true;
                            ending.Draw(spriteBatch, scale, false);
                            spriteBatch.Draw(exitButton, new Vector2((int)(graphics.PreferredBackBufferWidth * 0.01), 
                                    (int)(graphics.PreferredBackBufferHeight * 0.907)), null, Color.White,
                                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            isExitButtonVisible = true;
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