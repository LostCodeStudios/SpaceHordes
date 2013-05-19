using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;
using SpaceHordes.Entities.Templates.Objects;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using GameLibrary.GameStates.Screens;
using SpaceHordes.GameStates.Screens;

namespace SpaceHordes.Entities.Systems
{
    enum MusicState
    {
        TransitionOn,
        TransitionOff,
        Transitioned
    }

    enum SongType
    {
        Loop,
        Boss,
        Surge
    }

    public enum SpawnState
    {
        Wave,
        Surge,
        Boss,
        Peace,
        Endless,
        Victory
    }

    public class DirectorSystem : IntervalEntitySystem
    {
        #region Fields
        private static Random r = new Random();

        bool init = false;
        private Entity Base;
        private Entity Boss;
        private Entity[] Players;
        public int[] RespawnTime;
        int[] PlayerToSpawn;

        private double difficulty = 0;

#if WINDOWS
        private int maxMooks = 1;
        private double maxThugs = 0.1;
#endif

#if XBOX
        private int maxMooks = 1;
        private double maxThugs = 0.1;
#endif

        Queue<SpawnState> states = new Queue<SpawnState>();
        public SpawnState SpawnState = SpawnState.Peace;
        int waves = 0;

        int elapsedWarning = 0;
        int warningTime = 3000;
        public static float[] StateDurations = new float[]
        {
            45f,
            30f,
            0f,
            15f
        };

        public static int ElapsedSurge = 0;
        float nextSeconds = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        private float elapsedSeconds; private float secPerCall = 0.333f;
        private float elapsedMinutes; private float minPerCall = 0.333f / 60;
        private int timesCalled = 0;
        private float intervalSeconds = 0f;

        int level;

        public Action OnVictory;

        #endregion

        #region Song Tags

        string[] loopSongs = new string[]
        {
            "SpaceLoop",
            "Azimuth"
        };

        string[] bossSongs = new string[]
        {
            "Heartbeat",
            "4Tran"
        };

        string[] surgeSongs = new string[]
        {
            "Cephelopod",
            "AngryMod"
        };

        MusicState musicState = MusicState.Transitioned;
        float tempVolume = 0f;
        string nextSong = "";

        #endregion

        #region Spawn Rates

        public  int MookSpawnRate = 1;
        public  int ThugSpawnRate = 1;
        public  int GunnerSpawnRate = 1;
        public  int HunterSpawnRate = 1;
        public  int DestroyerSpawnRate = 1;

        public  int SpawnRate
        {
            set
            {
                MookSpawnRate = value;
                ThugSpawnRate = value;
                GunnerSpawnRate = value;
                HunterSpawnRate = value;
                DestroyerSpawnRate = value;
            }
        }

        #endregion

        #region Enemy Tags

        public string MookTemplate = "Mook";
        public string MookSprite = "";

        public string ThugTemplate = "Thug";
        public string ThugSprite = "";

        public string GunnerTemplate = "Gunner";
        public string GunnerSprite = "";

        public string HunterTemplate = "Hunter";
        public string HunterSprite = "";

        public string DestroyerTemplate = "Destroyer";
        public string BossTemplate = "Boss";

        public  void ResetTags()
        {
            MookSprite = "";
            ThugSprite = "";
            GunnerSprite = "";

            GunnerTemplate = "Gunner";
            HunterTemplate = "Hunter";
            DestroyerTemplate = "Destroyer";
            BossTemplate = "Boss";
        }

        #endregion

