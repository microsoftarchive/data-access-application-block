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
using System.IO;

namespace Data.SqlCe.Tests.VSTS
{
    public class TestConnectionString
    {
        private string connectionString;
        private string filename;

        public TestConnectionString() : this("test.sdf")
        {
        }

        public TestConnectionString(string filename)
        {
            this.filename = Path.Combine(Environment.CurrentDirectory, filename);
            this.connectionString = "Data Source='{0}'";
            this.connectionString = String.Format(connectionString, filename);
        }

        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        public void CopyFile()
        {
            if (File.Exists(filename))
                File.Delete(filename);

            string sourceFile = Path.Combine(Environment.CurrentDirectory, "TestDb.sdf");
            File.Copy(sourceFile, filename);
        }

        public void DeleteFile()
        {
            File.Delete(filename);
        }

        public string Filename
        {
            get { return this.filename; }
        }
    }
}
