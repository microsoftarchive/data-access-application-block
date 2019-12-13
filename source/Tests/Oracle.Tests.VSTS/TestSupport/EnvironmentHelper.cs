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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport
{
    internal class EnvironmentHelper
    {
        private static bool? oracleClientIsInstalled;
        private static string oracleClientNotInstalledErrorMessage;

        public static void AssertOracleClientIsInstalled()
        {
            if (!oracleClientIsInstalled.HasValue)
            {
                try
                {
                    var factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
                    var db = factory.Create("OracleTest");
                    var conn = db.CreateConnection();
                    conn.Open();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message != null && ex.Message.Contains("System.Data.OracleClient")
                        && ex.Message.Contains("8.1.7"))
                    {
                        oracleClientIsInstalled = false;
                        oracleClientNotInstalledErrorMessage = ex.Message;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (oracleClientIsInstalled.HasValue && oracleClientIsInstalled.Value == false)
            {
                Assert.Inconclusive(oracleClientNotInstalledErrorMessage);
            }
        }
    }
}
