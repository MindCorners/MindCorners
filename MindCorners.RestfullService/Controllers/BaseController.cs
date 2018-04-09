using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using MindCorners.Authentication;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MimeTypes;
using MindCorners.Common.Model;
using MindCorners.Models.Enums;

namespace MindCorners.RestfullService.Controllers
{
    public class BaseController : ApiController
    {
        protected ApplicationDbContext DbContext { get; set; }
        protected ApplicationUserManager _userManager;
        //protected UserProfileRepository _userProfileRepository;
        // protected Guid DbUser = new Guid("2eb330ac-ed03-49d0-83ee-8899561d7442");
        protected MindCornersEntities Context;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //private Guid dbUser;
        public Guid DbUser
        {
            get
            {
                var re = Request;
                if (re != null)
                {
                    var headers = re.Headers;
                    if (headers.Contains("User_Id"))
                    {
                        var userId = headers.GetValues("User_Id").First();
                        if (!string.IsNullOrEmpty(userId))
                        {
                            return Guid.Parse(userId);
                        }
                    }
                }
                return Guid.Empty;
            }
        }
        public string DbUserFullName
        {
            get
            {
                using (UserProfileRepository _userProfileRepository = new UserProfileRepository())
                {
                    var user = _userProfileRepository.GetById(DbUser);
                    if (user != null)
                    {
                        return $"{user.FirstName} {user.LastName}";
                    }
                    return null;
                }
            }
        }

        public string ServiseRootUrl
        {
            get
            {
                return Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority +
                       Request.GetRequestContext().VirtualPathRoot.TrimEnd('/') + "/";
            }
        }

        public BaseController()
        {
            DbContext = new ApplicationDbContext();
            Context = new MindCornersEntities();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(DbContext));
            //_userProfileRepository = new UserProfileRepository(Context, DbUser, null);
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetFile(int type, Guid id)
        {
            switch (type)
            {
                case (int)ChatType.Image:
                    return GetImage(id);
                case (int)ChatType.Audio:
                    return GetAudio(id);
                case (int)ChatType.Video:
                    return GetVideo(id);
            }

            return null;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetFileByNameTest(int type, string fileName)
        {
            return await GetProfileImageByName(fileName);
        }


        /*[HttpGet]
        public Task<HttpResponseMessage> GetFileByName(int type, string fileName)
        {
            switch (type)
            {
                case (int)FileType.Profile:
                    return GetProfileImageByName(fileName);

                case (int)FileType.Image:
                    return GetImageByName(fileName);

                case (int)FileType.Audio:
                    return GetAudioByName(fileName);

                case (int)FileType.Video:
                    return GetVideoByName(fileName);
            }
            return null;
            //if (String.IsNullOrEmpty(id))
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);

            //var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            //var serverPath = Path.Combine(rootPath, id);
            //var fileExtension = Path.GetExtension(id);

            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //CloudBlobContainer container = blobClient.GetContainerReference("");
            //CloudBlockBlob blob = container.GetBlockBlobReference("");




            //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            //Stream blobStream = await blob.Dow();

            //message.Content = new StreamContent(blobStream);
            //message.Content.Headers.ContentLength = Blob.Properties.Length;
            //message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Blob.Properties.ContentType);
            //message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "{blobname.txt}",
            //    Size = Blob.Properties.Length
            //};

            //return message;





            //using (var memStream = new MemoryStream())
            //{
            //    blob.DownloadToStream(memStream);
            //    // return File(memStream.ToArray(), blob.Properties.ContentType);
            //}




            ////  localFilePath = getFileFromID(id, out fileName, out fileSize);

            ////HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            ////response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));

            //var response = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(blob..ToArray())
            //};


            //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //response.Content.Headers.ContentDisposition.FileName = id;
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

            //return response;
        }
        */

        private async Task<HttpResponseMessage> GetFile(string folderName, string fileName)
        {
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(folderName))
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(folderName);
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                var isExist = await blob.ExistsAsync();

                if (!isExist)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "file not found");
                }

                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
                //Stream blobStream = await blob.OpenReadAsync();

