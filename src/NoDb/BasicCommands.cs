// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-05-15
// 


using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public class BasicCommands<T> : IBasicCommands<T> where T : class
    {
        public BasicCommands(
            ILogger<BasicCommands<T>> logger,
            IStoragePathResolver<T> pathResolver,
            IStringSerializer<T> serializer
            )
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (serializer == null) { throw new ArgumentNullException(nameof(serializer)); }
            if (pathResolver == null) { throw new ArgumentNullException(nameof(pathResolver)); }

            this.serializer = serializer;
            this.pathResolver = pathResolver;
            log = logger;
        }

        protected IStringSerializer<T> serializer;
        protected IStoragePathResolver<T> pathResolver;
        protected ILogger log;
        
        public virtual async Task CreateAsync(
            string projectId,
            string key,
            T obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            
            var pathToFile = await pathResolver.ResolvePath(
                projectId, 
                key, 
                obj, 
                serializer.ExpectedFileExtension, 
                true
                ).ConfigureAwait(false);

            if (File.Exists(pathToFile)) throw new InvalidOperationException("can't create file that already exists: " + pathToFile);

            var serialized = serializer.Serialize(obj);
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }
            
            
        }

        public virtual async Task UpdateAsync(
            string projectId,
            string key,
            T obj,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            
            var pathToFile = await pathResolver.ResolvePath(
                projectId, 
                key, 
                obj,
                serializer.ExpectedFileExtension,
                false).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't update file that doesn't exist: " + pathToFile);
            //TODO: if instead of deleting the existing file
            // we just replace its contents then it opens the possibility
            // for custom queries based on file creation and last modified dates
            // whereas by deleting the file we lose the original creation date
            File.Delete(pathToFile); // delete the old version
            

            var serialized = serializer.Serialize(obj);
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }

            
        }

        public virtual async Task DeleteAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            
            var pathToFile = await pathResolver.ResolvePath(
                projectId, 
                key,
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't delete item that does not exist: " + pathToFile);

            File.Delete(pathToFile);

            
        }
        

    }
}
