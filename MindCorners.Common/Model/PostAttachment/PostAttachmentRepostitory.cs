using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;

namespace MindCorners.Common.Model
{
    public class PostAttachmentRepostitory: GenericRepository<PostAttachment>
    {
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public PostAttachmentRepostitory(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public PostAttachmentRepostitory()
        {
            _context = new MindCornersEntities();
        }

        public List<Models.PostAttachment> GetAllByPostId(Guid id, string url)
        {
            var result = (from postAttachment in GetAll()
                          where postAttachment.PostId == id
                          select postAttachment).ToList().Select(p =>new Models.PostAttachment()
                          {
                              Id = p.Id,
                              IsMainAttachment = p.IsMainAttachment,
                              Type = p.Type,
                              Text = p.Text,
                              FilePath = p.FilePath,
                              FileUrl = url.GetFileUrl(p.Type, p.FilePath),
                              FileThumbnailUrl = url.GetVideoTumbnailUrl(p.FilePath),
                          }).ToList();

            return result;
        }

        public Models.PostAttachment GetMainByPostId(Guid id, string url)
        {
            var result = (from postAttachment in GetAll()
                          where postAttachment.PostId == id && postAttachment.IsMainAttachment
                          orderby postAttachment.DateCreated descending 
                          select postAttachment).ToList().Select(p => new Models.PostAttachment()
                          {
                              Id = p.Id,
                              IsMainAttachment = p.IsMainAttachment,
                              Type = p.Type,
                              Text = p.Text,
                              FilePath = p.FilePath,
                              FileUrl = url.GetFileUrl(p.Type, p.FilePath),
                              FileThumbnailUrl = url.GetVideoTumbnailUrl(p.FilePath),
                          }).FirstOrDefault();

            return result;
        }
    }
}
