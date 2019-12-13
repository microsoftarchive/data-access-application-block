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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteDatasetFixture
    {
        TestConnectionString testConnection;
        const string queryString = "Select * from Region";
        const string storedProc = "Ten Most Expensive Products";
        Database db;

        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection = new TestConnectionString();
            testConnection.DeleteFile();
        }

        [TestMethod]
        public void CanRetriveDataSetFromSqlString()
        {
            DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedure()
        {
            DataSet dataSet = db.ExecuteDataSet(storedProc);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }

        [TestMethod]
        public void CanRetriveDataSetFromSqlStringWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(transaction, CommandType.Text, queryString);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedureWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(transaction, storedProc);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }
    }
}
