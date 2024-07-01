using System.Reflection.Metadata;

namespace CloudNimble.BlazorEssentials.IndexedDb
{

    /// <summary>
    /// 
    /// </summary>
    internal static class IndexedDbConstants
    {
        internal const string BlazorEssentialsIndexedDb = "";

        // IndexedDbDatabase-related constants
        internal const string OpenDatabase = "openDatabase";
        internal const string DeleteDatabase = "deleteDatabase";
        internal const string GetDatabaseSchema = "getDbSchema";

        // IndexedDbObjectStore-related constants
        internal const string Count = "count";
        internal const string CountByKeyRange = "countByKeyRange";
        internal const string Get = "get";
        internal const string GetAll = "getAll";
        internal const string GetAllByKeyRange = "getAllByKeyRange";
        internal const string GetAllByArrayKey = "getAllByArrayKey";
        internal const string GetKey = "getKey";
        internal const string GetAllKeys = "getAllKeys";
        internal const string GetAllKeysByKeyRange = "getAllKeysByKeyRange";
        internal const string GetAllKeysByArrayKey = "getAllKeysByArrayKey";
        internal const string Query = "query";
        internal const string CountFromIndex = "countFromIndex";
        internal const string CountFromIndexByKeyRange = "countFromIndexByKeyRange";
        internal const string Add = "add";
        internal const string Put = "put";
        internal const string Delete = "delete";
        internal const string BatchAdd = "batchAdd";
        internal const string BatchPut = "batchPut";
        internal const string BatchDelete = "batchDelete";
        internal const string ClearStore = "clearStore";

        // IndexedDbIndex-related constants
        internal const string GetFromIndex = "getFromIndex";
        internal const string GetAllFromIndex = "getAllFromIndex";
        internal const string GetAllFromIndexByKeyRange = "getAllFromIndexByKeyRange";
        internal const string GetAllFromIndexByArrayKey = "getAllFromIndexByArrayKey";
        internal const string GetKeyFromIndex = "getKeyFromIndex";
        internal const string GetAllKeysFromIndex = "getAllKeysFromIndex";
        internal const string GetAllKeysFromIndexByKeyRange = "getAllKeysFromIndexByKeyRange";
        internal const string GetAllKeysFromIndexByArrayKey = "getAllKeysFromIndexByArrayKey";
        internal const string QueryFromIndex = "queryFromIndex";
    }
}