        #region Tutorial
        public MessageDialog CurrentDialog;

#if XBOX
        string[] tutorialMessages = new string[]
        {
            "WELCOME TO SPACE HORDES.",
            "MOVE WITH THE LEFT ANALOG STICK.",
            "AIM WITH RIGHT STICK AND HOLD DOWN THE RIGHT TRIGGER TO SHOOT.",
            "AN ENEMY SHIP IS COMING. YOU CAN SEE IT ON THE RADAR.",
            "SHOOT IT BEFORE IT HITS YOUR BASE.",
            "ENEMIES DROP CRYSTALS WHICH ARE USED AS AMMO.",
            "GRAY CRYSTALS ENHANCE YOUR GUNS.",
            "",
            "YOU HAVE 4 GUN TYPES: RED GREEN BLUE AND WHITE.",
            "BLUE BULLETS FREEZE ENEMIES. PRESS X TO SELECT.",
            "GREEN BULLETS DO POISON DAMAGE. PRESS A TO SELECT.",
            "RED BULLETS ARE FASTER. PRESS B TO SELECT.",
            "WHITE BULLETS USE NO AMMO. SELECT BY PRESSING THE BUTTON OF YOUR CURRENT GUN.",
            "YOUR STATUS BAR SHOWS HOW MANY YOU HAVE IN EACH GUN.",
            "HERE ARE SOME TOUGHER ENEMIES. TRY OUT YOUR DIFFERENT GUNS.",
            "",
            "",
            "",
            "",
            "YOU CAN BUILD DEFENSES TO PROTECT YOUR BASE.",
            "PRESS Y TO ENTER AND LEAVE BUILD MODE.",
            "IN BUILD MODE YOU CAN BUILD 3 TYPES OF DEFENSES.",
            "BARRIERS STOP ENEMIES. PRESS X TO BUILD.",
            "TURRETS SHOOT ENEMIES. PRESS A TO BUILD.",
            "MINES EXPLODE ON CONTACT WITH ENEMIES. PRESS B TO BUILD.",
            "YOU CANNOT SWITCH GUNS WHILE IN BUILD MODE.",
            "BUILDING DEFENSES USES UP YELLOW CRYSTALS. THE COSTS OF DEFENSES ARE SHOWN ON YOUR STATUS BAR WHILE IN BUILD MODE.",
            "YOU CAN SEE HOW MANY YELLOW CRYSTALS YOU HAVE ABOVE YOUR SHIP.",
            "",
            "BUILD SOME DEFENSES TO SEE HOW THEY WORK IN ACTION.",
            "",
            "",
            "",
            "",
            "THE OBJECT OF THE GAME IS TO DEFEND YOUR BASE.",
            "YOU WILL RESPAWN AFTER 3 SECONDS EVERY TIME YOU DIE.",
            "YOUR CRYSTALS ARE REPLENISHED AFTER RESPAWNING BUT YOUR DEFENSES ARE DESTROYED EVERY TIME YOU DIE.",
            "IF YOU ARE ABOUT TO LOSE YOU CAN SELECT ONE OF YOUR SPECIAL GUNS AND PRESS LEFT TRIGGER.",
            "THIS WILL SPEND ALL THE CRYSTALS YOU HAVE SELECTED AND THE BASE WILL SHOOT A RING OF BULLETS TO CLEAR THE SCREEN.",
            "",
            "NOW YOU HAVE LEARNED ALMOST EVERYTHING THERE IS TO KNOW.",
            "SEE IF YOU CAN PLAY ON YOUR OWN."
        };
#endif
#if WINDOWS
        string[] tutorialMessages = new string[]
        {
            "WELCOME TO SPACE HORDES.",
            "MOVE WITH W A S D.",
            "AIM WITH THE MOUSE AND CLICK TO SHOOT.",
            "AN ENEMY SHIP IS COMING. YOU CAN SEE IT ON THE RADAR.",
            "SHOOT IT BEFORE IT HITS YOUR BASE.",
            "ENEMIES DROP CRYSTALS WHICH ARE USED AS AMMO.",
            "GRAY CRYSTALS ENHANCE YOUR GUNS.",
            "",
            "YOU HAVE 4 GUN TYPES: RED GREEN BLUE AND WHITE.",
            "BLUE BULLETS FREEZE ENEMIES. SELECT WITH 1.",
            "GREEN BULLETS DO POISON DAMAGE. SELECT WITH 2.",
            "RED BULLETS ARE FASTER. SELECT WITH 3.",
            "WHITE BULLETS USE NO AMMO. SELECT BY PRESSING THE KEY OF YOUR CURRENT GUN.",
            "HERE ARE SOME TOUGHER ENEMIES. TRY OUT YOUR DIFFERENT GUNS.",
            "",
            "",
            "",
            "",
            "YOU CAN BUILD DEFENSES TO PROTECT YOUR BASE.",
            "PRESS 4 TO ENTER AND LEAVE BUILD MODE.",
            "IN BUILD MODE YOU CAN BUILD 3 TYPES OF DEFENSES.",
            "BARRIERS STOP ENEMIES. BUILD WITH 1.",
            "TURRETS SHOOT ENEMIES. BUILD WITH 2.",
            "MINES EXPLODE ON CONTACT WITH ENEMIES. BUILD WITH 3.",
            "YOU CANNOT SWITCH GUNS WHILE IN BUILD MODE.",
            "BUILDING DEFENSES USES UP YELLOW CRYSTALS. THE COSTS OF DEFENSES ARE SHOWN ON YOUR STATUS BAR WHILE IN BUILD MODE.",
            "YOU CAN SEE HOW MANY YELLOW CRYSTALS YOU HAVE ABOVE YOUR SHIP.",
            "",
            "BUILD SOME DEFENSES TO SEE HOW THEY WORK IN ACTION.",
            "",
            "",
            "",
            "",
            "THE OBJECT OF THE GAME IS TO DEFEND YOUR BASE.",
            "YOU WILL RESPAWN AFTER 3 SECONDS EVERY TIME YOU DIE.",
            "YOUR CRYSTALS ARE REPLENISHED AFTER RESPAWNING BUT YOUR DEFENSES ARE DESTROYED EVERY TIME YOU DIE.",
            "IF YOU ARE ABOUT TO LOSE YOU CAN SELECT ONE OF YOUR SPECIAL GUNS AND RIGHT CLICK.",
            "THIS WILL SPEND ALL THE CRYSTALS YOU HAVE SELECTED AND THE BASE WILL SHOOT A RING OF BULLETS TO CLEAR THE SCREEN.",
            "",
            "NOW YOU HAVE LEARNED ALMOST EVERYTHING THERE IS TO KNOW.",
            "SEE IF YOU CAN PLAY ON YOUR OWN."
        };
#endif
        int message = 0;
        #endregion

