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
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport
{
    public static class OracleTestConfigurationSource
    {
        public const string OracleConnectionString = "server=entlib;user id=testuser;password=testuser";
        public const string OracleConnectionStringName = "OracleTest";
        public const string OracleProviderName = "System.Data.OracleClient";

        public static DictionaryConfigurationSource CreateConfigurationSource()
        {
            DictionaryConfigurationSource configSource = TestConfigurationSource.CreateConfigurationSource();

            var connectionString = new ConnectionStringSettings(
                OracleConnectionStringName,
                OracleConnectionString,
                OracleProviderName);

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(connectionString);

            configSource.Add("connectionStrings", connectionStrings);
            return configSource;
        }
    }
}
