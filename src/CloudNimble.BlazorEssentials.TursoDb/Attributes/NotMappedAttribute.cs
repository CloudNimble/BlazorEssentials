using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Excludes a property from database mapping.
    /// Properties marked with this attribute will not be included in table creation or CRUD operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotMappedAttribute : Attribute
    {
    }

}
