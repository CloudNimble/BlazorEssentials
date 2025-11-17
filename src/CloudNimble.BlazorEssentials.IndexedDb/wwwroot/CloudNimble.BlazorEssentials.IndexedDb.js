import { getOpenDB, getDeleteDB } from './idb-loader.js';
/**
 * Allows for managing multiple instances of the IndexedDbManager, one for each database name.
 */
var instanceManager = {};
/**
 * Creates a new instance of the IndexedDbManager for the specified database name.
 */
export function createInstance(databaseName) {
    instanceManager[databaseName] = new IndexedDbManager();
}
export function openDatabase(database) {
    ensureDatabaseInstance(database.name);
    return instanceManager[database.name].open(database);
}
export function deleteDatabase(databaseName) {
    ensureDatabaseInstance(databaseName);
    instanceManager[databaseName].deleteDatabase();
}
export function closeDatabase(databaseName) {
    ensureDatabaseInstance(databaseName);
    instanceManager[databaseName].close();
}
export function getDbSchema(databaseName) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getDbSchema();
}
export function count(databaseName, storeName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].count(storeName, key);
}
export function countByKeyRange(databaseName, storeName, lower, upper, lowerOpen, upperOpen) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countByKeyRange(storeName, lower, upper, lowerOpen, upperOpen);
}
export function get(databaseName, storeName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].get(storeName, key);
}
export function getAll(databaseName, storeName, key, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAll(storeName, key, count);
}
export function getAllByKeyRange(databaseName, storeName, lower, upper, lowerOpen, upperOpen, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllByKeyRange(storeName, lower, upper, lowerOpen, upperOpen, count);
}
export function getAllByArrayKey(databaseName, storeName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllByArrayKey(storeName, key);
}
export function getKey(databaseName, storeName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getKey(storeName, key);
}
export function getAllKeys(databaseName, storeName, key, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeys(storeName, key, count);
}
export function getAllKeysByKeyRange(databaseName, storeName, lower, upper, lowerOpen, upperOpen, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysByKeyRange(storeName, lower, upper, lowerOpen, upperOpen, count);
}
export function getAllKeysByArrayKey(databaseName, storeName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysByArrayKey(storeName, key);
}
export function query(databaseName, storeName, key, filter, count = 0, skip = 0) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].query(storeName, key, filter, count, skip);
}
export function countFromIndex(databaseName, storeName, indexName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countFromIndex(storeName, indexName, key);
}
export function countFromIndexByKeyRange(databaseName, storeName, indexName, lower, upper, lowerOpen, upperOpen) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen);
}
export function getFromIndex(databaseName, storeName, indexName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getFromIndex(storeName, indexName, key);
}
export function getAllFromIndex(databaseName, storeName, indexName, key, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndex(storeName, indexName, key, count);
}
export function getAllFromIndexByKeyRange(databaseName, storeName, indexName, lower, upper, lowerOpen, upperOpen, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen, count);
}
export function getAllFromIndexByArrayKey(databaseName, storeName, indexName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndexByArrayKey(storeName, indexName, key);
}
export function getKeyFromIndex(databaseName, storeName, indexName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getKeyFromIndex(storeName, indexName, key);
}
export function getAllKeysFromIndex(databaseName, storeName, indexName, key, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndex(storeName, indexName, key, count);
}
export function getAllKeysFromIndexByKeyRange(databaseName, storeName, indexName, lower, upper, lowerOpen, upperOpen, count) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen, count);
}
export function getAllKeysFromIndexByArrayKey(databaseName, storeName, indexName, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndexByArrayKey(storeName, indexName, key);
}
export function queryFromIndex(databaseName, storeName, indexName, key, filter, count = 0, skip = 0) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].queryFromIndex(storeName, indexName, key, filter, count, skip);
}
export function add(databaseName, storeName, data, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].add(storeName, data, key);
}
export function put(databaseName, storeName, data, key) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].put(storeName, data, key);
}
export function deleteRecord(databaseName, storeName, id) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].delete(storeName, id);
}
export function batchAdd(databaseName, storeName, data) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchAdd(storeName, data);
}
export function batchPut(databaseName, storeName, data) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchPut(storeName, data);
}
export function batchDelete(databaseName, storeName, ids) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchDelete(storeName, ids);
}
export function clearStore(databaseName, storeName) {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].clearStore(storeName);
}
/**
 * Gets the schema for the specified database.
 */
function ensureDatabaseInstance(databaseName) {
    if (!instanceManager[databaseName]) {
        createInstance(databaseName);
    }
}
/**
 * Manages a particular instance of an IndexedDb database.
 */
