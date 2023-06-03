using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FortuneTeller;

public class Ending
{
    private readonly Texture2D happyEnd;
    private readonly Texture2D sadEnd;

    public Ending(ContentManager content)
    {
        happyEnd = content.Load<Texture2D>("happyEnd");
        sadEnd = content.Load<Texture2D>("sadEnd");
    }

    public void Update()
    {
        
    }

    public void Draw(SpriteBatch spriteBatch, float scale, bool isSadEnd)
    {
        spriteBatch.Draw(isSadEnd ? sadEnd : happyEnd, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}