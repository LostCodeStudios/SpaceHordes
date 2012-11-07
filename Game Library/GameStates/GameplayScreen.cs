using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Game_Library.Input;
using Game_Library.Model.Managers;
using Game_Library.Model;

namespace Game_Library.GameStates.Screens
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        EntityManager entities;
        SpriteFont gameFont;
        string fontName;

        float pauseAlpha;
        Texture2D spriteSheet;
        InputAction pauseAction;

        #endregion

        #region Properties

        /// <summary>
        /// The gameplay screen's content manager.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string fontName)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.fontName = fontName;

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }
        
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>(fontName);

            Spritesheet spriteSheet = new Spritesheet(Content, "Textures/spritesheet");

            Dictionary<string, Rectangle[]> sourceRectangles = new Dictionary<string, Rectangle[]>();

            //TODO: Add all the source rectangles.
            sourceRectangles.Add("base",
                new Rectangle[] {
	                new Rectangle(1, 1, 97, 78),
	                new Rectangle(99, 1, 97, 78),
	                new Rectangle(197, 1, 97, 78)
                });

            sourceRectangles.Add("eye",
                new Rectangle[] {
                    new Rectangle(295, 1, 72, 69)
                });

            sourceRectangles.Add("blaster",
                new Rectangle[] {
                    new Rectangle(368, 1, 76, 29)
                });

            sourceRectangles.Add("eyeshot",
                new Rectangle[] {
                    new Rectangle(368, 31, 11, 11)
                });

            sourceRectangles.Add("redspikeball",
                new Rectangle[] {
                    new Rectangle(380, 31, 13, 11)
                });

            sourceRectangles.Add("tanshipredgrille",
                new Rectangle[] {
                    new Rectangle(396, 31, 443, 51)
                });

            sourceRectangles.Add("brownplane",
                new Rectangle[] {
                   new Rectangle(368, 43, 27, 23)
                });

            sourceRectangles.Add("greyshipredspike",
                new Rectangle[] {
                    new Rectangle(396, 52, 24, 21),
                    new Rectangle(421, 52, 24, 21)
                });

            sourceRectangles.Add("player1",
                new Rectangle[] {
                    new Rectangle(1, 80, 25, 13)
                });

            sourceRectangles.Add("player2",
                new Rectangle[] {
                    new Rectangle(27, 80, 20, 17)
                });

            //TODO: Finish defining source rectangles

            sourceRectangles.Add("birdbody", new Rectangle[] { new Rectangle(1, 491, 184, 83) });
            sourceRectangles.Add("birdhead", new Rectangle[] { new Rectangle(1, 576, 32, 54) });

            spriteSheet.Animations = sourceRectangles;

            entities = new EntityManager();
            Entity test = new Entity(new Vector2(700, 300), 1.234f, 20);
            test.Sprites.Add("base", new Sprite(new Vector2(640, 360), spriteSheet, "base", Vector2.Zero));

            Entity Dragon = new Entity(new Vector2(700, 300), 0, 0);
            Dragon.Sprites.Add("hawtbody", new Sprite(new Vector2(700, 360), spriteSheet, "birdbody", Vector2.Zero));
            Dragon.Sprites.Add("head", new Sprite(new Vector2(700+76, 360+83), spriteSheet, "birdhead", Vector2.Zero));
            Dragon.Rotation = (float)(1 / 4 * Math.PI);
            Dragon.Velocity = 1;
            entities.Add("ShipMuhfuckka",test);
            entities.Add("dragon", Dragon);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Updates the state of the game. This method 
        /// checks the GameScreen.IsActive property, so the
        /// game will stop updating when the pause menu is 
        /// active, or if you tab away to a different
        /// application.
        /// </summary>
        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus, 
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Gradually fade in or out depending on if we are covered by another screen
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                
                //TODO: Game
                



                entities.Update(gameTime);
            }

        }

        /// <summary>
        /// Lets the game respond to user input. Only called when this screen
        /// is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            #if WINDOWS
                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            #endif
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            
            //TODO: All drawing for gameplay.
            entities.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}
