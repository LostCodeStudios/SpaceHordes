using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using SpaceHordes.Entities.Templates;
using SpaceHordes.Entities.Templates.Enemies;
using SpaceHordes.Entities.Templates.Objects;
using System.Collections.Generic;
using GameLibrary.Entities.Components.Render;

namespace SpaceHordes
{
    /// <summary>
    /// Space Hordes world implementation
    /// </summary>
    public class SpaceWorld : World
    {
        public SpaceWorld(Game game, SpriteSheet spriteSheet)
            : base(game, new Vector2(0, 0))
        {
            this._spriteSheet = spriteSheet;
            this._font = new ImageFont();
        }

        #region Initialization

        #region Content/Init

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
            scoreSystem.LoadContent(Base);
#if DEBUG   //Debug render system
            this._DebugSystem.LoadContent(SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", this.Player.GetComponent<Body>()),
                 new KeyValuePair<string, object>("Base", this.Base.GetComponent<Health>()),
                 new KeyValuePair<string, object>("EntitySystem Time:\n", this.SystemManager));

#endif
            radarRenderSystem.LoadContent(Content);
        }

        #endregion Content/Init

        #region Systems

        /// <summary>
        /// Builds all of the entity systems in space world.
        /// </summary>
        protected override void BuildSystems()
        {
            //Update Systems
            gunSystem = this.SystemManager.SetSystem(new GunSystem(), ExecutionType.Update);
            damageSystem = this.SystemManager.SetSystem(new DamageSystem(), ExecutionType.Update);
            bulletRemovalSystem = this.SystemManager.SetSystem(new BulletRemovalSystem(this.Camera), ExecutionType.Update);
            bulletCollisionSystem = this.SystemManager.SetSystem(new BulletCollisionSystem(), ExecutionType.Update);
            healthSystem = this.SystemManager.SetSystem<HealthSystem>(new HealthSystem(), ExecutionType.Update);
            enemySpawnSystem = this.SystemManager.SetSystem(new DirectorSystem(), ExecutionType.Update);
            slowSystem = this.SystemManager.SetSystem(new SlowSystem(), ExecutionType.Update);
            enemyMovementSystem = this.SystemManager.SetSystem(new AISystem(), ExecutionType.Update);
            playerControlSystem = this.SystemManager.SetSystem(new PlayerControlSystem(5f), ExecutionType.Update);
            explosionSystem = this.SystemManager.SetSystem(new ExplosionSystem(), ExecutionType.Update);
            this.SystemManager.SetSystem(new CrystalMovementSystem(), ExecutionType.Update);
            this.SystemManager.SetSystem(new BaseAnimationSystem(0.10f, 10), ExecutionType.Update);
            scoreSystem = this.SystemManager.SetSystem(new ScoreSystem(), ExecutionType.Update);

            //Draw Systems
            healthRenderSystem = this.SystemManager.SetSystem<HealthRenderSystem>(new HealthRenderSystem(this.SpriteBatch), ExecutionType.Draw, 3);
            hudRenderSystem = this.SystemManager.SetSystem<HUDRenderSystem>(new HUDRenderSystem(), ExecutionType.Draw, 1);
            starFieldRenderSystem = this.SystemManager.SetSystem<StarFieldRenderSystem>(new StarFieldRenderSystem(SpriteBatch), ExecutionType.Draw);
            radarRenderSystem = this.SystemManager.SetSystem<RadarRenderSystem>(new RadarRenderSystem(hudRenderSystem.RadarScreenLocation,
                new Rectangle(-ScreenHelper.Viewport.Width * 2, -ScreenHelper.Viewport.Height * 2,
                    ScreenHelper.Viewport.Width * 2, ScreenHelper.Viewport.Height * 2)),
                    ExecutionType.Draw, 2);
            base.BuildSystems();
        }

        #endregion Systems

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
            this.SetEntityTemplate("Thug", new ThugTemplate(_spriteSheet, this));

            #region Crystals

            this.SetEntityTemplate("Crystal", new CrystalTemplate(this, _spriteSheet));

            #endregion Crystals

            #region Player Bullets

            this.SetEntityTemplate("BlueBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot1"),
                new Velocity(new Vector2(15), 0f),
                new Bullet(1, "Enemies", e => 
                    {
                        e.AddComponent<Slow>(new Slow(500, 1f, 5.0f, new Vector2(4), 0.0f));
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = new Color(0, 0, 255);

                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 500));
                        else
                        {
                            e.GetComponent<SpriteEffect>().AddEffect(s, 500);
                        }

