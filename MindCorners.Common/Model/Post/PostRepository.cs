using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Models.Enums;

namespace MindCorners.Common.Model
{
    public partial class PostRepository : GenericRepository<Post>
    {

        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public PostRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public PostRepository()
        {
            _context = new MindCornersEntities();
        }

        public List<Models.Post> GetLatest(Guid userId, int skip, int take, string searchKey)
        {
            var result = _context.Posts_GetForList(userId, take, skip, searchKey);
            var list = result.Select(p => new Models.Post()
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                CircleId = p.CircleId,
                CircleName = p.CircleName,
                CreatorId = p.CreatorId,
                CreatorFullName = p.UserCreatorFullName,
                LastUpdatedUserId = p.LastUpdatedUserId,
                LastUpdatedUserFullName = p.UserLastUpdatedFullName,
                LastUpdatedDate = p.LastUpdatedDate,
                LastAttachmentId = p.LastAttachmentId,
                LastAttachmentType = p.LastAttachmentType,
                UserProfileImageName = p.UserProfileImageName,
                HasReplies = p.HasReplies,
                DateCreated = p.DateCreated

            }).ToList();



            //(from post in GetAll()
            //            join circle in _context.Circles on post.CircleId equals circle.Id
            //            join userCreator  in _context.UserProfiles on post.CreatorId equals userCreator.Id
            //            join userLastUpdated in _context.UserProfiles.DefaultIfEmpty() on post.LastUpdatedUserId equals userLastUpdated.Id
            //            join circleUser in _context.CircleUsers on circle.Id equals circleUser.CircleId
            //            join userProfile in _context.UserProfiles on circleUser.UserId equals userProfile.Id
            //            where circleUser.UserId == userId 
            //                    && post.DateDeleted == null
            //                    && circle.DateDeleted ==null
            //                    && circleUser.DateDeleted == null
            //                    && (post.Type == (int)PostTypes.Prompt || post.Type == (int)PostTypes.Reply)
            //            orderby post.DateCreated descending 
            //            select new MindCorners.Models.Post()
            //            {
            //                Id=post.Id,
            //                Title = post.Title,
            //                Type  = post.Type,
            //                CircleId=post.CircleId,
            //                CircleName=circle.Name,
            //                CreatorId= post.CreatorId,
            //                CreatorFullName = userCreator.FullName,
            //                LastUpdatedUserId = post.LastUpdatedUserId,
            //                LastUpdatedUserFullName = userLastUpdated.FullName,
            //                LastUpdatedDate =post.LastUpdatedDate
            //            }
            //            ).Take(count).ToList();
            return list;
        }

        public Models.Post GetByIdForChat(Guid id, string url)
        {
            var item = _context.Posts_GetThemeItem(id).FirstOrDefault();
            if (item != null)
            {
                return new Models.Post()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Type = item.Type,
                    CircleId = item.CircleId,
                    CircleName = item.CircleName,
                    CreatorId = item.CreatorId,
                    CreatorFullName = item.UserCreatorFullName,
                    LastUpdatedUserId = item.LastUpdatedUserId,
                    LastUpdatedUserFullName = item.UserLastUpdatedFullName,
                    LastUpdatedDate = item.LastUpdatedDate,
                    MainAttachment = GetMainPostAttachment(item.Id, url)
                };
            }
            return null;
        }

        public List<Models.Post> GetPostReplies(Guid id,string url)
        {
            var list = (from post in GetAll()
                join userCreator in _context.UserProfiles on post.CreatorId equals userCreator.Id
                where post.ParentId == id && post.Type == (int) PostTypes.Reply
                orderby post.DateCreated descending 
                select new Models.Post()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Type = post.Type,
                    CreatorId = post.CreatorId,
                    CreatorFullName = userCreator.FullName,
                    
                    LastUpdatedDate = post.DateCreated,
                    UserProfileImageName = userCreator.ProfileImageString,
                }).ToList();


            foreach (var post in list)
            {
             //   post.Attachments = GetAllPostAttachments(post.Id);

                post.MainAttachment = GetMainPostAttachment(post.Id, url);
            }

            return list.Where(p =>p.MainAttachment != null).ToList();
        }

        public List<Models.Post> GetReplyTellMeMores(Guid id)
        {
            var list = (from post in GetAll()
                        join userAddressat in _context.UserProfiles on post.CreatorId equals userAddressat.Id
                        where post.ParentId == id && post.Type == (int)PostTypes.TellMeMore
                        orderby post.DateCreated
                        select new Models.Post()
                        {
                            Id = post.Id,
                            Type = post.Type,
                            LastUpdatedDate = post.DateCreated,
                            CreatorFullName = userAddressat.FullName,
                            UserProfileImageName = userAddressat.ProfileImageString,
                        }).ToList();
            foreach (var post in list)
            {
                post.MainAttachment = GetMainPostAttachment(post.Id, null);
            }

            return list.Where(p => p.MainAttachment != null).ToList();
        }

        public List<Models.PostAttachment> GetAllPostAttachments(Guid postId, string url)
        {
            return _context.PostAttachments.Where(p => p.DateDeleted == null && p.PostId == postId).Select(p => new Models.PostAttachment()
            {
                Id = p.Id,
                IsMainAttachment = p.IsMainAttachment,
                Type = p.Type,
                Text = p.Text,
                FilePath = p.FilePath,
                FileUrl = url.GetFileUrl(p.Type, p.FilePath),
                FileThumbnailUrl = url.GetVideoTumbnailUrl(p.FilePath),
                FileDuration = p.FileDuration
            }).ToList();
        }

        public Models.PostAttachment GetMainPostAttachment(Guid id,string url)
        {
            var result = (from postAttachment in _context.PostAttachments
                          where postAttachment.DateDeleted == null && postAttachment.PostId == id && postAttachment.IsMainAttachment
                          orderby postAttachment.DateCreated descending
                          select postAttachment).ToList().Select(p => new Models.PostAttachment()
                          {
                              Id = p.Id,
                              IsMainAttachment = p.IsMainAttachment,
                              Type = p.Type,
                              Text = p.Text,
                              FilePath = p.FilePath,
                              FileUrl = url?.GetFileUrl(p.Type, p.FilePath),
                              FileThumbnailUrl = url?.GetVideoTumbnailUrl(p.FilePath),
                              FileDuration=p.FileDuration
                          }).FirstOrDefault();

            return result;
        }
    }
}
