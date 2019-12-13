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
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlParameterFixture
    {
        [TestMethod]
        public void CanInsertNullStringParameter()
        {
            Database db = null;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlString = "insert into Customers (CustomerID, CompanyName, ContactName) Values (@id, @name, @contact)";
                    DbCommand insert = db.GetSqlStringCommand(sqlString);
                    db.AddInParameter(insert, "@id", DbType.Int32, 1);
                    db.AddInParameter(insert, "@name", DbType.String, "fee");
                    db.AddInParameter(insert, "@contact", DbType.String, null);

                    db.ExecuteNonQuery(insert, transaction);
                    transaction.Rollback();
                }
            }
        }

        [TestMethod]
        public void CanExecuteProcedureWithUnicaodeParametersInSql()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            SqlDatabase db = factory.CreateDefault() as SqlDatabase;
            object procedureOutput = null;

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(transaction, CommandType.Text, @"CREATE PROCEDURE CanAddSqlTypeParameters @UnicodeParam nvarchar(50) AS SELECT @UnicodeParam");

                    DbCommand commandToCustOrderHist = db.GetStoredProcCommand("CanAddSqlTypeParameters");
                    db.AddInParameter(commandToCustOrderHist, "UnicodeParam", SqlDbType.NVarChar, "PROCEDURE INPUT \u0414");

                    procedureOutput = db.ExecuteScalar(commandToCustOrderHist, transaction);
                    transaction.Rollback();
                }
            }
            Assert.AreEqual("PROCEDURE INPUT \u0414", procedureOutput);
        }
    }
}
