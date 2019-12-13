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

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor.TestObjects;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor
{
    [TestClass]
    public class GenericDatabaseAccessorFixture : EntLibFixtureBase
    {
        private GenericDatabase genericDatabase = null;

        public GenericDatabaseAccessorFixture()
            : base("ConfigFiles.GenericDatabase.config")
        {
        }

        [TestInitialize]
        public void Setup()
        {
            WriteEmbeddedFileToDisk(Assembly.GetExecutingAssembly(), "Database.mdb");

            genericDatabase = new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault() as GenericDatabase;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenExecutingASprocAccessor()
        {
            genericDatabase.ExecuteSprocAccessor<PersonForGenericDB>("Test", 1);
        }

        [TestMethod]
        public void ValueIsReturnedWhenExecutingAStringAccessor()
        {
            var result = genericDatabase.ExecuteSqlStringAccessor<PersonForGenericDB>("SELECT PersonID, FirstName, Age FROM Person",
                        MapBuilder<PersonForGenericDB>.MapAllProperties()
                        .Map(c => c.ID).ToColumn("PersonID")
                        .Map(c => c.Name).ToColumn("FirstName")
                        .Build());

            var person = result.First();

            Assert.AreEqual(1, person.ID);
        }
    }
}

