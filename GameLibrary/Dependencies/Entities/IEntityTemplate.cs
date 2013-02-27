namespace GameLibrary.Dependencies.Entities
{
    public interface IEntityTemplate
    {
        Entity BuildEntity(Entity e, params object[] args);
    }
}