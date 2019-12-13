/*
Copyright 2013 Microsoft Corporation
Licensed under the Apache License, Version 2.0 (the "License");

you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Represents the operation of mapping a <see cref="IDataReader"/> to an enumerable of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type this result set mapper will be mapping to.</typeparam>
    public interface IResultSetMapper<TResult>
    {

        /// <summary>
        /// When implemented by a class, returns an enumerable of <typeparamref name="TResult"/> based on <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to map.</param>
        /// <returns>The enurable of <typeparamref name="TResult"/> that is based on <paramref name="reader"/>.</returns>
        IEnumerable<TResult> MapSet(IDataReader reader);
    
    }
}
