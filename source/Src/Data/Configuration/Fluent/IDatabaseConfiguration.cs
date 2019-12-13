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

using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Supports configuring the data connections via fluent-style interface.
    ///</summary>
    public interface IDatabaseConfiguration : IFluentInterface
    {
        ///<summary>
        /// Configure a named database.
        ///</summary>
        ///<param name="databaseName">Name of database to configure</param>
        ///<returns></returns>
        IDatabaseConfigurationProperties ForDatabaseNamed(string databaseName);
    }
}
