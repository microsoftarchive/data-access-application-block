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
using System.Data.SqlServerCe;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeUpdateDataSetBehaviorsFixture : UpdateDataSetBehaviorsFixture
    {
        TestConnectionString testConnection;

        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);

            base.SetUp();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            deleteCommand.Dispose();
            insertCommand.Dispose();
            updateCommand.Dispose();
            base.TearDown();
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }

        [TestMethod]
        public void UpdateWithTransactionalBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                                 deleteCommand, UpdateBehavior.Transactional);
            }
            catch (SqlCeException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(4, resultTable.Rows.Count);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void UpdateWithStandardBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
            }
            catch (SqlCeException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(8, resultTable.Rows.Count);
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void UpdateWithContinueBehavior()
        {
            AddRowsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual(502, (int)resultTable.Rows[6]["RegionID"]);
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }

        [TestMethod]
        public void UpdateWithContinueBehaviorAndBadData()
        {
            DataRow errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                             deleteCommand, UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.IsTrue(errRow.HasErrors);
            Assert.AreEqual(10, resultTable.Rows.Count);
            Assert.AreEqual(500, (int)resultTable.Rows[4]["RegionID"]);
            Assert.AreEqual(502, (int)resultTable.Rows[6]["RegionID"]);
        }

        [TestMethod]
        public void SqlUpdateWithTransactionalBehavior()
        {
            base.UpdateWithTransactionalBehavior();
        }

        [TestMethod]
        public void SqlUpdateWithStandardBehavior()
        {
            base.UpdateWithStandardBehavior();
        }

        protected override DataSet GetDataSetFromTable()
        {
            SqlCeDatabase db = (SqlCeDatabase)base.db;
            return db.ExecuteDataSetSql("select * from region");
        }

        protected override void CreateDataAdapterCommands()
        {
            SqlCeDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures() {}

        protected override void DeleteStoredProcedures() {}
    }
}
