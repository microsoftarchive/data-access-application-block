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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor
{
    //This class is defined as private in the SProcAcessor.cs
    public class CustomParameterMapper : IParameterMapper
    {
        readonly Database database;
        public CustomParameterMapper(Database database)
        {
            this.database = database;
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues.Length > 0)
            {
                GuardParameterDiscoverySupported();
                database.AssignParameters(command, parameterValues);
            }
        }

        private void GuardParameterDiscoverySupported()
        {
            if (!database.SupportsParemeterDiscovery)
            {
                //throw new InvalidOperationException(
                //    string.Format(Resources.Culture,
                //                  Resources.ExceptionParameterDiscoveryNotSupported,
                //                  database.GetType().FullName));
            }
        }
    }
}

