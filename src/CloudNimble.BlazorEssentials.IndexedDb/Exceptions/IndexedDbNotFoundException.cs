namespace CloudNimble.BlazorEssentials.IndexedDb
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public class IndexedDbNotFoundException(string message) : IndexedDbException(message)
    {
    }

}