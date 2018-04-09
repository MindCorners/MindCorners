using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MindCorners.Web.Code;
using MindCorners.Web.Code.BaseController;
using MindCorners.Web.Code.Helpers;
using DevExpress.Web;
using DevExpress.Web.Internal;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Code.Interfaces;
using MindCorners.Common.Model;
using ExportType = DevExpress.Export.ExportType;
using ViewType = System.Web.Mvc.ViewType;

namespace MindCorners.Web.Code.BaseController
{
   

    public class ListBaseController<TListFilter, TApplication, TListApplicationRepository, TListApplication> : BaseController
        where TListFilter : ListFilter, new()
        where TApplication : class, IEntity
        where TListApplicationRepository : ICustomBindingListRepository<TApplication, TListFilter, TListApplication>
    {
        #region Properties in session
        protected Dictionary<int, ListState<TListFilter>> ListStates
        {
            get{
                return Session["ListStates_"+_moduleName] as Dictionary<int, ListState<TListFilter>> ?? new Dictionary<int, ListState<TListFilter>>();
            }
            set { Session["ListStates_" + _moduleName] = value; }
        }

        protected bool TempRedirectedToList
        {
            get { return (bool?)TempData["RedirectedToList"] ?? false; }
            set { TempData["RedirectedToList"] = value; }
        }
        #endregion

        #region Private properties

        protected readonly int _moduleName;

        private TListApplicationRepository _applicationRepository;
        protected GridStateRepository _gridStateRepository;
        #endregion

        protected ListState<TListFilter> ListState
        {
            get { return ListStates.ContainsKey(_moduleName) ? ListStates[_moduleName] : new ListState<TListFilter>(); }
            set
            {
                var listStates = ListStates;
                if (ListStates != null)
                {
                    listStates.AddOrReplace(_moduleName, value as ListState<TListFilter>);
                    ListStates = listStates;
                }
            }
        }

        #region Constructors

        public ListBaseController(int moduleName) : base()
        {
            _moduleName = moduleName;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _applicationRepository = (TListApplicationRepository)Activator.CreateInstance(typeof(TListApplicationRepository), new object[] { Context, CurrentUserId, CurrentUserOrganizationID });
                _gridStateRepository = new GridStateRepository(Context);
            }
        }

        #endregion

        public virtual void LoadListViewStates(ListState<TListFilter> listState, out TListFilter listFilter, out string errorText)
        {
            errorText = string.Empty;
            bool havePermission = true;
            listFilter = null;
            // Update list state
            listState.ViewType = ViewBag.ViewType = listState.ViewType ?? _applicationRepository.GetDefaultViewType();

            listState.ShowFilter = listState.ShowFilter;

            if (!TempRedirectedToList)
                LoadListState(listState);
            ListState = listState;
            ViewBag.IsFilterEmpty = listState.ListFilter == null;
            var filter = listState.ListFilter ?? new TListFilter();
            listFilter = filter;
            errorText = string.Empty;
        }

        public virtual ActionResult SetRedirect(string target)
        {
            TempRedirectedToList = true;
            var collection = new FormCollection(Request.QueryString);
            var routeValues = new RouteValueDictionary();
            foreach (string key in collection.Keys)
            {
                if (key != "target")
                    routeValues.Add(key, collection[key]);
            }
            return RedirectToAction(target, routeValues);
        }
        
        public virtual ActionResult ShowHideListFilter()
        {
            var listState = ListState;
            listState.ShowFilter = !listState.ShowFilter;
            ListState = listState;
            return SetRedirect("Index");
        }
        
        protected void LoadListState(ListState<TListFilter> listState)
        {
            var gridState = _gridStateRepository.Load(CurrentUserId, _moduleName);
            if (gridState != null)
            {
                listState.ListFilter = gridState.Filter != null ? Serializer.Deserialize<TListFilter>(gridState.Filter) : null;
                listState.LayoutData = gridState.LayoutData;
                listState.ShowFilter = gridState.ShowFilter;
            }
        }

