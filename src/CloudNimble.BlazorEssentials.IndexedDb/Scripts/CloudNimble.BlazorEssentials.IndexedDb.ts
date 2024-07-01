//@ts-ignore
import { openDB, deleteDB, IDBPDatabase, IDBPObjectStore } from 'https://cdn.skypack.dev/idb';

/**
 * Allows for managing multiple instances of the IndexedDbManager, one for each database name.
 */
var instanceManager = {};

/**
 * Creates a new instance of the IndexedDbManager for the specified database name.
 */
export function createInstance(databaseName: string) {
    instanceManager[databaseName] = new IndexedDbManager();
}

export function openDatabase(database: IDatabase) {
    ensureDatabaseInstance(database.name);
    return instanceManager[database.name].open(database);
}

export function deleteDatabase(databaseName: string) {
    ensureDatabaseInstance(databaseName);
    instanceManager[databaseName].deleteDatabase();
}

export function closeDatabase(databaseName: string) {
    ensureDatabaseInstance(databaseName);
    instanceManager[databaseName].close();
}

export function getDbSchema(databaseName: string): Promise<IDatabase> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getDbSchema();
}

export function count(databaseName: string, storeName: string, key?: any): Promise<number> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].count(storeName, key);
}

export function countByKeyRange(databaseName: string, storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean): Promise<number> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countByKeyRange(storeName, lower, upper, lowerOpen, upperOpen);
}

export function get(databaseName: string, storeName: string, key: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].get(storeName, key);
}

export function getAll(databaseName: string, storeName: string, key?: any, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAll(storeName, key, count);
}

export function getAllByKeyRange(databaseName: string, storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllByKeyRange(storeName, lower, upper, lowerOpen, upperOpen, count);
}

export function getAllByArrayKey(databaseName: string, storeName: string, key: any[]): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllByArrayKey(storeName, key);
}

export function getKey(databaseName: string, storeName: string, key: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getKey(storeName, key);
}

export function getAllKeys(databaseName: string, storeName: string, key?: any, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeys(storeName, key, count);
}

export function getAllKeysByKeyRange(databaseName: string, storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysByKeyRange(storeName, lower, upper, lowerOpen, upperOpen, count);
}

export function getAllKeysByArrayKey(databaseName: string, storeName: string, key: any[]): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysByArrayKey(storeName, key);
}

export function query(databaseName: string, storeName: string, key: any, filter: string, count: number = 0, skip: number = 0): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].query(storeName, key, filter, count, skip);
}

export function countFromIndex(databaseName: string, storeName: string, indexName: string, key?: any): Promise<number> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countFromIndex(storeName, indexName, key);
}

export function countFromIndexByKeyRange(databaseName: string, storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean): Promise<number> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].countFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen);
}

export function getFromIndex(databaseName: string, storeName: string, indexName: string, key: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getFromIndex(storeName, indexName, key);
}

export function getAllFromIndex(databaseName: string, storeName: string, indexName: string, key?: any, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndex(storeName, indexName, key, count);
}

export function getAllFromIndexByKeyRange(databaseName: string, storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen, count);
}

export function getAllFromIndexByArrayKey(databaseName: string, storeName: string, indexName: string, key: any[]): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllFromIndexByArrayKey(storeName, indexName, key);
}

export function getKeyFromIndex(databaseName: string, storeName: string, indexName: string, key: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getKeyFromIndex(storeName, indexName, key);
}

export function getAllKeysFromIndex(databaseName: string, storeName: string, indexName: string, key?: any, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndex(storeName, indexName, key, count);
}

export function getAllKeysFromIndexByKeyRange(databaseName: string, storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndexByKeyRange(storeName, indexName, lower, upper, lowerOpen, upperOpen, count);
}

export function getAllKeysFromIndexByArrayKey(databaseName: string, storeName: string, indexName: string, key: any[]): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].getAllKeysFromIndexByArrayKey(storeName, indexName, key);
}

export function queryFromIndex(databaseName: string, storeName: string, indexName: string, key: any, filter: string, count: number = 0, skip: number = 0): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].queryFromIndex(storeName, indexName, key, filter, count, skip);
}

