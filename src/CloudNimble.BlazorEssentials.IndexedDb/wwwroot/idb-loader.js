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
        // Try online CDN first for latest version
        // @ts-ignore - Dynamic import from CDN
        const onlineModule = await import('https://cdn.skypack.dev/idb');
        console.log('Loaded idb from online CDN');
        return onlineModule;
    }
    catch (onlineError) {
        console.warn('Failed to load idb from CDN, falling back to bundled version:', onlineError);
        try {
            // Fallback to bundled version (will be available as static asset)
            // In Blazor, this would be loaded via JS interop or as a module
            const bundledModule = await import('../wwwroot/lib/index.js');
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