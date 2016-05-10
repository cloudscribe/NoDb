// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-29
// Last Modified:           2016-04-29
// 

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace NoDb.Helpers
{
    public class DataMigrator<T> : IDataMigrator<T> where T : class
    {
        public DataMigrator(
            ILogger<DataMigrator<T>> logger,
            IKeyResolver<T> keyResolver,
            IGetAllQuery<T> sourceQuery,
            ICreateCommand<T> targetCommand,
            DataMigratorOptions options = null)
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (keyResolver == null) { throw new ArgumentNullException(nameof(keyResolver)); }
            if (sourceQuery == null) { throw new ArgumentNullException(nameof(sourceQuery)); }
            if (targetCommand == null) { throw new ArgumentNullException(nameof(targetCommand)); }

            log = logger;
            this.keyResolver = keyResolver;
            this.sourceQuery = sourceQuery;
            this.targetCommand = targetCommand;
            this.options = options ?? new DataMigratorOptions();
        }

        protected ILogger log;
        protected IKeyResolver<T> keyResolver;
        protected IGetAllQuery<T> sourceQuery;
        protected ICreateCommand<T> targetCommand;
        protected DataMigratorOptions options;

        public virtual async Task<bool> MigrateData(string sourceProjectId, string destinationProjectId)
        {
            bool noErrors = true;

            var list = await sourceQuery.GetAllAsync(sourceProjectId).ConfigureAwait(false);
            foreach(var obj in list)
            {
                try
                {
                    string key = keyResolver.GetKey(obj);
                    await targetCommand.CreateAsync(destinationProjectId, key, obj).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    noErrors = false;
                    if(options.ContinueAfterError)
                    {
                        log.LogError("swallowed exception", ex);
                        continue;
                    }
                    else
                    {
                        throw ex;
                    }
                    
                }
            }

            return noErrors;
        }
    }
}