                using (var memStream = new MemoryStream())
                {
                    blob.DownloadToStream(memStream);
                    //message.Content =  new StreamContent(memStream);
                    var fileExtension = Path.GetExtension(fileName);

                    message.Content = new StreamContent(memStream);

                    //message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    //message.Content.Headers.ContentDisposition.FileName = fileName;
                    message.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));
                    message.Content.Headers.ContentLength = blob.Properties.Length;

                    //message.Content.Headers.ContentLength = blob.Properties.Length;
                    //message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blob.Properties.ContentType);
                    message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName,
                        Size = blob.Properties.Length
                    };

                    return message;
                }




            }
            catch (Exception ex)
            {
                return new HttpResponseMessage

                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };
            }
        }
        /*
                public async Task<IHttpActionResult> GetFileByName(int type, string fileName)
                {
                    var folderName = string.Empty;

                    switch (type)
                    {
                        case (int)FileType.Profile:
                            folderName = "profile-images"; break;
                        case (int)FileType.Image:
                            folderName = "post-attachment-images"; break;

                        case (int)FileType.Audio:
                            folderName = "post-attachment-audios"; break;

                        case (int)FileType.Video:
                            folderName = "post-attachment-videos"; break;
                    }


                    var containerName = folderName;
                    var report = await GetAzureBlob(containerName, fileName);

                    return AzureBlobOk(report);
                }
                */


        public HttpResponseMessage Get(int type, string fileName)
        {
            HttpResponseMessage result = null;
            var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            var serverPath = Path.Combine(rootPath, fileName);
            var fileExtension = Path.GetExtension(fileName);
            if (File.Exists(serverPath))
            {
                FileInfo fileInfo = new FileInfo(serverPath);
                var _content = File.ReadAllBytes(serverPath);
                // A MemoryStream is seekable allowing us to do ranges over it. Same goes for FileStream.
                MemoryStream memStream = new MemoryStream(_content);
                var _mediaType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));
                // Check to see if this is a range request (i.e. contains a Range header field)
                // Range requests can also be made conditional using the If-Range header field. This can be 
                // used to generate a request which says: send me the range if the content hasn't changed; 
                // otherwise send me the whole thing.
                if (Request.Headers.Range != null)
                {
                    try
                    {
                        HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                        partialResponse.Content = new ByteRangeStreamContent(memStream, Request.Headers.Range,
                            _mediaType);
                        return partialResponse;
                    }
                    catch (InvalidByteRangeException invalidByteRangeException)
                    {
                        return Request.CreateErrorResponse(invalidByteRangeException);
                    }
                }
                else
                {
                    // If it is not a range request we just send the whole thing as normal
                    HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                    fullResponse.Content = new StreamContent(memStream);
                    fullResponse.Content.Headers.ContentType = _mediaType;
                    return fullResponse;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }


        //[HttpGet]
        //public HttpResponseMessage Get(int type, string fileName)
        //{
        //    HttpResponseMessage result = null;
        //    var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
        //    var serverPath = Path.Combine(rootPath, fileName);
        //    var fileExtension = Path.GetExtension(fileName);
        //    if (File.Exists(serverPath))
        //    {
        //        FileInfo fileInfo = new FileInfo(serverPath);
        //        FileStream fs = new FileStream(serverPath, FileMode.Open);

        //        result = new HttpResponseMessage(HttpStatusCode.OK);

        //        result.Content = new StreamContent(fs);
        //        result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/mpeg");
        //        result.Content.Headers.ContentLength = fileInfo.Length;
        //        result.Content.Headers.ContentRange= new ContentRangeHeaderValue(0, fileInfo.Length);

        //       result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //        result.Content.Headers.ContentDisposition.FileName = fileName;
        //       // result.Headers.AcceptRanges.Add("bytes");
        //       // result.Headers.ConnectionClose = false;
        //    }
        //    else
        //    {
        //        // new AppHbLogEvent(filePath).Raise();
        //        result = new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //    return result;
        //}

        public async Task<HttpResponseMessage> GetFileByNameAzure(int type, string fileName)
        {
            var folderName = string.Empty;

            switch (type)
            {
                case (int)FileType.Profile:
                    folderName = "profile-images"; break;
                case (int)FileType.Image:
                    folderName = "post-attachment-images"; break;

                case (int)FileType.Audio:
                    folderName = "post-attachment-audios"; break;

                case (int)FileType.Video:
                    folderName = "post-attachment-videos"; break;
            }


            var containerName = folderName;
            var report = await GetAzureBlob(containerName, fileName);

            return AzureBlobOk(report);
        }

        public async Task<HttpResponseMessage> GetVideoFileStream(string fileName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var context = storageAccount.CreateCloudBlobClient();

            // Get and create the container 
            var blobContainer = context.GetContainerReference("post-attachment-videos");
            blobContainer.CreateIfNotExists();

            var blob = blobContainer.GetBlockBlobReference(fileName);

            var blobExists = await blob.ExistsAsync();
            if (!blobExists)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");
            }

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            Stream blobStream = await blob.OpenReadAsync();

            message.Content = new StreamContent(blobStream);
            message.Content.Headers.ContentLength = blob.Properties.Length;
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blob.Properties.ContentType);
            message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = blob.Name,
                Size = blob.Properties.Length
            };

            return message;
        }

        public async Task<HttpResponseMessage> GetFileByName(int type, string fileName)
        {
            var folderName = string.Empty;
            bool openFromSharedLocation = false;
            switch (type)
            {
                case (int)FileType.Profile:
                    folderName = "profile-images"; break;
                case (int)FileType.Image:
                    folderName = "post-attachment-images"; break;

                case (int)FileType.Audio:
                    folderName = "post-attachment-audios";
                    openFromSharedLocation = true;
                    break;
                case (int)FileType.Video:
                    {
                        folderName = "post-attachment-videos";
                        openFromSharedLocation = true;

                        /*
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                        var context = storageAccount.CreateCloudBlobClient();
                        var properties = context.GetServiceProperties();
                        properties.DefaultServiceVersion = "2013-08-15";
                        context.SetServiceProperties(properties);


                        // Get and create the container 
                        var blobContainer = context.GetContainerReference("post-attachment-videos");
                        blobContainer.CreateIfNotExists();

                        var blob = blobContainer.GetBlockBlobReference(fileName);

                        var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                        {
                            Permissions = SharedAccessBlobPermissions.Read,
                            SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),//Set this date/time according to your requirements
                        });
                        var urlToBePlayed = string.Format("{0}{1}", blob.Uri, sas);//This is the URI which should be embedded in your video player.


                        var blobExists = await blob.ExistsAsync();
                        if (!blobExists)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");
                        }

                        HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
                        Stream blobStream = await blob.OpenReadAsync();

                        message.Content = new StreamContent(blobStream);
                        message.Content.Headers.ContentLength = blob.Properties.Length;
                        message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blob.Properties.ContentType);
                        message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = blob.Name,
                            Size = blob.Properties.Length
                        };

                        return message;*/
                        break;
                    }
            }

            if (openFromSharedLocation)
            {
                var blob = await GetAzureBlobItem(folderName, fileName);
                var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1)//Assuming you want the link to expire after 1 hour
                });
                var blobUrl = string.Format("{0}{1}", blob.Uri.AbsoluteUri, sasToken);
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri(blobUrl);
                return response;
            }

            var report = await GetAzureBlob(folderName, fileName);
            return AzureBlobOk(report);

            /*
            HttpResponseMessage result = null;
            var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            var serverPath = Path.Combine(rootPath, fileName);
            var fileExtension = Path.GetExtension(fileName);
            if (File.Exists(serverPath))
            {
                FileInfo fileInfo = new FileInfo(serverPath);
                var _content = File.ReadAllBytes(serverPath);
                // A MemoryStream is seekable allowing us to do ranges over it. Same goes for FileStream.
                MemoryStream memStream = new MemoryStream(_content);
                var _mediaType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));
                // Check to see if this is a range request (i.e. contains a Range header field)
                // Range requests can also be made conditional using the If-Range header field. This can be 
                // used to generate a request which says: send me the range if the content hasn't changed; 
                // otherwise send me the whole thing.
                if (Request.Headers.Range != null)
                {
                    try
                    {
                        HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                        partialResponse.Content = new ByteRangeStreamContent(memStream, Request.Headers.Range,
                            _mediaType);
                        return partialResponse;
                    }
                    catch (InvalidByteRangeException invalidByteRangeException)
                    {
                        return Request.CreateErrorResponse(invalidByteRangeException);
                    }
                }
                else
                {
                    // If it is not a range request we just send the whole thing as normal
                    HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                    fullResponse.Content = new StreamContent(memStream);
                    fullResponse.Content.Headers.ContentType = _mediaType;
                    return fullResponse;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
            */

            //var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            //var serverPath = Path.Combine(rootPath, fileName);
            //var fileExtension = Path.GetExtension(fileName);
            ////  localFilePath = getFileFromID(id, out fileName, out fileSize);

            //// using (FileStream fileStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
            //{
            //    var response = Request.CreateResponse(HttpStatusCode.OK);
            //    /*response.Content = new StreamContent(fileStream);
            //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));
            //    //response.Content.Headers.Add("x-filename", azureBlob.FileName);
            //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //    response.Content.Headers.ContentDisposition.FileName = fileName;
            //    response.Content.Headers.ContentDisposition.Size = fileStream.Length;
            //    */
            //    response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));
            //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //    response.Content.Headers.ContentDisposition.FileName = fileName;

            //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

            //    return ResponseMessage(response);
            //}
        }

        [HttpGet]
        public HttpResponseMessage GetFileLocal(int type, string id)
        {
            if (String.IsNullOrEmpty(id))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            var serverPath = Path.Combine(rootPath, id);
            var fileExtension = Path.GetExtension(id);
            //  localFilePath = getFileFromID(id, out fileName, out fileSize);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = id;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

            return response;
        }

        /* protected IHttpActionResult AzureBlobOk(AzureBlobModel azureBlob)
         {
             var response = Request.CreateResponse(HttpStatusCode.OK);
             response.Content = new StreamContent(azureBlob.Stream);
             response.Content.Headers.ContentType = new MediaTypeHeaderValue(azureBlob.ContentType);
             response.Content.Headers.Add("x-filename", azureBlob.FileName);
             response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
             response.Content.Headers.ContentDisposition.FileName = azureBlob.FileName;
             response.Content.Headers.ContentDisposition.Size = azureBlob.FileSize;

             return ResponseMessage(response);
         }*/

        protected HttpResponseMessage AzureBlobOk(AzureBlobModel azureBlob)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(azureBlob.Stream);
            response.Content.Headers.ContentLength = azureBlob.FileSize;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(azureBlob.ContentType);
            response.Content.Headers.Add("x-filename", azureBlob.FileName);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = azureBlob.FileName;
            response.Content.Headers.ContentDisposition.Size = azureBlob.FileSize;

            return response;

            //var fileExtension = Path.GetExtension(azureBlob.FileName);
            //MemoryStream memStream = new MemoryStream();
            //azureBlob.Stream.CopyTo(memStream);
            //var _mediaType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

            //if (Request.Headers.Range != null)
            //{
            //    try
            //    {
            //        HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
            //        partialResponse.Content = new ByteRangeStreamContent(memStream, Request.Headers.Range, _mediaType);

            //       partialResponse.Content.Headers.ContentLength = azureBlob.FileSize;
            //       partialResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(azureBlob.ContentType);
            //       partialResponse.Content.Headers.Add("x-filename", azureBlob.FileName);
            //       partialResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //       partialResponse.Content.Headers.ContentDisposition.FileName = azureBlob.FileName;
            //        partialResponse.Content.Headers.ContentDisposition.Size = azureBlob.FileSize;
            //        return partialResponse;
            //    }
            //    catch (InvalidByteRangeException invalidByteRangeException)
            //    {
            //        return Request.CreateErrorResponse(invalidByteRangeException);
            //    }
            //}
            //else
            //{
            //    HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
            //    fullResponse.Content = new StreamContent(memStream);
            //    //fullResponse.Content.Headers.ContentType = _mediaType;
            //    //fullResponse.Content.Headers.Add("x-filename", azureBlob.FileName);
            //    fullResponse.Content.Headers.ContentType = _mediaType;
            //    fullResponse.Content.Headers.ContentLength = azureBlob.FileSize;
            //    fullResponse.Content.Headers.Add("x-filename", azureBlob.FileName);
            //    fullResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //    fullResponse.Content.Headers.ContentDisposition.FileName = azureBlob.FileName;
            //    fullResponse.Content.Headers.ContentDisposition.Size = azureBlob.FileSize;


            //    return fullResponse;
            //}
            // return response;
        }
        public async Task<CloudBlockBlob> GetAzureBlobItem(string folderName, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(folderName);
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

                return blob;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public async Task<AzureBlobModel> GetAzureBlob(string folderName, string fileName)
        {


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(folderName);
            // CloudBlockBlob blob = container.GetBlockBlobReference(fileName);


            var cloudBlockBlob = container.GetBlockBlobReference(fileName);
            var stream = await cloudBlockBlob.OpenReadAsync();

            var blob = new AzureBlobModel()
            {
                FileName = fileName,
                FileSize = cloudBlockBlob.Properties.Length,
                Stream = stream,
                ContentType = cloudBlockBlob.Properties.ContentType
            };

            return blob;
        }


        //[HttpGet]
        //public Task<HttpResponseMessage> GetProfileImage(string id)
        //{

        //    //var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
        //    //var serverPath = Path.Combine(rootPath, id);
        //    //var fileExtension = Path.GetExtension(id);

        //    //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //    //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    //CloudBlobContainer container = blobClient.GetContainerReference("");
        //    //CloudBlockBlob blob = container.GetBlockBlobReference("");

        //    //using (var memStream = new MemoryStream())
        //    //{
        //    //    blob.DownloadToStream(memStream);
        //    //    // return File(memStream.ToArray(), blob.Properties.ContentType);
        //    //}




        //    ////  localFilePath = getFileFromID(id, out fileName, out fileSize);

        //    //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    //response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));
        //    //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    //response.Content.Headers.ContentDisposition.FileName = id;
        //    //response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

        //    //return response;
        //}

        //public FileResult DownloadBlob(string folderName, string fileName)
        //{
        //    // The code in this section goes here.



        //    //return new FileContentResult()
        //}

        public Task<HttpResponseMessage> GetProfileImage(Guid userId)
        {
            using (UserProfileRepository userProfileRepository = new UserProfileRepository())
            {
                var user = userProfileRepository.GetById(userId);
                if (user != null)
                {
                    return GetProfileImageByName(user.ProfileImageString);
                }

                return null;
            }
        }

        public Task<HttpResponseMessage> GetProfileImageByName(string name)
        {
            return GetFile("profile-images", name);
        }

        private Task<HttpResponseMessage> GetImageByName(string name)
        {
            return GetFile("post-attachment-images", name);
        }

        private Task<HttpResponseMessage> GetImage(Guid fileId)
        {
            using (PostAttachmentRepostitory postAttachmentRepostitory = new PostAttachmentRepostitory())
            {
                var user = postAttachmentRepostitory.GetById(fileId);
                if (user != null)
                {
                    return GetImageByName(user.FilePath);
                }

                return null;
            }
        }
        private Task<HttpResponseMessage> GetAudioByName(string name)
        {
            return GetFile("post-attachment-audios", name);
        }
        private Task<HttpResponseMessage> GetAudio(Guid fileId)
        {
            using (PostAttachmentRepostitory postAttachmentRepostitory = new PostAttachmentRepostitory())
            {
                var user = postAttachmentRepostitory.GetById(fileId);
                if (user != null)
                {
                    return GetAudioByName(user.FilePath);
                }

                return null;
            }
        }
        private Task<HttpResponseMessage> GetVideoByName(string name)
        {
            return GetFile("post-attachment-videos", name);
        }

        private Task<HttpResponseMessage> GetVideo(Guid fileId)
        {
            using (PostAttachmentRepostitory postAttachmentRepostitory = new PostAttachmentRepostitory())
            {
                var user = postAttachmentRepostitory.GetById(fileId);
                if (user != null)
                {
                    return GetVideoByName(user.FilePath);
                }

                return null;
            }
        }

        //public async Task UploadStreamfile()
        //{
        //    string _AADTenantDomain = ConfigurationManager.AppSettings["AADTenantDomain"];
        //    string _RESTAPIEndpoint = ConfigurationManager.AppSettings["MediaServiceRESTAPIEndpoint"];

        //    var tokenCredentials = new AzureAdTokenCredentials(_AADTenantDomain, AzureEnvironments.AzureCloudEnvironment);
        //    var tokenProvider = new AzureAdTokenProvider(tokenCredentials);
        //    CloudMediaContext _context = new CloudMediaContext(new Uri(_RESTAPIEndpoint), tokenProvider);
        //    IAsset inputAsset = UploadFile(_context, @"C:\VideoFiles\BigBuckBunny.mp4", AssetCreationOptions.None);

        //}

        //public void UploadToMediaServices(Uri storageAddress)
        //{
        //    var filePath = storageAddress.ToString();
        //    var context = new CloudMediaContext("Name", "Key");
        //    var uploadAsset = context.Assets..Create(Path.GetFileNameWithoutExtension(fileP‌​ath), AssetCreationOptions.None);
        //    var assetFile = uploadAsset.AssetFiles.Create(Path.GetFileName(filePath));
        //    assetFile.Upload(filePath);
        //}
        //static public IAsset UploadFile(CloudMediaContext _context, string fileName, AssetCreationOptions options)
        //{
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    CloudBlobContainer container = blobClient.GetContainerReference(folder);
        //    container.CreateIfNotExists();
        //    CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

        //    blob.UploadFromStream(fileData);


        //    IAsset inputAsset = _context.Assets.CreateFromBlob(
        //        fileName,
        //        options,
        //        (af, p) =>
        //        {
        //            Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
        //        });

        //    Console.WriteLine("Asset {0} created.", inputAsset.Id);

        //    return inputAsset;
        //}
    }

    public class AzureBlobModel
    {
        public string FileName { get; set; }

        public long? FileSize { get; set; }

        public Stream Stream { get; set; }

        public string ContentType { get; set; }
    }
}
