using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EggDrop
{
    static class GameManager
    {
        static public List<Player> Bucket = new List<Player>(); // List of all possible players
        static public Player player;    // What the current player is
        static public Egg currentEgg;   // What the currently spawned egg is
        static public List<Egg> Eggs = new List<Egg>(); // List of all possible eggs
        static private Random randSpawn = new Random(); // Randomly choose where the egg will spawn
        static private Random randSpecial = new Random();   // Randomly spawn either a normal, gold or death egg
        static private Random randPowerUp = new Random();   // When allowed choose one of three power ups to spawn
        static private Random randPlayer = new Random();    // Randomly choose one of six players
        static public int eggsCollected = 0;   // The number of eggs collected, every 10 will spawn a power up
        static public Texture2D texture;    // The texture that game1 defined in LoadContent
        static public SpriteFont textFace;  // The textFace that game1 defined in LoadContent
        static public float eggSpawnDelay = 1f; // The default egg spawn rate of one egg a second
        static private float eggTimer;  // The timer used to test the against the delay
        static public int eggCounter;
        static public int score = 0;    // The score
        static public int lives = 3;    // The number of player lives
        static public int scoreMultiplier = 1;  // The score multiplier
        static public int eggSpeed = 12;    // How fact down the eggs move, 12 pixels a frame by default
        static public int randomSpawn;  // The number used for the random location of the eggs spawned
        static public int previousLocation = 0; // Store where on the X coordinate the last egg spawned

        static public void SetPlayer()  // Selects a random bucket to play as
        {
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(0, 40, 40, 40), Vector2.Zero, new Point(0, 1), null));     // Metal bucket (0)
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(40, 40, 40, 40), Vector2.Zero, new Point(1, 1), null));    // Wooden bucket (1)
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(80, 40, 40, 40), Vector2.Zero, new Point(2, 1), null));    // Rusty bucket AKA my favorite (2)
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(120, 40, 40, 40), Vector2.Zero, new Point(3, 1), null));   // Mr. Bucket (3)
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(160, 40, 40, 40), Vector2.Zero, new Point(4, 1), null));   // Red Plastic bucket (4)
            Bucket.Add(new Player(Vector2.Zero, texture, new Rectangle(200, 40, 40, 40), Vector2.Zero, new Point(5, 1), null));   // Swag Bucket (5)
            int randomPlayer = randPlayer.Next(0, 6);   // The wheel of random
            player = Bucket[randomPlayer];  // Set the random number to the players index
            player.position = new Vector2(400, 540);    // Move player to default position
        }

        static public void SetLocation()
        {
            if (previousLocation == 0)
            {
                randomSpawn = randSpawn.Next(50, 700);  // Random spawn location
                previousLocation = randomSpawn; // Store the random position
            }
            else
            {
                randomSpawn = previousLocation + randSpawn.Next(-150, 150); // Random spawn will add on to the previous spawn location
                previousLocation = randomSpawn; // Store the spawn location
            }
        }

        static public void CorrectPosition()
        {
            if (randomSpawn > 700)  // If we are off screen to the right
            {
                randomSpawn -= 200; // Move to the left
                previousLocation = randomSpawn; // Save the new number
            }
            else if (randomSpawn < 50)  // If we are off screen to the left
            {
                randomSpawn += 200; // Move to the right
                previousLocation = randomSpawn; // Save the new number
            }
        }

        static public void SpawnEggs()
        {
            if (eggsCollected < 10) // If eggsCollected is less than ten, spawn a normal type of egg
            {
                int randomSpecial = randSpecial.Next(0, 20);    // Roll to see which normal egg you get
                if (randomSpecial == 19)    // 19 gives you the golden egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(40, 0, 40, 40), Vector2.Zero, new Point(1, 0), "golden"));    // Golden egg
                    currentEgg = Eggs[eggCounter];   // Sets egg to index 1
                    eggCounter++; 
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Moves egg to the random spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to the eggSpeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                    currentEgg.Gotten = false;  // Have you gotten the egg, this is so on each frame you do not get the egg multiple times
                }
                else if (randomSpecial == 0 || randomSpecial == 1 || randomSpecial == 2)    // If you get a 0, 1, or 2 you get a death egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(120, 0, 40, 40), Vector2.Zero, new Point(3, 0), "death")); // Death egg
                    currentEgg = Eggs[eggCounter];   // Sets egg to index 3
                    eggCounter++;
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Moves egg to random spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to the eggSpeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                }
                else  // If the random is any other number you get a white egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(0, 0, 40, 40), Vector2.Zero, new Point(0, 0), "normal")); // Normal egg
                    currentEgg = Eggs[eggCounter];   // Adds on to the egg index
                    eggCounter++;
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Moves egg to random spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to eggSpeeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                    currentEgg.Gotten = false;  // Have you gotten the egg, this is so on each frame you do not get the egg multiple times
                    currentEgg.Dropped = false; // Used to check if you should lose a life for dropping the white egg
                }
            }

            else if (eggsCollected >= 10)   // If eggsCollected is greater than or equal to 10 then spawn power up eggs
            {
                eggsCollected = 0;  // Reset the eggsCollected to zero
                if (eggSpawnDelay > 0.3f)   // If the game is not hyper speed up it so it gets faster
                {
                    eggSpawnDelay -= 0.1f;  // Minus the spawn delay slightly for a faster egg spawn rate 
                    eggSpeed += 2;  // The eggs move faster now
                }
                int randomPowerUp = randPowerUp.Next(0, 3); // Rolls to see which power up you get
                if (randomPowerUp == 0) // If you get a 0 you get a 1-Up egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(80, 0, 40, 40), Vector2.Zero, new Point(2, 0), "1up"));  // 1-Up egg
                    currentEgg = Eggs[eggCounter];   // Adds to the egg index
                    eggCounter++;
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Move egg to spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to eggSpeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                    currentEgg.Gotten = false;  // Have you gotten the egg, this is so on each frame you do not get the egg multiple times
                }
                else if (randomPowerUp == 1)    // If you get 1 you get a score multiplier egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(160, 0, 40, 40), Vector2.Zero, new Point(4, 0), "multi")); // Double score multiplier egg
                    currentEgg = Eggs[eggCounter];   // Adds on to the egg index
                    eggCounter++;
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Move egg to spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to eggSpeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                    currentEgg.Gotten = false;  // Have you gotten the egg, this is so on each frame you do not get the egg multiple times
                }
                else if (randomPowerUp == 2)    // If you get a 2 you get a speed down egg
                {
                    SetLocation();  // Set the spawn location
                    CorrectPosition();  // Move it on screen if it is off screen
                    Eggs.Add(new Egg(new Vector2(0, -100), texture, new Rectangle(200, 0, 40, 40), Vector2.Zero, new Point(5, 0), "slow")); // Slow egg speed egg
                    currentEgg = Eggs[eggCounter];   // Adds on to the egg index
                    eggCounter++;
                    currentEgg.position = new Vector2(randomSpawn, -80);    // Move egg to spawn location
                    currentEgg.velocity.Y = eggSpeed;   // Sets the Y velocity to eggSpeed
                    currentEgg.Collected = false;   // Collected is false so it is drawn
                    currentEgg.Gotten = false;  // Have you gotten the egg, this is so on each frame you do not get the egg multiple times
                }
            }
        }

        public static void Update(GameTime gameTime)
        {
            eggTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;   // Update the timer
            player.Update(gameTime, new Rectangle(0, 0, 800, 600)); // Updates the player
            if(eggTimer >= eggSpawnDelay)   // Will spawn eggs when the eggTimer is greater than the eggDelay
            {
                SpawnEggs();    // Spawn the egg
                eggTimer = 0;   // Reset the timer
            }

            if (eggSpeed < 10)  // Maintains a minimum speed of 12
            {
                eggSpeed = 10;
            }

            foreach (Egg egg in Eggs)
            {
                egg.Update(gameTime, new Rectangle(0, 0, 800, 600));    // Update the eggs

                if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "normal" && egg.Gotten == false) // If you hit a white egg
                {
                    score += 10 * scoreMultiplier;  // Gain 10 * mutipliers points
                    eggsCollected += 1; // Eggs collected goes up by 1
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }

                if (egg.position.Y >= 610 && egg.eggType == "normal" && egg.Dropped == false && egg.Gotten == false)  // If you miss a white egg
                {
                    lives -= 1; // Lose a life
                    egg.Dropped = true; // Dropped is true
                }

                else if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "golden" && egg.Gotten == false)    // If you hit a gold egg
                {
                    score += 100 * scoreMultiplier; // Gain 100 * mutipliers points
                    eggsCollected += 1; // Eggs collected goes up by 1
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }

                else if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "1up" && egg.Gotten == false)    // If you hit a 1-Up egg
                {   
                    lives += 1;     // Gain one life
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }

                else if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "death" && egg.Gotten == false)    // If you hit a death egg
                {
                    lives -= 1;     // Lose one life
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }

                else if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "multi" && egg.Gotten == false)    // If you hit a score multiplier egg
                {
                    scoreMultiplier *= 2;   // Double the score multiplier
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }

                else if (egg.collisionRect.Intersects(player.collisionRect) && egg.eggType == "slow" && egg.Gotten == false)    // If you hit a slow down egg 
                {
                    eggSpawnDelay += .2f;   // Spawn delay is increased
                    eggSpeed -= 4;  // Egg speed is decreased
                    egg.Collected = true;   // Collected is true
                    egg.Gotten = true;  // Gotten is true
                }
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(gameTime, spriteBatch); // Draw the player
            spriteBatch.DrawString(textFace, "Score: " + GameManager.score.ToString(), new Vector2(0, 0), Color.Yellow);    // Draw the score
            spriteBatch.DrawString(textFace, "Multiplier: " + GameManager.scoreMultiplier.ToString(), new Vector2(350, 0), Color.Yellow);   // Draw the score multiplier
            spriteBatch.DrawString(textFace, "Lives: " + GameManager.lives.ToString(), new Vector2(650, 0), Color.Yellow);  // Draw the number of lives
            foreach (Egg eggs in Eggs)
            {
                if (eggs.Collected == false)    // Draw each egg, as long as they have not been collected
                {
                    eggs.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}