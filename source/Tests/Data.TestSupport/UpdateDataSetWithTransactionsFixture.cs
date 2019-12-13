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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public abstract class UpdateDataSetWithTransactionsFixture : UpdateDataSetStoredProcedureBase
    {
        protected bool rollback = true;
        protected DbTransaction transaction;

        public void AttemptToInsertBadRowInsideOfATransaction()
        {
            AddRowsWithErrorsToDataTable(startingData.Tables[0]);
            try
            {
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);
            }
            catch
            {
                transaction.Rollback();
                rollback = false;

                DataSet results = GetDataSetFromTableWithoutTransaction();

                Assert.AreEqual(8, results.Tables[0].Rows.Count);

                return;
            }

            Assert.Fail();
        }

        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand, transaction);
        }

        protected virtual DataSet GetDataSetFromTableWithoutTransaction()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        protected override void PrepareDatabaseSetup()
        {
            DbConnection connection = db.CreateConnection();
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        protected override void ResetDatabase()
        {
            if (rollback)
            {
                transaction.Rollback();
            }
            RestoreRegionTable();
        }

        void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }
    }
}
