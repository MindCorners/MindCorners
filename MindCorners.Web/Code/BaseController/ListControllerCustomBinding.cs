using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MindCorners.Web.Code.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Code.Interfaces;
using ViewType = System.Web.Mvc.ViewType;

namespace MindCorners.Web.Code.BaseController
{
    public class ListControllerCustomBinding<TListFilter, TApplication, TListApplication, TListApplicationRepository> : ListBaseController<TListFilter, TApplication, TListApplicationRepository, TListApplication>
        where TListFilter : ListFilter, new()
        where TApplication : class, IEntity
        where TListApplicationRepository : ICustomBindingListRepository<TApplication, TListFilter, TListApplication>
    {

        
        private TListApplicationRepository _listApplicationRepository;

     
        #region Constructors

        public ListControllerCustomBinding(int moduleName)
            : base(moduleName)
        {
           
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
               _listApplicationRepository = (TListApplicationRepository)Activator.CreateInstance(typeof(TListApplicationRepository), new object[] { Context, CurrentUserId, CurrentUserOrganizationID });
            }
        }

        #endregion

        public virtual ActionResult Index()
        {
            // Update list state
            var listState = ListState;
            var filter = new TListFilter();
            string errorText = string.Empty;
            LoadListViewStates(listState, out filter, out errorText);

            if (!string.IsNullOrEmpty(errorText))
            {
                return Content(errorText);
            }

           // var model = _applicationRepository.GetFilteredApplications(filter, listState.ViewType.Value, CurrentUser);
            return View();
        }
        
        #region DevexpressCustomBinding

