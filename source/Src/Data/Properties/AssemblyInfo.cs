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
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly: AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1311.0")]
[assembly: AssemblyInformationalVersion("6.0.1311-prerelease")]

[assembly: SecurityTransparent]

[assembly: ComVisible(false)]

[assembly: HandlesSection(DataAccessDesignTime.ConnectionStringSettingsSectionName)]
[assembly: HandlesSection(DatabaseSettings.SectionName, ClearOnly = true)]

[assembly: AddApplicationBlockCommand(
            DataAccessDesignTime.ConnectionStringSettingsSectionName,
            typeof(ConnectionStringsSection),
            TitleResourceName = "AddDataSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = DataAccessDesignTime.CommandTypeNames.AddDataAccessBlockCommand)]
