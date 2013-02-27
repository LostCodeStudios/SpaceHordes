namespace GameLibrary.Dependencies.Entities
{
    public interface IEntityGroupTemplate
    {
        /// <summary>
        /// Creates a group of entities using a specified entity world.
        /// Entities MUST BE INSTANTIATED WITH WORLD.CREATEENTITY();
        /// </summary>
        /// <param name="world">The world in which to create the entities.</param>
        /// <param name="args">The tempalte arguments.</param>
        /// <returns>A lsit of entities without a group name.</returns>
        Entity[] BuildEntityGroup(EntityWorld world, params object[] args);
    }
}