using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EggDrop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;     // Here by default
        SpriteBatch spriteBatch;    // Just the spritebatch
        Texture2D spritesheet;      // The spritesheet that has all the players and eggs on it
        Texture2D titleScreen;      // The title screen
        Texture2D instructions;     // The how to play screen
        Texture2D gameOver;         // The you lost screen
        Texture2D background;       // The background when playing the game
        SpriteFont typeface;        // The font used is called mishmash, a font that is default in windows 10, but I included it in the games folder
        private float delay = .75f; // Set so you can not hold enter to to skip the instructions screen
        private float timer;        // The timer used to check teh delay
        private bool isPaused = false;

        public enum GameState { TitleScreen, Instructions, Playing, GameOver };     // The 4 gamestates, I elected to not have pausing becuase I had it and when you unpause it was way to hard to catch the egg so you basically just lost a life on every pause
        public GameState state = GameState.TitleScreen; // Start on the title screen

        public Game1()  // More default stuff
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 800;    // Set the width of the window to 800 pixels
            graphics.PreferredBackBufferHeight = 600;   // Set the height of the window to 600 pixels
            graphics.ApplyChanges();    // Apply my changes
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>(@"Textures/EggsAndBucketsSpritesheet");   // Loading in the spritesheet
            instructions = Content.Load<Texture2D>(@"Textures/Instructions");   // Loading in the instructions
            titleScreen = Content.Load<Texture2D>(@"Textures/EggDrop"); // Loading in the title screen
            gameOver = Content.Load<Texture2D>(@"Textures/GameOver");   // Loading in the game over screen
            typeface = Content.Load<SpriteFont>(@"Fonts/Text");     // My mishmash font
            background = Content.Load<Texture2D>(@"Textures/Background");   // Loading in the wall used for the background
            GameManager.texture = spritesheet;  // Tells the GameManager class what the variable texture means in the GameManager class
            GameManager.textFace = typeface;    // Tells the GameManager class what the variable textFace means in the GameManager class

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;  // Increase the timer
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))    // Hit esc to quit
            {
                Exit();
            }
                
            switch (state)
            {
                case GameState.TitleScreen: // The titlescreen 
                    if(timer > delay && Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(1).IsButtonDown(Buttons.A))   // Press enter on keyboard or A on controller to go to the instructions
                    {
                        timer = 0;  // Reset timer
                        state = GameState.Instructions;
                    }
                    break;
                case GameState.Instructions:
                    if (timer > delay && Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(1).IsButtonDown(Buttons.A)) // Press enter on keyboard or A on controller to go to the main game
                    {
                        timer = 0;  // Reset timer
                        GameManager.score = 0;  // Reset score
                        GameManager.scoreMultiplier = 1;    // Resest multiplier
                        GameManager.lives = 3;  // Reset lives
                        GameManager.eggSpawnDelay = 0.6f; // Reset spawn rate
                        GameManager.eggSpeed = 10;  // Reset egg speed
                        GameManager.eggsCollected = 0; // Reset the power up counter
                        GameManager.SetPlayer();    // Randomly choose one of six possible players
                        state = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    if (GameManager.lives <= 0) // If you have zero lives you lose
                    {
                        state = GameState.GameOver;
                    }
                    if (timer > delay && Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        isPaused = !isPaused;
                        timer = 0;
                    }
                    if (isPaused == false)
                    {
                        GameManager.Update(gameTime);   // Call the gameManager update
                    }
                    break;
                case GameState.GameOver:
                    if (timer > delay && Keyboard.GetState().IsKeyDown(Keys.R) || GamePad.GetState(1).IsButtonDown(Buttons.A))  // Press R on keyboard or A on controller to replay the game
                    {
                        timer = 0;  // Reset timer
                        GameManager.score = 0;  // Reset score
                        GameManager.scoreMultiplier = 1;    // Reset multiplier
                        GameManager.lives = 3;  // Reset lives
                        GameManager.eggSpawnDelay = 0.6f; // Reset spawn rate
                        GameManager.eggSpeed = 10;  // Reset egg speed
                        GameManager.eggsCollected = 0; // Reset the power up counter
                        GameManager.SetPlayer();    // Randomly choose one of six possible players
                        state = GameState.Playing;
                    }
                    if (timer > delay && Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(1).IsButtonDown(Buttons.B))  // Press enter on keyboard or B on controller to go to the title screen
                    {
                        timer = 0;
                        state = GameState.TitleScreen;
                    }
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Fun glitches happen without this
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (state == GameState.TitleScreen) // Draw the title screen when on the title screen
            {
                spriteBatch.Draw(titleScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height), Color.White);
            }
            if (state == GameState.Instructions)    // Draw the instructions when on the instructions
            {
                spriteBatch.Draw(instructions, new Rectangle(0, 0, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height), Color.White);
            }
            if (state == GameState.Playing)     // Draw the background when on the main game
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height), Color.White);
                GameManager.Draw(gameTime, spriteBatch);    // Draw what the GameManager is drawing
            }
            if (state == GameState.GameOver)    // Draw the game over screen and your current score when you game over
            {
                spriteBatch.Draw(gameOver, new Rectangle(0, 0, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height), Color.White);
                spriteBatch.DrawString(typeface, "Your Score: " + GameManager.score.ToString(), new Vector2(400, 100), Color.Yellow);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
