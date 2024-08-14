using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace BlobToZipToSftp
{
    public class sFtp
    {
        [FunctionName("sFtp")]
        public void Run([BlobTrigger("zipped/{name}", Connection = "TestConnectionString")] Stream myBlob, string name, ILogger log,
                    [Blob("sftped/{name}", FileAccess.Write)] Stream zippped)
        {

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            string host = "eu-central-1.sftpcloud.io";
            int port = 22;
            string username = "7e0dfc14bd2c44af86242650d2b86c72";
            string password = "hdBbsJIPMzdoMTNuNpEfOhlHhof9t9tG"; // Or use a private key
            string remoteFilePath = $"/{name}";

            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();


                client.UploadFile(myBlob, remoteFilePath);


                client.Disconnect();
            }

            zippped = myBlob;
        }
    }
}
