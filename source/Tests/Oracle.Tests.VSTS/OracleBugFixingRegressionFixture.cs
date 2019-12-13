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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleBugFixingRegressionFixture
    {
        const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";
        const string OracleTestPackage1Prefix = "TESTPACKAGETOTRANSLATE";
        const string OracleTestPackage1Name = "TESTPACKAGE";
        const string OracleTestPackage2Prefix = "TESTPACKAGETOTRANSLATE2";
        const string OracleTestPackage2Name = "TESTPACKAGE2";

        Guid referenceGuid = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            CreateTableWithGuidAndBinary();
        }

        [TestCleanup]
        public void TearDown()
        {
            DropTableWithGuidAndBinary();
        }

        [TestMethod]
        public void CommandTextWithConfiguredPackageTranslationsShouldBeTranslatedToTheCorrectPackageBug1572()
        {
            DbCommand dBCommand = db.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);

            Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dBCommand.CommandText);
        }

        [TestMethod]
        public void CommandTextWithoutConfiguredPackageTranslationsShouldNotBeTranslatedBug1572()
        {
            DbCommand dBCommand = db.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithoutTranslation);

            Assert.AreEqual((object)OracleTestStoredProcedureInPackageWithoutTranslation, dBCommand.CommandText);
        }

        [TestMethod]
        public void CanGetGuidFromReader()
        {
            using (IDataReader reader = db.ExecuteReader(CommandType.Text, "SELECT * FROM GUID_BINARY_TABLE"))
            {
                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());
                Guid guidValue = reader.GetGuid(0);
                Assert.IsNotNull(guidValue);
                Assert.AreEqual(referenceGuid, guidValue);
                bool boolValue = reader.GetBoolean(1);
                Assert.IsTrue(boolValue);
                Assert.IsFalse(reader.Read());
            }
        }

        void CreateTableWithGuidAndBinary()
        {
            string commandText = null;
            string guidText = referenceGuid.ToString("N");

            commandText = @"DROP TABLE GUID_BINARY_TABLE";
            try
            {
                db.ExecuteNonQuery(CommandType.Text, commandText);
            }
            catch {}

            commandText = @"CREATE TABLE GUID_BINARY_TABLE(GUID_COL RAW(16), BOOL_COL VARCHAR2(10))";
            db.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = @"INSERT INTO GUID_BINARY_TABLE(GUID_COL, BOOL_COL) VALUES ('" + guidText + "', 'true')";
            db.ExecuteNonQuery(CommandType.Text, commandText);
        }

        void DropTableWithGuidAndBinary()
        {
            string commandText = null;

            commandText = @"DROP TABLE GUID_BINARY_TABLE";
            try
            {
                db.ExecuteNonQuery(commandText);
            }
            catch {}
        }
    }
}