        [OutputCache(Duration = 0, VaryByParam = "None")]
        public virtual ActionResult SaveListLayoutData()
        {
            var listState = ListState;
            try
            {
                _gridStateRepository.Insert(CurrentUserId, _moduleName,  listState.LayoutData, listState.ShowFilter, ListState.ListFilter != null ? Serializer.Serialize(ListState.ListFilter) : null);
                _gridStateRepository.Save();
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            return Content("Saving layout failed");
        }

        [OutputCache(Duration = 0, VaryByParam = "None")]
        public virtual ActionResult ResetListLayoutData()
        {
            var listState = ListState;
            listState.LayoutData = null;
            listState.ShowFilter = true;
            listState.ListFilter = null;
            ListState = listState;

            try
            {
                _gridStateRepository.Delete(CurrentUserId, _moduleName);
                _gridStateRepository.Save();
                TempData["GlobalAlertMessage"] = "Ok";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            GlobalAlertMessage = "Error";
            return SetRedirect("Index");
        }

        public virtual ActionResult ExportListTo(ExportFormat exportFormat = ExportFormat.Xlsx)
        {
            try
            {
                MindCorners.Web.Code.Helpers.ExportType exportType = GridViewHelper.ExportTypes.SingleOrDefault(x => x.Format == exportFormat);
                var filter = ListState.ListFilter ?? new TListFilter();

                var listState = ListState;
                if (listState.ViewType.HasValue)
                {
                    string filterString = listState.FilterExpression;
                    string where = GetFilterExpression(filterString);
                    var sortExpression = Utilities.GetSortExpression(listState.SortExpression, DefaultSortExpression());

                    var applicationList = _applicationRepository.GetFilteredApplications(filterString);
                    var gridviewSettings = CreateListExportSettings();
                    if (gridviewSettings != null && exportType != null)
                        return exportType.Method(gridviewSettings, applicationList);
                }

                GlobalAlertMessage = "Exporting failed";
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
            }
            return SetRedirect("Index");
        }


        private string GetFilterExpression(string filter)
        {
            string filterString = Utilities.GetFilterStringFromDevExFilter(filter);

            var additionalFilter = AdditionalFilter(ListState.ViewType);
            if (!string.IsNullOrEmpty(additionalFilter))
            {
                if (!string.IsNullOrEmpty(filterString))
                {
                    filterString = additionalFilter + " AND " + filterString;
                }
                else
                {
                    filterString = additionalFilter;
                }
            }

            return !string.IsNullOrEmpty(filterString) ? filterString : null;
        }

        public virtual string AdditionalFilter(MindCorners.Common.Code.ViewType? viewType)
        {
            return null;
        }

        public virtual string DefaultSortExpression()
        {
            return null;
        }
        protected virtual GridViewSettings CreateListExportSettings()
        {
            // throw new NotImplementedException();
            var settings = CreateListExportSettingsColumns();

            settings.Settings.ShowFooter = true;
            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.RowType == GridViewRowType.Data && e.VisibleIndex % 2 == 0)
                    e.BrickStyle.BackColor = System.Drawing.Color.FromArgb(0xEE, 0xEE, 0xEE);
            };

            settings.ClientLayout = (s, e) =>
            {
                if (e.LayoutMode == ClientLayoutMode.Loading)
                {
                    string layoutData = ListState.LayoutData;
                    if (layoutData != null)
                        e.LayoutData = ListState.LayoutData;
                }
            };

            return settings;
        }

        protected virtual GridViewSettings CreateListExportSettingsColumns()
        {
            throw new NotImplementedException();
        }

        public ActionResult ClearListFilter()
        {
            var listState = ListState;
            listState.ListFilter = null;
            ListState = listState;
            return SetRedirect("Index");
        }
    }
}