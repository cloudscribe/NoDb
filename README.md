# NoDb - a "no database" file system storage for .NET Core/ASP.NET Core

I named the project NoDb in a nod to the NoSql movement, which uses document databases rather than the more traditional, relational SQL databases. Not using a database at all seems to me taking the idea one step  further than NoSql. Myself, I think relational/SQL databases, NoSql document databases, and file system storage such as NoDb all have their places, according to the needs of the project. I think one size does not fit all and you should think deeply about the goals of your project and choose wisely.

If you have questions or just want to say hello, join me in the cloudscribe gitter chat
[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Rationale

Not every project needs a database, there can be many benefits to not using one including performance, scalability, portability, lower cost, less installation steps, ease of site migration and ease of making backup copies of the entire site/web application. It should even be possible to make a site/web application that runs from a thumb drive, or point in time backups that can run from CD/DVDROM.

In fact, for blogs, there has been kind of a trend towards using [Static Site Generators](https://www.staticgen.com/). By storing objects and content as json files it can get some of the same benefits and be used in a similar way to using a static site generator. For example you could host a localhost or intranet version of your site for producing and reviewing content, then when ready to publish you could commit the changes to a git repository and then do deployment from git to Azure for example, which would give you a highly scaleable site without the need or cost of a database and with a complete history of changes in git. 

Even more recently I've been hearing terms such as ["flat file cms"](https://www.google.com/#q=flat+file+cms), there is a PHP flat file cms named [grav](https://getgrav.org/) that is built by some Drupal developers, I heard about it on the [github podcast](https://soundcloud.com/githubcommunitycast/episode1) a while back. To me the term "flat file" is a bit of a misnomer, when I think of "flat file" I think of flat data structures like comma separated or pipe delimited, the data in what I call flat files is tabular in nature. Storing objects that have been serialized to json is not a flat structure and not what I would call a flat file. Perhaps "text file" would be a more accurate term, but whatever you call it, there are plenty of scenarios where this kind of storage is more than sufficient.

This NoDb project was born as part of my [SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent) project. That project started out as porting [Mads Kristensen's MiniBlog](https://github.com/madskristensen/MiniBlog) to ASP.NET Core. That project was already using xml file storage in the same format as [BlogEngine.NET](http://dotnetblogengine.net/) so I adopted the same format for blog posts. But then I also wanted to support simple cms pages in addition to blog posts and for that I decided to use json. Finally I refactored the code until I could use the same code for both xml posts and json pages, just plugging in different serializers. From there I started using it for pretty much any types I need to store, and then I got the idea to make NoDb its own separate project and code repository here.

## Who should use NoDb?

Personal blogs and sites and small brochure sites are good candidates for not using a database. It is particularly useful for building prototypes where you may come back later and implement storage using a relational or document database. With few moving parts, it is very easy to get going using NoDb storage without the overhead of creating a database and managing connection strings and connections.

The vast majority of sites on the internet get very little traffic. When you think about it the vast majority of websites on the internet are personal blogs and sites and small brochure or marketing sites. Therefore the vast majority of sites are candidates for not using a database.

## When Not to Use NoDb

*  If you have a lot of concurrent editing going on, you should probably use something more robust.
*  If the size or amount of data for your site/project is too much to load all of it into memory, then NoDb is probably not a good choice.
*  Beyond that, it is a judgement call and you should decide for yourself if your project would be a good fit for NoDb.

## How I plan to use it

I plan to use NoDb for most of my ASP.NET Core web projects at least in the begining unless I know in advance that the website is expected to get high volume transactions of any kind. 

For a high traffic marketing or brochure site where the contents are not frequently changing, I will still use NoDb. 

When architecting my projects I will always abstract the data access behind an interface implemented by a repository, that way I can implement the initial repository quick and easy with NoDb and later if the project does require a database, I can re-implement the repository using Entity Framework or MongoDb or DocumentDb. I can easily write code to import the data from the files and I can easily plugin a different repository implementation using dependency injection.

## Using NoDb

With NoDb it is very easy to store any class that is serializable to a string. The default StringSerializer uses NewtonSoft.Json, but if needed you can implement and plugin a custom serializer for your custom classes if the default serializer doesn't work for you.

Files are stored on disk like this:

    appRoot/[nodb_storage]/projects/[projectid]/[type]/[key].json

The primary classes are BasicCommands<T>, BasicQueries<T>, have those injected into your own repository class and use them to implement retrievsal and storage of your serializable types. The best example code is in my [SimpleContent.Storage.NoDb](https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/src/cloudscribe.SimpleContent.Storage.NoDb) project, you casn see how everything is wired up in the example.WebApp in that repository.


## Installation

Just add a dependency in your project.json file to get the nuget

    "NoDb": "1.0.0-*"
	
Then Visual Studio 2015 should automatically resolve the dependency, but if needed you can run dnu restore from the command line in either the solution or project folder.
  