        #region Initialization

        public DirectorSystem()
            : base(333)
        {
            ElapsedSurge = 0;
        }

        public void LoadContent(Entity Base, Entity[] Players, int level, params SpawnState[] spawns)
        {
            this.Base = Base;
            this.Players = Players;
            this.RespawnTime = new int[4];
            this.PlayerToSpawn = new int[4];

            if (!(world as SpaceWorld).Tutorial)
            {
                elapsedSeconds = 60f;
                elapsedMinutes = 1f;
            }

            for (int i = 0; i < Players.Length; ++i)
            {
                Entity e = Players[i];
                int x = i;
                e.GetComponent<Health>().OnDeath += z =>
                {
                    int id = int.Parse(e.Tag.Replace("P", "")) - 1;
                    RespawnTime[id] = 3000;
                    PlayerToSpawn[id] = x;
                };
            }

            this.level = level;

            foreach (SpawnState state in spawns)
            {
                states.Enqueue(state);
            }
        }

        #endregion

        /*********/
        #region Processing

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            SpaceWorld w = world as SpaceWorld;

            if (SpawnState == SpawnState.Victory)
            {
                if (OnVictory != null)
                {
                    OnVictory();
                    LevelSelectScreen.LevelCleared(level + 1);
                }
            }

            if (level == 3 || level == 4)
            {
                waves = 1;
            }

            switch (SpawnState)
            {
                case SpawnState.Wave:
                    difficulty = (waves + elapsedMinutes);
                    break;
                case SpawnState.Endless:
                    difficulty = elapsedMinutes;
                    break;
                case SpawnState.Surge:
                    difficulty = 2 * (waves + elapsedMinutes);
                    break;
                case SpawnState.Boss:
                    difficulty = 0.25f * (waves + elapsedMinutes);
                    break;
                case SpawnState.Peace:
                    difficulty = 0f;
                    break;
            }

            #region MUSIC

            if (timesCalled == 0)
            {
                //spawnBoss();
                setCategory(SongType.Loop);
            }

            updateMusic();
            
            #endregion

