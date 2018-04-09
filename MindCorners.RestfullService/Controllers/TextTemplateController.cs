using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Model;
using MindCorners.Models;

namespace MindCorners.RestfullService.Controllers
{
    public class TextTemplateController : BaseController
    {
        //private UserContactRepository _userContactRepository;
        public TextTemplateController()
        {
            //_userContactRepository = new UserContactRepository(Context, DbUser, null);
        }
        [HttpGet]
        public async Task<List<Models.TextTemplate>> GetAll()
        {
            var dbUser = DbUser;
            using (TextTemplateRepository _textTemplateRepository = new TextTemplateRepository(Context, dbUser, null))
            {
                var list = _textTemplateRepository.GetAll();
                if (list != null)
                {
                    return list.Select(p => new Models.TextTemplate()
                    {
                        Id = p.Id,
                        Text = p.Text
                    }).ToList();
                }

                return new List<Models.TextTemplate>();
            }
        }

        [HttpGet]
        public async Task<List<Contact>> GetOnlySelectedForCircle(Guid circleId)
        {
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, DbUser, null))
            {
                var selectedUsers = _userContactRepository.GetAllCircleUsers(circleId);
                if (selectedUsers != null)
                {
                    return selectedUsers.Select(p => new Contact()
                    {
                        Id = p.Id,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = p.ProfileImageString,
                        IsSelected = true
                    }).ToList();
                }

                return new List<Contact>();
            }
        }

        [HttpGet]
        public async Task<List<Contact>> GetAllWithSelectedForCircle(Guid circleId)
        {
            var dbUser = DbUser;
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                var selectedUsers = _userContactRepository.GetAllByCircleIdAndUserId(dbUser, circleId);
                if (selectedUsers != null)
                {
                    return selectedUsers.Select(p => new Contact()
                    {
                        Id = p.Id,
                        //Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = p.ProfileImageString,
                        IsSelected = p.IsSelectedInCircle
                    }).ToList();
                }

                return new List<Contact>();
            }
        }

        [HttpGet]
        public async Task<Contact> GetById(Guid userId)
        {
            var dbUser = DbUser;
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            {
                var user = _userProfileRepository.GetById(userId);
                if (user != null)
                {
                    return new Contact()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfileImageString = user.ProfileImageString
                    };
                }

                return new Contact();
            }
        }
    }
}
