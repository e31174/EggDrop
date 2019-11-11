using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EggDrop
{
    class Egg : MasterSprite
    {
        #region Constructor
        public Egg(Vector2 position, Texture2D texture, Rectangle frameLocation, Vector2 velocity, Point currentFrame, string eggType): base(position, texture, frameLocation, velocity, currentFrame, eggType)
        {

        }
        #endregion

        public override Vector2 direction       // Returns the speed so direction can use it
        {
            get
            {
                return velocity;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;  // Moves the enemies position via direction

            if (position.Y > clientBounds.Height + 100 || position.X < 0)
            {
                Dropped = true; // Sets the bool dropped when the egg goes off screen
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
