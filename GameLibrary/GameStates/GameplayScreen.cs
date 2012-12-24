using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameLibrary.Input;
using GameLibrary.Helpers;


namespace GameLibrary.GameStates.Screens
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        string fontName;

        float pauseAlpha;
        SpriteSheet spriteSheet;
        InputAction pauseAction;

        bool colliding = false;
        int x = 1000;

        bool multiplayer;

        Vector2 mouseLoc;

        long score = 100000;

        #endregion

        #region Properties

        /// <summary>
        /// The gameplay screen's content manager.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }

        /// <summary>
        /// The spritesheet.
        /// </summary>
        public SpriteSheet Spritesheet
        {
            get { return spriteSheet; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string fontName, bool multiplayer)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.multiplayer = multiplayer;
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

            ScreenHelper.Initialize(ScreenManager.GraphicsDevice);

            gameFont = content.Load<SpriteFont>(fontName);

            spriteSheet = new SpriteSheet(Content, "Textures/spritesheet");


            if (multiplayer)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ScreenManager.Input.GamePadWasConnected[x])
                    {
                       
                    }
                }
            }

            else
            {               
            }
            

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

            if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Space))
                GameOver();

            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            mouseLoc = input.MouseLocation;
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

           
            spriteBatch.DrawString(gameFont, "X: " + mouseLoc.X, new Vector2(300, 300), Color.White);
            spriteBatch.DrawString(gameFont, "Y: " + mouseLoc.Y, new Vector2(500, 300), Color.White);
            

            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion

        #region Methods

        private void GameOver()
        {
            ScreenManager.AddScreen(new BackgroundScreen("Textures/Hiscore"), ControllingPlayer);
            ScreenManager.AddScreen(new GameOverScreen(new PlayerIndex[] { (PlayerIndex)ControllingPlayer }, score), ControllingPlayer);
            ExitScreen();
        }

        #endregion

        #region Source Rectangles

        void SetSourceRectangles(SpriteSheet sheet)
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
                    new Rectangle(2, 81, 23, 11)
                });

            sourceRectangles.Add("player2",
                new Rectangle[] {
                    new Rectangle(27, 80, 20, 16)
                });

            sourceRectangles.Add("player3",
                new Rectangle[] {
                    new Rectangle(249, 140, 25, 19)
                });

            sourceRectangles.Add("player4",
                new Rectangle[] {
                    new Rectangle(112, 139, 21, 17)
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

            sourceRectangles.Add("splosion2",
                new Rectangle[] {
                    new Rectangle(1, 185, 23, 23),
                    new Rectangle(25, 185, 23, 23),
                    new Rectangle(49, 185, 23, 23),
                    new Rectangle(73, 185, 23, 23),
                    new Rectangle(97, 185, 23, 23),
                    new Rectangle(121, 185, 23, 23),
                    new Rectangle(145, 185, 23, 23),
                    new Rectangle(169, 185, 23, 23)
                });

            sourceRectangles.Add("splosion3",
                new Rectangle[] {
                    new Rectangle(1, 443, 47, 47),
                    new Rectangle(49, 443, 47, 47),
                    new Rectangle(97, 443, 47, 47),
                    new Rectangle(145, 443, 47, 47),
                    new Rectangle(193, 443, 47, 47),
                    new Rectangle(241, 443, 47, 47),
                    new Rectangle(289, 443, 47, 47)
                });

            sourceRectangles.Add("splosion4",
                new Rectangle[] {
                    new Rectangle(1, 395, 47, 47),
                    new Rectangle(49, 395, 47, 47),
                    new Rectangle(97, 395, 47, 47),
                    new Rectangle(145, 395, 47, 47),
                    new Rectangle(193, 395, 47, 47),
                    new Rectangle(241, 395, 47, 47),
                    new Rectangle(289, 395, 47, 47)
                });

            sourceRectangles.Add("barrier",
                new Rectangle[] {
                    new Rectangle(1, 139, 27, 41)
                });

            sourceRectangles.Add("graye",
                new Rectangle[] {
                    new Rectangle(29, 139, 25, 45)
                });

            sourceRectangles.Add("redstripehomingball",
                new Rectangle[] {
                    new Rectangle(55, 139, 18, 17),
                    new Rectangle(74, 139, 18, 17),
                    new Rectangle(93, 139, 18, 17)
                });

            sourceRectangles.Add("brownstripwithtwoprongs",
                new Rectangle[] {
                    new Rectangle(55, 157, 23, 17)
                });


            sourceRectangles.Add("grayshipwithtwoprongs",
                new Rectangle[] {
                    new Rectangle(79, 157, 26, 16)
                });

            sourceRectangles.Add("grayshipwithtwowings",
                new Rectangle[] {
                    new Rectangle(79, 157, 21, 21)
                });

            sourceRectangles.Add("blueshipwithbulb",
                new Rectangle[] {
                    new Rectangle(134, 145, 42, 25)
                });

            sourceRectangles.Add("brownthingwithbluelight",
                new Rectangle[] {
                    new Rectangle(177, 145, 23, 24),
                    new Rectangle(201, 145, 23, 24),
                    new Rectangle(225, 145, 23, 24)
                });

            sourceRectangles.Add("graytriangleship",
                new Rectangle[] {
                    new Rectangle(1, 209, 24, 17),
                    new Rectangle(26, 209, 24, 17),
                    new Rectangle(51, 209, 24, 17),
                    new Rectangle(76, 209, 24, 17),
                    new Rectangle(101, 209, 24, 17)
                });

            sourceRectangles.Add("graymissile",
                new Rectangle[] {
                    new Rectangle(126, 209, 56, 19)
                });

            sourceRectangles.Add("greenfacething",
                new Rectangle[] {
                    new Rectangle(193, 170, 55, 67)
                });

            sourceRectangles.Add("blimp",
                new Rectangle[] {
                    new Rectangle(249, 160, 93, 170),
                    new Rectangle(347, 160, 93, 170)
                });

            sourceRectangles.Add("bigredblobboss",
                new Rectangle[] {
                    new Rectangle(1, 229, 135, 111)
                });

            sourceRectangles.Add("purpleship",
                new Rectangle[] {
                    new Rectangle(137, 229, 20, 23)
                });

            sourceRectangles.Add("browntriangleship",
                new Rectangle[] {
                    new Rectangle(158, 229, 22, 23)
                });

            sourceRectangles.Add("greyshipbrownbulb",
                new Rectangle[] {
                    new Rectangle(182, 238, 26, 22)
                });

            sourceRectangles.Add("blueshipgraybulb",
                new Rectangle[] {
                    new Rectangle(209, 238, 26, 22)
                });

            sourceRectangles.Add("swastika",
                new Rectangle[] {
                    new Rectangle(137, 261, 47, 47)
                });

            sourceRectangles.Add("swastika2",
                new Rectangle[] {
                    new Rectangle(185, 261, 47, 47)
                });

            sourceRectangles.Add("blueshipredexhaust",
                new Rectangle[] {
                    new Rectangle(137, 309, 28, 23),
                    new Rectangle(166, 309, 28, 23),
                    new Rectangle(195, 309, 28, 23)
                });

            sourceRectangles.Add("massivebluemissile",
                new Rectangle[] {
                    new Rectangle(233, 291, 209, 45)
                });

            sourceRectangles.Add("redgunship",
                new Rectangle[] {
                    new Rectangle(1, 341, 137, 53),
                    new Rectangle(139, 341, 137, 53),
                    new Rectangle(277, 341, 137, 53)
                });

            sourceRectangles.Add("reddownmissile",
                new Rectangle[] {
                    new Rectangle(415, 337, 23, 48)
                });

            sourceRectangles.Add("brownarmship",
                new Rectangle[] {
                    new Rectangle(337, 395, 19, 23),
                    new Rectangle(357, 395, 19, 23),
                    new Rectangle(377, 395, 19, 23)
                });

            sourceRectangles.Add("brownfangship",
                new Rectangle[] {
                    new Rectangle(337, 419, 28, 21),
                    new Rectangle(366, 419, 28, 21),
                    new Rectangle(395, 419, 28, 21)
                });

            sourceRectangles.Add("redstar",
                new Rectangle[] {
                    new Rectangle(337, 441, 17, 19),
                    new Rectangle(355, 441, 17, 19),
                    new Rectangle(373, 441, 17, 19),
                    new Rectangle(391, 441, 17, 19)
                });

            sourceRectangles.Add("squidship",
                new Rectangle[] {
                    new Rectangle(409, 441, 26, 19)
                });

            sourceRectangles.Add("bluecrystalship",
                new Rectangle[] {
                    new Rectangle(337, 461, 24, 20),
                    new Rectangle(362, 461, 24, 20),
                    new Rectangle(387, 461, 24, 20)
                });

            sourceRectangles.Add("birdbody", 
                new Rectangle[] { 
                    new Rectangle(2, 492, 183, 82)
                });

            sourceRectangles.Add("birdhead",
                new Rectangle[] { 
                    new Rectangle(2, 576, 35, 50),
                    new Rectangle(39, 576, 35, 50),
                    new Rectangle(77, 576, 35, 50)
                });

            sourceRectangles.Add("shipwiththetwoboxgunthings",
                new Rectangle[] {
                    new Rectangle(115, 576, 66, 50)
                });

            sourceRectangles.Add("minikillerhead",
                new Rectangle[] {
                    new Rectangle(187, 491, 125, 111)
                });

            sourceRectangles.Add("minikillereyes",
                new Rectangle[] {
                    new Rectangle(313, 491, 54, 14),
                    new Rectangle(313, 506, 54, 14),
                    new Rectangle(313, 521, 54, 14),
                    new Rectangle(313, 536, 54, 14),
                    new Rectangle(313, 551, 54, 14),
                    new Rectangle(313, 566, 54, 14)
                });

            sourceRectangles.Add("brain",
                new Rectangle[] {
                    new Rectangle(368, 482, 79, 80)
                });

            sourceRectangles.Add("rednebula",
                new Rectangle[] {
                    new Rectangle(1, 628, 114, 120)
                });

            sourceRectangles.Add("greenbossship",
                new Rectangle[] {
                    new Rectangle(116, 627, 95, 101)
                });

            sourceRectangles.Add("clawbossthing",
                new Rectangle[] {
                    new Rectangle(212, 603, 113, 104),
                    new Rectangle(326, 603, 113, 104)
                });

            sourceRectangles.Add("smasher",
                new Rectangle[] {
                    new Rectangle(230, 708, 67, 70)
                });

            sourceRectangles.Add("chainlink",
                new Rectangle[] {
                    new Rectangle(212, 708, 12, 18)
                });

            sourceRectangles.Add("smasherball",
                new Rectangle[] {
                    new Rectangle(116, 729, 37, 37),
                    new Rectangle(154, 729, 37, 37),
                    new Rectangle(192, 729, 37, 37)
                });

            sourceRectangles.Add("satellite",
                new Rectangle[] {
                    new Rectangle(298, 708, 68, 55),
                    new Rectangle(367, 708, 68, 55)
                });

            sourceRectangles.Add("giantgraybossship",
                new Rectangle[] {
                    new Rectangle(1, 779, 117, 137),
                    new Rectangle(119, 779, 117, 137)
                });

            sourceRectangles.Add("killerhead",
                new Rectangle[] {
                    new Rectangle(316, 764, 130, 165)
                });

            sourceRectangles.Add("killerleftgun",
                new Rectangle[] {
                    new Rectangle(234, 779, 40, 105)
                });

            sourceRectangles.Add("killerrightgun",
                new Rectangle[] {
                    new Rectangle(275, 779, 40, 105)
                });

            sheet.Animations = sourceRectangles;
        }

        #endregion
    }
}