            #region REALGAME
            if (!w.Tutorial)
            {
                #region Scoring

                Score s = Base.GetComponent<Score>();

                if (intervalSeconds >= 1)
                {
                    ScoreSystem.GivePoints(10);
                    intervalSeconds = 0;
                }

                #endregion Scoring

                #region Spawning

                int structs = TurretTemplate.Turrets.Count + BarrierTemplate.barriers;
                int mooksToSpawn = Math.Min(doubleToInt(difficulty / 7) * MookSpawnRate, maxMooks * MookSpawnRate);
                int thugsToSpawn = Math.Min(doubleToInt(difficulty / 50) * ThugSpawnRate, doubleToInt(maxThugs) * ThugSpawnRate);
                int gunnersToSpawn = doubleToInt((double)structs / 50) * GunnerSpawnRate;
                int huntersToSpawn = doubleToInt((double)Players.Length / 75) * HunterSpawnRate;
                int destroyersToSpawn = doubleToInt((double)Players.Length / 300) * DestroyerSpawnRate;
                spawnMooks(mooksToSpawn);
                spawnThugs(thugsToSpawn);
                spawnGunners(gunnersToSpawn);
                spawnHunters(huntersToSpawn);
                spawnDestroyers(destroyersToSpawn);

                if (SpawnState == SpawnState.Boss && !init)
                {
                    spawnBoss();
                    init = true;
                }

                if (SpawnState == SpawnState.Wave && !init)
                {
                    spawnWave();
                    init = true;
                }

                if (SpawnState == SpawnState.Surge && !init)
                {
                    spawnSurge();
                    init = true;
                }

                #endregion Spawning
            }
            #endregion

            #region TUTORIAL

            else
            {
                ImageFont font = w._Font;

                if (message < tutorialMessages.Length)
                {
                    if (CurrentDialog == null)
                    {
                        makeDialog(font, tutorialMessages[message++]);
                        nextSeconds = 1000f;
                    }

                    if (CurrentDialog.Complete() && message < tutorialMessages.Length)
                    {
                        nextSeconds = elapsedSeconds + 1.5f;
                        CurrentDialog.Enabled = false;
                    }

                    if (nextSeconds <= elapsedSeconds)
                    {
                        //Special cases
                        if (message == 4)
                            spawnMook();

                        if (message == 14 || message == 15)
                        {
                            spawnThug();
                        }

                        if (message == 29 || message == 30 || message == 31)
                        {
                            spawnMooks(3);
                        }

                        if (message == 32)
                            spawnGunner();

                        if (message == 35)
                            spawnHunter();

                        if (message < tutorialMessages.Length)
                        {
                            makeDialog(font, tutorialMessages[message++]); //Move to the next message 1.5 seconds after the last one finished.
                            nextSeconds = 1000f;
                        }
                    }
                }
                else if (CurrentDialog.Complete())
                {
                    nextSeconds = elapsedSeconds + 2;
                    CurrentDialog.Enabled = false;
                }
                else if (nextSeconds < elapsedSeconds)
                {
                    w.GameScreen.tutorial = false;
                    w.Tutorial = false;
                    CurrentDialog.Visible = false;
                    elapsedMinutes = 1f;
                    elapsedSeconds = 60f;
                    Base.GetComponent<Health>().SetHealth(Base, Base.GetComponent<Health>().MaxHealth);
                }
            }

            #endregion

            #region Player Respawn

            for (int i = 0; i < RespawnTime.Length; ++i)
            {
                if (RespawnTime[i] > 0)
                {
                    RespawnTime[i] -= 333;

                    if (RespawnTime[i] <= 0)
                    {
                        RespawnTime[i] = 0;
                        Players[PlayerToSpawn[i]] = World.CreateEntity("Player", (PlayerIndex)i);
                        Entity e = Players[PlayerToSpawn[i]];
                        e.GetComponent<Health>().OnDeath += x =>
                        {
                            int id = int.Parse(e.Tag.Replace("P", "")) - 1;
                            RespawnTime[id] = 3000;
                        };
                        Players[PlayerToSpawn[i]].Refresh();
                    }
                }
            }

            #endregion

            updateTimes();
            ++timesCalled;
        }

        #endregion
        /*********/

        #region Helpers

