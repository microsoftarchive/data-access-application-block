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
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlUpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();

            try
            {
                DeleteStoredProcedures();
            }
            catch { }
            CreateStoredProcedures();
            base.SetUp();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void SqlModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlModifyRowsWithStoredProcedureAndBatchUpdate()
        {
            base.ModifyRowsWithStoredProcedureAndBatchUpdate();
        }

        [TestMethod]
        public void SqlDeleteRowWithStoredProcedure()
        {
            base.DeleteRowWithStoredProcedure();
        }

        [TestMethod]
        public void SqlInsertRowWithStoredProcedure()
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

        protected override void CreateDataAdapterCommands()
        {
            SqlDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            SqlDataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            SqlDataSetHelper.DeleteStoredProcedures(db);
        }

        protected override void AddTestData()
        {
            SqlDataSetHelper.AddTestData(db);
        }
    }
}
