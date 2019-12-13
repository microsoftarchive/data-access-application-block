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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
    /// <summary>
    /// Represents the description of an Oracle package mapping.
    /// </summary>
    /// <remarks>
    /// <see cref="IOraclePackage"/> is used to specify how to transform store procedure names 
    /// into package qualified Oracle stored procedure names.
    /// </remarks>
    /// <seealso cref="OracleDatabase"/>
    public interface IOraclePackage
    {
        /// <summary>
        /// When implemented by a class, gets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        string Name
        { get; }

        /// <summary>
        /// When implemented by a class, gets the prefix for the package.
        /// </summary>
        /// <value>
        /// The prefix for the package.
        /// </value>
        string Prefix
        { get; }
    }
}
