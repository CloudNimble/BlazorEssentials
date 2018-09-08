using System.Collections.Generic;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ODataResultList<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Microsoft's SimpleJson implementation matches property names in a case-insensitive way.
        /// </remarks>
        public List<T> Value { get; set; }

    }

}
