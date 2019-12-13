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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructDatabaseWithNullConnectionStringThrows()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            new TestDatabase(null, factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructDatabaseWithNullDbProviderFactoryThrows()
        {
            new TestDatabase("foo", null);
        }

        class TestDatabase : Database
        {
            public TestDatabase(string connectionString,
                                DbProviderFactory dbProviderFactory)
                : base(connectionString, dbProviderFactory) {}

            //protected override char ParameterToken
            //{
            //    get { return 'a'; }
            //}

            protected override void DeriveParameters(DbCommand discoveryCommand) {}
        }
    }
}
