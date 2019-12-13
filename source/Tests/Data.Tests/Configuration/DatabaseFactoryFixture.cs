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

using System.Data.Common;
using System.Data.Odbc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class DatabaseFactoryFixture
    {
        public const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        public const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        public const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";

        private DatabaseProviderFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            this.factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
        }

        [TestMethod]
        public void CanCreateSqlDatabase()
        {
            Database database = this.factory.Create("Service_Dflt");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(SqlDatabase), database.GetType());
        }

        [TestMethod]
        public void CanCreateGenericDatabase()
        {
            Database database = this.factory.Create("OdbcDatabase");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(GenericDatabase), database.GetType());

            // provider factories aren't exposed
            DbCommand command = database.GetStoredProcCommand("ignore");
            Assert.AreSame(typeof(OdbcCommand), command.GetType());
        }
    }
}
