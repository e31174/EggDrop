using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EggDrop
{
    public class Player: MasterSprite
    {
        #region Constructor
        public Player(Vector2 position, Texture2D texture, Rectangle frameLocation, Vector2 velocity, Point currentFrame, string type): base(position, texture, frameLocation, velocity, currentFrame, type)
        {
   
        }
        #endregion

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;  // Default movement is 0
                
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(1).IsButtonDown(Buttons.DPadLeft))
                {
                    inputDirection.X -= 15;  // Move the player to the left
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(1).IsButtonDown(Buttons.DPadRight))
                {
                    inputDirection.X += 15;  // Move the player to the right
                }
                return inputDirection;  // Return where the sprite is moving
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;  // Moves the player

            if (position.X < 0)
            {
                position.X = 0; // If the player is going off screen to the left then stop it from doing so
            }

            if (position.X > clientBounds.Width - 60)
            {
                position.X = clientBounds.Width - 60;  // If the player is going off screen to the right then stop it from doing so
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
