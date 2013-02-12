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

        #region Content

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
            radarRenderSystem.LoadContent(Content);
        }

        #endregion

        #region Systems

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
            crystalSystem = this.SystemManager.SetSystem(new CrystalCollisionSystem(), ExecutionType.Update);

            //Draw Systems
            //healthRenderSystem = this.SystemManager.SetSystem<HealthRenderSystem>(new HealthRenderSystem(this.SpriteBatch), ExecutionType.Draw);
            hudRenderSystem = this.SystemManager.SetSystem<HUDRenderSystem>(new HUDRenderSystem(), ExecutionType.Draw, 1);
            starFieldRenderSystem = this.SystemManager.SetSystem<StarFieldRenderSystem>(new StarFieldRenderSystem(SpriteBatch), ExecutionType.Draw);
            radarRenderSystem = this.SystemManager.SetSystem<RadarRenderSystem>(new RadarRenderSystem(hudRenderSystem.radarLocation,
                new Rectangle(-ScreenHelper.Viewport.Width*2, -ScreenHelper.Viewport.Height*2,
                    ScreenHelper.Viewport.Width*2, ScreenHelper.Viewport.Height*2))
                    , ExecutionType.Draw, 2);
            base.BuildSystems();
        }

        #endregion

        #region Templates

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

            #region Crystals

            this.SetEntityTemplate("RedCrystal1", new CrystalTemplate(
                new Sprite(_spriteSheet, "redcrystal"),
                new Velocity(new Vector2(5), 0f),
                new Crystal(Color.Red, 1, "Players")));

            #endregion

            #region Player Bullets

            this.SetEntityTemplate("BlueBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot1"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(1, "Enemies", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BlueBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot2"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(2, "Enemies", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BlueBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot3"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(3, "Enemies", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));

            this.SetEntityTemplate("GreenBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot1"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(2, "Enemies", null
                    )));

            this.SetEntityTemplate("GreenBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot2"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(4, "Enemies", null
                    )));

            this.SetEntityTemplate("GreenBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot3"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(6, "Enemies", null
                    )));

            this.SetEntityTemplate("RedBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot1"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(4, "Enemies", null
                    )));

            this.SetEntityTemplate("RedBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot2"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(8, "Enemies", null
                    )));

            this.SetEntityTemplate("RedBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot3"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(12, "Enemies", null
                    )));

            this.SetEntityTemplate("WhiteBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot1"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(1, "Enemies", null
                    )));

            this.SetEntityTemplate("WhiteBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot2"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(2, "Enemies", null
                    )));

            this.SetEntityTemplate("WhiteBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot3"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(3, "Enemies", null
                    )));

            #endregion

            #region Enemy Bullets

            this.SetEntityTemplate("BBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot1"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(1, "Players", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot2"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(2, "Players", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot3"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(3, "Players", e => e.AddComponent<Slow>(new Slow(1f, 5.0f, new Vector2(4), 0.0f))
                    )));

            this.SetEntityTemplate("GBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot1"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(2, "Players", null
                    )));

            this.SetEntityTemplate("GBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot2"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(4, "Players", null
                    )));

            this.SetEntityTemplate("GBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot3"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(6, "Players", null
                    )));

            this.SetEntityTemplate("RBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot1"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(4, "Players", null
                    )));

            this.SetEntityTemplate("RBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot2"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(8, "Players", null
                    )));

            this.SetEntityTemplate("RBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot3"),
                new Velocity(new Vector2(6), 0f),
                new Bullet(12, "Players", null
                    )));

            this.SetEntityTemplate("WBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot1"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(1, "Players", null
                    )));

            this.SetEntityTemplate("WBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot2"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(2, "Players", null
                    )));

            this.SetEntityTemplate("WBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot3"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(3, "Players", null
                    )));

            #endregion

            this.SetEntityTemplate("Star", new StarTemplate(_spriteSheet));
            this.SetEntityGroupTemplate("StarField", new StarFieldTemplate());
            base.BuildTemplates(Content, args);
        }

        #endregion

        #region Entities

        /// <summary>
        /// Builds all of the entities in the SpaceWorld.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="args">{0-4} Player indices in use.</param>
        protected override void BuildEntities(ContentManager Content, params object[] args)
        {
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

            CreateEntityGroup("StarField", "Stars");

            //Set up base.
            Base = this.CreateEntity("Base");
            Base.Refresh();
            enemySpawnSystem.LoadContent(Base);
            this.CreateEntity("RedCrystal1", new Transform(Vector2.UnitX, 0f));
#if DEBUG
            //Camera.TrackingBody = Player.GetComponent<Body>();
#endif
        }

        #endregion

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
        CrystalCollisionSystem crystalSystem;

        //Draw Systems
        HealthRenderSystem healthRenderSystem;
        HUDRenderSystem hudRenderSystem;
        StarFieldRenderSystem starFieldRenderSystem;
        RadarRenderSystem radarRenderSystem;

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
