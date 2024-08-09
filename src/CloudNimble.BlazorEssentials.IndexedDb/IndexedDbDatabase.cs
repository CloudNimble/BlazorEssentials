using CloudNimble.BlazorEssentials.IndexedDb.Schema;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.IndexedDb
{
    /// <summary>
    /// Provides functionality for accessing IndexedDB from Blazor application
    /// </summary>
    public abstract class IndexedDbDatabase
    {

        #region Private Members

        private bool _isOpen;
        private readonly IJSRuntime _jsRuntime;
        private readonly JsModule _indexedDbModule;

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public List<IndexedDbObjectStore> ObjectStores { get; private set; } = [];

        /// <summary>
        /// 
        /// </summary>
        public IndexedDbDatabaseDefinition DatabaseDefinition { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        public IndexedDbDatabase(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            var assemblyName = typeof(IndexedDbDatabase).Assembly.GetName().Name;
            _indexedDbModule = new(jsRuntime, assemblyName[(assemblyName.IndexOf('.') + 1)..], assemblyName);
            Name = GetType().Name;

            foreach (var prop in GetType().GetProperties().Where(c => typeof(IndexedDbObjectStore).IsAssignableFrom(c.PropertyType)))
            {
                IndexedDbObjectStore store;
                var attribute = prop.GetCustomAttribute<ObjectStoreAttribute>();
                if (attribute is not null)
                {
                    store = new IndexedDbObjectStore(this, attribute);
                }
                else
                {
                    store = (IndexedDbObjectStore)Activator.CreateInstance(prop.PropertyType, this, null);
                }

                // RWM: If the instance name is IndexedDbObjectStore, then we know we're not dealing with a subclass.
                //      Otherwise the name would have defaulted to the subclassed type name.
                if (string.IsNullOrWhiteSpace(store.Name) || store.Name == nameof(IndexedDbObjectStore))
                {
                    store.Name = prop.Name;
                }

                // RWM: Check for IndexAttributes on the property and add them to the IndexedDbObjectStore.
                var attributes = prop.GetCustomAttributes<IndexAttribute>();
                foreach (var indexAttribute in attributes)
                {
                    // RWM: The index is added to the Store in the constructor below, so nothing else is needed here.
                    _ = new IndexedDbIndex(store, indexAttribute.Name, indexAttribute.Path);
                }

                prop.SetValue(this, store);
            }
        }

        #endregion

        /// <summary>
        /// Opens the IndexedDB defined in the DbDatabase. Under the covers will create the database if it does not exist
        /// and create the stores defined in DbDatabase.
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            DatabaseDefinition = IndexedDbDatabaseDefinition.GetDatabaseDefinition(Name, Version, ObjectStores);
            // RWM: We're not passing the name in here as a parameter because it's already part of the DatabaseDefinition.
            _isOpen = await CallJavaScriptAsync<bool>(IndexedDbConstants.OpenDatabase, DatabaseDefinition);
        }

        /// <summary>
        /// Deletes this IndexedDb instance from the browser.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteDatabaseAsync()
        {
            await CallJavaScriptAsync(IndexedDbConstants.DeleteDatabase, Name);
            _isOpen = false;
        }

        /// <summary>
        /// Load database schema from databaseName
        /// </summary>
        /// <returns></returns>
        public async Task LoadSchemaAsync()
        {
            var result = await CallJavaScriptAsync<IndexedDbDatabaseDefinition>(IndexedDbConstants.GetDatabaseSchema, Name);

            Version = result.Version;

            ObjectStores.Clear();
            foreach (var item in result.ObjectStores)
            {
                var store = new IndexedDbObjectStore(this)
                {
                    Name = item.Name,
                    KeyPath = item.KeyPath,
                    AutoIncrement = item.AutoIncrement
                };
                ObjectStores.Add(store);
            }
        }

        /// <summary>
        /// This function provides the means to add a store to an existing database,
        /// </summary>
        /// <param name="objectStore"></param>
        /// <returns></returns>
        public async Task CreateObjectStoreAsync(IndexedDbObjectStore objectStore)
        {
            if (objectStore is null || ObjectStores.Any(s => s.Name == objectStore.Name)) return;

            await EnsureIsOpenAsync();

            ObjectStores.Add(objectStore);
            Version++;

            //RWM: Calling Open again here will upgrade the Database and add the new ObjectStore.
            await OpenAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="IndexedDbException"></exception>
        public async Task ConsoleLog(params object[] args)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.log", args);
            }
            catch (JSException e)
            {
                throw new IndexedDbException(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="IndexedDbException"></exception>
        public async Task CallJavaScriptAsync(string functionName, params object[] args)
        {
            try
            {
                await _indexedDbModule.InvokeVoidAsync(functionName, args);
            }
            catch (JSException e)
            {
                throw new IndexedDbException(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="IndexedDbException"></exception>
        public async Task<TResult> CallJavaScriptAsync<TResult>(string functionName, params object?[] args)
        {
            try
            {
                return await _indexedDbModule.InvokeAsync<TResult>(functionName, args);
            }
            catch (JSException e)
            {
                throw new IndexedDbException(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task EnsureIsOpenAsync()
        {
            if (!_isOpen) await OpenAsync();
        }

    }

}
