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
using SpaceHordes.Entities.Templates.Enemies;

namespace SpaceHordes
{
    /// <summary>
    /// Space Hordes world implementation
    /// </summary>
    public class SpaceWorld : World
    {
        public SpaceWorld(Game game,SpriteSheet spriteSheet)
            : base(game, new Vector2(0,0))
        {
            this._spriteSheet = spriteSheet;
            this._font = new ImageFont();
        }

        #region Initialization
        /// <summary>
        /// Initializes the spaceworld
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Loads the content of the Space World
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="args"></param>
        public override void LoadContent(ContentManager Content, params object[] args)
        {
            base.LoadContent(Content, args);
           // healthRenderSystem.LoadContent(Content.Load<SpriteFont>("Fonts/gamefont"));
            _font.LoadContent(Content, "Textures/gamefont");
            hudRenderSystem.LoadContent(_font, Content.Load<Texture2D>("Textures/HUD"));
#if DEBUG   //Debug render system
            this._DebugSystem.LoadContent(SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", this.Player.GetComponent<Body>()),
                 new KeyValuePair<string, object>("Base", this.Base.GetComponent<Health>()),
                 new KeyValuePair<string, object>("EntitySystem Time:\n", this.SystemManager));
#endif
        }

        /// <summary>
        /// Builds all of the entity systems in space world.
        /// </summary>
        protected override void BuildSystems()
        {
            //Update Systems
            gunSystem = this.SystemManager.SetSystem(new GunSystem(), ExecutionType.Update);
            bulletRemovalSystem = this.SystemManager.SetSystem(new BulletRemovalSystem(this.Camera), ExecutionType.Update);
            bulletCollisionSystem = this.SystemManager.SetSystem(new BulletCollisionSystem(), ExecutionType.Update);
            healthSystem = this.SystemManager.SetSystem<HealthSystem>(new HealthSystem(), ExecutionType.Update);
            enemySpawnSystem = this.SystemManager.SetSystem(new DirectorSystem(), ExecutionType.Update);
            slowSystem = this.SystemManager.SetSystem(new SlowSystem(), ExecutionType.Update);
            enemyMovementSystem = this.SystemManager.SetSystem(new AISystem(), ExecutionType.Update);
            playerControlSystem = this.SystemManager.SetSystem(new PlayerControlSystem(5f), ExecutionType.Update);

            //Draw Systems
            //healthRenderSystem = this.SystemManager.SetSystem<HealthRenderSystem>(new HealthRenderSystem(this.SpriteBatch), ExecutionType.Draw);
            hudRenderSystem = this.SystemManager.SetSystem<HUDRenderSystem>(new HUDRenderSystem(), ExecutionType.Draw, 1);
            starFieldRenderSystem = this.SystemManager.SetSystem<StarFieldRenderSystem>(new StarFieldRenderSystem(SpriteBatch), ExecutionType.Draw);
            base.BuildSystems();
        }

        /// <summary>
        /// Builds all of the templates in the space world.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="args"></param>
        protected override void BuildTemplates(ContentManager Content, params object[] args)
        {
            this.SetEntityTemplate("Player", new PlayerTemplate(this, _spriteSheet));
            this.SetEntityTemplate("Base", new BaseTemplate(this, _spriteSheet)); //TEST
            this.SetEntityTemplate("Mook", new MookTemplate(_spriteSheet, this));
            //Test bullet
            //this.SetEntityTemplate("TestBullet", new BulletTemplate(
            //    new Sprite(_spriteSheet.Texture, _spriteSheet.Animations["redshot1"][0]),
            //    new Velocity(new Vector2(5), 0f))
            //    );

            //Bullets
            this.SetEntityTemplate("FrostBullet", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot1"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(12, "Enemies", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("Star", new StarTemplate(_spriteSheet));
            this.SetEntityGroupTemplate("StarField", new StarFieldTemplate());
            base.BuildTemplates(Content, args);
        }

        /// <summary>
        /// Builds all of the entities in the SpaceWorld.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="args">{0-4} Player indices in use.</param>
        protected override void BuildEntities(ContentManager Content, params object[] args)
        {
            CreateEntityGroup("StarField", "Stars");
            
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
            Base = this.CreateEntity("Base");
            Base.Refresh();
            enemySpawnSystem.LoadContent(Base);

#if DEBUG
            Camera.TrackingBody = Player.GetComponent<Body>();
#endif
        }

        #endregion

        #region Functioning Loop

        #endregion

        #region Fields

        #region Entity
        //Update Systems
        GunSystem gunSystem;
        BulletRemovalSystem bulletRemovalSystem;
        BulletCollisionSystem bulletCollisionSystem;
        //HealthRenderSystem healthRenderSystem;
        HealthSystem healthSystem;
        DirectorSystem enemySpawnSystem;
        AISystem enemyMovementSystem;
        SlowSystem slowSystem;
        PlayerControlSystem playerControlSystem;

        //Draw Systems
        HealthRenderSystem healthRenderSystem;
        HUDRenderSystem hudRenderSystem;
        StarFieldRenderSystem starFieldRenderSystem;

        //Entities for safe keeping
        public Entity Player;
        public Entity Base;
        #endregion

        SpriteSheet _spriteSheet;
        Texture2D _hud;
        ImageFont _font;

        #endregion
    }
}
