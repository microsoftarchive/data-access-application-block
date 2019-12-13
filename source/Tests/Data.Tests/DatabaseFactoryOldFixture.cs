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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFactoryOldFixture
    {
#pragma warning disable 612, 618
        private static readonly Database[] databases = new Database[]
        {
            new SqlDatabase(".\\sqlexpress"),
            new OracleDatabase("test")
        };
#pragma warning restore 612, 618

        [TestMethod]
        public void CanCreateDefaultDatabase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.CreateDefault();
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CanGetDatabaseByName()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.Create("NewDatabase");
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CallingTwiceReturnsDifferenceDatabaseInstances()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database firstDb = factory.Create("NewDatabase");
            Database secondDb = factory.Create("NewDatabase");

            Assert.AreNotSame(firstDb, secondDb);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionThrownWhenAskingForDatabaseWithUnknownName()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection), false);

            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
            }
            finally
            {
                DatabaseFactory.ClearDatabaseProviderFactory();
            }
        }

        [TestMethod]
        public void SettingDatabaseFactoryTheFirstTimeDoesNotThrow()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(sn => null));
        }

        [TestMethod]
        public void SettingDatabaseFactoryTheASecondTimeDoesThrows()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(sn => null));
            try
            {
                DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(sn => null));
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
                // expected
            }
        }

        [TestMethod]
        public void SettingDatabaseFactoryTheASecondTimeWithOverrideDoesNotThrow()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(sn => null));
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(sn => null), false);
        }

        [TestMethod]
        public void CreateDatabaseDefaultDatabaseWithDatabaseFactory()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection), false);

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                Assert.IsNotNull(db);
            }
            finally
            {
                DatabaseFactory.ClearDatabaseProviderFactory();
            }
        }

        [TestMethod]
        public void CreateNamedDatabaseWithDatabaseFactory()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection), false);

            try
            {
                Database db = DatabaseFactory.CreateDatabase("OracleTest");
                Assert.IsNotNull(db);
            }
            finally
            {
                DatabaseFactory.ClearDatabaseProviderFactory();
            }
        }

        [TestMethod]
        public void SetDatabases()
        {
            try
            {
                DatabaseFactory.SetDatabases(() => databases.First(),
                                             (name) =>
                                             {
                                                 if (name == "sql")
                                                     return databases.First();
                                                 else if (name == "oracle")
                                                     return databases.Last();
                                                 else
                                                     return null;
                                             },
                                             false);

                var defaultDb = DatabaseFactory.CreateDatabase();
                Assert.AreEqual<Database>(databases.First(), defaultDb);

                Assert.AreEqual<Database>(databases.First(), DatabaseFactory.CreateDatabase("sql"));
                Assert.AreEqual<Database>(databases.Last(), DatabaseFactory.CreateDatabase("oracle"));

                Assert.IsNull(DatabaseFactory.CreateDatabase("invalid"));
            }
            finally
            {
                DatabaseFactory.ClearDatabaseProviderFactory();
            }
        }
    }
}
