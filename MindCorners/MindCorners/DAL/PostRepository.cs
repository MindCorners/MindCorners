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
    public class PostRepository : RestService
    {
        
        public async Task<List<Post>> GetLatest()
        {
            return await GetResult<List<Post>>(string.Format("Post/GetLatest"));
        }
        public async Task<List<Post>> GetForArchive(int skip, int take, string searchText)
        {
            return await GetResult<List<Post>>(string.Format("Post/GetForArchive?skip={0}&take={1}&searchText={2}",skip, take, searchText));
        }
        public async Task<Post> GetItem(Guid id)
        {
            return await GetResult<Post>(string.Format("Post/GetItem?id={0}", id));
        }
        public async Task<IdResult> Submit(Post post)
        {
            return await PostResult<IdResult, Post>("api/Post/Submit", post);
        }

        public async Task<IdResult> SubmitAttachment(PostAttachment postAttachment)
        {
            return await PostResult<IdResult, PostAttachment>("api/Post/SubmitAttachment", postAttachment);
        }
      
        public async Task<List<Post>> GetRepleis(Guid id)
        {
            return await GetResult<List<Post>>(string.Format("Post/GetReplies?postId={0}", id));
        }

        public async Task<List<Post>> GetReplyTellMeMores(Guid id)
        {
            return await GetResult<List<Post>>(string.Format("Post/GetReplyTellMeMores?replyId={0}", id));
        }

        public async Task<BoolResult> Delete(Post post)
        {
            return await PostResult<BoolResult, Post>("api/Post/Delete", post);
        }

		public async Task<FilePathResult> SubmitPostAndAttachment(PostAndPostAttachment post)
		{
			return await PostResult<FilePathResult, PostAndPostAttachment>("api/Post/SubmitPostAndAttachment", post);
		}
    }
}
