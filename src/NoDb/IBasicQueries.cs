// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2018-10-10
// 


using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    

    public interface IBasicQueries<T> : IGetAllQuery<T> where T : class
    {
        Task<T> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<int> GetCountAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
        
        //Task<IEnumerable<T>> GetPage(
        //    string projectId,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    );

        //Task<IEnumerable<T>> GetPage(
        //    string projectId,
        //    DateTime modifiedBeginDate,
        //    DateTime modifiedEndDate,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    );
    }

    
}
