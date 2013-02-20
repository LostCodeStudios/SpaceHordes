using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class HealthSystem : EntityProcessingSystem
    {
        ComponentMapper<Health> healthMapper;
        static Random r = new Random();

        public HealthSystem()
            : base(typeof(Health))
        {
        }

        public override void Added(Entity e)
        {
            if (e.HasComponent<Sprite>()) //Change color to red when hit
                e.GetComponent<Health>().OnDamage +=
                    (setter) =>
                    {
                        Sprite s = e.GetComponent<Sprite>();
                        s.Color = Color.Red;
                        e.Refresh();

                        Console.WriteLine(e.Id + " damaged by: " + setter.Id);
                        if (e.Tag != null && e.Tag == "Base")
                            Console.Write("Base damaged by:" + setter.Id);
                    };
            base.Added(e);
        }

        public override void Initialize()
        {
            healthMapper = new ComponentMapper<Health>(world);
        }

        public override void Process(Entity e)
        {
        }
    }
}
