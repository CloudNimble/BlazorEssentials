using System.Collections.Generic;

namespace CloudNimble.BlazorEssentials.IndexedDb.Schema
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexedDbObjectStoreDefinition
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string? KeyPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IndexedDbIndexDefinition> Indexes { get; set; } = [];

    }


}