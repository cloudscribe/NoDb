# NoDb - a "no database" file system storage for .NET Core/ASP.NET Core

I named the project NoDb in a nod to the NoSql movement, which uses document databases rather than the more traditional, relational SQL databases. Not using a database at all seems to me taking the idea one step  further than NoSql. Myself, I think relational/SQL databases, NoSql document databases, and file system storage such as NoDb all have their places, according to the needs of the project. I think one size does not fit all and you should think deeply about the actual needs and requirements of your project in order to choose wisely. I recommend abstracting the data access in a way that you can easily plug in different storage later if needs change.

If you have questions please visit our community forum https://www.cloudscribe.com/forum

### Build Status

| Windows  | Linux/Mac |
| ------------- | ------------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/sda05djph49420d0?svg=true)](https://ci.appveyor.com/project/joeaudette/nodb)  | [![Build Status](https://travis-ci.org/cloudscribe/NoDb.svg?branch=master)](https://travis-ci.org/cloudscribe/NoDb)  |

## Rationale

Not every project needs a database, there can be many benefits to not using one including performance, scalability, portability, lower cost, less installation steps, ease of site migration and ease of making backup copies of the entire site/web application. It should even be possible to make a site/web application that runs from a thumb drive, or point in time backups that can run from CD/DVDROM.

In fact, for blogs, there has been kind of a trend towards using [Static Site Generators](https://www.staticgen.com/). By storing objects and content as json files, NoDb can get some of the same benefits and be used in a similar way to using a static site generator. For example you could host a localhost or intranet version of your site for producing and reviewing content, then when ready to publish you could commit the changes to a git repository and then do deployment from git to Azure for example, which would give you a highly scaleable site without the need or cost of a database and with a complete history of changes in git. 

Even more recently I've been hearing terms such as ["flat file cms"](https://www.google.com/#q=flat+file+cms), there is a PHP flat file cms named [grav](https://getgrav.org/) that is built by some Drupal developers, I heard about it on the [github podcast](https://soundcloud.com/githubcommunitycast/episode1) a while back. To me the term "flat file" is a bit of a misnomer, when I think of "flat file" I think of flat data structures like comma separated or pipe delimited, the data in what I call flat files is tabular in nature. Storing objects that have been serialized to json is not a flat structure and not what I would call a flat file. Perhaps "text file" would be a more accurate term, but whatever you call it, there are plenty of scenarios where this kind of storage is more than sufficient.

This NoDb project was born as part of my [SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent) project. That project started out as porting [Mads Kristensen's MiniBlog](https://github.com/madskristensen/MiniBlog) to ASP.NET Core. MiniBlog was already using xml file storage in the same format as [BlogEngine.NET](http://dotnetblogengine.net/) so I adopted the same format for blog posts. But then I also wanted to support simple cms pages in addition to blog posts and for that I decided to use json. Finally I refactored the code until I could use the same code for both xml posts and json pages, just plugging in different serializers. From there I started using it for pretty much any types I need to store, and then I got the idea to make NoDb its own separate project and code repository here.

## Who should use NoDb?

Personal blogs and sites and small brochure sites are good candidates for not using a database. It is particularly useful for building prototypes where you may come back later and implement storage using a relational or document database. With few moving parts, it is very easy to get going using NoDb storage without the overhead of creating a database and managing connection strings and connections.

The vast majority of sites on the internet get very little traffic. When you think about it the vast majority of websites on the internet are personal blogs and sites and small brochure or marketing sites. Therefore the vast majority of sites are candidates for not using a database.

## When Not to Use NoDb

*  If you have a lot of concurrent editing going on, you should probably use something more robust.
*  If the size or amount of data for your site/project is too much to load all of it into memory, then NoDb is probably not a good choice. But there are ways to avoid loading all the data at once if needed. For example a blog can accumulate a lot of posts over years and it might be better to not load all the posts at once. By [using a custom storagepathreolver, posts can be stored in year/month sub folders](https://github.com/joeaudette/cloudscribe.SimpleContent/blob/master/src/cloudscribe.SimpleContent.Storage.NoDb/PostStoragePathResolver.cs) and implementing some custom query logic we can selectively load more recent posts and only load older posts if/when they are actually requested instead of loading all posts at once. Individual items are retrieved by key so for some types you may never need to load them any way but by key and can avoid loading all the data for that type.
*  Beyond that, it is a judgement call and you should decide for yourself if your project would be a good fit for NoDb.

## How I plan to use it

I plan to use NoDb for most of my ASP.NET Core web projects at least in the begining unless I know in advance that the website is expected to get high volume transactions of any kind. But even then I may use it during the prototype phase and only change to more robust storage once the data model is well defined.

For a high traffic marketing or brochure site where the contents are not frequently changing, I will still use NoDb. 

When architecting my projects I will always abstract the data access behind a interfaces and implementations of interfaces, that way I can implement the initial implementations quick and easy with NoDb and later if the project does require a database, I can re-implement them using Entity Framework or MongoDb or DocumentDb. I can easily write code to import the data from the files and I can easily plugin a different implementations using dependency injection.

Databases have always been a bit of a friction point for unit testing. People often do elaborate mocking of data in order to test without the database because it can be dodgy to rely on a database connection during testing and tests may fail due to networking issues. When the data is just files stored on disk it can be a lot more friendly for testing even against the actual data in the system. Even if you use a database for production NoDb could be useful for mocking data for unit testing.

NoDb could also be useful as a backup tool. Even if I use a database for production I might find it useful to implement repositories in NoDb as a way to back up the project and its data so that I can actually run the backups from CD/DVDROM and be able to see what my site/project looked like at different points in time. Given two repositories that implement different storage for the same type, it would be very easy to write code to migrate data from one repository to another.

## Using NoDb

With NoDb it is very easy to store any class that is serializable to a string. The default StringSerializer uses NewtonSoft.Json, but if needed you can implement and plugin a custom serializer for your custom classes if the default serializer doesn't work for you.

By default, files are stored on disk like this:

    appRoot/[nodb_storage]/projects/[projectid]/[type]/[key].json
	
but if needed you can plugin your own custom StoragePathResolver for your type if you want to store things in a different location or using a different structure.

The primary classes are [BasicCommands](https://github.com/joeaudette/NoDb/blob/master/src/NoDb/BasicCommands.cs) and [BasicQueries](https://github.com/joeaudette/NoDb/blob/master/src/NoDb/BasicQueries.cs). Have those injected into your own repository class and use them to implement retrieval and storage of your serializable types. The best example code is in my [SimpleContent.Storage.NoDb](https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/src/cloudscribe.SimpleContent.Storage.NoDb) project, you can see how everything is wired up in the example.WebApp in that repository.

All the components of NoDb are loosley coupled so you can inject your own implementation to override what you want such as serialization or storage location. You can also inherit from BasicCommands and/or BasicQueries if you need to override any of the methods since they are virtual.

Basic CRUD (Create, Retrieve, Update, Delete) commands and queries are provided but you can load all of the type with queries.GetAllAsync and then query that any way you like using Linq, so you are not limited to the provided queries.

Note that NoDb does NOT provide any caching. Use of caching is a good idea but should be higher up the stack than NoDb. Think of NoDb just like the real metal of a database, you don't have output caching on sql queries, you cache things higher up the stack. I recommend implement your own repository that internally uses NoDb commands and queries, then wrap that with a CachingRepository using the decorator pattern. Unfortunately the built in DI (dependency injection) system for ASP.NET Core does not provide the functionality to implement the decorator pattern but you can do it easily using Autofac or most other advanced DI containers.


## Installation

Just add a dependency in your .csproj file to get the nuget

    <PackageReference Include="NoDb" Version="1.2.1" />
	
Then Visual Studio 2017 should automatically resolve the dependency, but if needed you can run dotnet restore from the command line in either the solution or project folder. You should use whatever the newest versiion is.

In Startup you then register services for any types that you want to persist with NoDb

    // if you want to plugin a custom serializer or pathresolver for your type,
	// register those before calling .AddNoDb<YourType>();
    //services.AddScoped<NoDb.IStringSerializer<YourType>, YourTypeSerializer>();
    //services.AddScoped<NoDb.IStoragePathResolver<YourType>, YourTypeStoragePathResolver>();
	
    services.AddNoDb<Page>();
	services.AddNoDb<Post>();

The above will register the BasicCommands and BasicQueries so you can take a constructor dependency wherever you need those injected, as seen in my SimpleContent project [PageQueries](https://github.com/cloudscribe/cloudscribe.SimpleContent/blob/master/src/cloudscribe.SimpleContent.Storage.NoDb/PageQueries.cs), [PageCommands](https://github.com/cloudscribe/cloudscribe.SimpleContent/blob/master/src/cloudscribe.SimpleContent.Storage.NoDb/PageCommands.cs), [PostQueries](https://github.com/cloudscribe/cloudscribe.SimpleContent/blob/master/src/cloudscribe.SimpleContent.Storage.NoDb/PostQueries.cs), and [PostCommands](https://github.com/cloudscribe/cloudscribe.SimpleContent/blob/master/src/cloudscribe.SimpleContent.Storage.NoDb/PostCommands.cs)

    
  
