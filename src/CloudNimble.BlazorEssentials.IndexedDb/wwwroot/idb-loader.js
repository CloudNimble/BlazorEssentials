import { IDB_VERSION } from './generated/idb-version';
let idbModule = null;
let isLoading = false;
let loadPromise = null;
export async function getOpenDB() {
    await ensureIdbLoaded();
    return idbModule.openDB;
}
export async function getDeleteDB() {
    await ensureIdbLoaded();
    return idbModule.deleteDB;
}
async function ensureIdbLoaded() {
    if (idbModule)
        return;
    if (isLoading && loadPromise) {
        await loadPromise;
        return;
    }
    isLoading = true;
    loadPromise = loadIdbWithFallback();
    try {
        idbModule = await loadPromise;
    }
    finally {
        isLoading = false;
        loadPromise = null;
    }
}
async function loadIdbWithFallback() {
    try {
        // Try online CDN first
        // @ts-ignore - Dynamic import from CDN
        const onlineModule = await import(`https://cdn.jsdelivr.net/npm/idb@${IDB_VERSION}/+esm`);
        console.log('Loaded idb from online CDN');
        return onlineModule;
    }
    catch (onlineError) {
        console.warn('Failed to load idb from CDN, falling back to bundled version:', onlineError);
        try {
            // Fallback to bundled version (will be available as static asset)
            // Use import.meta.url so the path resolves correctly at runtime regardless of
            // deployment location (e.g., /_content/BlazorEssentials.IndexedDb/lib/index.js)
            const libUrl = new URL('./lib/index.js', import.meta.url).href;
            const bundledModule = await import(libUrl);
            console.log('Loaded idb from bundled version');
            return bundledModule;
        }
        catch (localError) {
            console.error('Failed to load idb from bundled version:', localError);
            throw new Error('Unable to load idb library from any source');
        }
    }
}
//# sourceMappingURL=idb-loader.js.map