namespace ServiceForWorkingWithBooks
{
    /// <summary>
    /// Interface which contain methods which  describe base behavior of all storage classes, such as save and load methods.
    /// </summary>
    /// <typeparam name="T">Type of storage.</typeparam>
    public interface IStorage<T>
    {
        /// <summary>
        /// Saves the specified data into storage.
        /// </summary>
        /// <param name="data">The data to save.</param>
        public void Save(T data);

        /// <summary>
        /// Loads data from storage.
        /// </summary>
        /// <returns>Loaded data from storage.</returns>
        public T Load();
    }
}
