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

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// An interface representing an object that wraps a database operation.
    /// An Accessor is executed, at which point it will go out to the database
    /// and return a <see cref="IEnumerable{TResult}"/> of whatever type <typeparam name="TResult"/>
    /// is.
    /// </summary>
    public abstract class DataAccessor<TResult>
    {
        /// <summary>
        /// Execute the database operation synchronously, returning the
        /// <see cref="IEnumerable{TResult}"/> sequence containing the
        /// resulting objects.
        /// </summary> 
        /// <param name="parameterValues">Parameters to pass to the database.</param>
        /// <returns>The sequence of result objects.</returns>
        public abstract IEnumerable<TResult> Execute(params object[] parameterValues);
        
        /// <summary>Begin executing the database object asynchronously, returning
        /// a <see cref="IAsyncResult"/> object that can be used to retrieve
        /// the result set after the operation completes.</summary>
        /// <param name="callback">Callback to execute when the operation's results are available. May
        /// be null if you don't wish to use a callback.</param>
        /// <param name="state">Extra information that will be passed to the callback. May be null.</param>
        /// <param name="parameterValues">Parameters to pass to the database.</param>
        /// <remarks>This operation will throw if the underlying <see cref="Database"/> object does not
        /// support asynchronous operation.</remarks>
        /// <exception cref="InvalidOperationException">The underlying database does not support asynchronous operation.</exception>
        /// <returns>The <see cref="IAsyncResult"/> representing this operation.</returns>
        public abstract IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues);

        /// <summary>Complete an operation started by <see cref="BeginExecute"/>.</summary>
        /// <returns>The result sequence.</returns>
        public abstract IEnumerable<TResult> EndExecute(IAsyncResult asyncResult);
    }
}
