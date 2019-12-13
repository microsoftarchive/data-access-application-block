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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Summary description for OracleDatabaseDataExtraInformationFixture
    /// </summary>
    [TestClass]
    public class OracleDatabaseDataExtraInformationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            OracleConnectionSettings rwSettings = new OracleConnectionSettings();

            OracleConnectionData rwOracleConnectionData = new OracleConnectionData();
            rwOracleConnectionData.Name = "name0";
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package1", "pref1"));
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package2", "pref2"));
            rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);
            rwOracleConnectionData = new OracleConnectionData();
            rwOracleConnectionData.Name = "name1";
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package3", "pref3"));
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package4", "pref4"));
            rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[OracleConnectionSettings.SectionName] = rwSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            OracleConnectionSettings roSettings = (OracleConnectionSettings)configurationSource.GetSection(OracleConnectionSettings.SectionName);
            Assert.AreEqual(2, roSettings.OracleConnectionsData.Count);
            Assert.AreEqual("name0", roSettings.OracleConnectionsData.Get(0).Name);
            Assert.AreEqual(2, roSettings.OracleConnectionsData.Get(0).Packages.Count);
        }
    }
}
