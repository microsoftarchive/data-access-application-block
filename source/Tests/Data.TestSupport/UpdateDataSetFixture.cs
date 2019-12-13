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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public abstract class UpdateDataSetFixture : UpdateDataSetStoredProcedureBase
    {
        public void DeleteRowWithMissingInsertAndUpdateCommands()
        {
            startingData.Tables[0].Rows[5].Delete();

            db.UpdateDataSet(startingData, "Table", null, null, deleteCommand,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(7, resultTable.Rows.Count);
        }

        public void DeleteRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[5].Delete();

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(7, resultTable.Rows.Count);
        }

        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        public void InsertRowWithMissingUpdateAndDeleteCommands()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", insertCommand, null, null,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(9, resultTable.Rows.Count);
            Assert.AreEqual(1000, Convert.ToInt32(resultTable.Rows[8]["RegionID"]));
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }

        public void InsertRowWithStoredProcedure()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(9, resultTable.Rows.Count);

            int result = Convert.ToInt32(resultTable.Rows[8]["RegionID"]);
            Assert.AreEqual(1000, result);
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }

        public void ModifyRowsWithStoredProcedureAndBatchUpdate()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";
            startingData.Tables[0].Rows[5]["RegionDescription"] = "Australia";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue, 5);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
            Assert.AreEqual("Australia", resultTable.Rows[5]["RegionDescription"].ToString().Trim());
        }

        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        protected override void PrepareDatabaseSetup() {}

        protected override void ResetDatabase()
        {
            RestoreRegionTable();
        }

        void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }

        public void UpdateDataSetWithAllCommandsMissing()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", null, null, null,
                             UpdateBehavior.Standard);
        }

        public void UpdateDataSetWithNullTable()
        {
            db.UpdateDataSet(null, null, null, null, null, UpdateBehavior.Standard);
        }

        public void UpdateRowWithMissingInsertAndDeleteCommands()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", null, updateCommand, null,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        public void UpdateSetWithNullDataSet()
        {
            db.UpdateDataSet(null, "Table", insertCommand, null, null,
                             UpdateBehavior.Standard);
        }
    }
}
