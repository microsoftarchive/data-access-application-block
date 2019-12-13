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

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    /// <summary>
    /// Summary description for DatabaseConnectionWrapperFixture
    /// </summary>
    [TestClass]
    public class DatabaseConnectionWrapperFixture
    {
        private DbConnection connection;

        [TestInitialize]
        public void Setup()
        {
            var factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            var db = factory.CreateDefault();
            connection = db.CreateConnection();
            connection.Open();
        }

        [TestCleanup]
        public void Teardown()
        {
            if(connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }

        [TestMethod]
        public void ConnectionIsClosedWhenDisposingWrapper()
        {
            DatabaseConnectionWrapper wrapper;
            using(wrapper = new DatabaseConnectionWrapper(connection))
            {
            }

            AssertDisposed(wrapper);
        }

        [TestMethod]
        public void AddRefRequiresExtraClose()
        {
            var wrapper = new DatabaseConnectionWrapper(connection);
            using(wrapper.AddRef())
            {
            }

            AssertNotDisposed(wrapper);
        }

        [TestMethod]
        public void MultipleDisposesCleanupMultipleAddRefs()
        {
            // Start at refcount 1
            var wrapper = new DatabaseConnectionWrapper(connection);

            wrapper.AddRef();
            wrapper.AddRef();

            wrapper.Dispose();
            AssertNotDisposed(wrapper);

            wrapper.Dispose();
            AssertNotDisposed(wrapper);

            wrapper.Dispose();
            AssertDisposed(wrapper);
        }

        private void AssertDisposed(DatabaseConnectionWrapper wrapper)
        {
            Assert.IsTrue(wrapper.IsDisposed);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        private void AssertNotDisposed(DatabaseConnectionWrapper wrapper)
        {
            Assert.IsFalse(wrapper.IsDisposed);
            Assert.AreEqual(ConnectionState.Open, connection.State);
        }

    }
}