        private void updateTimes()
        {
            elapsedSeconds += secPerCall;
            elapsedMinutes += minPerCall;

            if (!(world as SpaceWorld).Tutorial)
            {
                if (SpawnState == SpawnState.Wave || SpawnState == SpawnState.Surge)
                    intervalSeconds += secPerCall;

                if (SpawnState == SpawnState.Surge)
                    ElapsedSurge += 333;

                if (!String.IsNullOrEmpty(HUDRenderSystem.SurgeWarning))
                {
                    elapsedWarning += 333;
                    if (elapsedWarning >= warningTime)
                    {
                        HUDRenderSystem.SurgeWarning = "";
                        elapsedWarning = 0;
                    }
                }

                if (SpawnState != SpawnState.Boss && SpawnState != SpawnState.Endless && SpawnState != SpawnState.Victory)
                {
                    if (elapsedSeconds >= StateDurations[(int)SpawnState])
                    {
                        elapsedSeconds = 0f;
                        elapsedMinutes = 0f;

                        if (SpawnState == SpawnState.Surge)
                        {
                            setCategory(SongType.Loop);
                        }

                        SpawnState = states.Dequeue();
                        init = false;
                    }
                }
                else if (SpawnState == SpawnState.Boss)
                {
                    if (Boss != null && (!Boss.HasComponent<Health>() || !Boss.GetComponent<Health>().IsAlive))
                    {
                        elapsedSeconds = 0f;
                        elapsedMinutes = 0f;

                        SpawnState = states.Dequeue();
                        init = false;
                        setCategory(SongType.Loop);
                    }
                }
            }
        }

        private void spawnWave()
        {
            waves++;

            HUDRenderSystem.SurgeWarning = "Wave " + waves.ToString();
        }

        #endregion

        #region Spawn Helpers

        private void spawnDestroyer()
        {
            world.CreateEntity(DestroyerTemplate).Refresh();
        }

        private void spawnDestroyers(int i)
        {
            for (; i > 0; --i)
            {
                spawnDestroyer();
            }
        }

        private void spawnCrystal(int index)
        {
            Vector2 poss = Base.GetComponent<ITransform>().Position;
            world.CreateEntity("Crystal", poss, Color.Gray, 3, Players[index], true);
        }

        public void SpawnCrystal(Vector2 position, Color color, int amount, int index)
        {
            world.CreateEntity("Crystal", position, color, amount, Players[index], true);
        }

        private void spawnBoss()
        {
            Boss = World.CreateEntity(BossTemplate, Base.GetComponent<Body>());
            Boss.GetComponent<Health>().OnDeath += new Action<Entity>(BossDeath);
            Boss.Refresh();
            setCategory(SongType.Boss);
            HUDRenderSystem.SurgeWarning = "Boss Approaching";
        }

        private void BossDeath(Entity e)
        {
            elapsedSeconds = 0f;
            elapsedMinutes = 0f;
            SpawnState = states.Dequeue();
            init = false;
            setCategory(SongType.Loop);
        }

        private void spawnSurge()
        {
            HUDRenderSystem.SurgeWarning = "Surge";

            for (int i = 0; i < Players.Length; ++i)
            {
                spawnCrystal(i);
            }

            foreach (Entity e in TurretTemplate.Turrets)
            {
                Inventory inv = e.GetComponent<Inventory>();
                inv.CurrentGun.PowerUp((int)StateDurations[(int)SpawnState.Surge] * 1000, 3);
            }

            setCategory(SongType.Surge);
        }

        private void spawnHunter()
        {
            int type = r.Next(0, 3);
            int target = r.Next(0, Players.Length);
            Body b = Players[target].GetComponent<Body>();
            if (string.IsNullOrEmpty(HunterSprite))
                World.CreateEntity(HunterTemplate, type, b).Refresh();
            else
                World.CreateEntity(HunterTemplate, type, b, HunterSprite).Refresh();
        }

        private void spawnGunner()
        {
            int type = r.Next(5);
            if (string.IsNullOrEmpty(GunnerSprite))
                World.CreateEntity(GunnerTemplate, type).Refresh();
            else
                World.CreateEntity(GunnerTemplate, type, GunnerSprite).Refresh();
        }

        private void spawnThug()
        {
            int type = r.Next(6);
            if (string.IsNullOrEmpty(ThugSprite))
                World.CreateEntity(ThugTemplate, type, Base.GetComponent<Body>()).Refresh();
            else
                World.CreateEntity(ThugTemplate, type, Base.GetComponent<Body>(), ThugSprite).Refresh();
        }

        private void spawnMook()
        {
            int type = r.Next(7);
            if (string.IsNullOrEmpty(MookSprite))
                World.CreateEntity(MookTemplate, type, Base.GetComponent<Body>()).Refresh();
            else
                World.CreateEntity(MookTemplate, type, Base.GetComponent<Body>(), MookSprite).Refresh();
        }

