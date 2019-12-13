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

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Used with the Database.UpdateDataSet method. Provides control over behavior when the Data
    /// Adapter's update command encounters an error.
    /// </summary>
    public enum UpdateBehavior
    {
        /// <summary>
        /// No interference with the DataAdapter's Update command. If Update encounters
        /// an error, the update stops.  Additional rows in the Datatable are uneffected.
        /// </summary>
        Standard,
        /// <summary>
        /// If the DataAdapter's Update command encounters an error, the update will
        /// continue. The Update command will try to update the remaining rows. 
        /// </summary>
        Continue,
        /// <summary>
        /// If the DataAdapter encounters an error, all updated rows will be rolled back.
        /// </summary>
        Transactional
    }
}
