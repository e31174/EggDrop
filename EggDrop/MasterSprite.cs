using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EggDrop
{
    public class MasterSprite
    {
        public Texture2D Texture;   // All sprites need a texture
        public Vector2 position;    // All sprites need a postion
        public Vector2 velocity;    // All sprites need a velocity
        private Rectangle frame;    // All sprites need to know where their frame is
        Point currentFrame;         // All sprites need to know what frame they are
        public int collisionOffset;
        public bool Collected = false;  // Was the egg collected, if so do not draw it
        public bool Dropped = false;    // Used by white eggs for live calculation
        public bool Gotten = false;     // Did you get the egg
        public string eggType;

        #region Constructor
        public MasterSprite(Vector2 position, Texture2D texture, Rectangle frameLocation, Vector2 velocity, Point currentFrame, string type)
        {
            this.position = position;
            Texture = texture;
            this.velocity = velocity;
            frame = frameLocation;
            this.currentFrame = currentFrame;
            eggType = type;
        }
        #endregion

        #region Things all sprites need to know
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)    // All sprites need to be able to draw
        {
            spriteBatch.Draw(Texture, position, new Rectangle(currentFrame.X * 40, currentFrame.Y * 40, 40, 40), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)   // All sprites update in someway 
        {

        }

        public virtual Vector2 direction    // All sprites have a direction
        {
            get;
        }

        public Rectangle collisionRect  // All sprites can collide with things
        {
            get
            {
                return new Rectangle((int)position.X + collisionOffset, (int)position.Y + collisionOffset, 40 - (collisionOffset * 2), 40 - (collisionOffset * 2));
            }
        }
        #endregion
    }
}
