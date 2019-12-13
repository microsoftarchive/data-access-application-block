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
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class Given_GenericDatabaseInstance : ArrangeActAssert
    {
        protected Database Database { get; private set; }
        private const string northwind = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true";

        protected override void Arrange()
        {
            base.Arrange();

            Database = new GenericDatabase(northwind, SqlClientFactory.Instance);
        }
    }

    [TestClass]
    public class When_UsingGenericDatabase : Given_GenericDatabaseInstance
    {
        [TestMethod]
        public void Then_SupportsAsyncIsFalse()
        {
            Assert.IsFalse(Database.SupportsAsync);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Then_AsyncOperationThrows()
        {
            var command = Database.GetStoredProcCommand("Ten Most Popular Products");
            Database.BeginExecuteReader(command, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Then_AsyncAccessorThrows()
        {
            var accessor = Database.CreateSprocAccessor<object>("Ten Most Popular Products");
            accessor.BeginExecute(null, null);
        }
    }

}
