using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Events
{
    public class ScoreTextTemplate : IEntityTemplate
    {
        public ScoreTextTemplate()
        {
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            string text = (string)args[0];
            Vector2 position = (Vector2)args[1];
            float time = 0.9f;

            e.AddComponent<FadingText>(new FadingText(text, time, position));

            e.Refresh();
            return e;
        }
    }
}