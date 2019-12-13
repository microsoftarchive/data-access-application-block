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
    public class ExecuteNonQueryFixture
    {
        DbCommand countCommand;
        string countQuery;
        Database db;
        DbCommand insertionCommand;
        string insertString;

        public ExecuteNonQueryFixture(Database db,
                                      string insertString,
                                      string countQuery,
                                      DbCommand insertionCommand,
                                      DbCommand countCommand)
        {
            this.db = db;
            this.insertString = insertString;
            this.countQuery = countQuery;
            this.insertionCommand = insertionCommand;
            this.countCommand = countCommand;
        }

        public void CanExecuteNonQueryThroughTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    int rowsAffected = db.ExecuteNonQuery(insertionCommand, transaction.Transaction);

                    int count = Convert.ToInt32(db.ExecuteScalar(countCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                    Assert.AreEqual(1, rowsAffected);
                }
            }
        }

        public void CanExecuteNonQueryWithCommandTextAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(transaction, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithCommandTextAsStoredProcAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    connection.Open();
                    db.ExecuteNonQuery(transaction, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(transaction, CommandType.Text, insertionCommand.CommandText);
                    transaction.Commit();
                }
            }

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void CanExecuteNonQueryWithDbCommand()
        {
            db.ExecuteNonQuery(insertionCommand);

            int count = Convert.ToInt32(db.ExecuteScalar(countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = db.GetSqlStringCommand(cleanupString);
            int rowsAffected = db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, rowsAffected);
        }

        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            db.ExecuteNonQuery(null, (string)null);
        }

        public void ExecuteNonQueryWithNullDbTransaction()
        {
            db.ExecuteNonQuery(insertionCommand, null);
        }

        public void TransactionActuallyRollsBack()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    db.ExecuteNonQuery(insertionCommand, transaction.Transaction);
                }
            }

            DbCommand wrapper = db.GetSqlStringCommand(countQuery);
            int count = Convert.ToInt32(db.ExecuteScalar(wrapper));
            Assert.AreEqual(4, count);
        }
    }
}
