using System;

namespace CloudNimble.BlazorEssentials.IndexedDb
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="message"></param>
    public class IndexedDbException(string message) : Exception(message)
    {
    }

}