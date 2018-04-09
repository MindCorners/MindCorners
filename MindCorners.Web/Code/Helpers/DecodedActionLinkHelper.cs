using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MindCorners.Web.Code.Helpers
{
    public static class DecodedActionLinkHelper
    {
        #region Decoded ActionLink
        public static MvcHtmlString DecodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, Dictionary<string, object> htmlAttributes)
        {
            var tempValues = new RouteValueDictionary();
            for (var i = 0; i < routeValues.Count; i++)
            {
                var routeValue = routeValues.ElementAt(i);
                var temp = Guid.NewGuid().ToString();
                tempValues.Add(routeValue.Key, temp);
            }

            var lnk = htmlHelper.ActionLink(linkText, actionName, controllerName, tempValues, htmlAttributes).ToString();

            foreach (var tempValue in tempValues)
            {
                lnk = lnk.Replace(tempValue.Value.ToString(), routeValues[tempValue.Key].ToString());
            }

            return MvcHtmlString.Create(lnk);
        }

        public static MvcHtmlString DecodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, Dictionary<string, object> htmlAttributes)
        {
            var tempValues = new RouteValueDictionary();
            for (var i = 0; i < routeValues.Count; i++)
            {
                var routeValue = routeValues.ElementAt(i);
                var temp = Guid.NewGuid().ToString();
                tempValues.Add(routeValue.Key, temp);
            }

            var lnk = htmlHelper.ActionLink(linkText, actionName, tempValues, htmlAttributes).ToString();

            foreach (var tempValue in tempValues)
            {
                lnk = lnk.Replace(tempValue.Value.ToString(), routeValues[tempValue.Key].ToString());
            }

            return MvcHtmlString.Create(lnk);
        }

        public static MvcHtmlString DecodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return DecodedActionLink(htmlHelper, linkText, actionName, null, new RouteValueDictionary(routeValues), null);
        }
        #endregion

        #region Decoded Image ActionLink
        public static MvcHtmlString DecodedImageActionLink(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues,
            string imageUrl, string alternativeText, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var routeVals = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var tempValues = new RouteValueDictionary();
            for (var i = 0; i < routeVals.Count; i++)
            {
                var routeValue = routeVals.ElementAt(i);
                var temp = Guid.NewGuid().ToString();
                tempValues.Add(routeValue.Key, temp);
            }

            var imgLnk = htmlHelper.ImageActionLink(actionName, controllerName, tempValues, imageUrl, alternativeText, linkHtmlAttributes, imageHtmlAttributes);

            foreach (var tempValue in tempValues)
            {
                imgLnk = imgLnk.Replace(tempValue.Value.ToString(), routeVals[tempValue.Key].ToString());
            }

            return MvcHtmlString.Create(imgLnk);
        }
        #endregion
    }
}