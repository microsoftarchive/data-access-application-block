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
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests
{
    [TestClass]
    public class SqlCeUpdateDataSetWithTransactionsFixture : UpdateDataSetWithTransactionsFixture
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
        public void Dispose()
        {
            deleteCommand.Dispose();
            insertCommand.Dispose();
            updateCommand.Dispose();
            base.TearDown();
            transaction.Connection.Dispose();
            transaction.Dispose();
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }

        [TestMethod]
        public void SqlModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlAttemptToInsertBadRowInsideOfATransaction()
        {
            base.AttemptToInsertBadRowInsideOfATransaction();
        }

        protected override DataSet GetDataSetFromTable()
        {
            using (DbCommand selectCommand = db.GetSqlStringCommand("select * from region"))
            {
                return db.ExecuteDataSet(selectCommand, transaction);
            }
        }

        protected override DataSet GetDataSetFromTableWithoutTransaction()
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

        protected override void AddTestData()
        {
            SqlCeDataSetHelper.AddTestData(db);
        }
    }
}
