// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2018-10-10
// 


using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    

    /// <summary>
    /// T must be a class serializable to json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBasicCommands<T> : ICreateCommand<T> where T : class
    {
        
        Task UpdateAsync(
            string projectId,
            string key,
            T obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task DeleteAsync(
            string projectId, 
            string key, 
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }


}