export function add(databaseName: string, storeName: string, data: any, key?: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].add(storeName, data, key);
}

export function put(databaseName: string, storeName: string, data: any, key?: any): Promise<any> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].put(storeName, data, key);
}

export function deleteRecord(databaseName: string, storeName: string, id: any): Promise<void> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].delete(storeName, id);
}

export function batchAdd(databaseName: string, storeName: string, data: any[]): Promise<any[]> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchAdd(storeName, data);
}

export function batchPut(databaseName: string, storeName: string, data: any[]): Promise<any[]> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchPut(storeName, data);
}

export function batchDelete(databaseName: string, storeName: string, ids: any[]): Promise<void> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].batchDelete(storeName, ids);
}

export function clearStore(databaseName: string, storeName: string): Promise<void> {
    ensureDatabaseInstance(databaseName);
    return instanceManager[databaseName].clearStore(storeName);
}

/**
 * Gets the schema for the specified database.
 */
function ensureDatabaseInstance(databaseName: string) {
    if (!instanceManager[databaseName]) {
        createInstance(databaseName);
    }
}

/**
 * Manages a particular instance of an IndexedDb database.
 */
export class IndexedDbManager {

    static E_DB_CLOSED: string = "Database is closed";

    private dbInstance?: IDBPDatabase = undefined;
    private databaseName: string = "";

    constructor() { }

    public open = async (database: IDatabase): Promise<boolean> => {
        var upgradeError = "";
        try {
            if (!this.dbInstance || this.dbInstance.version < database.version) {
                if (this.dbInstance) {
                    this.dbInstance.close();
                    this.dbInstance = undefined;
                }
                this.dbInstance = await openDB(database.name, database.version, {
                    upgrade(db, oldVersion, newVersion, transaction) {
                        try {
                            IndexedDbManager.upgradeDatabase(db, oldVersion, newVersion, database);
                        } catch (error) {
                            upgradeError = error.toString();
                            throw(error);
                        }
                    },
                });
            }
            return true;
        } catch (error) {
            throw error.toString()+' '+upgradeError;
        }
        return false;
    }

    public close = (): void => {
        try {
            this.dbInstance?.close();
            this.dbInstance = undefined;
        } catch (error) {
            throw error.toString();
        }
    }

    public deleteDatabase = async(): Promise<void> => {
        try {
            this.close();
            await deleteDB(this.databaseName);

        } catch (error) {
            throw `Database ${this.databaseName}, ${error.toString()}`;
        }
    }

    public getDbSchema = async () : Promise<IDatabase> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const dbInstance = this.dbInstance;

            const dbInfo: IDatabase = {
                name: dbInstance.name,
                version: dbInstance.version,
                objectStores: []
            }

            for (let s = 0; s < dbInstance.objectStoreNames.length; s++) {
                let dbStore = dbInstance.transaction(dbInstance.objectStoreNames[s], 'readonly').store;
                let objectStore: IObjectStore = {
                    name: dbStore.name,
                    keyPath: Array.isArray(dbStore.keyPath) ? dbStore.keyPath.join(',') : dbStore.keyPath,
                    autoIncrement: dbStore.autoIncrement,
                    indexes: []
                }
                for (let i = 0; i < dbStore.indexNames.length; i++) {
                    const dbIndex = dbStore.index(dbStore.indexNames[i]);
                    let index: IIndex = {
                        name: dbIndex.name,
                        keyPath: Array.isArray(dbIndex.keyPath) ? dbIndex.keyPath.join(',') : dbIndex.keyPath,
                        multiEntry: dbIndex.multiEntry,
                        unique: dbIndex.unique
                    }
                    objectStore.indexes.push(index);
                }
                dbInfo.objectStores.push(objectStore);
            }

