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
    public class DataAccessTestsFixture
    {
        DbCommand command;
        DataSet dataSet;
        Database db;
        string sqlQuery = "SELECT * FROM Region";

        public DataAccessTestsFixture(Database db)
        {
            this.db = db;
            dataSet = new DataSet();
            command = db.GetSqlStringCommand(sqlQuery);
        }

        public void CanGetNonEmptyResultSet()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables["Foo"].Rows.Count);
        }

        public void CanGetNonEmptyResultSetUsingTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void CanGetTablePositionally()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void CannotLoadDataSetWithEmptyTableName()
        {
            db.LoadDataSet(command, dataSet, "");
            Assert.Fail("Cannot call LoadDataSet with empty SourceTable name");
        }

        public void ExecuteCommandNullCommand()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(null, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteCommandNullDataset()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, null, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteCommandNullTableName()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, (string)null, transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteCommandNullTransaction()
        {
            db.LoadDataSet(command, dataSet, "Foo", null);
        }

        public void ExecuteDataSetWithCommand()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteDataSetWithDbTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteNullCommand()
        {
            db.LoadDataSet(null, null, (string)null);
        }
    }
}
