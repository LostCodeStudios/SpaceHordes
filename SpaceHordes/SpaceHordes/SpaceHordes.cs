using GameLibrary.GameStates;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceHordes.GameStates.Screens;
using System;
using System.Collections.Generic;
using SpaceHordes.Entities.Systems;
using Microsoft.Xna.Framework.Storage;

/***Some documentation notes:
 * From this point, herein, standard regions for classes must be use and stuff. lol.

        #region Functioning Loop

        #endregion Functioning Loop

        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Methods

        #endregion Methods

        #region Helpers

        #endregion Helpers

 * *
 */

namespace SpaceHordes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceHordes : Microsoft.Xna.Framework.Game
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private ScreenManager screenManager;

#if XBOX
        private IAsyncResult result;
        bool needResult = true;
        public static bool needStorageDevice = false;
        int debug = 0;
        public static PlayerIndex controllingIndex;
#endif

        #endregion Fields 

        #region Initalization

        public SpaceHordes()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Space Hordes";
            graphics.PreferMultiSampling = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsFixedTimeStep = true;
            graphics.ApplyChanges();
            Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
#if WINDOWS
            this.IsMouseVisible = true;
#endif

            screenManager = new ScreenManager(this);
            ScreenHelper.Initialize(graphics, GraphicsDevice);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            Components.Add(screenManager);

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
#if WINDOWS
            ApplySettings();
#endif
            base.LoadContent();

            // TODO: use this.Content to load your game content here
            screenManager.AddScreen(new BackgroundScreen("Textures/Hiscore", TransitionType.Fade), null);
            screenManager.AddScreen(new StartScreen(), null);

            ScreenHelper.SpriteSheet = new SpriteSheet(Content.Load<Texture2D>("Textures/spritesheet"));
            ScreenHelper.SpriteFont = Content.Load<SpriteFont>("Fonts/menufont");
            SetSourceRectangles(ScreenHelper.SpriteSheet);
            SetSoundEffects();
            SetSongs();

#if WINDOWS
            MusicManager.PlaySong("Title");
#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion Initalization

        #region Update & Draw

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if XBOX //Storage Device stuff
            try
            {

            //UPDATE
            // Set the request flag
                if (needStorageDevice)
                {
                    if (!Guide.IsVisible && needResult)
                    {
                        result = StorageDevice.BeginShowSelector(controllingIndex, null, null);
                        needResult = false;
                    }

                    if (result != null && result.IsCompleted)
                    {
                        StorageDevice device = StorageDevice.EndShowSelector(result);
                        if (device != null && device.IsConnected)
                        {
                            ScreenManager.Storage = device;
                            try
                            {
                                ApplySettings();
                            }
                            catch
                            {
                                screenManager.AddScreen(new StartScreen(true), null);
                                result = null;
                                needStorageDevice = false;
                                needResult = true;
                                return;
                            }
                            MusicManager.PlaySong("Title");
                            screenManager.AddScreen(new MainMenuScreen("Space Hordes"), null);
                            needStorageDevice = false;
                        }
                        else
                        {
                            result = null;
                            needResult = true;
                        }
                    }
                }

            }
            catch (GuideAlreadyVisibleException)
            {
            }
