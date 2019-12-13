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
using System.Data.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    /// <summary>
    /// Used by a few test fixtures to simplify the code to rollback a transaction
    /// </summary>
    public class RollbackTransactionWrapper : IDisposable
    {
        private DbTransaction transaction;

        public RollbackTransactionWrapper(DbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        public DbTransaction Transaction
        {
            get { return transaction; }
        }
    }
}

