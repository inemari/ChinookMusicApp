namespace ChinookMusicApp.Repositories
{
    public interface ICrudRepository<T, ID>
    {
        /// <summary>
        /// Retrieves all instances from the database.
        /// </summary>
        /// <returns>A list of all instances.</returns>
        List<T> GetAll();

        /// <summary>
        /// Retrieves a particular instance from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the instance to retrieve.</param>
        /// <returns>The instance with the specified ID, or null if not found.</returns>
        T GetById(ID id);

        /// <summary>
        /// Retrieves a particular instance from the database by its name.
        /// </summary>
        /// <param name="name">The name of the instance to retrieve.</param>
        /// <returns>The instance with the specified name, or null if not found.</returns>
        T GetByName(string name);

        /// <summary>
        /// Inserts a new row into the database based on the provided instance.
        /// </summary>
        /// <param name="obj">The instance to insert into the database.</param>
        void Add(T obj);

        /// <summary>
        /// Updates an existing row in the database based on the provided instance.
        /// </summary>
        /// <param name="obj">The instance with updated data.</param>
        void Update(T obj);


    }
}