            return dbInfo;
        } catch (error) {
            throw `Database ${this.databaseName}, ${error.toString()}`;
        }
    }

    // IDBObjectStore
    public count = async (storeName: string, key?: any): Promise<number> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let result = await tx.store.count(key ?? undefined);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public countByKeyRange = async (storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean): Promise<number> => {
        try {
            return await this.count(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public get = async (storeName: string, key: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let result = await tx.store.get(key);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAll = async (storeName: string, key?: any, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let results = await tx.store.getAll(key ?? undefined, count ?? undefined);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAllByKeyRange = async (storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            return await this.getAll(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAllByArrayKey = async (storeName: string, key: any[]): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let results: any[] = [];

            for (let index = 0; index < key.length; index++) {
                const element = key[index];
                results = results.concat(await tx.store.getAll(element));
            }

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getKey = async (storeName: string, key: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let result = await tx.store.getKey(key);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAllKeys = async (storeName: string, key?: any, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let results = await tx.store.getAllKeys(key ?? undefined, count ?? undefined);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAllKeysByKeyRange = async (storeName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            return await this.getAllKeys(storeName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public getAllKeysByArrayKey = async (storeName: string, key: any[]): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let results: any[] = [];

            for (let index = 0; index < key.length; index++) {
                const element = key[index];
                results = results.concat(await tx.store.getAllKeys(element));
            }

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public query = async (storeName: string, key: any, filter: string, count: number = 0, skip: number = 0): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            try {
                var func = new Function('obj', filter);
            } catch (error) {
                throw `${error.toString()} in filter { ${filter} }`
            }

            var row = 0;
            var errorMessage = "";

            let results: any[] = [];

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let cursor = await tx.store.openCursor(key ?? undefined);
            while (cursor) {
                if (!cursor) {
                    return;
                }
                try {
                    var out = func(cursor.value);
                    if (out) {
                        row ++;
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
                cursor = await cursor.continue();
            }

            await tx.done;

            if (errorMessage) {
                throw errorMessage;
            }

            return results;
        } catch (error) {
            throw `Store ${storeName} ${error.toString()}`;
        }
    }

    // IDBIndex functions
    public countFromIndex = async (storeName: string, indexName: string, key?: any): Promise<number> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let result = await tx.store.index(indexName).count(key ?? undefined);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public countFromIndexByKeyRange = async (storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean): Promise<number> => {
        try {
            return await this.countFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen));
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getFromIndex = async (storeName: string, indexName: string, key: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            const results = await tx.store.index(indexName).get(key);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllFromIndex = async (storeName: string, indexName: string, key?: any, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            const results = await tx.store.index(indexName).getAll(key ?? undefined, count ?? undefined);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllFromIndexByKeyRange = async (storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            return await this.getAllFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllFromIndexByArrayKey = async (storeName: string, indexName: string, key: any[]): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');
            const dx = tx.store.index(indexName);

            let results: any[] = [];

            for (let index = 0; index < key.length; index++) {
                const element = key[index];
                results = results.concat(await dx.getAll(element));
            }

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getKeyFromIndex = async (storeName: string, indexName: string, key: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            const results = await tx.store.index(indexName).getKey(key);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllKeysFromIndex = async (storeName: string, indexName: string, key?: any, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            const results = await tx.store.index(indexName).getAllKeys(key ?? undefined, count ?? undefined);

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllKeysFromIndexByKeyRange = async (storeName: string, indexName: string, lower: any, upper: any, lowerOpen: boolean, upperOpen: boolean, count?: number): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            return await this.getAllKeysFromIndex(storeName, indexName, IDBKeyRange.bound(lower, upper, lowerOpen, upperOpen), count);
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public getAllKeysFromIndexByArrayKey = async (storeName: string, indexName: string, key: any[]): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readonly');
            const dx = tx.store.index(indexName);

            let results: any[] = [];

            for (let index = 0; index < key.length; index++) {
                const element = key[index];
                results = results.concat(await dx.getAllKeys(element));
            }

            await tx.done;

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public queryFromIndex = async (storeName: string, indexName: string, key: any, filter: string, count: number = 0, skip: number = 0): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            try {
                var func = new Function('obj', filter);
            } catch (error) {
                throw `${error.toString()} in filter { ${filter} }`
            }

            var row = 0;
            var errorMessage = "";

            let results: any[] = [];

            const tx = this.dbInstance.transaction(storeName, 'readonly');

            let cursor = await tx.store.index(indexName).openCursor(key ?? undefined);
            while (cursor) {
                if (!cursor) {
                    return;
                }
                try {
                    var out = func(cursor.value);
                    if (out) {
                        row ++;
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
                cursor = await cursor.continue();
            }

            await tx.done;

            if (errorMessage) {
                throw errorMessage;
            }

            return results;
        } catch (error) {
            throw `Store ${storeName}, Index ${indexName}, ${error.toString()}`;
        }
    }

    public add = async (storeName: string, data: any, key?: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            data = this.checkForKeyPath(tx.store, data);

            const result = await tx.store.add(data, key ?? undefined);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public put = async (storeName: string, data: any, key?: any): Promise<any> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            const result = await tx.store.put(data, key ?? undefined);

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public delete = async (storeName: string, id: any): Promise<void> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            await tx.store.delete(id);

            await tx.done;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public batchAdd = async (storeName: string, data: any[]): Promise<any[]> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            let result: any[] = [];

            data.forEach(async element => {
                let item = this.checkForKeyPath(tx.store, element);
                result.push(await tx.store.add(item));
            });

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public batchPut = async (storeName: string, data: any[]): Promise<any[]> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            let result: any[] = [];

            data.forEach(async element => {
                result.push(await tx.store.put(element));
            });

            await tx.done;

            return result;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public batchDelete = async (storeName: string, ids: any[]): Promise<void> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            ids.forEach(async element => {
                await tx.store.delete(element);
            });

            await tx.done;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    public clearStore = async (storeName: string): Promise<void> => {
        try {
            if (!this.dbInstance) throw IndexedDbManager.E_DB_CLOSED;

            const tx = this.dbInstance.transaction(storeName, 'readwrite');

            await tx.store.clear();

            await tx.done;
        } catch (error) {
            throw `Store ${storeName}, ${error.toString()}`;
        }
    }

    private checkForKeyPath(objectStore: IDBPObjectStore<any, any>, data: any) {
        if (!objectStore.autoIncrement || !objectStore.keyPath) {
            return data;
        }

        if (typeof objectStore.keyPath !== 'string') {
            return data;
        }

        const keyPath = objectStore.keyPath as string;

        if (!data[keyPath]) {
            delete data[keyPath];
        }
        return data;
    }

    private static upgradeDatabase(upgradeDB: IDBPDatabase, oldVersion: number, newVersion: number | null, dbDatabase: IDatabase) {
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

    private static getKeyPath(keyPath?: string): string | string[] | undefined {
        if (keyPath) {
            var multiKeyPath = keyPath.split(',');
            return multiKeyPath.length > 1 ? multiKeyPath : keyPath;
        }
        else {
            return undefined;
        }
    }

    private static addNewStore(upgradeDB: IDBPDatabase, store: IObjectStore) {
        try {

            const newStore = upgradeDB.createObjectStore(store.name,
                {
                    keyPath: this.getKeyPath(store.keyPath),
                    autoIncrement: store.autoIncrement
                }
            );

            for (var index of store.indexes) {
                try {

                    newStore.createIndex(index.name,
                        this.getKeyPath(index.keyPath) ?? index.name,
                        {
                            multiEntry: index.multiEntry,
                            unique: index.unique
                        }
                    );
                } catch (error) {
                    throw `index ${index.name}, ${error.toString()}`;
                }
            }
        }
        catch (error) {
            throw `store ${store.name}, ${error.toString()}`;
        }
    }
}

/**Defines the Database to open or create.*/
export interface IDatabase {
    /**the name of the database*/
    name: string;
    /**The version for this instance. This value is used when opening a database to determine if it needs to be updated*/
    version: number;
    /**Defines the stores to be created in the database defined as IStoreSchema*/
    objectStores: IObjectStore[];
}

/**Defines a store to be created in the database. */
export interface IObjectStore {
    name: string;
    keyPath?: string;
    autoIncrement: boolean;
    indexes: IIndex[];
}
/** */

/**Index definition for a store */
export interface IIndex {
    name: string;
    keyPath?: string;
    multiEntry: boolean;
    unique: boolean;
}

export interface IInformation {
    version: number;
    objectStoreNames: string[];
}
