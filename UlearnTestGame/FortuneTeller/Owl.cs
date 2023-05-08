using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FortuneTeller;

public class Owl
{
    private Texture2D owlRight;
    private Texture2D owlLeft;
    private Texture2D currentTexture;
    private float timeElapsed;
    private float timeToChange;

    public Owl(ContentManager content)
    {
        owlRight = content.Load<Texture2D>("owlright");;
        owlLeft = content.Load<Texture2D>("owlleft");;
        currentTexture = owlLeft;
        timeElapsed = 0f;
        timeToChange = 2f;
    }

    public void Update(GameTime gameTime)
    {
        timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timeElapsed > timeToChange)
        {
            timeElapsed = 0f;

            currentTexture = currentTexture == owlRight ? owlLeft : owlRight;
        }
    }

    public void Draw(SpriteBatch spriteBatch, float scale)
    {
        spriteBatch.Draw(currentTexture, Vector2.Zero, null, Color.White,
            0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}