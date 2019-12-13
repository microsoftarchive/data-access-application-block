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
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeUpdateDataSetFixture : UpdateDataSetFixture
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
        public void SqlModifyRowWithCommand()
        {
            base.ModifyRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlDeleteRowWithCommand()
        {
            base.DeleteRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlInsertRowWithCommand()
        {
            base.InsertRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlDeleteRowWithMissingInsertAndUpdateCommands()
        {
            base.DeleteRowWithMissingInsertAndUpdateCommands();
        }

        [TestMethod]
        public void SqlUpdateRowWithMissingInsertAndDeleteCommands()
        {
            base.UpdateRowWithMissingInsertAndDeleteCommands();
        }

        [TestMethod]
        public void SqlInsertRowWithMissingUpdateAndDeleteCommands()
        {
            base.InsertRowWithMissingUpdateAndDeleteCommands();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SqlUpdateDataSetWithAllCommandsMissing()
        {
            base.UpdateDataSetWithAllCommandsMissing();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SqlUpdateDataSetWithNullTable()
        {
            base.UpdateDataSetWithNullTable();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SqlUpdateSetWithNullDataSet()
        {
            base.UpdateSetWithNullDataSet();
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

        protected override void CreateStoredProcedures() { }

        protected override void DeleteStoredProcedures() { }

        protected override void AddTestData()
        {
            SqlCeDataSetHelper.AddTestData(db);
        }
    }
}
