using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Models;
using MindCorners.Models.Results;

namespace MindCorners.DAL
{
    public class PostAttachmentRepository : RestService
    {
        public async Task<List<PostAttachment>> GetAttachments(Guid id)
        {
            return await GetResult<List<PostAttachment>>(string.Format("Post/GetAttachments?postId={0}", id));
        }
        public async Task<PostAttachment> GetMainAttachment(Guid id)
        {
            return await GetResult<PostAttachment>(string.Format("Post/GetMainAttachment?postId={0}", id));
        }

        public async Task<FilePathResult> UploadFile(AttachmentFile item)
        {
            return await PostResult<FilePathResult, AttachmentFile>("api/Post/UploadFile", item);
        }
    }
}
