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
    public class ConfigController : ListControllerCustomBinding<ListFilter, Config, Configs_GetAll_Result, ConfigRepository>
    {
        #region Private Properties

        private ConfigRepository _applicationRepository;
        #endregion

        #region Constructors
        public ConfigController()
            : base((int)ModuleName.Config)
        {

        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _applicationRepository = new ConfigRepository(Context, CurrentUserId, CurrentUserOrganizationID);
        }

        #endregion

        #region CustomBinding

        protected override GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "Id";
            viewModel.Columns.Add("OrganizationId");
            viewModel.Columns.Add("FriendlyName");
            viewModel.Columns.Add("Type");
            return viewModel;
        }

        protected override void LoadListHeaderFilters(CriteriaOperator where)
        {
            ViewBag.Types = Utilities.EnumToDictionary<ConfigTypes>();
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
            return View("Form", new Config() {Type = -1});
        }

        public ActionResult Edit(Guid id)
        {
           
            var application = _applicationRepository.GetById(id);
            LoadViewBags(application.Type);
            return View("Form", application);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApplication(Config application, bool close = false)
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

        private void LoadViewBags(short? configType = null)
        {

            var allConfigTypes = Enum.GetValues(typeof (ConfigTypes)).Cast<ConfigTypes>().Select(p =>(short)p).ToList();
            var usedConfigTypes =_applicationRepository.GetAllByOrganizationId(CurrentUserOrganizationID).Select(p => p.Type).ToList();
            if (configType.HasValue)
            {
                usedConfigTypes.Remove(configType.Value);
            }
            
            var firstNotSecond = allConfigTypes.Except(usedConfigTypes).ToList();
            ViewBag.Types = Utilities.EnumValuesToDictionary<ConfigTypes, short>(firstNotSecond);
        }

        #endregion
    }
}