                        e.Refresh();
                    }
                    )));
            this.SetEntityTemplate("BlueBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot2"),
                new Velocity(new Vector2(15), 0f),
                new Bullet(2, "Enemies", e =>
                    {
                        e.AddComponent<Slow>(new Slow(1000, 1f, 5.0f, new Vector2(4), 0.0f));
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Blue;
                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 1000));
                        else
                        {
                            e.GetComponent<SpriteEffect>().AddEffect(s, 1000);
                        }
                        
                        e.Refresh();
                    }
                    )));
            this.SetEntityTemplate("BlueBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot3"),
                new Velocity(new Vector2(15), 0f),
                new Bullet(3, "Enemies", e => 
                    {
                        e.AddComponent<Slow>(new Slow(1500, 1f, 5.0f, new Vector2(4), 0.0f));
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Blue;

                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 1500));
                        else
                        {
                            e.GetComponent<SpriteEffect>().AddEffect(s, 1500);
                        }
                        e.Refresh();
                    }
                    )));

            this.SetEntityTemplate("RedBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot1"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(2, "Enemies", null
                    )));

            this.SetEntityTemplate("RedBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot2"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(4, "Enemies", null
                    )));

            this.SetEntityTemplate("RedBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "redshot3"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(6, "Enemies", null
                    )));

            this.SetEntityTemplate("GreenBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot1"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(4, "Enemies",
                    e =>
                    {
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Green;

                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 500));
                        else
                            e.GetComponent<SpriteEffect>().AddEffect(s, 500);
                        e.AddComponent<Damage>(new Damage(1, 3, 500));
                        e.Refresh();
                    }
                    )));

            this.SetEntityTemplate("GreenBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot2"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(8, "Enemies",
                    e =>
                    {
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Green;

                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 1000));
                        else
                            e.GetComponent<SpriteEffect>().AddEffect(s, 1000);
                        e.AddComponent<Damage>(new Damage(2, 5, 1000));
                        e.Refresh();
                    }
                    )));

            this.SetEntityTemplate("GreenBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "greenshot3"),
                new Velocity(new Vector2(40), 0f),
                new Bullet(12, "Enemies",
                    e =>
                    {
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Green;

                        if (!e.HasComponent<SpriteEffect>())
                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 1500));
                        else
                            e.GetComponent<SpriteEffect>().AddEffect(s, 1500);
                        e.AddComponent<Damage>(new Damage(3, 10, 1500));
                        e.Refresh();
                    }
                    )));

            this.SetEntityTemplate("WhiteBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot1"),
                new Velocity(new Vector2(12), 0f),
                new Bullet(1, "Enemies", null
                    )));

            this.SetEntityTemplate("WhiteBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot2"),
                new Velocity(new Vector2(12), 0f),
                new Bullet(2, "Enemies", null
                    )));

            this.SetEntityTemplate("WhiteBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "whiteshot3"),
                new Velocity(new Vector2(12), 0f),
                new Bullet(3, "Enemies", null
                    )));

            #endregion Player Bullets

            #region Enemy Bullets

            this.SetEntityTemplate("BBullet1", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot1"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(1, "Players", e => e.AddComponent<Slow>(new Slow(10, 1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BBullet2", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot2"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(2, "Players", e => e.AddComponent<Slow>(new Slow(20, 1f, 5.0f, new Vector2(4), 0.0f))
                    )));
            this.SetEntityTemplate("BBullet3", new BulletTemplate(
                new Sprite(_spriteSheet, "blueshot3"),
                new Velocity(new Vector2(5), 0f),
                new Bullet(3, "Players", e => e.AddComponent<Slow>(new Slow(30, 1f, 5.0f, new Vector2(4), 0.0f))
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

            #endregion Enemy Bullets

            this.SetEntityTemplate("Star", new StarTemplate(_spriteSheet));
            this.SetEntityTemplate("Explosion", new ExplosionTemplate(this, _spriteSheet));
            this.SetEntityGroupTemplate("BigExplosion", new BigExplosionTemplate(_spriteSheet));
            this.SetEntityGroupTemplate("StarField", new StarFieldTemplate());
            base.BuildTemplates(Content, args);
        }

        #endregion Templates

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
#if DEBUG

            //Camera.TrackingBody = Player.GetComponent<Body>();
#endif
        }

        #endregion Entities

        #endregion Initialization

        #region Functioning Loop

        #endregion Functioning Loop

        #region Fields

        #region Entity

        //Update Systems
        private GunSystem gunSystem;

        private BulletRemovalSystem bulletRemovalSystem;
        private BulletCollisionSystem bulletCollisionSystem;

        //HealthRenderSystem healthRenderSystem;
        private HealthSystem healthSystem;

        private DirectorSystem enemySpawnSystem;
        private AISystem enemyMovementSystem;
        private SlowSystem slowSystem;
        private PlayerControlSystem playerControlSystem;
        private ExplosionSystem explosionSystem;
        private DamageSystem damageSystem;
        private ScoreSystem scoreSystem;

        //Draw Systems
        private HealthRenderSystem healthRenderSystem;

        private HUDRenderSystem hudRenderSystem;
        private StarFieldRenderSystem starFieldRenderSystem;
        private RadarRenderSystem radarRenderSystem;

        //Entities for safe keeping
        public Entity Player;

        public Entity Base;

        #endregion Entity

        private SpriteSheet _spriteSheet;
        private Texture2D _hud;
        private ImageFont _font;

        #endregion Fields
    }
}