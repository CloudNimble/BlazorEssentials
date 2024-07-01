using System;

namespace CloudNimble.BlazorEssentials.IndexedDb
{

    /// <summary>
    /// Helps define the structure of a <see cref="IndexedDbObjectStore" /> so you don't have to subclass one for every object store.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectStoreAttribute : Attribute
    {

        /// <summary>
        /// Specifies the name of the Object Store. Defaults to the the name of the property in the <see cref="IndexedDbDatabase" />.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the name of the Key for this <see cref="IndexedDbObjectStore" />. Defaults to "id".
        /// </summary>
        public string KeyPath { get; set; }

        /// <summary>
        /// Specifies if the keys for this <see cref="IndexedDbObjectStore" /> should auto-increment. Defaults to false.
        /// </summary>
        public bool AutoIncrementKeys { get; set; }

    }

}
