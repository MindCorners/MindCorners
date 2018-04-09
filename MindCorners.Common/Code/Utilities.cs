using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Web.Mvc;
using System.Reflection;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models.Enums;
using MindCorners.Models.Results;
using NLog.Internal;

namespace MindCorners.Common.Code
{
    public static class Utilities
    {
        public static string GetFilterStringFromDevExFilter(string filterExpression)
        {
            CriteriaOperator op = CriteriaOperator.Parse(filterExpression, 0);
            return DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere(op);
        }
        public static string GetSortExpression(System.Collections.ObjectModel.ReadOnlyCollection<GridViewColumnState> columns, string defaultSort)
        {
            if (columns != null)
            {
                List<string> sortList = new List<string>();
                foreach (var column in columns)
                {

                    sortList.Add(string.Format("[{0}] {1}", column.FieldName, column.SortOrder == ColumnSortOrder.Ascending ? "ASC" : "DESC"));
                }
                if (sortList.Any())
                {
                    return string.Join(", ", sortList);
                }
            }

            return defaultSort;
        }


        public static ServerModeOrderDescriptor[] GetSortExpression(
            System.Collections.ObjectModel.ReadOnlyCollection<GridViewColumnState> columns,
            System.Collections.ObjectModel.ReadOnlyCollection<GridViewColumnState> defaultSort)
        {

            ServerModeOrderDescriptor[] orderDescriptors = null;

            if (columns.Any() && defaultSort.Any())
            {
                var joind = columns.ToList();
                joind.AddRange(defaultSort);
                orderDescriptors =
                    joind.Select(
                        c =>
                            new ServerModeOrderDescriptor(new OperandProperty(c.FieldName),
                                c.SortOrder == ColumnSortOrder.Descending)).ToArray();
            }
            else if (columns.Any())
            {
                orderDescriptors =
                    columns.Select(
                        c =>
                            new ServerModeOrderDescriptor(new OperandProperty(c.FieldName),
                                c.SortOrder == ColumnSortOrder.Descending)).ToArray();
            }
            else if (defaultSort.Any())
            {
                orderDescriptors =
                    defaultSort.Select(
                        c =>
                            new ServerModeOrderDescriptor(new OperandProperty(c.FieldName),
                                c.SortOrder == ColumnSortOrder.Descending)).ToArray();
            }
            return orderDescriptors;
        }

        public static Dictionary<TValue, string> EnumValuesToDictionary<T, TValue>(List<TValue> values)
        {
            var dictionary = new Dictionary<TValue, string>();
            var enumerationType = typeof(T);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            foreach (var value in values)
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            return dictionary;
        }

        public static Dictionary<int, string> EnumToDictionary<T>()
        {
            //Dictionary<int, string> resultList = new Dictionary<int, string>();
            //Array itemValues = System.Enum.GetValues(typeof(T));
            //Array itemNames = System.Enum.GetNames(typeof(T));

            //for (int i = 0; i <= itemNames.Length - 1; i++)
            //{
            //    var itemValue = itemNames[i];
            //    resultList.Add((int) , itemValues[i]);
            //}

            //return resultList;

            var enumerationType = typeof(T);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            return dictionary;
        }
        public static bool TryParse<T>(string param, out T thing)
        {
            try
            {
                MethodInfo methodInfo = typeof(T).GetMethod("TryParse",
                    BindingFlags.Public | BindingFlags.Static,
                    Type.DefaultBinder,
                    new[] { typeof(string), typeof(T).MakeByRefType() },
                    null);

                var inputParams = new object[] { param, null };
                methodInfo.Invoke(null, inputParams);

                thing = (T)inputParams[1];

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }

            thing = default(T);
            return false;
        }

        public static string GetActivationCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        }

        public static string GetFileUrl(this string rootUrl, int type, string fileName)
        {   
            return new Uri(new Uri(rootUrl), string.Format("api/Post/GetFileByName?fileName={0}&type={1}", fileName, type)).ToString();
        }

        public static string GetVideoTumbnailUrl(this string rootUrl, string fileName)
        {
            return new Uri(new Uri(rootUrl), string.Format("api/Post/GetFileByName?fileName=Thumb_{0}.png&type={1}", Path.GetFileNameWithoutExtension(fileName), (int)FileType.Video)).ToString();
        }

        public static string UploadBlob(string folder, string fileName, byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData, writable: false))
            {
                return UploadBlob(folder, fileName, stream);
            }

            /*Uncoment this if local storage*/
            /*

            var fileUnicName = Guid.NewGuid();
            var fileExtention = Path.GetExtension(fileName);
            var fileNewName = string.Format("{0}{1}", fileUnicName, fileExtention);
            var rootPath = System.Configuration.ConfigurationManager.AppSettings["RootPathToFiles"];
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var fileSource = Path.Combine(rootPath, fileName);
            try
            {
                File.WriteAllBytes(fileSource, fileData);

                return fileName;
            }
            catch (Exception e)
            {
                return null;
            }*/
        }

        public static string UploadBlob(string folder, string fileName, Stream fileData)
        {
            // The code in this section goes here.MediaServiceRESTAPIEndpoint
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(folder);
                container.CreateIfNotExists();
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                
                blob.UploadFromStream(fileData);
                return fileName;
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
                return null;
            }
        }

        public static string GetAzureFolderByFileType(int fileType)
        {
            switch (fileType)
            {
                case (int)FileType.Profile:
                    return "profile-images";
                case (int)FileType.Image:
                    return "post-attachment-images";
                case (int)FileType.Audio:
                    return "post-attachment-audios";
                case (int)FileType.Video:
                    return "post-attachment-videos";
                case (int)FileType.Thumbnail:
                    return "post-attachment-thumbnails";
            }
            
            return "test-blob-container";
        }

        public static void AddNotification(Guid senderUserId, Guid recieverUserId, Guid sourceId, int type, string body)
        {
            using (MindCornersEntities context = new MindCornersEntities())
            using (NotificationRepository notificationRepository = new NotificationRepository(context, senderUserId, null))
            using (UserProfileRepository userProfileRepository = new UserProfileRepository(context, senderUserId, null))
            {
                //var senderUser = userProfileRepository.GetById(senderUserId);
                
                notificationRepository.Create(new Notification()
                {
                    SenderId = senderUserId,
                    UserId = recieverUserId,
                    SourceId = sourceId,
                    Type = type,
                    Body = body,
                });

                try
                {
                    notificationRepository.SaveChanges();
                }
                catch (Exception e)
                {
                    LogHelper.WriteError(e);
                }
            }
        }
    }
}
