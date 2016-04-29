// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-29
// Last Modified:           2016-04-29
// 

using System.Threading.Tasks;

namespace NoDb.Helpers
{
    public interface IDataMigrator<T> where T : class
    {
        Task<bool> MigrateData(string sourceProjectId, string destinationProjectId);
    }
}