        private void spawnMooks(int i)
        {
            for (; i > 0; --i)
            {
                spawnMook();
            }
        }

        private void spawnThugs(int i)
        {
            for (; i > 0; --i)
            {
                spawnThug();
            }
        }

        private void spawnGunners(int i)
        {
            for (; i > 0; --i)
            {
                spawnGunner();
            }
        }

        private void spawnHunters(int i)
        {
            for (; i > 0; --i)
            {
                spawnHunter();
            }
        }

        #endregion

        #region Math Helpers

        /// <summary>
        /// Converts a float to an int, resolving the decimal d to become another whole based on d% chance.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int floatToInt(float f)
        {
            float i = (int)f;

            float c = r.Next(0, 101)/100f;
            if (c < f - i)
                ++i;

           

            return (int)i;
        }

        /// <summary>
        /// Converts a double to an int, resolving the decimal d to become another whole based on d% chance.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int doubleToInt(double d)
        {
            double i = (int)d;
            float c = r.Next(0, 101) / 100f;
            if (c < d - i)
                ++i;
            return (int)i;
        }

        public float ClampInverse(float value, float min, float max)
        {
            if (value > min && value < max)
            {
                if (MathHelper.Distance(min, value) < MathHelper.Distance(max, value))
                    return min;
                else
                    return max;
            }
            else
                return value;
        }

        #endregion

        #region Tutorial Helpers

        private void makeDialog(ImageFont font, string message)
        {
            Vector2 pos = new Vector2(ScreenHelper.Viewport.Width / 2 - font.MeasureString(message).X / 2, ScreenHelper.TitleSafeArea.Y);
            CurrentDialog = new MessageDialog(font, pos, message, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.3));
            CurrentDialog.Enabled = true;
        }

        #endregion

        #region Music Helpers

        void updateMusic()
        {
            switch (musicState)
            {
                case MusicState.TransitionOff:
                    MusicManager.Volume -= 0.1f;
                    if (MusicManager.Volume <= 0)
                    {
                        MusicManager.Volume = 0;
                        musicState = MusicState.TransitionOn;
                        MusicManager.PlaySong(nextSong);
                    }
                    break;
                case MusicState.TransitionOn:
                    MusicManager.Volume += 0.1f;
                    if (MusicManager.Volume >= tempVolume)
                    {
                        MusicManager.Volume = tempVolume;
                        musicState = MusicState.Transitioned;
                    }
                    break;
            }
        }

        private void setCategory(SongType type)
        {
            int song = 0;
            string key = "";

            switch (type)
            {
                case SongType.Loop:
                    song = r.Next(0, loopSongs.Length);
                    key = loopSongs[song];
                    MusicManager.IsRepeating = true;
                    break;

                case SongType.Boss:
                    song = r.Next(0, bossSongs.Length);
                    key = bossSongs[song];
                    MusicManager.IsRepeating = false;
                    break;

                case SongType.Surge:
                    song = r.Next(0, surgeSongs.Length);
                    key = surgeSongs[song];
                    MusicManager.IsRepeating = false;
                    break;
            }

            changeSong(key);
        }

        void changeSong(string songKey)
        {
            if (MusicManager.IsPlaying)
            {
                tempVolume = MusicManager.Volume;
                musicState = MusicState.TransitionOff;
                nextSong = songKey;
            }
            else
            {
                tempVolume = MusicManager.Volume;
                MusicManager.Volume = 0;
                musicState = MusicState.TransitionOn;
                MusicManager.PlaySong(songKey);
            }
        }

        #endregion

        #region Crystal Color Gen

        //static Random r = new Random();
        public static Color CrystalColor()
        {
            Color crystalColor = Color.Red;
            int colorchance = r.Next(100);
            if (colorchance > 22)
            {
                crystalColor = Color.Yellow;
            }
            if (colorchance > 44)
            {
                crystalColor = Color.Blue;
            }
            if (colorchance > 66)
            {
                crystalColor = Color.Green;
            }
            if (colorchance > 90)
            {
                crystalColor = Color.Gray;
            }

            return crystalColor;
        }

        #endregion
    }
}