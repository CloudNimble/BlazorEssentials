var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
//@ts-ignore
import { openDB, deleteDB } from 'https://cdn.skypack.dev/idb';
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
        this.open = (database) => __awaiter(this, void 0, void 0, function* () {
            var upgradeError = "";
            try {
                if (!this.dbInstance || this.dbInstance.version < database.version) {
                    if (this.dbInstance) {
                        this.dbInstance.close();
                        this.dbInstance = undefined;
                    }
                    this.dbInstance = yield openDB(database.name, database.version, {
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
        });
        this.close = () => {
            var _a;
            try {
                (_a = this.dbInstance) === null || _a === void 0 ? void 0 : _a.close();
                this.dbInstance = undefined;
            }
            catch (error) {
                throw error.toString();
            }
        };
        this.deleteDatabase = () => __awaiter(this, void 0, void 0, function* () {
            try {
                this.close();
                yield deleteDB(this.databaseName);
            }
            catch (error) {
                throw `Database ${this.databaseName}, ${error.toString()}`;
            }
        });
        this.getDbSchema = () => __awaiter(this, void 0, void 0, function* () {
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
        });
        // IDBObjectStore
        this.count = (storeName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = yield tx.store.count(key !== null && key !== void 0 ? key : undefined);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.countByKeyRange = (storeName, lower, upper, lowerOpen, upperOpen) => __awaiter(this, void 0, void 0, function* () {
            try {
                return yield this.count(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.get = (storeName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = yield tx.store.get(key);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAll = (storeName, key, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = yield tx.store.getAll(key !== null && key !== void 0 ? key : undefined, count !== null && count !== void 0 ? count : undefined);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAllByKeyRange = (storeName, lower, upper, lowerOpen, upperOpen, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return yield this.getAll(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAllByArrayKey = (storeName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(yield tx.store.getAll(element));
                }
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getKey = (storeName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = yield tx.store.getKey(key);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAllKeys = (storeName, key, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = yield tx.store.getAllKeys(key !== null && key !== void 0 ? key : undefined, count !== null && count !== void 0 ? count : undefined);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAllKeysByKeyRange = (storeName, lower, upper, lowerOpen, upperOpen, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return yield this.getAllKeys(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.getAllKeysByArrayKey = (storeName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(yield tx.store.getAllKeys(element));
                }
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.query = (storeName_1, key_1, filter_1, ...args_1) => __awaiter(this, [storeName_1, key_1, filter_1, ...args_1], void 0, function* (storeName, key, filter, count = 0, skip = 0) {
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
                let cursor = yield tx.store.openCursor(key !== null && key !== void 0 ? key : undefined);
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
                        return;
                    }
                    if (count > 0 && results.length >= count) {
                        return;
                    }
                    cursor = yield cursor.continue();
                }
                yield tx.done;
                if (errorMessage) {
                    throw errorMessage;
                }
                return results;
            }
            catch (error) {
                throw `Store ${storeName} ${error.toString()}`;
            }
        });
        // IDBIndex functions
        this.countFromIndex = (storeName, indexName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                let result = yield tx.store.index(indexName).count(key !== null && key !== void 0 ? key : undefined);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.countFromIndexByKeyRange = (storeName, indexName, lower, upper, lowerOpen, upperOpen) => __awaiter(this, void 0, void 0, function* () {
            try {
                return yield this.countFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getFromIndex = (storeName, indexName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = yield tx.store.index(indexName).get(key);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllFromIndex = (storeName, indexName, key, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = yield tx.store.index(indexName).getAll(key !== null && key !== void 0 ? key : undefined, count !== null && count !== void 0 ? count : undefined);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllFromIndexByKeyRange = (storeName, indexName, lower, upper, lowerOpen, upperOpen, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return yield this.getAllFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllFromIndexByArrayKey = (storeName, indexName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const dx = tx.store.index(indexName);
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(yield dx.getAll(element));
                }
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getKeyFromIndex = (storeName, indexName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = yield tx.store.index(indexName).getKey(key);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllKeysFromIndex = (storeName, indexName, key, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const results = yield tx.store.index(indexName).getAllKeys(key !== null && key !== void 0 ? key : undefined, count !== null && count !== void 0 ? count : undefined);
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllKeysFromIndexByKeyRange = (storeName, indexName, lower, upper, lowerOpen, upperOpen, count) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                return yield this.getAllKeysFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.getAllKeysFromIndexByArrayKey = (storeName, indexName, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readonly');
                const dx = tx.store.index(indexName);
                let results = [];
                for (let index = 0; index < key.length; index++) {
                    const element = key[index];
                    results = results.concat(yield dx.getAllKeys(element));
                }
                yield tx.done;
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.queryFromIndex = (storeName_1, indexName_1, key_1, filter_1, ...args_1) => __awaiter(this, [storeName_1, indexName_1, key_1, filter_1, ...args_1], void 0, function* (storeName, indexName, key, filter, count = 0, skip = 0) {
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
                let cursor = yield tx.store.index(indexName).openCursor(key !== null && key !== void 0 ? key : undefined);
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
                        return;
                    }
                    if (count > 0 && results.length >= count) {
                        return;
                    }
                    cursor = yield cursor.continue();
                }
                yield tx.done;
                if (errorMessage) {
                    throw errorMessage;
                }
                return results;
            }
            catch (error) {
                throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
            }
        });
        this.add = (storeName, data, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                data = this.checkForKeyPath(tx.store, data);
                const result = yield tx.store.add(data, key !== null && key !== void 0 ? key : undefined);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.put = (storeName, data, key) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                const result = yield tx.store.put(data, key !== null && key !== void 0 ? key : undefined);
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.delete = (storeName, id) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                yield tx.store.delete(id);
                yield tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.batchAdd = (storeName, data) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                let result = [];
                data.forEach((element) => __awaiter(this, void 0, void 0, function* () {
                    let item = this.checkForKeyPath(tx.store, element);
                    result.push(yield tx.store.add(item));
                }));
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.batchPut = (storeName, data) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                let result = [];
                data.forEach((element) => __awaiter(this, void 0, void 0, function* () {
                    result.push(yield tx.store.put(element));
                }));
                yield tx.done;
                return result;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.batchDelete = (storeName, ids) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                ids.forEach((element) => __awaiter(this, void 0, void 0, function* () {
                    yield tx.store.delete(element);
                }));
                yield tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
        this.clearStore = (storeName) => __awaiter(this, void 0, void 0, function* () {
            try {
                if (!this.dbInstance)
                    throw IndexedDbManager.E_DB_CLOSED;
                const tx = this.dbInstance.transaction(storeName, 'readwrite');
                yield tx.store.clear();
                yield tx.done;
            }
            catch (error) {
                throw `Store ${storeName}, ${error.toString()}`;
            }
        });
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
        var _a;
        try {
            const newStore = upgradeDB.createObjectStore(store.name, {
                keyPath: this.getKeyPath(store.keyPath),
                autoIncrement: store.autoIncrement
            });
            for (var index of store.indexes) {
                try {
                    newStore.createIndex(index.name, (_a = this.getKeyPath(index.keyPath)) !== null && _a !== void 0 ? _a : index.name, {
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