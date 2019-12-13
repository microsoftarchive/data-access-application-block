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
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class OracleDatabaseFixture
    {
        IConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            configurationSource = OracleTestConfigurationSource.CreateConfigurationSource();
        }

        [TestMethod]
        public void CanConnectToOracleAndExecuteAReader()
        {
            var oracleDatabase = new DatabaseSyntheticConfigSettings(this.configurationSource).GetDatabase("OracleTest").BuildDatabase();

            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }

        [TestMethod]
        public void CanExecuteCommandWithEmptyPackages()
        {
            ConnectionStringSettings data = ConfigurationManager.ConnectionStrings["OracleTest"];

            OracleDatabase oracleDatabase = new OracleDatabase(data.ConnectionString);
            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingAnOracleDatabaseWithNullPackageListThrows()
        {
            ConnectionStringSettings data = ConfigurationManager.ConnectionStrings["OracleTest"];

            new OracleDatabase(data.ConnectionString, null);
        }
    }
#pragma warning restore 612, 618
}