#endif

            SoundManager.UpdateRumble(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

#if WINDOWS
        protected override void OnExiting(object sender, EventArgs args)
        {
            Win32.ConsoleLibrary.FreeConsole();
            base.OnExiting(sender, args);
        }
#endif

        #endregion Update & Draw

        #region Source Rectangles

        private void SetSourceRectangles(SpriteSheet sheet)
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

            sourceRectangles.Add("Player1",
                new Rectangle[] {
                    new Rectangle(137, 309, 28, 23),
                    new Rectangle(166, 309, 28, 23),
                    new Rectangle(195, 309, 28, 23)
                });

            sourceRectangles.Add("Player2",
                new Rectangle[] {
                    new Rectangle(27, 80, 20, 16)
                });

            sourceRectangles.Add("Player3",
                new Rectangle[] {
                    new Rectangle(249, 140, 25, 19)
                });

            sourceRectangles.Add("Player4",
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

            sourceRectangles.Add("whiteshot1",
                new Rectangle[] {
                    new Rectangle(397, 405, 6, 3)
                });

            sourceRectangles.Add("whiteshot2",
                new Rectangle[] {
                    new Rectangle(404, 404, 11, 5)
                });

            sourceRectangles.Add("whiteshot3",
                new Rectangle[] {
                    new Rectangle(416, 403, 11, 7)
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
                    new Rectangle(409, 144, 15, 9)
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
                    new Rectangle(106, 157, 21, 21)
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
                    new Rectangle(249, 160, 97, 130),
                    new Rectangle(347, 160, 97, 130)
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

            sourceRectangles.Add("swastika", //WHAT TJE FUIUCK
                new Rectangle[] {
                    new Rectangle(137, 261, 47, 47)
                });

            sourceRectangles.Add("swastika2",
                new Rectangle[] {
                    new Rectangle(185, 261, 47, 47)
                });

            sourceRectangles.Add("massivebluemissile",
                new Rectangle[] {
                    new Rectangle(361, 932, 45, 209)
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

            sourceRectangles.Add("nebula",
                new Rectangle[]
                {
                    new Rectangle(21, 494, 134, 128)
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

            sourceRectangles.Add("shipexhaust1",
                new Rectangle[] {
                    new Rectangle(1, 749, 23, 23),
                    new Rectangle(25, 749, 23, 23),
                    new Rectangle(49, 749, 23, 23),
                    new Rectangle(73, 749, 23, 23)
                });

            sourceRectangles.Add("shipexaust2",
                new Rectangle[] {
                    new Rectangle(368, 563, 23, 22),
                    new Rectangle(392, 563, 23, 22),
                    new Rectangle(416, 563, 23, 22)
                });

            sourceRectangles.Add("junkrock",
                new Rectangle[] {
                    new Rectangle(1, 917, 141, 142)
                });

            sourceRectangles.Add("rotatinglightball",
                new Rectangle[] {
                    new Rectangle(143, 917, 21, 21),
                    new Rectangle(165, 917, 21, 21),
                    new Rectangle(187, 917, 21, 21),
                    new Rectangle(209, 917, 21, 21)
                });

            sourceRectangles.Add("greenlaser",
                new Rectangle[] {
                    new Rectangle(143, 939, 71, 23),
                    new Rectangle(143, 963, 71, 23)
                });

            sourceRectangles.Add("redlaser",
                new Rectangle[] {
                    new Rectangle(215, 939, 71, 23),
                    new Rectangle(215, 963, 71, 23)
                });

            sourceRectangles.Add("bluelaser",
                new Rectangle[] {
                    new Rectangle(287, 939, 71, 23),
                    new Rectangle(287, 963, 71, 23)
                });

            sourceRectangles.Add("flamer",
                new Rectangle[] {
                    new Rectangle(152, 1004, 184, 58)
                });

            sheet.Animations = sourceRectangles;
        }

        #endregion Source Rectangles

        #region Sound Effects

        private void SetSoundEffects()
        {
            SoundManager.Add("Explosion1", Content.Load<SoundEffect>("Sounds/Explosion1"));
            SoundManager.Add("Explosion2", Content.Load<SoundEffect>("Sounds/Explosion2"));
            SoundManager.Add("Explosion3", Content.Load<SoundEffect>("Sounds/Explosion3"));
            SoundManager.Add("Explosion4", Content.Load<SoundEffect>("Sounds/Explosion4"));
            SoundManager.Add("Construction", Content.Load<SoundEffect>("Sounds/Construction"));
            SoundManager.Add("SelectChanged", Content.Load<SoundEffect>("Sounds/mouse_over4"));
            SoundManager.Add("MenuCancel", Content.Load<SoundEffect>("Sounds/electric_deny2"));
            SoundManager.Add("Selection", Content.Load<SoundEffect>("Sounds/melodic1_click"));
            SoundManager.Add("DialogSound", Content.Load<SoundEffect>("Sounds/analogue_click"));
            SoundManager.Add("Shot1", Content.Load<SoundEffect>("Sounds/shot"));
            SoundManager.Add("Shot2", Content.Load<SoundEffect>("Sounds/shot2"));
            SoundManager.Add("Pickup1", Content.Load<SoundEffect>("Sounds/pickup"));
            SoundManager.Add("SurgeAlarm", Content.Load<SoundEffect>("Sounds/beep-5"));
            SoundManager.Add("SpaceElephant", Content.Load<SoundEffect>("Sounds/SpaceElephant"));
        }

        #endregion Sound Effects

        #region Music

        private void SetSongs()
        {
            MusicManager.AddSong("Cephelopod", Content.Load<Song>("Music/Cephelopod"));
            MusicManager.AddSong("Title", Content.Load<Song>("Music/Space Fighter Loop"));
            MusicManager.AddSong("Heartbeat", Content.Load<Song>("Music/In a Heartbeat"));
            MusicManager.AddSong("SpaceLoop", Content.Load<Song>("Music/DST-2ndBallad"));
            MusicManager.AddSong("4Tran", Content.Load<Song>("Music/DST-4Tran"));
            MusicManager.AddSong("Azimuth", Content.Load<Song>("Music/DST-Azimuth"));
            MusicManager.AddSong("AngryMod", Content.Load<Song>("Music/DST-AngryMod"));
        }

        #endregion Music

        #region Helpers

        public static void ApplySettings()
        {
            int sound;
            int music;
            bool fullscreen;
            bool rumbleOn;

            OptionsMenuScreen.ReadSettings(out sound, out music, out fullscreen, out rumbleOn);

            SoundManager.Volume = (float)sound / 10f;
            MusicManager.Volume = (float)music / 10f;
            SoundManager.Rumble = rumbleOn;

            if (ScreenHelper.Graphics.IsFullScreen != fullscreen)
            {
                ScreenHelper.Graphics.IsFullScreen = fullscreen;
                ScreenHelper.Graphics.ApplyChanges();
                HUDRenderSystem.ApplyScaling();
            }

            BossScreen.ClearedBosses = BossScreen.ReadData();
        }

        #endregion Helpers
    }
}