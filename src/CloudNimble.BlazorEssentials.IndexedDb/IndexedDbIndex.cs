using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.IndexedDb
{
    /// <summary>
    /// Defines an Index for a given object store.
    /// </summary>
    public class IndexedDbIndex
    {

        /// <summary>
        /// 
        /// </summary>
        public IndexedDbDatabase Database { get; init; }

        /// <summary>
        /// the identifier for the property in the object/record that is saved and is to be indexed.
        /// can be multiple properties separated by comma
        /// if null will default to index name
        /// </summary>
        public string KeyPath { get; }

        /// <summary>
        /// Affects how the index behaves when the result of evaluating the index's key path yields an array.
        /// If true, there is one record in the index for each item in an array of keys.
        /// If false, then there is one record for each key that is an array.
        /// </summary>
        /// <value></value>
        public bool MultiEntry { get; }

        /// <summary>
        /// The name of the index.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Only use for indexes
        /// If true, this index does not allow duplicate values for a key.
        /// </summary>
        public IndexedDbObjectStore ObjectStore { get; init; }

        /// <summary>
        /// Only use for indexes
        /// If true, this index does not allow duplicate values for a key.
        /// </summary>
        public bool Unique { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectStore"></param>
        /// <param name="name"></param>
        /// <param name="keyPath"></param>
        /// <param name="multiEntry"></param>
        /// <param name="unique"></param>
        /// <exception cref="IndexedDbException"></exception>
        public IndexedDbIndex(IndexedDbObjectStore objectStore, string name, string keyPath, bool multiEntry = false, bool unique = false)
        {
            if (objectStore.Indexes.Any(i => i.Name == name))
            {
                throw new IndexedDbException($"Store {objectStore.Name}, Index {name} already exists");
            }

            Name = name;
            KeyPath = keyPath;
            MultiEntry = multiEntry;
            Unique = unique;

            objectStore.Indexes.Add(this);

            ObjectStore = objectStore;
            Database = objectStore.Database;
        }

        /// <summary>
        /// Count records in Index
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.CountFromIndex, Database.Name, ObjectStore.Name, Name);
        }

        /// <summary>
        /// Count records in Index
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<TKey>(TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.CountFromIndex, Database.Name, ObjectStore.Name, Name, key);
        }

        /// <summary>
        /// Count records in Index
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<TKey>(KeyRange<TKey> key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<int>(IndexedDbConstants.CountFromIndexByKeyRange, Database.Name, ObjectStore.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen);
        }

        /// <summary>
        /// Returns the first record that matches a query against a given index
        /// </summary>
        /// <param name="queryValue"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TKey, TResult>(TKey queryValue)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TResult>(IndexedDbConstants.GetFromIndex, Database.Name, ObjectStore.Name, Name, queryValue);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TResult>(int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllFromIndex, Database.Name, ObjectStore.Name, Name, null, count);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllFromIndex, Database.Name, ObjectStore.Name, Name, key, count);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(KeyRange<TKey> key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllFromIndexByKeyRange, Database.Name, ObjectStore.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen, count);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey[] key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllFromIndexByArrayKey, Database.Name, ObjectStore.Name, Name, key);
        }

        /// <summary>
        /// Returns the first record keys that matches a query against a given index
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TResult> GetKeyAsync<TKey, TResult>(TKey key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<TResult>(IndexedDbConstants.GetKeyFromIndex, Database.Name, ObjectStore.Name, Name, key);
        }

        /// <summary>
        /// Gets all of the records keys that match a given query in the specified index.
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TResult>(int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysFromIndex, Database.Name, ObjectStore.Name, Name, null, count);
        }

        /// <summary>
        /// Gets all of the records keys that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysFromIndex, Database.Name, ObjectStore.Name, Name, key, count);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(KeyRange<TKey> key, int? count = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysFromIndexByKeyRange, Database.Name, ObjectStore.Name, Name, key.Lower, key.Upper, key.LowerOpen, key.UpperOpen, count);
        }

        /// <summary>
        /// Gets all of the records that match a given query in the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey[] key)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.GetAllKeysFromIndexByArrayKey, Database.Name, ObjectStore.Name, Name, key);
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
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.QueryFromIndex, Database.Name, ObjectStore.Name, Name, null, filter, count, skip);
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
        public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, TKey key, int? count = null, int? skip = null)
        {
            await Database.EnsureIsOpenAsync();
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.QueryFromIndex, Database.Name, ObjectStore.Name, Name, key, filter, count, skip);
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
            return await Database.CallJavaScriptAsync<List<TResult>>(IndexedDbConstants.QueryFromIndex, Database.Name, ObjectStore.Name, Name, key, filter, count, skip);
        }
    }
}
