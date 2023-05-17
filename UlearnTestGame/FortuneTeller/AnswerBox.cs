using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FortuneTeller;

public class AnswerBox
{
    private readonly string[] options;
    private readonly int correctAnswerIndex;
    private int selectedAnswerIndex = -1;
    private readonly SpriteFont font;
    private bool isVisible;
    private readonly Rectangle rect;
    private bool isAnswerSelected;

    public AnswerBox(SpriteFont font, string[] options, int correctAnswerIndex, 
        GraphicsDevice graphicsDevice)
    {
        this.font = font;
        this.options = options;
        isVisible = true;
        this.correctAnswerIndex = correctAnswerIndex;
        rect = new Rectangle(0, graphicsDevice.Viewport.Height - graphicsDevice.Viewport.Height / 4,
            graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height / 4);
    }

    public void Update()
    {
        if (isVisible)
        {
            float offsetY = rect.Y + 20;
            float offsetX = rect.X + 20;
            
            for (var i = 0; i < options.Length; i++)
            { 
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle((int)offsetX, (int)offsetY,
                        (int)font.MeasureString(options[i]).X, 
                        (int)font.MeasureString(options[i]).Y).Contains(Mouse.GetState().Position))
                {
                    selectedAnswerIndex = i;
                    isAnswerSelected = true;
                    isVisible = false;
                }
                
                offsetY += font.MeasureString(options[i]).Y + 10;
                
                if (i == 0)
                {
                    offsetY += 60;
                    offsetX += 40;
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        if (isVisible)
        {
            var texture = GenerateTexture(graphicsDevice, rect, Color.Black);
            var lineTexture = GenerateTexture(graphicsDevice, rect, Color.White);
            spriteBatch.Draw(texture, rect, Color.Black);
            spriteBatch.Draw(lineTexture, new Rectangle(rect.X, (int)(rect.Y + rect.Height / 1.97), rect.Width, 2), Color.White);
            float offsetY = rect.Y + 20;
            float offsetX = rect.X + 20;

            foreach (var option in options)
            {
                var color = Color.White;

                if (new Rectangle((int)offsetX, (int)offsetY, 
                        (int)font.MeasureString(option).X, 
                        (int)font.MeasureString(option).Y).Contains(Mouse.GetState().Position))
                {
                    color = Color.Green;
                }

                var wrappedText = WrapText(option, rect.Width - 40);
                
                foreach (var line in wrappedText)
                {
                    spriteBatch.DrawString(font, line, new Vector2(offsetX, offsetY), color);
                    offsetY += font.MeasureString(line).Y;
                }
                
                offsetY += 20;
            }
        }
    }

    private static Texture2D GenerateTexture(GraphicsDevice graphicsDevice, Rectangle rect, Color color)
    {
        var texture = new Texture2D(graphicsDevice, rect.Width, rect.Height);
        var data = new Color[rect.Width * rect.Height];

        for (var i = 0; i < data.Length; i++)
            data[i] = color;
        
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
    
    public bool IsRightAnswer()
    {
        return selectedAnswerIndex == correctAnswerIndex;
    }

    public bool IsAnswerSelected()
    {
        return isAnswerSelected;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}