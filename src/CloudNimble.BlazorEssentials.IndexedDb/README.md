[![Build status](https://dev.azure.com/cloudnimble/BlazorEssentials/_apis/build/status/BlazorEssentials-CI)](https://dev.azure.com/cloudnimble/BlazorEssentials/_build/latest?definitionId=3)

[![Nuget](https://img.shields.io/nuget/v/BlazorEssentials.IndexedDb?style=flat-square)](https://www.nuget.org/packages/BlazorEssentials.IndexedDb/)

# [BlazorEssentials.IndexedDb](https://github.com/CloudNimble/BlazorEssentials/tree/main/src/CloudNimble.BlazorEssentials.IndexedDb)

This is a [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) library for accessing IndexedDB, it uses Jake Archibald's [idb library](https://github.com/jakearchibald/idb) for handling access to [IndexedDB API](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API).

It maps as closely to the browser IndexedDB API as possible, but in a .NET way, so you can use public [documentation](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API).

## Features

- A clean, simple, intuitive async-first .NET API for IndexedDB
- Create in-browser databases that feel more like the Entity Framework DbContext
- Support for multiple databases in the same application
- Uses Blazor JavaScript Isolation, no script installation required
- Supports .NET 8.0 using idb 8.0.0

### Code Provenance

This library is a fork of [BlazorIndexedDbJs](https://github.com/kattunga/BlazorIndexedDbJs), which in turn is a fork of [Blazor.IndexedDB](https://github.com/wtulloch/Blazor.IndexedDB).

The original library was licensed under the MIT License, and this library is as well.

### Differences from BlazorIndexedDbJs

- __Refactored to be .NET-first__
  - Objects prefixed with `IndexedDb` instead of `IDB` (the later conflicts with C# interface naming conventions)
  - Async method names now end in `Async`
- __More auto*magic*__
  - Constructor-based, attribute-based, or reflection-based initialization (see examples below)
  - The default codepath sets the name of your Database and ObjectStores based on the class or property name
- __Redesigned JSInterop__
  - Uses JS Isolation (Blazor dynamically loads the JS as modules)
  - Multiple database instance support
  - Better tracking for database open state for fewer runtime errors
- __Re-engineered build process__
  - Uses MSBuild-based TypeScript compilation
  - Eliminates WebPack
  - Uses SkyPack to load third-party modules like `idb` remotely, resulting in smaller package sizes

## Demo

You can see a demo of using `IndexedDbDatabase` and ViewModels together in our [Sample App](https://github.com/CloudNimble/BlazorEssentials/blob/main/src/CloudNimble.BlazorEssentials.TestApp/ViewModels/IndexedDbViewModel.cs).

## Using the library

### requires
NET 8.0 or newer

### Step 1: Install NuGet package

```
Install-Package BlazorEssentials.IndexedDb
```

or

```
dotnet add package BlazorEssentials.IndexedDb
```

### Step 2: Make the necessary classes available in your Razor files

Add the following to your _Imports.razor file:
```CSharp
@using CloudNimble.BlazorEssentials.IndexedDb
```

### Step 3: Create an `IndexedDBDatabase` class

This file should feel very similar to a DbContext class. Here is a basic implementation, using one of my favorite childhood restaurants as an example:

`Data/TheSpaghettiFactoryDb.cs`
```CSharp
using Microsoft.JSInterop;
using CloudNimble.BlazorEssentials.IndexedDb;

namespace BlazorEssentials.IndexedDb.Demo.Data
{

    public class TheFactoryDb: IndexedDbDatabase
    {

        public IndexedDbObjectStore Employees { get; }

        public TheSpaghettiFactoryDb(IJSRuntime jsRuntime): base(jsRuntime)
        {
            Name = "TheSpaghettiFactory";
            Version = 1;
        }

    }

}
```

Or you can customize it with attributes. In the below example:
- the database name will be "TheSpaghettiFactoryDb"
- the table name will be "FiredEmployees"
- the ID for the table is the "id" property
- you wll be expected to manage your own keys
- there will be an index on the `firstName` property for the FiredEmployees `IndexedDbObjectStore` (table)

`Data/TheSpaghettiFactoryDb.cs`
```CSharp
using Microsoft.JSInterop;
using CloudNimble.BlazorEssentials.IndexedDb;

namespace BlazorEssentials.IndexedDb.Demo.Data
{

    public class TheSpaghettiFactoryDb: IndexedDbDatabase
    {

        [ObjectStore(Name = "FiredEmployees", AutoIncrementKeys = false)]
        [Index(Name = "FirstName", KeyPath = "firstName")]]
        public IndexedDbObjectStore Employees { get; }

        public TheSpaghettiFactoryDb(IJSRuntime jsRuntime): base(jsRuntime)
        {
            Version = 1;
        }

    }

}
```

### Step 4. Add each database to your Blazor application's Dependency Injection container

For Blazor WebAssembly, in `program.cs`
```CSharp
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // RWM: You can add this as a Singleton because a WebAssembly app runs in the browser and only has one "user".
            builder.Services.AddSingleton<TheSpaghettiFactoryDb>();

            await builder.Build().RunAsync();
        }
    }
```

For Blazor Web, in `startup.cs`
```CSharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            // RWM: Here the database is scoped because each user has their own session.
            services.AddScoped<TheSpaghettiFactoryDb>();
        }
```


## Step 5: Use the database in your Blazor components

For the following examples we are going to assume that we have Person class which is defined as follows:

```CSharp
    public class Person
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
```

Mote that you DO NOT have to decorate your objects with any attributes. As IndexedDb is a NoSQL database, it is schema-less, so your object will be serialized and deserialized using the default JSON serializer.

You can also mix types in the same ObjectStore (table), but be careful is you use numbers for keys or objects may collide.

### Accessing the IndexedDbDatabase

To use IndexedDB in a component or page, first inject the `IndexedDbDatabase` instance, in this case the `TheSpaghettifactoryDb` class.

```CSharp
@inject TheSpaghettiFactoryDb database
```

### Open database
This will create the database if it not exists and will upgrade schema to new version if it is older.

__NOTE:__ Query calls will do this automatically if the database is not already open.
```CSharp
await database.Open()
```

### Getting all records from a store
```CSharp
var people = await database.Employees.GetAllAsync<Person>();
```

### Get one record by Id
```CSharp
var person = await database.Employees.GetAsync<long, Person>(id);
```

### Getting one record using an index
```CSharp
var person = await database.Employees.FirstName.GetAsync<string, Person>("John");
```

### Getting all records from an index
```CSharp
var people = await database.Employees.FirstName.GetAllAsync<string, Person>("John");
```

### Adding a record to an IDBObjectStore
```CSharp
var newPerson = new Person() {
    FirstName = "John",
    LastName = "Doe"
};

await database.Employees.AddAsync(newPerson);
```

### Updating a record
```CSharp
await database.Employees.PutAsync<Person>(recordToUpdate)
```

### Deleting a record
```CSharp
await database.Employees.DeleteAsync<int>(id)
```

### Clear all records from a store
```CSharp
await database.Employees.ClearAsync()
```

### Deleting the database
```CSharp
await database.DeleteDatabaseAsync()
```

## API

#### [IndexedDbDatabase](https://developer.mozilla.org/en-US/docs/Web/API/IDBDatabase)

**Properties**

##### [name](https://developer.mozilla.org/en-US/docs/Web/API/IDBDatabase/name)
```CSharp
public string Name
```

##### [version](https://developer.mozilla.org/en-US/docs/Web/API/IDBDatabase/version)
```CSharp
public int Version
```

##### [objectStores]()
```CSharp
public IList<IndexedDBObjectStore> ObjectStores
```

**Constructor**
```CSharp
public IndexedDBDatabase(IJSRuntime jsRuntime)
```

**Methods**

##### [open()]()
```CSharp
public async Task OpenAsync();
```

##### [deleteDatabase()]()
```CSharp
public async Task DeleteDatabaseAsync();
```

#### [IndexedDbObjectStore](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore)

**Properties**

##### [name](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/name)
```CSharp
public string Name
```

##### [keyPath](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/keyPath)
```CSharp
public string? KeyPath
```

##### [autoIncrement](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/autoIncrement)
```CSharp
public bool AutoIncrement
```

##### [Indexes]()
```CSharp
public IList<IndexedDbIndex> Indexes
```

##### [IndexedDbDatabase]()
```CSharp
public IndexedDbDatabase IndexedDbDatabase
```

**Constructors**

```CSharp
public IndexedDbObjectStore(IndexedDbDatabase database, ObjectStoreAttribute attribute = null);
public IndexedDbObjectStore(IndexedDbDatabase database, string name, string keyPath = "id", bool autoIncrement = false)
```

**Methods**

##### [add()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/add)
```CSharp
public async Task AddAsync<TData>(TData data);
public async Task AddAsync<TData, TKey>(TData data, TKey key);
```

##### [put()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/put)
```CSharp
public async Task PutAsync<TData>(TData data);
public async Task PutAsync<TData, TKey>(TData data, TKey key);
```

##### [delete()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/delete)
```CSharp
public async Task DeleteAsync<TKey>(TKey key);
```

##### [clear()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/clear)
```CSharp
public async Task ClearStoreAsync();
```

##### [Batch (add/put/delete) functions]()
```CSharp
public async Task BatchAddAsync<TData>(TData[] data);
public async Task BatchPutAsync<TData>(TData[] data);
public async Task BatchDeleteAsync<TKey>(TKey[] key);
```

##### [count()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/count)
```CSharp
public async Task<int> CountAsync();
public async Task<int> CountAsync<TKey>(TKey key);
public async Task<int> CountAsync<TKey>(IDBKeyRange<TKey> key);
```

##### [get()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/get)
```CSharp
public async Task<TResult?> GetAsync<TKey, TResult>(TKey key);
```

##### [getAll()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/getAll)
```CSharp
public async Task<List<TResult>> GetAllAsync<TResult>(int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey key, int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(IDBKeyRange<TKey> key, int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey[] key);
```

##### [getAllKeys()](https://developer.mozilla.org/en-US/docs/Web/API/IDBObjectStore/getAllKeys)
```CSharp
public async Task<List<TResult>> GetAllKeysAsync<TResult>(int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey key, int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(IDBKeyRange<TKey> key, int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey[] key);
```

##### [Query](#advanced-query-functions)
```CSharp
public async Task<List<TResult>> QueryAsync<TResult>(string filter, int? count = null, int? skip = null);
public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, TKey key, int? count = null, int? skip = null);
public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, IDBKeyRange<TKey> key, int? count = null, int? skip = null)
```

#### [IndexedDbIndex](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex)

**Properties**

##### [name](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/name)
```CSharp
public string Name
```

##### [keyPath](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/keyPath)
```CSharp
public string KeyPath
```

##### [multiEntry](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/multiEntry)
```CSharp
public bool MultiEntry
```

##### [unique](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/unique)
```CSharp
public bool Unique
```

##### [objectStore](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/objectStore)
```CSharp
public IndexedDbObjectStore ObjectStore
```

**Constructor**
```CSharp
public IndexedDbIndex(IndexedDbObjectStore idbStore, string name, string keyPath, bool multiEntry = false, bool unique = false);
```

**Methods**

##### [count()](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/count)
```CSharp
public async Task<int> CountAsync(string indexName);
public async Task<int> CountAsync<TKey>(TKey key);
public async Task<int> CountAsync<TKey>(IDBKeyRange<TKey> key);
```

##### [get()](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/get)
```CSharp
public async Task<TResult> GetAsync<TKey, TResult>(TKey queryValue);
```

##### [getAll()](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/getAll)
```CSharp
public async Task<List<TResult>> GetAllAsync<TResult>(int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey key, int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(IDBKeyRange<TKey> key, int? count = null);
public async Task<List<TResult>> GetAllAsync<TKey, TResult>(TKey[] key);
```

##### [getKey()](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/getKey)
```CSharp
public async Task<TResult> GetKeyAsync<TKey, TResult>(TKey queryValue);
```

##### [getAllKeys()](https://developer.mozilla.org/en-US/docs/Web/API/IDBIndex/getAllKeys)
```CSharp
public async Task<List<TResult>> GetAllKeysAsync<TResult>(int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey key, int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(IDBKeyRange<TKey> key, int? count = null);
public async Task<List<TResult>> GetAllKeysAsync<TKey, TResult>(TKey[] key);
```

##### [Query](#advanced-query-functions)
```CSharp
public async Task<List<TResult>> QueryAsync<TResult>(string filter, int? count = null, int? skip = null);
public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, TKey key, int? count = null, int? skip = null);
public async Task<List<TResult>> QueryAsync<TKey, TResult>(string filter, IDBKeyRange<TKey> key, int? count = null, int? skip = null)
```

## Advanced query functions

The filter expression is the body of a function that receives de parameter `obj` than handle each record of ObjectStore.
The function must return an Object of type TResult, that will be included in the ```List<TResult>``` result and can be one of the following options:
* the same object
* a new object
* an array of new objects (unwind)
* undefined (record is not included in result)

for example, return a list of objects that contains the world `"per"` in property `firstName` ordered using index `lastName`.
```CSharp
List<Person> result = await theFactoryDb.Store("people").Index("lastName").Query<Person>(
    "if (obj.firstName.toLowerCase().includes('per')) return obj;"
);
```