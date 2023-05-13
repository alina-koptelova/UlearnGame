using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Text;

namespace FortuneTeller;

public class DialogBox
{
    private string text;
    private SpriteFont font;
    private bool isVisible;
    private Rectangle rect;

    public DialogBox(SpriteFont font, string text, GraphicsDevice graphicsDevice)
    {
        this.font = font;
        this.text = text;
        isVisible = true;
        rect = new Rectangle(0, graphicsDevice.Viewport.Height - graphicsDevice.Viewport.Height / 4,
            graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height / 4);
    }

    public void Update()
    {
        if (isVisible && Mouse.GetState().LeftButton == ButtonState.Pressed 
                      && rect.Contains(Mouse.GetState().Position))
            isVisible = false;
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        if (isVisible)
        {
            var screenWidth = graphicsDevice.Viewport.Width;
            var texture = GenerateTexture(graphicsDevice, rect);
            spriteBatch.Draw(texture, rect, Color.Black);

            var lines = WrapText(text, screenWidth - 40);
            float offsetY = rect.Y + 20;
            float offsetX = rect.X + 20 ;
            
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