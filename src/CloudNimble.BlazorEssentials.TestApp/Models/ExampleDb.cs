using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;

namespace CloudNimble.BlazorEssentials.TestApp.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class ExampleDb : IndexedDbDatabase
    {

        /// <summary>
        /// 
        /// </summary>
        [ObjectStore(KeyPath = "Id", AutoIncrementKeys = true)]
        public IndexedDbObjectStore Events { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        public ExampleDb(IJSRuntime jsRuntime) : base(jsRuntime)
        {
        }

    }

}
