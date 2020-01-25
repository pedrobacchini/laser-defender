namespace GameSystem.ObjectPool
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IPooledObjects
    {
        /// <summary>
        /// Called when the object is spawned by an object pooler.
        /// </summary>
        void OnObjectSpawn();

        ///// <summary>
        ///// Called when the object is spawned by an object pooler.
        ///// This will iterate all children to call their instance.
        ///// </summary>
        //void OnChildrenSpawn();
    }
}
