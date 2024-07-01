using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.IndexedDb
{
    /// <summary>
    /// Defines a store to add to database
    /// </summary>
    public class IndexedDbObjectStore
    {

        #region Public Properties

        /// <summary>
        /// If true, the object store has a key generator. Defaults to false.
        /// Note that every object store has its own separate auto increment counter.
        /// </summary>
        /// <value></value>
        public bool AutoIncrement { get; init; }

        /// <summary>
        /// IDMManager
        /// </summary>
        public IndexedDbDatabase Database { get; init; }

        /// <summary>
        /// Provides a set of additional indexes if required.
        /// </summary>
        public List<IndexedDbIndex> Indexes { get; init; } = [];

        /// <summary>
        /// the identifier for the property in the object/record that is saved and is to be indexed.
        /// can be multiple properties separated by comma
        /// If this property is null, the application must provide a key for each modification operation.
        /// </summary>
        public string? KeyPath { get; init; }

        /// <summary>
        /// The name for the store
        /// </summary>
        public string Name { get; internal set; } = "";

        #endregion

        #region Constructor

        /// <summary>
        /// Add new ObjectStore definition
        /// </summary>
        /// <param name="database"></param>
        /// <param name="attribute"></param>
        public IndexedDbObjectStore(IndexedDbDatabase database, ObjectStoreAttribute attribute = null)
        {
            Database = database;

            Name = !string.IsNullOrWhiteSpace(attribute?.Name) ? attribute.Name : GetType().Name;
            KeyPath = !string.IsNullOrWhiteSpace(attribute?.KeyPath) ? attribute.KeyPath : "id";
            AutoIncrement = attribute?.AutoIncrementKeys ?? false;

            Database.ObjectStores.Add(this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Count records in ObjectStore
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.Count, Database.Name, Name);
        }

        /// <summary>
        /// Count records in ObjectStore
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<TKey>(TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.Count, Database.Name, Name, key);
        }

        /// <summary>
        /// Count records in ObjectStore
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<TKey>(KeyRange<TKey> key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.CountByKeyRange, Database.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen);
        }

        /// <summary>
        /// Retrieve a record by Key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">the key of the record</param>
        /// <returns></returns>
        public async Task<TResult?> GetAsync<TKey, TResult>(TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TResult?>(IndexedDbConstants.Get, Database.Name, Name, key);
        }

        /// <summary>
        /// Gets all of the records in a given store.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TResult>(int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAll, Database.Name, Name, null, count);
        }

        /// <summary>
        /// Gets all of the records by Key in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAll, Database.Name, Name, key, count);
        }

        /// <summary>
        /// Gets all of the records by KeyRange in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(KeyRange<TKey> key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllByKeyRange, Database.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen, count);
        }

        /// <summary>
        /// Gets all of the records by ArrayKey in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey[] key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllByArrayKey, Database.Name, Name, key);
        }

        /// <summary>
        /// Retrieve a record key by Key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">the key of the record</param>
        /// <returns></returns>
        public async Task<TResult?> GetKeyAsync<TKey, TResult>(TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TResult?>(IndexedDbConstants.GetKey, Database.Name, Name, key);
        }

        /// <summary>
        /// Gets all of the records keys in a given store.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TResult>(int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeys, Database.Name, Name, null, count);
        }

        /// <summary>
        /// Gets all of the records keys by Key in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeys, Database.Name, Name, key, count);
        }

        /// <summary>
        /// Gets all of the records by KeyRange in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(KeyRange<TKey> key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysByKeyRange, Database.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen, count);
        }

        /// <summary>
        /// Gets all of the records by ArrayKey in a given store.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey[] key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysByArrayKey, Database.Name, Name, key);
        }

        /// <summary>
        /// Gets all of the records using a filter expression
        /// </summary>
        /// <param name="filter">expression that evaluates to true/false, each record es passed to "obj" parameter</param>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TResult>(string filter, int? count = null, int? skip = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.Query, Database.Name, Name, null, filter, count, skip);
        }

        /// <summary>
        /// Gets all of the records using a filter expression
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter">expression that evaluates to true/false, each record es passed to "obj" parameter</param>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TKey, TResult>(string name, string filter, TKey key, int? count = null, int? skip = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.Query, Database.Name, name, key, filter, count, skip);
        }

        /// <summary>
        /// Gets all of the records using a filter expression
        /// </summary>
        /// <param name="filter">expression that evaluates to true/false, each record es passed to "obj" parameter</param>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, KeyRange<TKey> key, int? count = null, int? skip = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.Query, Database.Name, Name, key, filter, count, skip);
        }

        /// <summary>
        /// Adds a new record/object to the specified ObjectStore
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public async Task AddAsync<TData>(TData data) where TData : notnull
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.Add, Database.Name, Name, data);
        }

        /// <summary>
        /// Adds a new record/object to the specified ObjectStore
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey> AddAsync<TData, TKey>(TData data)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey>(IndexedDbConstants.Add, Database.Name, Name, data);
        }

        /// <summary>
        /// Adds a new record/object to the specified ObjectStore
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey> AddAsync<TData, TKey>(TData data, TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey>(IndexedDbConstants.Add, Database.Name, Name, data, key);
        }

        /// <summary>
        /// Updates and existing record
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public async Task PutAsync<TData>(TData data) where TData : notnull
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.Put, Database.Name, Name, data);
        }

        /// <summary>
        /// Updates and existing record
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey> PutAsync<TData, TKey>(TData data)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey>(IndexedDbConstants.Put, Database.Name, Name, data);
        }

        /// <summary>
        /// Updates and existing record
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey> PutAsync<TData, TKey>(TData data, TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey>(IndexedDbConstants.Put, Database.Name, Name, data, key);
        }

        /// <summary>
        /// Deletes a record from the store based on the id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DeleteAsync<TKey>(TKey key) where TKey : notnull
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.Delete, Database.Name, Name, key);
        }

        /// <summary>
        /// Add an array of new record/object in one transaction to the specified store
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public async Task BatchAddAsync<TData>(TData[] data)
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.BatchAdd, Database.Name, Name, data);
        }

        /// <summary>
        /// Add an array of new record/object in one transaction to the specified store
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey[]> BatchAddAsync<TData, TKey>(TData[] data)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey[]>(IndexedDbConstants.BatchAdd, Database.Name, Name, data);
        }

        /// <summary>
        /// Put an array of new record/object in one transaction to the specified store
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public async Task BatchPutAsync<TData>(TData[] data)
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.BatchPut, Database.Name, Name, data);
        }

        /// <summary>
        /// Put an array of new record/object in one transaction to the specified store
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<TKey[]> BatchPutAsync<TData, TKey>(TData[] data)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TKey[]>(IndexedDbConstants.BatchPut, Database.Name, Name, data);
        }

        /// <summary>
        /// Delete multiple records from the store based on the id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchDeleteAsync<TKey>(TKey[] ids)
        {
            await Database.EnsureIsOpenAsync();
            await Database.CallJavaScriptAsync(IndexedDbConstants.BatchDelete, Database.Name, Name, ids);
        }

        /// <summary>
        /// Clears all of the records from a given store.
        /// </summary>
        /// <returns></returns>
        public async Task ClearStoreAsync()
        {
            await Database.EnsureIsOpenAsync();
            var result =  await Database.CallJavaScriptAsync<string>(IndexedDbConstants.ClearStore, Database.Name, Name);
        }

        #endregion

    }

}
