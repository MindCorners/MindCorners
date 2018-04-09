using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Xml.Linq;

namespace MindCorners.Web.Code.Helpers
{
    public static class HtmlHelpers
    {
        private static TagBuilder BuildImage(string src, string alt, object htmlAttributes)
        {
            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("src", src);
            imageBuilder.MergeAttribute("alt", alt);
            imageBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return imageBuilder;
        }

        public static string ImageActionLink(this HtmlHelper html, string action, string controller, object routeValues,
            string imageUrl, string alternativeText, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", url.Action(action, controller, new RouteValueDictionary(routeValues)));
            linkBuilder.InnerHtml = BuildImage(imageUrl, alternativeText, imageHtmlAttributes).ToString(TagRenderMode.SelfClosing);
            linkBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            return linkBuilder.ToString(TagRenderMode.Normal);
        }

        public static string ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, object imageHtmlAttributes, string actionName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = BuildImage(imageUrl, altText, imageHtmlAttributes);

            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions);
            return link.ToString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing));
        }

        public static string ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, object imageHtmlAttributes, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = BuildImage(imageUrl, altText, imageHtmlAttributes);

            var link = helper.ActionLink("[replaceme]", actionName, controllerName, routeValues, ajaxOptions);      // Resharper override
            return link.ToString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing));
        }


        public static string ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, object linkHtmlAttributes, object imageHtmlAttributes, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            //var url = new UrlHelper(helper.ViewContext.RequestContext);
            var builder = BuildImage(imageUrl, altText, imageHtmlAttributes);

            ////var linkBuilder = new TagBuilder("a");
            ////// add attributes
            ////linkBuilder.MergeAttribute("href", url.Action(actionName, controllerName, new RouteValueDictionary(routeValues)));
            ////linkBuilder.InnerHtml = builder.ToString(TagRenderMode.SelfClosing);
            ////linkBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            var link = helper.ActionLink("[replaceme]", actionName, controllerName, routeValues, ajaxOptions, linkHtmlAttributes);      // Resharper override
            return link.ToString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing));
        }

        #region Custom DropDownListFor
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return CustomDropDownListFor(htmlHelper, expression, selectList, null /* optionLabel */, null /* htmlAttributes */);
        }


        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return CustomDropDownListFor(htmlHelper, expression, selectList, null /* optionLabel */, new RouteValueDictionary(htmlAttributes));
        }


        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return CustomDropDownListFor(htmlHelper, expression, selectList, null /* optionLabel */, htmlAttributes);
        }


        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return CustomDropDownListFor(htmlHelper, expression, selectList, optionLabel, null /* htmlAttributes */);
        }


        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return CustomDropDownListFor(htmlHelper, expression, selectList, optionLabel, new RouteValueDictionary(htmlAttributes));
        }


        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Users cannot use anonymous methods with the LambdaExpression type")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }


            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);


            IDictionary<string, object> validationAttributes = htmlHelper
                .GetUnobtrusiveValidationAttributes(ExpressionHelper.GetExpressionText(expression), metadata);


            htmlAttributes = htmlAttributes == null ? validationAttributes : htmlAttributes.Concat(validationAttributes).ToDictionary(k => k.Key, v => v.Value);


            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel, htmlAttributes); // Resharper override
        }


        public static MvcHtmlString MyDropDownList(this HtmlHelper html, IEnumerable<SelectListItem> selectList, Dictionary<string, KeyValuePair<string, string>> attributeList)
        {
            var selectDoc = XDocument.Parse(html.DropDownList("", (IEnumerable<SelectListItem>)selectList).ToString());

            var options = from XElement el in selectDoc.Element("select").Descendants()
                          select el;

            foreach (var item in options)
            {
                var itemValue = item.Attribute("value");
                if (attributeList.Keys.Contains(itemValue.Value))
                {
                    var attribute = attributeList.FirstOrDefault(p => p.Key == itemValue.Value).Value;
                    item.SetAttributeValue(attribute.Key, attribute.Value);
                }
            }

            // rebuild the control, resetting the options with the ones you modified
            selectDoc.Root.ReplaceNodes(options.ToArray());
            return MvcHtmlString.Create(selectDoc.ToString());
        }

        //This overload is extension method accepts name, list and htmlAttributes as parameters.
        public static MvcHtmlString Custom_DropdownList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, object htmlAttributes)
        {
            //Creating a select element using TagBuilder class which will create a dropdown.
            TagBuilder dropdown = new TagBuilder("select");
            //Setting the name and id attribute with name parameter passed to this method.
            dropdown.Attributes.Add("name", name);
            dropdown.Attributes.Add("id", name);

            //Created StringBuilder object to store option data fetched oen by one from list.
            StringBuilder options = new StringBuilder();
            //Iterated over the IEnumerable list.
            foreach (var item in list)
            {
                //Each option represents a value in dropdown. For each element in the list, option element is created and appended to the stringBuilder object.
                options = options.Append("<option value='" + item.Value + "'>" + item.Text + "</option>");
            }
            //assigned all the options to the dropdown using innerHTML property.
            dropdown.InnerHtml = options.ToString();
            //Assigning the attributes passed as a htmlAttributes object.
            dropdown.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            //Returning the entire select or dropdown control in HTMLString format.
            return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
        }

        #endregion

        /// <summary>
        /// This gives correct names to html elements in partial view
        /// </summary>
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string partialViewName)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = name
                }
            };
            return helper.Partial(partialViewName, model, viewData);
        }

        public static string IdPrefixFrom<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                  Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
        }

        public static string NamePrefixFrom<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                  Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString IndexHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object value)
        {
            return helper.Hidden(string.Format("{0}.Index", helper.NamePrefixFrom(expression)), value);
        }
    }

    public class CustomSelectItem : SelectListItem
    {
        public bool Enabled { get; set; }
    }
}