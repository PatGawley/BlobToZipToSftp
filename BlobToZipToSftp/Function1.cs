using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace BlobToZipToSftp
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([BlobTrigger("to-zip/{name}", Connection = "TestConnectionString")] Stream myBlob, string name, ILogger log,
                    [Blob("zipped/{name}.zip", FileAccess.Write)] Stream zippped)
        {

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (ZipOutputStream s = new ZipOutputStream(zippped))
            {

                s.SetLevel(9);
                s.Password = "bollox";
                ZipEntry entry = new(name);
                s.PutNextEntry(entry);
                myBlob.CopyTo(s);
                s.Finish();
            }
        }
    }
 
}
