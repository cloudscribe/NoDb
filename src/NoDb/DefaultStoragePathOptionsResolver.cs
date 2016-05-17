// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-05-17
// 


using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace NoDb
{
    public class DefaultStoragePathOptionsResolver : IStoragePathOptionsResolver
    {
        public DefaultStoragePathOptionsResolver(
            IHostingEnvironment appEnv)
        {
            env = appEnv;
        }

        private IHostingEnvironment env;

        public Task<StoragePathOptions> Resolve(string projectId)
        {
            //if (!IsValidProjectId(projectId))
            //{
            //    throw new ArgumentException("invalid blog id");
            //}

            var result = new StoragePathOptions();
            result.AppRootFolderPath = env.ContentRootPath;
            //result.ProjectIdFolderName = projectId;

            return Task.FromResult(result);
        }
    }
}