        public override ActionResult ExportListTo(ExportFormat exportFormat = ExportFormat.Xlsx)
        {
            try
            {
                ExportType exportType = GridViewHelper.ExportTypes.SingleOrDefault(x => x.Format == exportFormat);
                var filter = ListState.ListFilter ?? new TListFilter();

                var listState = ListState;
                if (listState.ViewType.HasValue)
                {
                    string filterString = listState.FilterExpression;
                    var where = GetFilterExpressionState(listState.FilterExpression);
                    //var sortExpression = Common.Code.Utilities.GetSortExpression(listState..SortedColumns, DefaultSortExpressionState());
                    //var filterString = GetFilterExpressionState(e.State.FilterExpression);
                    var applicationList = _listApplicationRepository.GetFilteredApplications(where);
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

        public ActionResult AdvancedCustomBindingPartial(string gridViewName, string listPartial, bool? showFilter)
        {
            var listState = ListState;
            var filter = new TListFilter();
            string errorText = string.Empty;
            LoadListViewStates(listState, out filter, out errorText);


            var viewModel = GridViewExtension.GetViewModel(gridViewName);
            if (viewModel == null)
                viewModel = CreateGridViewModel();
            if (string.IsNullOrEmpty(viewModel.FilterExpression) && listState != null && !string.IsNullOrEmpty(listState.LayoutData))
            { // Session["gvSettings"] stores the client layout
                var filterParams = listState.LayoutData
                    .ToString()
                    .Split('|')
                    .ToList();
                var lengthParam = filterParams.SingleOrDefault(x => x.StartsWith("filter"));
                var pageParam = filterParams.SingleOrDefault(x => x.StartsWith("page"));
                var pageSizeParam = filterParams.SingleOrDefault(x => x.StartsWith("size"));
                if (!string.IsNullOrEmpty(lengthParam))
                {
                    var index = filterParams.IndexOf(lengthParam);
                    var savedFilterExpression = filterParams[index + 1];
                    viewModel.FilterExpression = savedFilterExpression;
                }

                if (!string.IsNullOrEmpty(pageParam))
                {
                    viewModel.Pager.PageIndex = int.Parse(pageParam.Replace("page", "")) - 1;
                }
                if (!string.IsNullOrEmpty(pageSizeParam))
                {
                    viewModel.Pager.PageSize = int.Parse(pageSizeParam.Replace("size", ""));
                }
            }

            

            return AdvancedCustomBindingCore(listPartial, viewModel);
        }

        // Paging
        public ActionResult AdvancedCustomBindingPagingAction(string gridViewName, string listPartial, GridViewPagerState pager)
        {
            var viewModel = GridViewExtension.GetViewModel(gridViewName);
            viewModel.ApplyPagingState(pager);
            return AdvancedCustomBindingCore(listPartial, viewModel);
        }
        // Filtering
        public ActionResult AdvancedCustomBindingFilteringAction(string gridViewName, string listPartial, GridViewFilteringState filteringState)
        {
            var viewModel = GridViewExtension.GetViewModel(gridViewName);
            viewModel.ApplyFilteringState(filteringState);
            return AdvancedCustomBindingCore(listPartial, viewModel);
        }
        // Sorting
        public ActionResult AdvancedCustomBindingSortingAction(string gridViewName, string listPartial, GridViewColumnState column, bool reset)
        {
            var viewModel = GridViewExtension.GetViewModel(gridViewName);
            viewModel.ApplySortingState(column, reset);
            return AdvancedCustomBindingCore(listPartial, viewModel);
        }
 // Sorting
        public ActionResult AdvancedCustomBindingGroupingAction(string gridViewName, string listPartial, GridViewColumnState column)
        {
            var viewModel = GridViewExtension.GetViewModel(gridViewName);
            viewModel.ApplyGroupingState(column);
            return AdvancedCustomBindingCore(listPartial, viewModel);
        }

        private PartialViewResult AdvancedCustomBindingCore(string listPartial, GridViewModel viewModel)
        {
            viewModel.ProcessCustomBinding(GetDataRowCountAdvanced, GetDataAdvanced, null, GetGroupingInfoAdvanced, GetUniqueHeaderFilterValuesAdvanced);
            var where = GetFilterExpressionState(viewModel.FilterExpression);

            ListState.FilterExpression = viewModel.FilterExpression;
            ListState.SortExpression = viewModel.SortedColumns;

            LoadListHeaderFilters(where);
            
            return PartialView(listPartial, viewModel);
        }
        protected virtual void LoadListHeaderFilters(CriteriaOperator where)
        {
            
        }
        protected virtual GridViewModel CreateGridViewModel()
        {
            return null;
        }

        public virtual void GetDataRowCountAdvanced(GridViewCustomBindingGetDataRowCountArgs e)
        {
            var filterString = GetFilterExpressionState(e.FilterExpression);
            e.DataRowCount = _listApplicationRepository.GetFilteredApplicationsCount(filterString);
        }

        public static void GetUniqueHeaderFilterValuesAdvanced(GridViewCustomBindingGetUniqueHeaderFilterValuesArgs e)
        {
           
        }

        public void GetGroupingInfoAdvanced(GridViewCustomBindingGetGroupingInfoArgs e)
        {
           
        }

        public virtual CriteriaOperator AdditionalFilter(CriteriaOperator op, MindCorners.Common.Code.ViewType? viewType)
        {
            return op;
        }

        public virtual System.Collections.ObjectModel.ReadOnlyCollection<GridViewColumnState> DefaultSortExpressionState()
        {
            return null;
        }
        
        protected CriteriaOperator GetFilterExpressionState(string filter)
        {
            //string filterString =   CriteriaOperator op = CriteriaOperator.Parse(filterExpression, 0);;
            CriteriaOperator op = CriteriaOperator.Parse(filter, 0);

            var additionalFilter = AdditionalFilter(op, ListState.ViewType);
            if (additionalFilter != null)
            {
                if (op != null)
                {
                    return CriteriaOperator.And(additionalFilter,op);
                    //return op.CriteriaOperator.And(additionalFilter);
                }
                else
                {
                    return additionalFilter;
                }

            }
            return op;
        }

      
        public virtual void GetDataAdvanced(GridViewCustomBindingGetDataArgs e)
        {
            var filterString =  GetFilterExpressionState(e.State.FilterExpression);
            var sortExpression = Utilities.GetSortExpression(e.State.SortedColumns, DefaultSortExpressionState());
            e.Data = _listApplicationRepository.GetFilteredApplications(filterString)
                .MakeOrderBy(new CriteriaToExpressionConverter(), sortExpression)
                .Skip(e.StartDataRowIndex)
                .Take(100);
        }

        #endregion
        
        private Dictionary<string, object> Extract(CriteriaOperator op)
        {
            if (op == null)
            {
                return null;
            }
            Dictionary<string, object> dict = new Dictionary<string, object>();
            GroupOperator opGroup = op as GroupOperator;
            if (ReferenceEquals(opGroup, null))
            {
                ExtractOne(dict, op);
            }
            else
            {
                if (opGroup.OperatorType == GroupOperatorType.And)
                {
                    foreach (var opn in opGroup.Operands)
                    {
                        ExtractOne(dict, opn);
                    }
                }
            }
            return dict;
        }
        private void ExtractOne(Dictionary<string, object> dict, CriteriaOperator op)
        {
            BinaryOperator opBinary = op as BinaryOperator;
            if (ReferenceEquals(opBinary, null)) return;
            OperandProperty opProperty = opBinary.LeftOperand as OperandProperty;
            OperandValue opValue = opBinary.RightOperand as OperandValue;
            if (ReferenceEquals(opBinary, null) || ReferenceEquals(opValue, null)) return;
            dict.Add(opProperty.PropertyName, opValue.Value);
        }
    }
}