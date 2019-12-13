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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public class TestConfigurationSource
    {
        public const string NorthwindDummyUser = "entlib";
        public const string NorthwindDummyPassword = "hdf7&834k(*KA";

        public static DictionaryConfigurationSource CreateConfigurationSource()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();

            DatabaseSettings settings = new DatabaseSettings();
            settings.DefaultDatabase = "Service_Dflt";

            OracleConnectionSettings oracleConnectionSettings = new OracleConnectionSettings();
            OracleConnectionData data = new OracleConnectionData();
            data.Name = "OracleTest";
            data.Packages.Add(new OraclePackageData("TESTPACKAGE", "TESTPACKAGETOTRANSLATE"));
            oracleConnectionSettings.OracleConnectionsData.Add(data);

            source.Add(DatabaseSettings.SectionName, settings);
            source.Add(OracleConnectionSettings.SectionName, oracleConnectionSettings);

            return source;
        }
    }
}
