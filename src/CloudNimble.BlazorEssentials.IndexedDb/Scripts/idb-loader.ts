/**
 * Dynamically loads the idb library with fallback support.
 * Tries to load from CDN first (to get the latest version), then falls back to local node_modules if offline.
 */

import * as localIdb from 'idb';

let idbModule: any = null;
let loadingPromise: Promise<any> | null = null;

async function loadIdb() {
    if (idbModule) {
        return idbModule;
    }

    if (loadingPromise) {
        return loadingPromise;
    }

    loadingPromise = (async () => {
        try {
            // Try loading from CDN first for latest version
            // @ts-ignore - Dynamic import of CDN URL
            idbModule = await import(/* webpackIgnore: true */ 'https://cdn.skypack.dev/idb');
            console.log('BlazorEssentials.IndexedDb: Loaded idb from CDN');
        } catch (error) {
            console.warn('BlazorEssentials.IndexedDb: CDN unavailable, falling back to local idb', error);
            // Fallback to local bundled version
            idbModule = localIdb;
            console.log('BlazorEssentials.IndexedDb: Loaded idb from local bundle');
        }
        return idbModule;
    })();

    return loadingPromise;
}

export async function getOpenDB() {
    const module = await loadIdb();
    return module.openDB;
}

export async function getDeleteDB() {
    const module = await loadIdb();
    return module.deleteDB;
}

// Export type references for TypeScript (these will be resolved at compile time from the idb package)
export type { IDBPDatabase, IDBPObjectStore } from 'idb';
