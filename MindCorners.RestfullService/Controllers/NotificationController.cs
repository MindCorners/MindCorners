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
using MindCorners.Annotations;
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
    public class NotificationController : BaseController
    {

        [HttpGet]
        public async Task<List<Models.Notification>> Get(int skip, int take)
        {
            var dbUser = DbUser;
            using (NotificationRepository _notificationRepository = new NotificationRepository(Context, dbUser, null))
            {
                var list = _notificationRepository.Get(dbUser, skip, take);
                if (list != null)
                {
                    var result = list.ToList();
                    result.ForEach(
                        p => p.UserProfileImageName = Request.GetFileUrl((int)FileType.Profile, p.UserProfileImageName));
                    return result;
                }
                return new List<Models.Notification>();
            }
        }
        [HttpGet]
        public async Task<int> GetUnreadCount()
        {
            var dbUser = DbUser;
            using (NotificationRepository _notificationRepository = new NotificationRepository(Context, dbUser, null))
            {
                var item = _notificationRepository.GetCountUnread(dbUser);
                return item;
            }
        }
        [HttpGet]
        public async Task<Models.Notification> GetItem(Guid id)
        {
            var dbUser = DbUser;
            using (NotificationRepository _notificationRepository = new NotificationRepository(Context, dbUser, null))
            {
                var item = _notificationRepository.GetById(id);
                return new Models.Notification()
                {
                    Id = item.Id,
                    Type = item.Type,
                    SourceId = item.SourceId
                };
            }
        }

        [Route("api/Notification/Submit")]
        [HttpPost]
        public async Task<IdResult> Submit(Models.Notification item)
        {
            if (item == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No post info"
                };
            }

            Common.Model.Notification itemDb = null;
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NotificationRepository _notificationRepository = new NotificationRepository(Context, dbUser, null))
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

                            itemDb = new Common.Model.Notification()
                            {

                            };
                            _notificationRepository.Create(itemDb);
                        }
                        else
                        {
                            itemDb = _notificationRepository.GetById(item.Id);
                            _notificationRepository.Update(itemDb);
                        }

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

        [Route("api/Notification/UpdateRead")]
        [HttpPost]
        public async Task<IdResult> UpdateRead(Models.Notification item)
        {
            if (item == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No notification info"
                };
            }

            Common.Model.Notification itemDb = null;
            var dbUser = DbUser;

            using (NotificationRepository _notificationRepository = new NotificationRepository(Context, dbUser, null))
            {
                itemDb = _notificationRepository.GetById(item.Id);
                if (itemDb != null)
                {
                    try
                    {
                        _notificationRepository.Update(itemDb);
                        itemDb.ReadDate = DateTime.Now;
                        Context.SaveChanges();
                        return new IdResult()
                        {
                            IsOk = true,
                            Id = itemDb.Id
                        };
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError(e);
                        return new IdResult()
                        {
                            IsOk = false,
                            ErrorMessage = "Error On Save"
                        };
                    }
                }
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No notification info"
                };
            }
        }
    }
}
