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
    public class DirectorSystem : IntervalEntitySystem
    {
        private Random r = new Random();

        private Entity Base;
        private Entity Boss;
        private Entity[] Players;
        public int[] RespawnTime;
        int[] PlayerToSpawn;

        private double difficulty = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        private float elapsedSeconds;
        private float elapsedMinutes;

        private int mooksToSpawn = 0;
        private int thugsToSpawn = 0;
        private int gunnersToSpawn = 0;
        private int huntersToSpawn = 0;
        private int destroyersToSpawn = 0;

        public  int MookSpawnRate = 0;
        public  int ThugSpawnRate = 0;
        public  int GunnerSpawnRate = 0;
        public  int HunterSpawnRate = 0;
        public  int DestroyerSpawnRate = 0;

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

        private int timesCalled = 0;
        private float intervalSeconds = 0f;

        private int lastBoss = 1; //private int lastBoss = 0; //DEBUG PURPOSES
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
            "GUNNERS WILL ATTEMPT TO DESTROY YOUR DEFENSES.",
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

        public bool Surge = false;
        int elapsedWarning = 0;
        public static int ElapsedSurge = 0;
        int warningTime = 3000;
        public static int SurgeTime = 20000;
        float nextSeconds = 0;

        public DirectorSystem()
            : base(333)
        {
            timesCalled = 0;
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

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            ++timesCalled;
            elapsedSeconds += .333f;
            intervalSeconds += .333f;
            elapsedMinutes += .333f / 60f;

            SpaceWorld w = world as SpaceWorld;
            difficulty = elapsedMinutes * w.Players;

            if (Surge)
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
                    SpawnRate = 0;
                }
            }

            #region REALGAME

            if (!(World as SpaceWorld).Tutorial)
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

                //Every 5/3 seconds spawn

                if (timesCalled % 5 == 0)
                {
                    mooksToSpawn = (MookSpawnRate != -1) ? ((MookSpawnRate == 0) ? (int)difficulty : (int)difficulty * MookSpawnRate) : 0;
                    if (ThugSpawnRate != -1)
                    {
                        thugsToSpawn = (ThugSpawnRate == 0) ?
                            ((r.Next(1, 100) * (int)difficulty > 90) ? 1 : 0) :
                            ((r.Next(1, 100) * ThugSpawnRate) > 90 ?
                                (int)difficulty * ThugSpawnRate : 0);
                    }

                    if (GunnerSpawnRate != -1)
                    {
                        gunnersToSpawn = (GunnerSpawnRate == 0) ?
                            ((r.Next(1, 100) * (int)difficulty > 40) ? 1 : 0) :
                            ((r.Next(1, 100) * GunnerSpawnRate) > 90 ?
                                (int)difficulty * GunnerSpawnRate : 0);
                    }

                    if (HunterSpawnRate != -1)
                    {
                        huntersToSpawn = (HunterSpawnRate == 0) ?
                            ((r.Next(1, 100) * (int)difficulty > 90) ? 1 : 0) :
                            ((r.Next(1, 100) * HunterSpawnRate) > 90 ?
                                (int)difficulty * HunterSpawnRate : 0);
                    }
                    //huntersToSpawn = (HunterSpawnRate != -1) ? ((HunterSpawnRate == 0) ? (int)(difficulty) : difficulty * (int)(HunterSpawnRate)) : 0;
                    destroyersToSpawn = (DestroyerSpawnRate != -1) ? ((DestroyerSpawnRate != 0) ? (int)(difficulty / 30) : (int)difficulty * (int)(DestroyerSpawnRate / 30)) : 0;

                    for (int i = 0; i < mooksToSpawn; ++i)
                    {
                        spawnMook();
                    }

                    for (int i = 0; i < thugsToSpawn; ++i)
                    {
                        spawnThug();
                    }

                    for (int i = 0; i < gunnersToSpawn; ++i)
                    {
                        spawnGunner();
                    }

                    for (int i = 0; i < huntersToSpawn; ++i)
                    {
                        spawnHunter();
                    }
                }

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
                ImageFont font = (World as SpaceWorld)._Font;

                if (message < tutorialMessages.Length)
                {
                    if (CurrentDialog == null)
                    {
                        makeDialog(font, tutorialMessages[message++]);
                        nextSeconds = 1000f;
                    }
                    if (CurrentDialog.Complete())
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

                        if (message == 27 || message == 28 || message == 29)
                        {
                            spawnMook();
                            spawnMook();
                            spawnMook();
                        }

                        if (message == 30)
                            spawnGunner();

                        if (message == 31)
                            spawnHunter();

                        if (message < tutorialMessages.Length)
                        {
                            makeDialog(font, tutorialMessages[message++]); //Move to the next message 1.5 seconds after the last one finished.
                            nextSeconds = 1000f;
                        }
                    }
                }
                else if (CurrentDialog.Complete() && nextSeconds < elapsedSeconds)
                {
                    (world as SpaceWorld).GameScreen.tutorial = false;
                    (world as SpaceWorld).Tutorial = false;
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

        private void spawnBoss()
        {
            int tier = Math.Min((int)lastBoss, 3);
            Boss = World.CreateEntity(BossTemplate, tier, Base.GetComponent<Body>());
            Boss.Refresh();
            ++lastBoss;
        }

        private void spawnSurge()
        {
            Surge = true;
            HUDRenderSystem.SurgeWarning = true;

            for (int i = 0; i < Players.Length; ++i)
            {
                SpawnCrystalFor(i);
            }

            SpawnRate = 2;

            foreach (Entity e in TurretTemplate.Turrets)
            {
                Inventory inv = e.GetComponent<Inventory>();
                inv.CurrentGun.PowerUp(SurgeTime, 3);
            }


            ++lastBoss;
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
            int type = r.Next(5);
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

        private void SpawnCrystalFor(int index)
        {
            Vector2 poss = Base.GetComponent<ITransform>().Position;
            world.CreateEntity("Crystal", poss, Color.Gray, 3, Players[index], true);
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

        private void makeDialog(ImageFont font, string message)
        {
            Vector2 pos = new Vector2(ScreenHelper.Viewport.Width / 2 - font.MeasureString(message).X / 2, 0);
            CurrentDialog = new MessageDialog(font, pos, message, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.3));
            CurrentDialog.Enabled = true;
        }
    }
}