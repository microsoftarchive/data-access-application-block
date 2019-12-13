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
    public class OracleDataAccessTestsFixture
    {
        Database db;
        DataAccessTestsFixture baseFixture;

        [TestInitialize]
        public void TestInitialize()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            baseFixture = new DataAccessTestsFixture(db);
        }

        [TestMethod]
        public void CanGetResultSetBackWithParamaterizedQuery()
        {
            string sqlCommand = "SELECT RegionDescription FROM Region where regionId = :ID";
            DataSet ds = new DataSet();
            DbCommand cmd = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(cmd, ":ID", DbType.Int32, 4);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(cmd, ds, "Foo", transaction);
                    Assert.AreEqual(1, ds.Tables[0].Rows.Count);
                }
            }
        }

        // ********************************

        [TestMethod]
        public void CanGetNonEmptyResultSet()
        {
            baseFixture.CanGetNonEmptyResultSet();
        }

        [TestMethod]
        public void CanGetTablePositionally()
        {
            baseFixture.CanGetTablePositionally();
        }

        [TestMethod]
        public void CanGetNonEmptyResultSetUsingTransaction()
        {
            baseFixture.CanGetNonEmptyResultSetUsingTransaction();
        }

        [TestMethod]
        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
            baseFixture.CanGetNonEmptyResultSetUsingTransactionWithNullTableName();
        }

        [TestMethod]
        public void ExecuteDataSetWithCommand()
        {
            baseFixture.ExecuteDataSetWithCommand();
        }

        [TestMethod]
        public void ExecuteDataSetWithDbTransaction()
        {
            baseFixture.ExecuteDataSetWithDbTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotLoadDataSetWithEmptyTableName()
        {
            baseFixture.CannotLoadDataSetWithEmptyTableName();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNullCommand()
        {
            baseFixture.ExecuteNullCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullTransaction()
        {
            baseFixture.ExecuteCommandNullTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullDataset()
        {
            baseFixture.ExecuteCommandNullDataset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullCommand()
        {
            baseFixture.ExecuteCommandNullCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteCommandNullTableName()
        {
            baseFixture.ExecuteCommandNullTableName();
        }
    }
}
