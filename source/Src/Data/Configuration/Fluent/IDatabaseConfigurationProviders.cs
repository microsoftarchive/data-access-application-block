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

using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Extension point for database providers to connect to the data configuration fluent-api.
    ///</summary>
    /// <seealso cref="DataConfigurationSourceBuilderExtensions"/>
    /// <seealso cref="DatabaseConfigurationExtension"/>
    public interface IDatabaseConfigurationProviders : IFluentInterface
    {
    }
}
