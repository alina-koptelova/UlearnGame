using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FortuneTeller;

public class MessageBox
{
    private string text;
    private SpriteFont font;
    private Rectangle rect;
    private Texture2D letter;
    private bool isVisible = false;
    private float timer;
    private const float displayTime = 3f;

    public MessageBox(SpriteFont font, string text, GraphicsDevice graphicsDevice, ContentManager content)
    {
        this.font = font;
        this.text = text;
        isVisible = true;
        rect = new Rectangle((int)(graphicsDevice.Viewport.Width * 0.65), graphicsDevice.Viewport.Height / 28,
            (int)(graphicsDevice.Viewport.Width * 0.28), graphicsDevice.Viewport.Height / 5);
        letter = content.Load<Texture2D>("letter");
    }

    public void Update(GameTime gameTime)
    {
        if (isVisible)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= displayTime)
            {
                isVisible = false;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        if (isVisible)
        {
            var screenWidth = graphicsDevice.Viewport.Width;
            var texture = GenerateTexture(graphicsDevice, rect);
            var spritePosition = new Vector2((int)(graphicsDevice.Viewport.Width * 0.86), graphicsDevice.Viewport.Height / 7);
            spriteBatch.Draw(texture, rect, Color.Black);
            spriteBatch.Draw(letter, spritePosition, Color.White);
            var lines = WrapText(text, (int)(screenWidth * 0.27));
            float offsetY = rect.Y + 10;
            float offsetX = rect.X + 10 ;
            
            foreach (var line in lines)
            {
                var textSize = font.MeasureString(line);
                spriteBatch.DrawString(font, line, new Vector2(offsetX, offsetY), Color.White);
                offsetY += textSize.Y;
            }
        }
    }

    private static Texture2D GenerateTexture(GraphicsDevice graphicsDevice, Rectangle rect)
    {
        var texture = new Texture2D(graphicsDevice, rect.Width, rect.Height);
        var data = new Color[rect.Width * rect.Height];

        for (var i = 0; i < data.Length; i++)
            data[i] = Color.Black;
        
        texture.SetData(data);

        return texture;
    }

    private List<string> WrapText(string text, float maxLineWidth)
    {
        var lines = new List<string>();
        var words = text.Split(' ');
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            if (word == "n")
            {
                lines.Add(currentLine.ToString().TrimEnd());
                currentLine.Clear();
            }
            else if (font.MeasureString(currentLine + word).X > maxLineWidth)
            {
                lines.Add(currentLine.ToString().TrimEnd());
                currentLine.Clear().Append(word).Append(' ');
            }
            else
            {
                currentLine.Append(word).Append(' ');
            }
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString().TrimEnd());
        }

        return lines;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}