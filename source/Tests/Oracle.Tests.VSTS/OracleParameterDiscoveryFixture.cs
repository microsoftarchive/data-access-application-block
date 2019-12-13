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
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleParameterDiscoveryFixture
    {
        ParameterCache cache;
        Database db;
        DbConnection connection;
        ParameterDiscoveryFixture baseFixture;
        DbCommand storedProcedure;

        [TestInitialize]
        public void TestInitialize()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
            connection = db.CreateConnection();
            connection.Open();
            storedProcedure.Connection = connection;
            cache = new ParameterCache();

            baseFixture = new ParameterDiscoveryFixture(storedProcedure);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }

        [TestMethod]
        public void CanGetParametersForStoredProcedure()
        {
            cache.SetParameters(storedProcedure, db);
            Assert.AreEqual(2, storedProcedure.Parameters.Count);
            Assert.AreEqual("CUR_OUT", ((IDataParameter)storedProcedure.Parameters["CUR_OUT"]).ParameterName);
            Assert.AreEqual("VCUSTOMERID", ((IDataParameter)storedProcedure.Parameters["VCUSTOMERID"]).ParameterName);
        }

        [TestMethod]
        public void IsCacheUsed()
        {
            ParameterDiscoveryFixture.TestCache testCache = new ParameterDiscoveryFixture.TestCache();
            testCache.SetParameters(storedProcedure, db);

            DbCommand storedProcDuplicate = db.GetStoredProcCommand("CustOrdersOrders");
            storedProcDuplicate.Connection = connection;
            testCache.SetParameters(storedProcDuplicate, db);

            Assert.IsTrue(testCache.CacheUsed, "Cache is not used");
        }

        [TestMethod]
        public void CanDiscoverFeaturesWhileInsideTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
                storedProcedure.Connection = transaction.Connection;
                storedProcedure.Transaction = transaction;

                db.DiscoverParameters(storedProcedure);

                Assert.AreEqual(2, storedProcedure.Parameters.Count);
            }
        }

        [TestMethod]
        public void CanCreateStoredProcedureCommand()
        {
            baseFixture.CanCreateStoredProcedureCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetParametersWithNullCommandThrows()
        {
            cache.SetParameters(null, db);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetParametersWithNullDatabaseThrows()
        {
            cache.SetParameters(storedProcedure, null);
        }
    }
}
