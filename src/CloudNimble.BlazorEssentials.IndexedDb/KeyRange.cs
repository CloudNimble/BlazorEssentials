namespace CloudNimble.BlazorEssentials.IndexedDb
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyRange<TKey>
    {

        /// <summary>
        /// 
        /// </summary>
        public TKey? Lower { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TKey? Upper { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool LowerOpen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UpperOpen { get; set; }

    }

}