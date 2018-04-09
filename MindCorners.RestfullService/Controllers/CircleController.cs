using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models;
using MindCorners.Models.Results;
using Circle = MindCorners.Models.Circle;

namespace MindCorners.RestfullService.Controllers
{
    public class CircleController : BaseController
    {
        // private CircleRepository _circleRepostiRepository;
        // private InvitationRepository _invitationRepository;
        // private UserContactRepository _userContactRepository;
        public CircleController()
        {

            // _invitationRepository = new InvitationRepository(Context, DbUser, null);
            // _userContactRepository = new UserContactRepository(Context, DbUser, null);
            // _circleRepostiRepository = new CircleRepository(Context, DbUser, null);



        }

        [HttpGet]
        public async Task<List<Circle>> GetAll(Guid userId)
        {
            var resultList = (from circleUser in Context.CircleUsers
                              join circle in Context.Circles on circleUser.CircleId equals circle.Id
                              where circleUser.DateDeleted == null && circle.DateDeleted == null
                              && circleUser.UserId == userId
                              && circle.IsGroup
                              select circle
                                   ).Distinct().ToList();
            return resultList.Select(p => new Circle() { Id = p.Id, Name = p.Name, IsCreatedByUser = p.CreatorId == userId }).ToList();

        }
        [HttpGet]
        public async Task<Circle> GetById(Guid circleId)
        {
            var dbUser = DbUser;
            using (CircleRepository _circleRepostiRepository = new CircleRepository(Context, dbUser, null))
            {
                var circle = _circleRepostiRepository.GetById(circleId);
                if (circle != null)
                {
                    return new Circle { Id = circle.Id, Name = circle.Name, IsCreatedByUser = circle.CreatorId == dbUser };
                }
                return null;
            }
        }

        [Route("api/Circle/Submit")]
        [HttpPost]
        public async Task<IdResult> Submit(Circle circle)
        {

            if (circle == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No circle info"
                };
            }
            if (circle.SelectedContacts == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "Circle can not be empty"
                };
            }

            Common.Model.Circle circleItem = null;
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (CircleRepository _circleRepostiRepository = new CircleRepository(Context, dbUser, null))
            {
                {
                    try
                    {

                        if (circle.Id == Guid.Empty)
                        {
                            circleItem = new Common.Model.Circle()
                            {
                                Name = circle.Name,
                                IsGroup = true,
                            };
                            _circleRepostiRepository.Create(circleItem);
                            Context.SaveChanges();
                            _circleRepostiRepository.AddMainPersonToCircleUser(circleItem.Id, dbUser);
                        }
                        else
                        {
                            circleItem = _circleRepostiRepository.GetById(circle.Id);
                            circleItem.Name = circle.Name;
                            _circleRepostiRepository.Update(circleItem);
                        }

                        var selectedContacts = circle.SelectedContacts.Select(p => p.Id).ToList();
                        _circleRepostiRepository.SynchCircleUsers(circleItem.Id, selectedContacts,circle.Name, dbUser);
                        Context.SaveChanges();
                        transactionScope.Complete();
                        return new IdResult()
                        {
                            IsOk = true,
                            Id = circleItem.Id
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

        [Route("api/Circle/Leave")]
        [HttpPost]
        public async Task<BoolResult> Leave(Circle circle)
        {   
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (CircleRepository _circleRepostiRepository = new CircleRepository(Context, dbUser, null))
            {
                try
                {
                    var circleItem = _circleRepostiRepository.GetById(circle.Id);
                    if (circleItem == null)
                    {
                        return new BoolResult()
                        {
                            IsOk = false,
                            ErrorMessage = "No circle info"
                        };
                    }
                    _circleRepostiRepository.LeaveCircle(circle.Id, dbUser);
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

        [Route("api/Circle/Delete")]
        [HttpPost]
        public async Task<BoolResult> Delete(Circle circle)
        {
            var dbUser = DbUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (CircleRepository _circleRepostiRepository = new CircleRepository(Context, dbUser, null))
            {
                try
                {
                    var circleItem = _circleRepostiRepository.GetById(circle.Id);
                    if (circleItem == null)
                    {
                        return new BoolResult()
                        {
                            IsOk = false,
                            ErrorMessage = "No circle info"
                        };
                    }
                    _circleRepostiRepository.Delete(circleItem);
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
    }
}
