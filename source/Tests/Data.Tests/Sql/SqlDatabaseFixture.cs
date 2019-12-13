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
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlDatabaseFixture
    {
        [TestMethod]
        public void ConnectionTest()
        {
            DatabaseSyntheticConfigSettings settings = new DatabaseSyntheticConfigSettings(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = settings.GetConnectionStringSettings("NewDatabase");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);

            DbConnection connection = sqlDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is SqlConnection);
            connection.Open();
            DbCommand cmd = sqlDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 60;
            Assert.AreEqual(cmd.CommandTimeout, 60);
        }

        [TestMethod]
        public void CanGetConnectionWithoutCredentials()
        {
            DatabaseSyntheticConfigSettings settings = new DatabaseSyntheticConfigSettings(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = settings.GetConnectionStringSettings("DbWithSqlServerAuthn");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);

            Assert.AreEqual(@"server=(localdb)\v11.0;database=northwind;", sqlDatabase.ConnectionStringWithoutCredentials);
        }

        [TestMethod]
        public void CanGetConnectionForStringWithNoCredentials()
        {
            DatabaseSyntheticConfigSettings settings = new DatabaseSyntheticConfigSettings(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = settings.GetConnectionStringSettings("NewDatabase");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);

            Assert.AreEqual(@"server=(localdb)\v11.0;database=northwind;integrated security=true;", sqlDatabase.ConnectionStringWithoutCredentials);
        }

        [TestMethod]
        public void CheckNoPasswordInConnectionStringWithPersistInfoEqualsFalse()
        {
            try
            {
                CreateUser();
                DatabaseSyntheticConfigSettings settings = new DatabaseSyntheticConfigSettings(TestConfigurationSource.CreateConfigurationSource());
                ConnectionStringSettings data = settings.GetConnectionStringSettings("NorthwindPersistFalse");
                SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);
                DbConnection dbConnection = sqlDatabase.CreateConnection();
                dbConnection.Open();
                dbConnection.Close();
                string connectionString = dbConnection.ConnectionString;
                if (connectionString.ToLower().Contains("pwd") || connectionString.ToLower().Contains("password"))
                {
                    Assert.Fail();
                }
            }
            finally
            {
                DeleteUser();
            }
        }

        void CreateUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database adminDb = factory.CreateDefault();
            using (DbConnection connection = adminDb.CreateConnection())
            {
                connection.Open();
                string query;
                DbCommand addUser;
                try
                {
                    query = string.Format("exec sp_addlogin '{0}', '{1}', 'Northwind'", TestConfigurationSource.NorthwindDummyUser, TestConfigurationSource.NorthwindDummyPassword);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    query = string.Format("exec sp_grantdbaccess '{0}', '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch {}
                try
                {
                    query = string.Format("exec sp_addrolemember N'db_owner', N'{0}'", TestConfigurationSource.NorthwindDummyUser);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch {}
            }
        }

        void DeleteUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database adminDb = factory.CreateDefault();
            using (DbConnection connection = adminDb.CreateConnection())
            {
                connection.Open();
                string query;
                DbCommand dropUser;
                try
                {
                    query = string.Format("exec sp_revokedbaccess '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    dropUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(dropUser);
                }
                catch {}
                try
                {
                    query = string.Format("exec sp_droplogin '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    dropUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(dropUser);
                }
                catch {}
            }
        }

        [TestMethod]
        public void CheckNoPasswordWithPersistInfoEqualsFalseForDynamicConnectionString()
        {
            try
            {
                CreateUser();
                ConnectionString testString =
                    new ConnectionString(@"server=(localdb)\v11.0;database=Northwind;uid=entlib;pwd=hdf7&834k(*KA;Persist Security Info=false", "UserId,UId", "Password,Pwd");
                SqlDatabase sqlDatabase = new SqlDatabase(testString.ToString());
                DbConnection dbConnection = sqlDatabase.CreateConnection();
                dbConnection.Open();
                dbConnection.Close();
                string connectionString = dbConnection.ConnectionString;
                if (connectionString.ToLower().Contains("pwd") || connectionString.ToLower().Contains("password"))
                {
                    Assert.Fail();
                }
            }
            finally
            {
                DeleteUser();
            }
        }
    }
}
