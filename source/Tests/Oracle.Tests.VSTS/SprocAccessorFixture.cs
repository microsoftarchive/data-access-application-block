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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class WhenSprocAccessorIsCreatedForOracleDatabase : ArrangeActAssert
    {
        OracleDatabase database;

        protected override void Arrange()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            database = new OracleDatabase("server=entlib;user id=testuser;password=testuser");
        }

        [TestMethod]
        public void ThenCanExecuteParameterizedSproc()
        {
            var accessor = database.CreateSprocAccessor<Customer>("GetCustomersTest");
            var result = accessor.Execute("BLAUS", null).ToArray();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        private class Customer
        {
            public string ContactName { get; set; }
        }
    }
#pragma warning restore 612, 618
}
