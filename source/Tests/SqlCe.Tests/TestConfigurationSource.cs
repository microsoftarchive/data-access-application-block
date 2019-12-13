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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;

namespace Data.SqlCe.Tests.VSTS
{
    public class TestConfigurationSource
    {
        public static DictionaryConfigurationSource CreateConfigurationSource()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();

            DatabaseSettings settings = new DatabaseSettings();
            settings.DefaultDatabase = "SqlCeTestConnection";

            ConnectionStringsSection section = new ConnectionStringsSection();
            section.ConnectionStrings.Add(new ConnectionStringSettings("SqlCeTestConnection", "Data Source='testdb.sdf'", "System.Data.SqlServerCe.4.0"));

            source.Add(DatabaseSettings.SectionName, settings);
            source.Add("connectionStrings", section);

            return source;
        }
    }
}
