using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MindCorners.Web.Controllers
{
    public class BlobsController : Controller
    {
        // GET: Blobs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateBlobContainer()
        {
            // The code in this section goes here.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
   CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");
            container.CreateIfNotExists();
            return View();
        }

        public EmptyResult UploadBlob(string folder, string fileName, byte[] fileData)
        {
            // The code in this section goes here.

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(folder);
            container.CreateIfNotExists();
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            using (var stream = new MemoryStream(fileData, writable: false))
            {
                blob.UploadFromStream(stream);
            }
            return new EmptyResult();
        }
    }
}