export class IndexedDbManager {
    constructor() {
        this.dbInstance = undefined;
        this.databaseName = "";
        this.open = async (database) => {
            var upgradeError = "";
            try {
                if (!this.dbInstance || this.dbInstance.version < database.version) {
                    if (this.dbInstance) {
                        this.dbInstance.close();
                        this.dbInstance = undefined;
                    }
                    const openDB = await getOpenDB();
                    this.dbInstance = await openDB(database.name, database.version, {
                        upgrade(db, oldVersion, newVersion, transaction) {
                            try {
                                IndexedDbManager.upgradeDatabase(db, oldVersion, newVersion, database);
                            }
                            catch (error) {
                                upgradeError = error.toString();
                                throw (error);
                            }
                        },
                    });
                }
                return true;
            }
            catch (error) {
                throw error.toString() + ' ' + upgradeError;
            }
            return false;
        };
        this.close = () => {
            try {
                this.dbInstance?.close();
                this.dbInstance = undefined;
            }
            catch (error) {
                throw error.toString();
            }
        };
        this.deleteDatabase = async () => {
            try {
                this.close();
                const deleteDB = await getDeleteDB();
                await deleteDB(this.databaseName);
            }
            catch (error) {
                throw `Database ${this.databaseName}, ${error.toString()}`;
            }
        };
        this.getDbSchema = async () => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const dbInstance = this.dbInstance;
                const dbInfo = {
                    name: dbInstance.name,
                    version: dbInstance.version,
                    objectStores: []
                };
                for (let s = 0; s < dbInstance.objectStoreNames.length; s++) {
                    let dbStore = dbInstance.transaction(dbInstance.objectStoreNames[s], 'readonly').store;
                    let objectStore = {
                        name: dbStore.name,
                        keyPath: Array.isArray(dbStore.keyPath) ? dbStore.keyPath.join(',') : dbStore.keyPath,
                        autoIncrement: dbStore.autoIncrement,
                        indexes: []
                    };
                    for (let i = 0; i < dbStore.indexNames.length; i++) {
                        const dbIndex = dbStore.index(dbStore.indexNames[i]);
                        let index = {
                            name: dbIndex.name,
                            keyPath: Array.isArray(dbIndex.keyPath) ? dbIndex.keyPath.join(',') : dbIndex.keyPath,
                            multiEntry: dbIndex.multiEntry,
                            unique: dbIndex.unique
                        };
                        objectStore.indexes.push(index);
                    }
                    dbInfo.objectStores.push(objectStore);
                }
                return dbInfo;
            }
            catch (error) {
                throw `Database ${this.databaseName}, ${error.toString()}`;
            }
        };
        // IDBObjectStore
        this.count = async (storeName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = await tx.store.count(key ?? undefined);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.countByKeyRange = async (storeName, lower, upper, lowerOpen, upperOpen) => {
            try {
                return await this.count(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.get = async (storeName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = await tx.store.get(key);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAll = async (storeName, key, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = await tx.store.getAll(key ?? undefined, count ?? undefined);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAllByKeyRange = async (storeName, lower, upper, lowerOpen, upperOpen, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return await this.getAll(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAllByArrayKey = async (storeName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(await tx.store.getAll(element));
                }
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getKey = async (storeName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = await tx.store.getKey(key);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAllKeys = async (storeName, key, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = await tx.store.getAllKeys(key ?? undefined, count ?? undefined);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAllKeysByKeyRange = async (storeName, lower, upper, lowerOpen, upperOpen, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return await this.getAllKeys(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.getAllKeysByArrayKey = async (storeName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(await tx.store.getAllKeys(element));
                }
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.query = async (storeName, key, filter, count = 0, skip = 0) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                try {
                    var func = new Function('obj', filter);
                }
                catch (error) {
                    throw `${error.toString()} in filter { ${filter} }`;
                }
                var row = 0;
                var errorMessage = "";
                let results = [];
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let cursor = await tx.store.openCursor(key ?? undefined);
                while (cursor) {
                    if (!cursor) {
                        return;
                    }
                    try {
                        var out = func(cursor.value);
                        if (out) {
                            row++;
                            if (row > skip) {
                                results.push(out);
                            }
                        }
                    }
                    catch (error) {
                        errorMessage = `obj: ${JSON.stringify(cursor.value)}\nfilter: ${filter}\nerror: ${error.toString()}`;
                        break;
                    }
                    if (count > 0 && results.length >= count) {
                        break;
                    }
                    cursor = await cursor.continue();
                }
                await tx.done;
                if (errorMessage) {
                    throw errorMessage;
                }
                return results;
            }
            catch (error) {
                throw `Store ${storeName} ${error.toString()}`;
            }
        };
        // IDBIndex functions
        this.countFromIndex = async (storeName, indexName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = await tx.store.index(indexName).count(key ?? undefined);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.countFromIndexByKeyRange = async (storeName, indexName, lower, upper, lowerOpen, upperOpen) => {
            try {
                return await this.countFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getFromIndex = async (storeName, indexName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = await tx.store.index(indexName).get(key);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllFromIndex = async (storeName, indexName, key, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = await tx.store.index(indexName).getAll(key ?? undefined, count ?? undefined);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllFromIndexByKeyRange = async (storeName, indexName, lower, upper, lowerOpen, upperOpen, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return await this.getAllFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllFromIndexByArrayKey = async (storeName, indexName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const dx = tx.store.index(indexName);
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(await dx.getAll(element));
                }
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getKeyFromIndex = async (storeName, indexName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = await tx.store.index(indexName).getKey(key);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllKeysFromIndex = async (storeName, indexName, key, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = await tx.store.index(indexName).getAllKeys(key ?? undefined, count ?? undefined);
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllKeysFromIndexByKeyRange = async (storeName, indexName, lower, upper, lowerOpen, upperOpen, count) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return await this.getAllKeysFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.getAllKeysFromIndexByArrayKey = async (storeName, indexName, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const dx = tx.store.index(indexName);
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(await dx.getAllKeys(element));
                }
                await tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.queryFromIndex = async (storeName, indexName, key, filter, count = 0, skip = 0) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                try {
                    var func = new Function('obj', filter);
                }
                catch (error) {
                    throw `${error.toString()} in filter { ${filter} }`;
                }
                var row = 0;
                var errorMessage = "";
                let results = [];
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let cursor = await tx.store.index(indexName).openCursor(key ?? undefined);
                while (cursor) {
                    if (!cursor) {
                        return;
                    }
                    try {
                        var out = func(cursor.value);
                        if (out) {
                            row++;
                            if (row > skip) {
                                results.push(out);
                            }
                        }
                    }
                    catch (error) {
                        errorMessage = `obj: ${JSON.stringify(cursor.value)}\nfilter: ${filter}\nerror: ${error.toString()}`;
                        break;
                    }
                    if (count > 0 && results.length >= count) {
                        break;
                    }
                    cursor = await cursor.continue();
                }
                await tx.done;
                if (errorMessage) {
                    throw errorMessage;
                }
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        };
        this.add = async (storeName, data, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                // @ts-ignore - Type mismatch between readwrite and readonly store types
                data = this.checkForKeyPath(tx.store, data);
                const result = await tx.store.add(data, key ?? undefined);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.put = async (storeName, data, key) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                const result = await tx.store.put(data, key ?? undefined);
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.delete = async (storeName, id) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                await tx.store.delete(id);
                await tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.batchAdd = async (storeName, data) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                let result = [];
                data.forEach(async (element) => {
                    // @ts-ignore - Type mismatch between readwrite and readonly store types
                    let item = this.checkForKeyPath(tx.store, element);
                    result.push(await tx.store.add(item));
                });
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.batchPut = async (storeName, data) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                let result = [];
                data.forEach(async (element) => {
                    result.push(await tx.store.put(element));
                });
                await tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.batchDelete = async (storeName, ids) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                ids.forEach(async (element) => {
                    await tx.store.delete(element);
                });
                await tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
        this.clearStore = async (storeName) => {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                await tx.store.clear();
                await tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        };
    }
    checkForKeyPath(objectStore, data) {
        if (!objectStore.autoIncrement || !objectStore.keyPath) {
            return data;
        }
        if (typeof objectStore.keyPath !== 'string') {
            return data;
        }
        const keyPath = objectStore.keyPath;
        if (!data[keyPath]) {
            delete data[keyPath];
        }
        return data;
    }
    static upgradeDatabase(upgradeDB, oldVersion, newVersion, dbDatabase) {
        if (newVersion && newVersion > oldVersion) {
            if (dbDatabase.objectStores) {
                for (var store of dbDatabase.objectStores) {
                    if (!upgradeDB.objectStoreNames.contains(store.name)) {
                        this.addNewStore(upgradeDB, store);
                    }
                }
            }
        }
    }
    static getKeyPath(keyPath) {
        if (keyPath) {
            var multiKeyPath = keyPath.split(',');
            return multiKeyPath.length > 1 ? multiKeyPath : keyPath;
        }
        else {
            return undefined;
        }
    }
    static addNewStore(upgradeDB, store) {
        try {
            const newStore = upgradeDB.createObjectStore(store.name, {
                keyPath: this.getKeyPath(store.keyPath),
                autoIncrement: store.autoIncrement
            });
            for (var index of store.indexes) {
                try {
                    newStore.createIndex(index.name, this.getKeyPath(index.keyPath) ?? index.name, {
                        multiEntry: index.multiEntry,
                        unique: index.unique
                    });
                }
                catch (error) {
                    throw `index ${index.name}, ${error.toString()}`;
                }
            }
        }
        catch (error) {
            throw `store ${store.name}, ${error.toString()}`;
        }
    }
}
IndexedDbManager.E_DB_CLOSED = "Database is closed";
//# sourceMappingURL=CloudNimble.BlazorEssentials.IndexedDb.js.map