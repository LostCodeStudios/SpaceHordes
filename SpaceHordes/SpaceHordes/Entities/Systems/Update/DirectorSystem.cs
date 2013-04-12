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

    public class DirectorSystem : IntervalEntitySystem
    {
        #region Fields
        private Random r = new Random();

        private Entity Base;
        private Entity Boss;
        private Entity[] Players;
        public int[] RespawnTime;
        int[] PlayerToSpawn;

        private double difficulty = 0;
        public bool Surge = false;
        int elapsedWarning = 0;
        public static int ElapsedSurge = 0;
        int warningTime = 3000;
        public static int SurgeTime = 20000;
        float nextSeconds = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        private float elapsedSeconds;
        private float elapsedMinutes;
        private int timesCalled = 0;
        private float intervalSeconds = 0f;

        private int lastBoss = 0;

        #endregion

        #region Song Tags

        string[] loopSongs = new string[]
        {
            "SpaceLoop",
        };

        string[] bossSongs = new string[]
        {
            "Heartbeat",
            "Ropocalypse"
        };

        string[] surgeSongs = new string[]
        {
            "Unending",
            "Cephelopod",
            "KickShock"
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

        string[] tutorialMessages = new string[]
        {
            "WELCOME TO SPACE HORDES.",
            "MOVE WITH THE LEFT ANALOG STICK.",
            "AIM WITH RIGHT STICK AND SHOOT WITH RIGHT TRIGGER.",
            "AN ENEMY SHIP IS COMING. YOU CAN SEE IT ON THE RADAR.",
            "SHOOT IT BEFORE IT HITS YOUR BASE.",
            "ENEMIES DROP CRYSTALS WHICH ARE USED AS AMMO.",
            "GRAY CRYSTALS ARE POWERUPS.",
            "",
            "YOU HAVE 4 GUN TYPES: RED GREEN BLUE AND WHITE.",
            "RED BULLETS ARE FASTER.",
            "BLUE BULLETS FREEZE ENEMIES.",
            "GREEN BULLETS DO POISON DAMAGE.",
            "WHITE BULLETS USE NO AMMO.",
            "SELECT GUN TYPES BY PRESSING THE BUTTON WITH THEIR COLOR: A B OR X.",
            "RESELECT WHITE BULLETS BY PRESSING THE BUTTON OF THE GUN YOU HAVE CURRENTLY SELECTED.",
            "HERE ARE SOME TOUGHER ENEMIES. TRY OUT YOUR DIFFERENT GUNS.",
            "",
            "",
            "",
            "",
            "YOU CAN BUILD DEFENSES TO PROTECT YOUR BASE.",
            "PRESS Y TO ENTER AND LEAVE BUILD MODE.",
            "YOU CAN BUILD BARRIERS MINES AND TURRETS.",
            "PRESS X TO BUILD A BARRIER.",
            "PRESS A TO BUILD A TURRET.",
            "PRESS B TO BUILD A MINE.",
            "BUILDING DEFENSES USES UP YELLOW CRYSTALS.",
            "YOU CAN SEE HOW MANY YOU HAVE ABOVE YOUR SHIP.",
            "THE COSTS OF DEFENSES ARE SHOWN ON YOUR STATUS BAR WHILE IN BUILD MODE.",
            "BUILD SOME DEFENSES TO SEE HOW THEY WORK IN ACTION.",
            "",
            "",
            "",
            "",
            "GUNNERS WILL ATTEMPT TO DESTROY YOUR DEFENSES. THEIR BULLETS WILL NOT HURT YOU.",
            "HUNTERS WILL FLY AFTER YOU.",
            "",
            "OCCASIONALLY YOU WILL FACE MASSIVE SURGES OF ENEMIES.",
            "IF YOU ARE ABOUT TO LOSE YOU HAVE ONE MORE OPTION.",
            "PRESS LEFT TRIGGER TO USE A POWERFUL EMERGENCY ATTACK.",
            "THIS WILL SPEND ALL THE CRYSTALS YOU HAVE SELECTED.",
            "NOW YOU HAVE LEARNED ALMOST EVERYTHING THERE IS TO KNOW.",
            "SEE IF YOU CAN PLAY ON YOUR OWN."
        };

        int message = 0;
        #endregion

        #region Initialization

        public DirectorSystem()
            : base(333)
        {
        }

        public void LoadContent(Entity Base, Entity[] Players)
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
        }

        #endregion

        /*********/
        #region Processing

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            SpaceWorld w = world as SpaceWorld;
            difficulty = elapsedMinutes * w.Players;

            ++timesCalled;
            elapsedSeconds += .333f;
            intervalSeconds += .333f;
            elapsedMinutes += .333f / 60f;

            if (Surge)
            {
                updateSurge();
            }

            #region MUSIC

            if (timesCalled == 1)
            {
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
                    ScoreSystem.GivePoints(10 * (int)difficulty);
                    intervalSeconds = 0;
                }

                #endregion Scoring

                #region Spawning

                int mooksToSpawn = doubleToInt(difficulty / 7) * MookSpawnRate;
                int thugsToSpawn = doubleToInt(difficulty / 50) * ThugSpawnRate;
                int gunnersToSpawn = doubleToInt(difficulty / 100) * GunnerSpawnRate;
                int huntersToSpawn = doubleToInt(difficulty / 75) * HunterSpawnRate;
                int destroyersToSpawn = doubleToInt(difficulty / 300) * DestroyerSpawnRate;
                spawnMooks(mooksToSpawn);
                spawnThugs(thugsToSpawn);
                spawnGunners(gunnersToSpawn);
                spawnHunters(huntersToSpawn);
                spawnDestroyers(destroyersToSpawn);

                if ((int)(elapsedMinutes) > lastBoss)
                {

                    int chance = r.Next(1, 100);

                    if (chance > 66)
                    {
                        //SURGE
                        spawnSurge();
                    }
                    else
                    {
                        //Boss.
                        spawnBoss();
                    }
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
                    Base.GetComponent<Health>().SetHealth(Base, 10);
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
        }

        #endregion
        /*********/

        #region Helpers

        private void updateSurge()
        {
            elapsedWarning += 333;
            ElapsedSurge += 333;

            if (elapsedWarning >= warningTime)
            {
                HUDRenderSystem.SurgeWarning = false;
                elapsedWarning = 0;
            }

            if (ElapsedSurge >= SurgeTime)
            {
                Surge = false;
                ElapsedSurge = 0;
                SpawnRate = 1;
                setCategory(SongType.Loop);
            }
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

        private void spawnBoss()
        {
            int tier = (int)MathHelper.Clamp(lastBoss, 1, 3);
            Boss = World.CreateEntity(BossTemplate, tier, Base.GetComponent<Body>());
            Boss.GetComponent<Health>().OnDeath += new Action<Entity>(BossDeath);
            Boss.Refresh();
            ++lastBoss;
            setCategory(SongType.Boss);
        }

        private void BossDeath(Entity e)
        {
            setCategory(SongType.Loop);
        }

        private void spawnSurge()
        {
            Surge = true;
            HUDRenderSystem.SurgeWarning = true;

            for (int i = 0; i < Players.Length; ++i)
            {
                spawnCrystal(i);
            }

            SpawnRate = 2;

            foreach (Entity e in TurretTemplate.Turrets)
            {
                Inventory inv = e.GetComponent<Inventory>();
                inv.CurrentGun.PowerUp(SurgeTime, 3);
            }


            ++lastBoss;
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
            Vector2 pos = new Vector2(ScreenHelper.Viewport.Width / 2 - font.MeasureString(message).X / 2, 0);
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
    }
}