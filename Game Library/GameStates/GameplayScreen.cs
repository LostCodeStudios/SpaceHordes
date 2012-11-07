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

            //Get all the sprite source rectangles.
            SetSourceRectangles(spriteSheet);

            entities = new EntityManager();
            Entity test = new Entity(new Vector2(700, 300), 1.234f, 20);
            test.Sprites.Add("base", new Sprite(new Vector2(640, 360), spriteSheet, "base", Vector2.Zero, AnimationType.None));

            Entity Dragon = new Entity(new Vector2(700, 300), 0, 0);
            Dragon.Sprites.Add("hawtbody", new Sprite(new Vector2(640, 0), spriteSheet, "birdbody", Vector2.Zero));
            Dragon.Sprites.Add("head", new Sprite(new Vector2(640+76, 63), spriteSheet, "birdhead", Vector2.Zero));
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

                if (entities["dragon"].Sprites["hawtbody"].IsBoxColliding(entities["ShipMuhfuckka"].Sprites["base"].HitBox))
                {
                    entities["dragon"].Sprites["hawtbody"].Location = new Vector2(640, 0);
                    entities["dragon"].Sprites["head"].Location = new Vector2(640 + 76, 63);
                    entities["ShipMuhfuckka"].Sprites["base"].AdvanceFrame();
                }


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

        #region Source Rectangles

        void SetSourceRectangles(Spritesheet sheet)
        {
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

            sourceRectangles.Add("orangebubble",
                new Rectangle[] {
                    new Rectangle(1, 98, 17, 17),
                    new Rectangle(19, 98, 17, 17),
                    new Rectangle(37, 98, 17, 17)
                });

            sourceRectangles.Add("redshot1",
                new Rectangle[] {
                    new Rectangle(55, 82, 6, 3)
                });

            sourceRectangles.Add("redshot2",
                new Rectangle[] {
                    new Rectangle(62, 81, 11, 5)
                });

            sourceRectangles.Add("redshot3",
                new Rectangle[] {
                    new Rectangle(74, 80, 11, 7)
                });

            sourceRectangles.Add("greenshot1",
                new Rectangle[] {
                    new Rectangle(55, 90, 6, 3)
                });

            sourceRectangles.Add("greenshot2",
                new Rectangle[] {
                    new Rectangle(62, 89, 11, 5)
                });

            sourceRectangles.Add("greenshot3",
                new Rectangle[] {
                    new Rectangle(74, 88, 11, 7)
                });

            sourceRectangles.Add("blueshot1",
                new Rectangle[] {
                    new Rectangle(55, 98, 6, 3)
                });

            sourceRectangles.Add("blueshot2",
                new Rectangle[] {
                    new Rectangle(62, 97, 11, 5)
                });

            sourceRectangles.Add("blueshot3",
                new Rectangle[] {
                    new Rectangle(74, 96, 11, 7)
                });

            sourceRectangles.Add("bluespark",
                new Rectangle[] {
                    new Rectangle(55, 104, 11, 11),
                    new Rectangle(67, 104, 11, 11),
                    new Rectangle(79, 104, 11, 11)
                });

            sourceRectangles.Add("greenspark",
                new Rectangle[] {
                    new Rectangle(0, 116, 23, 22),
                    new Rectangle(24, 116, 23, 22),
                    new Rectangle(48, 116, 23, 22),
                    new Rectangle(72, 116, 23, 22),
                    new Rectangle(96, 116, 23, 22),
                    new Rectangle(120, 116, 23, 22)
                });

            sourceRectangles.Add("graybulbwithsidegunthings",
                new Rectangle[] {
                    new Rectangle(86, 80, 25, 23)
                });

            sourceRectangles.Add("longflame",
                new Rectangle[] {
                    new Rectangle(112, 80, 54, 17),
                    new Rectangle(167, 80, 54, 17)
                });

            sourceRectangles.Add("redcrystal",
                new Rectangle[] {
                    new Rectangle(112, 101, 7, 14)
                });

            sourceRectangles.Add("greencrystal",
                new Rectangle[] {
                    new Rectangle(120, 101, 7, 14)
                });

            sourceRectangles.Add("bluecrystal",
                new Rectangle[] {
                    new Rectangle(128, 101, 7, 14)
                });

            sourceRectangles.Add("yellowcrystal",
                new Rectangle[] {
                    new Rectangle(136, 101, 7, 14)
                });

            sourceRectangles.Add("graycrystal",
                new Rectangle[] {
                    new Rectangle(144, 101, 7, 14)
                });

            sourceRectangles.Add("bluemissile",
                new Rectangle[] {
                    new Rectangle(152, 98, 54, 25)
                });

            sourceRectangles.Add("8prongbrownthingwithfangs",
                new Rectangle[] {
                    new Rectangle(222, 80, 28, 47),
                    new Rectangle(251, 80, 28, 47),
                    new Rectangle(280, 80, 28, 47)
                });

            sourceRectangles.Add("minibrownclawboss",
                new Rectangle[] {
                    new Rectangle(309, 71, 52, 56)
                });

            sourceRectangles.Add("redgrayblobship",
                new Rectangle[] {
                    new Rectangle(362, 74, 46, 85)
                });

            sourceRectangles.Add("redgrayairplane",
                new Rectangle[] {
                    new Rectangle(409, 74, 27, 69)
                });

            sourceRectangles.Add("miniturret",
                new Rectangle[] {
                    new Rectangle(409, 144, 19, 11)
                });

            sourceRectangles.Add("minifire",
                new Rectangle[] {
                    new Rectangle(144, 128, 27, 16),
                    new Rectangle(172, 128, 27, 16),
                    new Rectangle(200, 128, 27, 16)
                });

            sourceRectangles.Add("splosion1",
                new Rectangle[] {
                    new Rectangle(228, 128, 11, 11),
                    new Rectangle(240, 128, 11, 11),
                    new Rectangle(252, 128, 11, 11),
                    new Rectangle(264, 128, 11, 11),
                    new Rectangle(276, 128, 11, 11),
                    new Rectangle(288, 128, 11, 11),
                    new Rectangle(300, 128, 11, 11)
                });
            
            sourceRectangles.Add("birdbody", new Rectangle[] { new Rectangle(1, 491, 184, 83) });
            sourceRectangles.Add("birdhead", new Rectangle[] { new Rectangle(1, 576, 37, 51) });

            sheet.Animations = sourceRectangles;
        }

        #endregion
    }
}
