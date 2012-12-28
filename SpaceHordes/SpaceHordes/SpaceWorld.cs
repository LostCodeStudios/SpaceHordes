using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Entities.Systems;
using GameLibrary.Entities;
using SpaceHordes.Entities.Templates;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Entities.Components;
using SpaceHordes.Entities.Systems;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes
{
    /// <summary>
    /// Space Hordes world implementation
    /// </summary>
    class SpaceWorld : World
    {
        public SpaceWorld(Camera camera,SpriteBatch spriteBatch,  SpriteSheet spriteSheet)
            : base(camera, spriteBatch)
        {
            this._spriteSheet = spriteSheet;
        }

        #region Functioning Loop
        /// <summary>
        /// Intializes the SpaceWorld
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="args"></param>
        public override void Initialize()
        {
            #region Systems
            gunSystem = this.SystemManager.SetSystem(new GunSystem(), ExecutionType.Update);
            bulletRemovalSystem = this.SystemManager.SetSystem(new BulletRemovalSystem(this.Camera), ExecutionType.Update);
            #endregion
            base.Initialize();
        }

        /// <summary>
        /// Loads the content of the Space World
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="args">{0-4} Player indices in use.</param>
        public override void LoadContent(ContentManager Content, params object[] args)
        {
            #region Templates
            //Templates
            this.SetEntityTemplate("Player", new PlayerTemplate(this, _spriteSheet));
            this.SetEntityTemplate("Base", new BaseTemplate(this, _spriteSheet));
            this.SetEntityTemplate("Enemy", new EnemyTemplate(this, _spriteSheet));
            this.SetEntityTemplate("TestBullet", new BulletTemplate(
                new Sprite(_spriteSheet.Texture, _spriteSheet.Animations["redshot1"][0]),
                new Velocity(new Vector2(1), 1f))
                );
            #endregion

            #region Entities
            //Set up player(s)
            if (args != null && args.Length > 0 && args[0] != null) //IF MULTIPLAYER
                for (int i = 0; i < args.Length && i < 4; i++)
                {
                    Player = this.CreateEntity("Player", (PlayerIndex)i);
                    Player.Refresh();
                }
            else //IF SINGLEPLAYER
            {
                Player = this.CreateEntity("Player", (PlayerIndex.One));
                Player.Refresh();
            }

            //Set up base.
            Entity Base = this.CreateEntity("Base");
            Base.Refresh();

            Enemies = new List<Entity>();

            for (int i = 0; i < EnemyCount; i++)
            {
                Enemies.Add(this.CreateEntity("Enemy"));
                Enemies[i].Refresh();
            }


            #endregion

            //Camera
            //Camera.TrackingBody = Player.GetComponent<Body>("Body");
            Camera.MaxPosition = new Vector2(100, 100);
            Camera.MinPosition = new Vector2(-100, -100);

#if DEBUG   //Debug render system
            this._DebugRenderSystem.LoadContent(_SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", this.Player.GetComponent<Body>()),
                 new KeyValuePair<string, object>("EntitySystem Time:\n", this.SystemManager));
#endif
            base.LoadContent(Content, args);
        }

        /// <summary>
        /// Updates the SpaceWorld.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Body pBody = Player.GetComponent<Body>();


            for (int i = 0; i < EnemyCount; i++)
            {
                if((int)gameTime.TotalGameTime.Milliseconds % 500 == 0)
                    Enemies[i].GetComponent<Gun>().BulletsToFire++;

                if (i == 0)
                {
                    Enemies[i].GetComponent<Body>().Position += 
                        ((pBody.Position - Enemies[i].GetComponent<Body>().Position) * new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds + 0.03f));
                    Enemies[i].GetComponent<Body>().Rotation = pBody.Rotation - 0.01f;
                }
                else
                {
                    Enemies[i].GetComponent<Body>().Position +=
                        ((Enemies[i-1].GetComponent<Body>().Position - Enemies[i].GetComponent<Body>().Position) * new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds + 0.03f));
                    Enemies[i].GetComponent<Body>().Rotation = Enemies[i-1].GetComponent<Body>().Rotation - 0.01f;
                }
            }



            base.Update(gameTime);
        }

        #endregion

        #region Fields

        #region Entity
        //Systems
        GunSystem gunSystem;
        BulletRemovalSystem bulletRemovalSystem;

        //Entities for safe keeping
        public Entity Player;
        public List<Entity> Enemies;
        #endregion

        private SpriteSheet _spriteSheet;
        #endregion

        public const int EnemyCount = 60;
    }
}
