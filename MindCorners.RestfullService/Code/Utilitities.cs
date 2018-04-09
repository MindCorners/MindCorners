using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models.Enums;

namespace MindCorners.RestfullService.Code
{
    public static class Utilitities
    {
        public static string GetFileUrl(this HttpRequestMessage request, int type, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var rootSiteUrl = request.RequestUri.Scheme + "://" + request.RequestUri.Authority + request.GetRequestContext().VirtualPathRoot.TrimEnd('/') + "/";//request.GetRequestContext().VirtualPathRoot;
                return new Uri(new Uri(rootSiteUrl), string.Format("api/Post/GetFileByName?fileName={0}&type={1}", fileName, type)).ToString();
            }
            
            return type == (int)FileType.Profile ? "profilePlaceholder.png" : "images.png";
        }
    }
}