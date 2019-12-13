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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.CommonDatabase
{
    [TestClass()]
    public class CommonFixture : EntLibFixtureBase
    {
        public CommonFixture()
            : base("ConfigFiles.OlderTestsConfiguration.config")
        {
        }

        /// <summary>
        /// Initialize() is called once during test execution before
        /// test methods in this test class are executed.
        /// </summary>
        [TestInitialize()]
        public override void Initialize()
        {
            ConnectionStringsSection section = (ConnectionStringsSection)base.ConfigurationSource.GetSection("connectionStrings");
            connStr = section.ConnectionStrings["DataSQLTest"].ConnectionString;

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from TestNullTable", conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            base.Cleanup();
        }

        #region Connection String Tests

        [TestMethod()]
        public void ConnectionStringWithCredentialsIsCreatedFromAnotherConnectionString()
        {
            string connectionString = "server=bg1016-ent;user id=testone;password=testone;";
            string userIdTokens = "user id=,uid=";
            string passwordTokens = "password=,pwd=";
            ConnectionString target = new ConnectionString(connectionString, userIdTokens, passwordTokens);
            ConnectionString actual = target.CreateNewConnectionString(connectionString);
            Assert.AreEqual("testone", actual.Password);
            Assert.AreEqual("testone", actual.UserName);
        }

        [TestMethod()]
        public void ConnectionStringWithoutCredentialsIsCreatedFromAnotherConnectionString()
        {
            string connectionString = "server=bg1016-ent;user id=testone;password=testone;";
            string userIdTokens = "testone";
            string passwordTokens = "testone";
            ConnectionString target = new ConnectionString(connectionString, userIdTokens, passwordTokens);
            string sAc1 = target.ToStringNoCredentials();
            Assert.AreEqual(connectionString, sAc1);
            string cString1 = "server=bg1016-ent;user id=u;password=p;";
            ConnectionString target1 = new ConnectionString(cString1, "u", "p");
            string sAc2 = target1.ToStringNoCredentials();
            Assert.AreEqual("server=bg1016-ent;", sAc2);
        }

        #endregion

        #region Null Values tests

        [TestMethod]
        public void NullValueIsInsertedWithDbType()
        {
            Database db = DatabaseFactory.CreateDatabase("DataSQLTest");
            int? age = null;
            DbCommand dbIns = db.GetStoredProcCommand("TestNullType");
            db.AddInParameter(dbIns, "@eno", DbType.Int32, 1);
            db.AddInParameter(dbIns, "@ename", DbType.String, "Narayan");
            db.AddInParameter(dbIns, "@age", DbType.Int32, age);
            db.ExecuteNonQuery(dbIns);
            int numberOfRows = (int)db.ExecuteScalar(CommandType.Text, "select count(eno) from TestNullTable");
            Assert.AreEqual(numberOfRows, 1);
        }

        [TestMethod]
        public void NullValueIsInsertedWithSqlDbType()
        {
            SqlDatabase db = (SqlDatabase)DatabaseFactory.CreateDatabase("DataSQLTest");
            int? age = null;
            DbCommand dbIns = db.GetStoredProcCommand("TestNullType");
            db.AddInParameter(dbIns, "@eno", SqlDbType.Int, 1);
            db.AddInParameter(dbIns, "@ename", SqlDbType.VarChar, "Narayan");
            db.AddInParameter(dbIns, "@age", SqlDbType.Int, age);
            db.ExecuteNonQuery(dbIns);
            int numberOfRows = (int)db.ExecuteScalar(CommandType.Text, "select count(eno) from TestNullTable");
            Assert.AreEqual(numberOfRows, 1);
        }

        [TestMethod]
        public void NullValueIsInsertedWithSqlTypeWhenNoValueSpecified()
        {
            SqlDatabase db = (SqlDatabase)DatabaseFactory.CreateDatabase("DataSQLTest");

            DbCommand dbIns = db.GetStoredProcCommand("TestNullType");
            db.AddInParameter(dbIns, "@eno", SqlDbType.Int, 1);
            db.AddInParameter(dbIns, "@ename", SqlDbType.VarChar, "Narayan");
            db.AddInParameter(dbIns, "@age", SqlDbType.Int);
            db.ExecuteNonQuery(dbIns);
            int numberOfRows = (int)db.ExecuteScalar(CommandType.Text, "select count(eno) from TestNullTable");
            Assert.AreEqual(numberOfRows, 1);
        }

        #endregion
    }
}

