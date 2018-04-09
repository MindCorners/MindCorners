using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Interfaces;
using System.Web;
using System.Web.Caching;
using MindCorners.Common.Code.Enums;
using NLog;

namespace MindCorners.Common.Model
{
    public class ConfigRepository : GenericRepository<Config>, ICustomBindingListRepository<Config, ListFilter, Configs_GetAll_Result>
    {
        #region Constructors
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        private readonly Guid? _currentUserOrganizationId;
        public ConfigRepository()
        {
            _context = new MindCornersEntities();
        }
        public ConfigRepository(Guid currentUserId)
        {
            _currentUserId = currentUserId;
            _context = new MindCornersEntities();
        }
        public ConfigRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId)
            : base(context, currentUserId, currentUserOrganizationId)
        {
            _currentUserId = currentUserId;
            _context = context;
            _currentUserOrganizationId = currentUserOrganizationId;
        }
        #endregion

        #region DB Methods

        public List<Config> GetAllByOrganizationId(Guid? organizationId)
        {   
            return _context.Configs.Where(p => p.OrganizationId == organizationId && p.DateDeleted == null).ToList();
        }

        #endregion


        public IQueryable GetFilteredApplications(CriteriaOperator @where)
        {
            return _context.Configs_GetAll().AppendWhere(new CriteriaToEFExpressionConverter(), where);
        }

        public int GetFilteredApplicationsCount(CriteriaOperator @where)
        {
            var result = GetFilteredApplications(where).Count();

            return result;
        }

        public ViewType GetDefaultViewType()
        {
            return ViewType.AllApplications;
        }

        #region StaticProperties

        #region Email
        
        public static string SmtpServer(Guid? currentUserOrganizationId)
        {
            return GetSettingValue(ConfigTypes.SmtpServer, currentUserOrganizationId);
        }
       
        public static int SmtpServerPort(Guid? currentUserOrganizationId)
        {
            return GetSettingValue<int>(ConfigTypes.SmtpServerPort, currentUserOrganizationId);
        }
        public static string SmtpServerFrom(Guid? currentUserOrganizationId)
        {
            return GetSettingValue(ConfigTypes.SmtpServerFrom, currentUserOrganizationId);
        }
        public static string SmtpServerUsername(Guid? currentUserOrganizationId)
        {
            return GetSettingValue(ConfigTypes.SmtpServerUsername, currentUserOrganizationId);
        }
        public static string SmtpServerPassword(Guid? currentUserOrganizationId)
        {
            return GetSettingValue(ConfigTypes.SmtpServerPassword, currentUserOrganizationId);
        }
        #endregion

        public static string GetSetting(ConfigTypes type, Guid? currentUserOrganizationId)
        {
            using (var ent = new MindCornersEntities())
            {
                string res = string.Empty;
                var setting = ent.Configs.FirstOrDefault(x => x.Type == (short)type && x.OrganizationId == currentUserOrganizationId);
                if (setting != null)
                    res = setting.Value;

                return res;
            }
        }

        private static string GetSettingValue(ConfigTypes type, Guid? currentUserOrganizationId)
        {
            var settingName = Enum.GetName(typeof(ConfigTypes), type) ?? "";
            object cachedSetting = HttpRuntime.Cache[settingName];
            string settingValue;

            if (cachedSetting == null || string.IsNullOrEmpty(cachedSetting.ToString()))
            {
                settingValue = GetSetting(type, currentUserOrganizationId);

                HttpRuntime.Cache.Add(settingName, settingValue, null, DateTime.Now.AddDays(1),
                                      Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            else
                settingValue = cachedSetting.ToString();

            return settingValue;
        }


        //private string GetSettingValue(short type)
        //{

        //    object cachedSetting = HttpRuntime.Cache[settingValue];

        //    if (cachedSetting == null || string.IsNullOrEmpty(cachedSetting.ToString()))
        //    {
        //        settingValue = GetSetting(type);

        //        HttpRuntime.Cache.Add(settingValue, type, null, DateTime.Now.AddDays(1),
        //                              Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        //    }
        //    else
        //        settingValue = cachedSetting.ToString();

        //    return settingValue;
        //}

        private static T GetSettingValue<T>(ConfigTypes type, Guid? currentUserOrganizationId)
        {
            T res;
            Utilities.TryParse(GetSettingValue(type, currentUserOrganizationId), out res);
            return res;
        }
        #endregion
    }
}
