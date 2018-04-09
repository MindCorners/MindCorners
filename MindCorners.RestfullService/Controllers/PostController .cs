using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
//using MindCorners.Annotations;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Models.Results;
using Circle = MindCorners.Models.Circle;
using Post = MindCorners.Common.Model.Post;
using System.Net.Http.Headers;
using System.Web;
using MimeTypes;
using MindCorners.Common.Code;
using MindCorners.RestfullService.Code;

namespace MindCorners.RestfullService.Controllers
{
    public class PostController : BaseController
    {

        [HttpGet]
        public async Task<List<Models.Post>> GetLatest()
        {
            var dbUser = DbUser;
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                var selectedUsers = _postRepository.GetLatest(dbUser, 0, 10, null);
                if (selectedUsers != null)
                {
                    var result = selectedUsers.ToList();
                    result.ForEach(
                        p => p.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, p.UserProfileImageName));
                    return result;
                }
                return new List<Models.Post>();
            }
        }

        [HttpGet]
        public async Task<List<Models.Post>> GetForArchive(int skip, int take, string searchText)
        {
            var dbUser = DbUser;
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                var selectedUsers = _postRepository.GetLatest(dbUser, skip, take, searchText);
                if (selectedUsers != null)
                {
                    var result = selectedUsers.ToList();
                    result.ForEach(
                        p => p.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, p.UserProfileImageName));
                    return result;
                }
                return new List<Models.Post>();
            }
        }

        [HttpGet]
        public async Task<Models.Post> GetItem(Guid id)
        {
            var dbUser = DbUser;
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                var rootUrl = ServiseRootUrl;
                var item = _postRepository.GetByIdForChat(id, rootUrl);
                item.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, item.UserProfileImageName);
                return item;
            }
        }

        [HttpGet]
        public async Task<Models.PostAttachment> GetMainAttachment(Guid postId)
        {
            var dbUser = DbUser;
            using (PostAttachmentRepostitory _postAttachmentRepostitory = new PostAttachmentRepostitory(Context, dbUser, null))
            {
                var rootUrl = ServiseRootUrl;
                var item = _postAttachmentRepostitory.GetMainByPostId(postId, rootUrl);

                return item;
            }
        }

        [HttpGet]
        public async Task<List<Models.PostAttachment>> GetAttachments(Guid postId)
        {
            var dbUser = DbUser;
            using (PostAttachmentRepostitory _postAttachmentRepostitory = new PostAttachmentRepostitory(Context, dbUser, null))
            {
                var rootUrl = ServiseRootUrl;
                var item = _postAttachmentRepostitory.GetAllByPostId(postId, rootUrl);

                return item;
            }
        }

        [HttpGet]
        public async Task<List<Models.Post>> GetReplies(Guid postId)
        {
            var dbUser = DbUser;
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                var rootUrl = ServiseRootUrl;
                var item = _postRepository.GetPostReplies(postId, rootUrl);

                var result = item.ToList();
                result.ForEach(
                    p => p.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, p.UserProfileImageName));
                return result;

            }
        }
        [HttpGet]
        public async Task<List<Models.Post>> GetReplyTellMeMores(Guid replyId)
        {
            var dbUser = DbUser;
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                var item = _postRepository.GetReplyTellMeMores(replyId);

                var result = item.ToList();
                result.ForEach(
                    p => p.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, p.UserProfileImageName));
                return result;

            }
        }

        [Route("api/Post/Submit")]
        [HttpPost]
        public async Task<IdResult> Submit(Models.Post item)
        {
            if (item == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No post info"
                };
            }

            Common.Model.Post itemDb = null;
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                {
                    try
                    {
                        if (item.Id == Guid.Empty)
                        {
                            //if (item.SelectedContact != null)
                            //{
                            //    var contactCircle = _userContactRepository.GetCircleByUsersPair(dbUser,
                            //        item.SelectedContact.Id);
                            //    if (contactCircle != null)
                            //    {
                            //        item.SelectedCircle = new Circle() { Id = contactCircle.Id };
                            //    }
                            //}

                            itemDb = new Common.Model.Post()
                            {
                                Title = item.Title,
                                Type = item.Type,
                                CircleId = item.CircleId ?? item.SelectedCircle?.Id,
                                UserId = item.UserId ?? item.SelectedContact?.Id,
                                ParentId = item.ParentId
                                //LastUpdatedDate = DateTime.Now,
                                //LastUpdatedUserId = dbUser,
                            };
                            _postRepository.Create(itemDb);
                        }
                        else
                        {
                            itemDb = _postRepository.GetById(item.Id);
                            itemDb.Title = item.Title;
                            itemDb.Type = item.Type;
                            _postRepository.Update(itemDb);
                        }

                        Context.SaveChanges();
                        if (itemDb != null)
                        {
                            if (itemDb.Type == (int)PostTypes.Prompt)
                            {
                                if (itemDb.UserId.HasValue)
                                {
                                    Utilities.AddNotification(dbUser, itemDb.UserId.Value, itemDb.Id, (int)NotificationTypes.NewPromptInCircle,
                                        string.Format("You've got new Prompt"));
                                }
                                else if (itemDb.CircleId.HasValue)
                                {
                                    var usersOfCircle = _userContactRepository.GetAllCircleUsers(itemDb.CircleId.Value);
                                    if (usersOfCircle != null)
                                    {
                                        foreach (var userProfile in usersOfCircle)
                                        {
                                            Utilities.AddNotification(dbUser, userProfile.Id, itemDb.Id, (int)NotificationTypes.NewPromptInCircle, string.Format("You've got new Prompt"));
                                        }
                                    }
                                }
                            }
                            else if (itemDb.Type == (int)PostTypes.Reply && itemDb.ParentId.HasValue)
                            {
                                var parentPost = _postRepository.GetById(itemDb.ParentId.Value);
                                if (parentPost != null)
                                {
                                    Utilities.AddNotification(dbUser, parentPost.CreatorId, parentPost.Id, (int)NotificationTypes.Reply,
                                        string.Format("You've got new Reply on you post:{0}", parentPost.Title));
                                }
                            }
                            else if (itemDb.Type == (int)PostTypes.TellMeMore && itemDb.ParentId.HasValue)
                            {
                                var parentReplyPost = _postRepository.GetById(itemDb.ParentId.Value);
                                if (parentReplyPost != null && parentReplyPost.ParentId.HasValue)
                                {
                                    var parentPost = _postRepository.GetById(parentReplyPost.ParentId.Value);
                                    if (parentPost != null)
                                    {
                                        Utilities.AddNotification(dbUser, parentReplyPost.CreatorId, parentPost.Id, (int)NotificationTypes.TellMeMore,
                                      string.Format("You've got TellMeMore on you reply in post:{0}", parentPost.Title));
                                    }
                                }
                            }

                            transactionScope.Complete();
                            return new IdResult()
                            {
                                IsOk = true,
                                Id = itemDb.Id
                            };
                        }
                        return new IdResult()
                        {
                            IsOk = false,
                            ErrorMessage = "Error On Save"
                        };
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError(e);
                        return new IdResult()
                        {
                            IsOk = false,
                            ErrorMessage = e.ToString()
                        };
                    }
                }
            }
        }


        [Route("api/Post/Delete")]
        [HttpPost]
        public async Task<BoolResult> Delete(Post post)
        {
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            {
                try
                {
                    var postItem = _postRepository.GetById(post.Id);
                    if (postItem == null)
                    {
                        return new BoolResult()
                        {
                            IsOk = false,
                            ErrorMessage = "No post info"
                        };
                    }
                    _postRepository.Delete(postItem);
                    Context.SaveChanges();
                    transactionScope.Complete();
                    return new BoolResult()
                    {
                        IsOk = true
                    };
                }
                catch (Exception e)
                {
                    LogHelper.WriteError(e);
                    return new BoolResult()
                    {
                        IsOk = false,
                        ErrorMessage = e.ToString()
                    };
                }

            }
        }


        [Route("api/Post/SubmitAttachment")]
        [HttpPost]
        public async Task<IdResult> SubmitAttachment(Models.PostAttachment item)
        {
            if (item == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No post attachment info"
                };
            }

            Common.Model.PostAttachment itemDb = null;
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (PostAttachmentRepostitory _postAttachmentRepostitory = new PostAttachmentRepostitory(Context, dbUser, null))
            {
                {
                    try
                    {
                        itemDb = new Common.Model.PostAttachment()
                        {
                            PostId = item.PostId,
                            Text = item.Text,
                            Type = item.Type.HasValue ? item.Type.Value : (int)ChatType.Text,
                            FilePath = item.FilePath,
                            FileDuration = item.FileDuration,
                            IsMainAttachment = item.IsMainAttachment
                            //LastUpdatedDate = DateTime.Now,
                            //LastUpdatedUserId = dbUser,
                        };
                        _postAttachmentRepostitory.Create(itemDb);

                        Context.SaveChanges();
                        if (itemDb != null)
                        {
                            transactionScope.Complete();
                            return new IdResult()
                            {
                                IsOk = true,
                                Id = itemDb.Id
                            };
                        }
                        return new IdResult()
                        {
                            IsOk = false,
                            ErrorMessage = "Error On Save"
                        };
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError(e);
                        return new IdResult()
                        {
                            IsOk = false,
                            ErrorMessage = e.ToString()
                        };
                    }
                }
            }
        }


        //[Route("api/Post/UploadFile")]
        //[HttpPost]
        //public async Task<FilePathResult> UploadFile(Models.AttachmentFile file)
        //{
        //    if (file == null || file.FileData == null)
        //    {
        //        return new FilePathResult()
        //        {
        //            IsOk = false,
        //            ErrorMessage = "No file info"
        //        };
        //    }

        //    var dbUser = DbUser;
        //    var fileUnicName = Guid.NewGuid();
        //    var fileExtention = file.FileExtention;
        //    var fileNewName = string.Format("{0}{1}", fileUnicName, fileExtention);
        //    var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
        //    if (!Directory.Exists(rootPath))
        //        Directory.CreateDirectory(rootPath);

        //    var fileSource = Path.Combine(rootPath, file.FileName);
        //    try
        //    {
        //        File.WriteAllBytes(fileSource, file.FileData);

        //        return new FilePathResult()
        //        {
        //            IsOk = true,
        //            FileName = fileNewName,
        //            //  FileUrl = FormUrlEncodedContent.
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.WriteError(e);
        //        //Console.WriteLine(e.Message);
        //        return new FilePathResult()
        //        {
        //            IsOk = false,
        //            ErrorMessage = "Errow while uploading file"
        //        };
        //    }
        //}

        [Route("api/Post/UploadFile")]
        [HttpPost]
        public async Task<FilePathResult> UploadFile(Models.AttachmentFile file)
        {
            if (file == null || file.FileData == null)
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No file info"
                };
            }

            var dbUser = DbUser;
            var fileUnicName = Guid.NewGuid();
            var fileExtention = Path.GetExtension(file.FileName);
            var fileNewName = string.Format("{0}{1}", fileUnicName, fileExtention);

            Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType(file.AttachmentType), fileNewName, file.FileData);
            //ConfigurationManager.AppSettings["RootPathToFiles"];
            //var request = ;

            try
            {
                string thumbnailUrl = null;

                if (file.AttachmentType == (int)ChatType.Video)
                {
                    var thumbNailFile = string.Format("Thumb_{0}.png", fileUnicName);
                    Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType(file.AttachmentType), thumbNailFile, file.ThumbnailFileData);
                    thumbnailUrl = Request.GetFileUrl((int)FileType.Video, thumbNailFile);


                    //var rootPath = HttpContext.Current.Server.MapPath("~/TempFolder");
                    //if (!Directory.Exists(rootPath))
                    //    Directory.CreateDirectory(rootPath);

                    //var fileSource = Path.Combine(rootPath, fileNewName);

                    //Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType(file.AttachmentType), fileNewName, file.FileData);
                    //File.WriteAllBytes(fileSource, file.FileData);

                    //thumbnailUrl = SaveThumbnail(fileNewName, string.Format("Thumb_{0}.jpg", fileUnicName));
                }

                return new FilePathResult()
                {
                    IsOk = true,
                    FileName = fileNewName,
                    FileUrl = Request.GetFileUrl(file.AttachmentType, fileNewName),
                    ThumbnailUrl = thumbnailUrl
                };
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
                //Console.WriteLine(e.Message);
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "Errow while uploading file"
                };
            }
        }

        [Route("api/Post/SubmitPostAndAttachment")]
        [HttpPost]
        public async Task<FilePathResult> SubmitPostAndAttachment(Models.PostAndPostAttachment post)
        {
            var file = post.AttachmentFile;
            var item = post.PostAttachment;

            if (post.Post == null)
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No post info"
                };
            }
            if (item == null)
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No post attachment info"
                };
            }

            if (post.IsFile && (post.AttachmentFile == null || post.AttachmentFile.FileData == null))
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No file info"
                };
            }
            Common.Model.Post itemDb = null;
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (PostRepository _postRepository = new PostRepository(Context, dbUser, null))
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            using (PostAttachmentRepostitory _postAttachmentRepostitory = new PostAttachmentRepostitory(Context, dbUser, null))
            {
                {
                    try
                    {
                        if (post.Post.Id == Guid.Empty)
                        {
                            //if (item.SelectedContact != null)
                            //{
                            //    var contactCircle = _userContactRepository.GetCircleByUsersPair(dbUser,
                            //        item.SelectedContact.Id);
                            //    if (contactCircle != null)
                            //    {
                            //        item.SelectedCircle = new Circle() { Id = contactCircle.Id };
                            //    }
                            //}

                            itemDb = new Common.Model.Post()
                            {
                                Title = post.Post.Title,
                                Type = post.Post.Type,
                                CircleId = post.Post.CircleId ?? post.Post.SelectedCircle?.Id,
                                UserId = post.Post.UserId ?? post.Post.SelectedContact?.Id,
                                ParentId = post.Post.ParentId
                                //LastUpdatedDate = DateTime.Now,
                                //LastUpdatedUserId = dbUser,
                            };
                            _postRepository.Create(itemDb);
                        }
                        else
                        {
                            itemDb = _postRepository.GetById(post.Post.Id);
                            itemDb.Title = post.Post.Title;
                            itemDb.Type = post.Post.Type;
                            _postRepository.Update(itemDb);
                        }

                        Context.SaveChanges();

                        /*Add Attachment*/

                        Common.Model.PostAttachment itemAttachmentDb = null;
                        try
                        {
                            itemAttachmentDb = new Common.Model.PostAttachment()
                            {
                                PostId = itemDb.Id,
                                Text = item.Text,
                                Type = item.Type.HasValue ? item.Type.Value : (int)ChatType.Text,
                                FilePath = item.FilePath,
                                FileDuration = item.FileDuration,
                                IsMainAttachment = item.IsMainAttachment
                                //LastUpdatedDate = DateTime.Now,
                                //LastUpdatedUserId = dbUser,
                            };
                            _postAttachmentRepostitory.Create(itemAttachmentDb);
                            Context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteError(e);
                            return new FilePathResult()
                            {
                                IsOk = false,
                                ErrorMessage = e.ToString()
                            };
                        }

                        if (itemDb != null)
                        {
                            if (itemDb.Type == (int)PostTypes.Prompt)
                            {
                                if (itemDb.UserId.HasValue)
                                {
                                    Utilities.AddNotification(dbUser, itemDb.UserId.Value, itemDb.Id,
                                        (int)NotificationTypes.NewPromptInCircle,
                                        string.Format("You've got new Prompt"));
                                }
                                else if (itemDb.CircleId.HasValue)
                                {
                                    var usersOfCircle = _userContactRepository.GetAllCircleUsers(itemDb.CircleId.Value);
                                    if (usersOfCircle != null)
                                    {
                                        foreach (var userProfile in usersOfCircle)
                                        {
                                            Utilities.AddNotification(dbUser, userProfile.Id, itemDb.Id,
                                                (int)NotificationTypes.NewPromptInCircle,
                                                string.Format("You've got new Prompt"));
                                        }
                                    }
                                }
                            }
                            else if (itemDb.Type == (int)PostTypes.Reply && itemDb.ParentId.HasValue)
                            {
                                var parentPost = _postRepository.GetById(itemDb.ParentId.Value);
                                if (parentPost != null && parentPost.CreatorId != dbUser)
                                {
                                    Utilities.AddNotification(dbUser, parentPost.CreatorId, parentPost.Id,
                                        (int)NotificationTypes.Reply,
                                        string.Format("You've got new Reply on you post:{0}", parentPost.Title));
                                }
                            }
                            else if (itemDb.Type == (int)PostTypes.TellMeMore && itemDb.ParentId.HasValue)
                            {
                                var parentReplyPost = _postRepository.GetById(itemDb.ParentId.Value);
                                if (parentReplyPost != null && parentReplyPost.ParentId.HasValue)
                                {
                                    var parentPost = _postRepository.GetById(parentReplyPost.ParentId.Value);
                                    if (parentPost != null)
                                    {
                                        Utilities.AddNotification(dbUser, parentReplyPost.CreatorId, parentPost.Id,
                                            (int)NotificationTypes.TellMeMore,
                                            string.Format("You've got TellMeMore on you reply in post:{0}",
                                                parentPost.Title));
                                    }
                                }
                            }
                        }

                        if (!post.IsFile)
                        {
                            transactionScope.Complete();
                            return new FilePathResult()
                            {
                                PostId = itemDb.Id,
                                Id = itemAttachmentDb.Id,
                                IsOk = true
                            };
                        }

                        /*Add file here*/
                        var fileUnicName = Guid.NewGuid();
                        var fileExtention = Path.GetExtension(file.FileName);
                        var fileNewName = string.Format("{0}{1}", fileUnicName, fileExtention);
                        Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType(file.AttachmentType), fileNewName, file.FileData);
                        //ConfigurationManager.AppSettings["RootPathToFiles"];
                        //var request = ;

                        try
                        {
                            itemAttachmentDb.FilePath = fileNewName;
                            Context.SaveChanges();

                            string thumbnailUrl = null;

                            if (file.AttachmentType == (int)ChatType.Video)
                            {
                                var thumbNailFile = string.Format("Thumb_{0}.png", fileUnicName);
                                Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType(file.AttachmentType), thumbNailFile, file.ThumbnailFileData);
                                thumbnailUrl = Request.GetFileUrl((int)FileType.Video, thumbNailFile);
                            }



                            transactionScope.Complete();
                            return new FilePathResult()
                            {
                                PostId = itemDb.Id,
                                Id = itemAttachmentDb.Id,
                                IsOk = true,
                                FileName = fileNewName,
                                FileUrl = Request.GetFileUrl(file.AttachmentType, fileNewName),
                                ThumbnailUrl = thumbnailUrl
                            };

                            //return new FilePathResult()
                            //{
                            //    IsOk = false,
                            //    ErrorMessage = "Error On Save"
                            //};

                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteError(e);
                            //Console.WriteLine(e.Message);
                            return new FilePathResult()
                            {
                                IsOk = false,
                                ErrorMessage = "Errow while uploading file"
                            };
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError(e);
                        return new FilePathResult()
                        {
                            IsOk = false,
                            ErrorMessage = e.ToString()
                        };
                    }
                }
            }
        }




        //[HttpGet]
        //public HttpResponseMessage GetFile(string id)
        //{
        //    if (String.IsNullOrEmpty(id))
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
        //    var serverPath = Path.Combine(rootPath, id);
        //    var fileExtension = Path.GetExtension(id);
        //    //  localFilePath = getFileFromID(id, out fileName, out fileSize);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));
        //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = id;
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

        //    return response;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetVideoThumbnail(string id)
        //{
        //    if (String.IsNullOrEmpty(id))
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
        //    var serverPath = Path.Combine(rootPath, id);
        //    var fileExtension = Path.GetExtension(id);
        //    //  localFilePath = getFileFromID(id, out fileName, out fileSize);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(new FileStream(serverPath, FileMode.Open, FileAccess.Read));
        //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = id;
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(fileExtension));

        //    return response;
        //}

        private string SaveThumbnail(string videoPath, string thumbnailPath)
        {
            if (String.IsNullOrEmpty(videoPath))
                return null;
            //return Request.CreateResponse(HttpStatusCode.BadRequest);

            var rootPath = HttpContext.Current.Server.MapPath("~/TempFolder");
            //ConfigurationManager.AppSettings["RootPathToFiles"];
            var videServerPath = Path.Combine(rootPath, videoPath);
            var thumbnailServerPath = Path.Combine(rootPath, thumbnailPath);
            //var fileExtension = Path.GetExtension(videoPath);

            try
            {
                string parameters = string.Format("-ss {0} -i {1} -f image2 -vframes 1 -y {2}", 0, videServerPath, thumbnailServerPath);

                var processInfo = new ProcessStartInfo();
                processInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utilities", "ffmpeg.exe");
                processInfo.Arguments = parameters;
                //processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;

                // File.Delete(saveThumbnailTo);

                using (var process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                    LogHelper.WriteError(new Exception("BlaBlaBla"));
                    process.WaitForExit();
                }
                var fileData = File.ReadAllBytes(thumbnailServerPath);
                Utilities.UploadBlob(Utilities.GetAzureFolderByFileType((int)FileType.Thumbnail), thumbnailPath, fileData);
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
            }

            try
            {
                var processInfo = new ProcessStartInfo();
                processInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", @"notepad.exe");
                processInfo.UseShellExecute = false;
                using (var process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
            }
            //delete video
            if (File.Exists(videServerPath))
            {
                File.Delete(videServerPath);
            }
            //delete thumbnail
            if (File.Exists(thumbnailServerPath))
            {
                File.Delete(thumbnailServerPath);
            }

            return Request.GetFileUrl((int)FileType.Thumbnail, thumbnailPath);
        }

        [HttpGet]
        public HttpResponseMessage SaveThumbnailFromFile(string videoPath, string thumbnailPath)
        {
            var rootPath = ConfigurationManager.AppSettings["RootPathToFiles"];
            var videServerPath = Path.Combine(rootPath, videoPath);
            var thumbnailServerPath = Path.Combine(rootPath, thumbnailPath);

            var thumbnailUrl = SaveThumbnail(videoPath, thumbnailPath);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(thumbnailServerPath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = thumbnailPath;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(".jpg"));

            return response;
        }

        public static Bitmap GetThumbnail(string video, string thumbnail)
        {
            var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(5000);

            return LoadImage(thumbnail);
        }

        static Bitmap LoadImage(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path));
            return (Bitmap)Image.FromStream(ms);
        }
    }
}
