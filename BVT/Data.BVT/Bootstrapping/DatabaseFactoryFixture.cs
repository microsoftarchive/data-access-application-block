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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Bootstrapping
{
    [TestClass]
    public class DatabaseFactoryFixture : EntLibFixtureBase
    {
        public DatabaseFactoryFixture()
            : base(@"ConfigFiles.DatabaseFactoryFixture.config")
        {
        }

        [TestCleanup]
        public override void Cleanup()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenDatabaseFactoryIsNotInitialized()
        {
            DatabaseFactory.CreateDatabase();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenDatabaseFactoryIsInitializedTwice()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource), false);

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource));
        }

        [TestMethod]
        public void NoExceptionIsThrownWhenDatabaseFactoryIsInitializedTwiceAndNoThrowOnError()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource), false);
            var db = DatabaseFactory.CreateDatabase("DefaultSql123");
            Assert.IsTrue(db.ConnectionString.Contains("database=Northwind123"));

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
            var db1 = DatabaseFactory.CreateDatabase("DataAccessQuickStart");
            Assert.IsTrue(db1.ConnectionString.Contains("EntLibQuickStarts"));
            DatabaseFactory.ClearDatabaseProviderFactory();
        }

        [TestMethod]
        public void ConnectionStringsAreSetWhenDatabaseFactoryIsClearedAndSetAgain()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource), false);
            var db = DatabaseFactory.CreateDatabase("DefaultSql123");
            Assert.IsTrue(db.ConnectionString.Contains("database=Northwind123"));
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);

            var db1 = DatabaseFactory.CreateDatabase("DataAccessQuickStart");
            Assert.IsTrue(db1.ConnectionString.Contains("EntLibQuickStarts"));
            DatabaseFactory.ClearDatabaseProviderFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenNamedConnectionStringNotInConfig()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource), false);
            var db = DatabaseFactory.CreateDatabase("DoesnotExist");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenMappingDoesNotExist()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(base.ConfigurationSource), false);
            var db = DatabaseFactory.CreateDatabase("InvalidMapping");
        }
    }
}

