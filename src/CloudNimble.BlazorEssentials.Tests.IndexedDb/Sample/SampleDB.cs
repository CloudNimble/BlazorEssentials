using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;

namespace CloudNimble.BlazorEssentials.Tests.IndexedDb.Sample
{

    /// <summary>
    /// 
    /// </summary>
    public class SampleDB : IndexedDbDatabase
    {

        [ObjectStore(Name = "SampleEmployees", AutoIncrementKeys = true)]
        public IndexedDbObjectStore SampleEmployees { get; set; }

        [Index(Name = "firstName", Path = "firstName")]
        public IndexedDbObjectStore SampleCustomers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        public SampleDB(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            Version = 1;
        }

    }

}
