using System;

namespace CloudNimble.BlazorEssentials.IndexedDb.Schema
{

    /// <summary>
    /// 
    /// </summary>
    public class IndexedDbIndexDefinition
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
        public bool MultiEntry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Unique { get; set; }

    }

}