using System.Collections.Generic;

namespace CloudNimble.BlazorEssentials.IndexedDb.Schema
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexedDbDatabaseDefinition
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IndexedDbObjectStoreDefinition> ObjectStores { get; set; } = [];


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="objectStores"></param>
        /// <returns></returns>
        public static IndexedDbDatabaseDefinition GetDatabaseDefinition(string name, int version, List<IndexedDbObjectStore> objectStores)
        {
            var def = new IndexedDbDatabaseDefinition
            {
                Name = name,
                Version = version
            };

            foreach (var store in objectStores)
            {
                var obj = new IndexedDbObjectStoreDefinition
                {
                    Name = store.Name,
                    KeyPath = store.KeyPath,
                    AutoIncrement = store.AutoIncrement
                };

                foreach (var storeIndex in store.Indexes)
                {
                    var idx = new IndexedDbIndexDefinition()
                    {
                        Name = storeIndex.Name,
                        KeyPath = storeIndex.KeyPath,
                        MultiEntry = storeIndex.MultiEntry,
                        Unique = storeIndex.Unique
                    };
                    obj.Indexes.Add(idx);
                }

                def.ObjectStores.Add(obj);
            }

            return def;
        }

    }


}