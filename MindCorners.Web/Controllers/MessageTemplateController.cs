using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Web.Mvc;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Model;
using MindCorners.Web.Code;
using MindCorners.Web.Code.BaseController;

namespace MindCorners.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MessageTemplateController : ListControllerCustomBinding<ListFilter, MessageTemplate, MessageTemplates_GetAll_Result, MessageTemplateRepository>
    {
        #region Private Properties

        private MessageTemplateRepository _applicationRepository;
        #endregion

        #region Constructors
        public MessageTemplateController()
            : base((int)ModuleName.MessageTemplate)
        {

        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _applicationRepository = new MessageTemplateRepository(Context, CurrentUserId, CurrentUserOrganizationID);
        }

        #endregion

        #region CustomBinding

        protected override GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "Id";
            viewModel.Columns.Add("Name");
            viewModel.Columns.Add("Type");
            viewModel.Columns.Add("IsEmail");
            return viewModel;
        }

        protected override void LoadListHeaderFilters(CriteriaOperator where)
        {
            ViewBag.Types = Utilities.EnumToDictionary<MessageTemplateTypes>();
        }

        public override ReadOnlyCollection<GridViewColumnState> DefaultSortExpressionState()
        {
            GridViewColumnState gsState = new GridViewColumnState() { SortIndex = 0, FieldName = "DateCreated", SortOrder = ColumnSortOrder.Descending };
            return new ReadOnlyCollection<GridViewColumnState>(new List<GridViewColumnState>() { gsState });
        }


        #endregion

        #region Actions

        public ActionResult Create()
        {
            LoadViewBags();
            return View("Form", new MessageTemplate());
        }

        public ActionResult Edit(Guid id)
        {
            LoadViewBags();
            var application = _applicationRepository.GetById(id);
            return View("Form", application);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApplication(MessageTemplate application, bool close = false)
        {
            if (ModelState.IsValid)
            {
                application.Id = Guid.NewGuid();
                _applicationRepository.Create(application);
                Context.SaveChanges();
            }

            return close ? RedirectToAction("Index") : RedirectToAction("Edit", new { id = application.Id });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditApplication(Guid id, FormCollection collection, bool close = false)
        {
            var application = _applicationRepository.GetById(id);

            if (application != null && TryUpdateModel(application))
            {
                _applicationRepository.Update(application);
                Context.SaveChanges();
            }
            return close ? RedirectToAction("Index") : RedirectToAction("Edit", new { id = id });
        }

        public ActionResult Delete(Guid id)
        {
            var application = _applicationRepository.GetById(id);

            if (application != null)
            {
                _applicationRepository.Delete(application);
                Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Overrides

        #endregion

        #region Private Methods

        private void LoadViewBags()
        {
            ViewBag.Types = Utilities.EnumToDictionary<MessageTemplateTypes>();
        }

        #endregion
    }
}