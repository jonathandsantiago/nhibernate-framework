using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

namespace Apresentacao.Models.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString CommonActionButtons<TId>(this IHtmlHelper helper, TId route, string defaultAction = "Detail", string descriptionAction = "Detalhes", bool editable = true, bool removible = true, params Tuple<string, string>[] customActions)
        {
            string actions = string.Empty;

            if (customActions.Length > 0)
            {
                foreach (Tuple<string, string> action in customActions)
                {
                    string description = action.Item1 ?? action.Item2;
                    actions += helper.ActionLink(description, action.Item1, new { id = route }).ToHtmlString();
                }
            }
            string actionEdit = editable ? helper.ActionLink("Editar", "Edit", new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString() : string.Empty;
            string actionDelete = removible ? helper.ActionLink("Deletar", "Delete", new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString() : string.Empty;
            string html = $@"<div class='btn-group'>
                                {helper.ActionLink(descriptionAction, defaultAction, new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString()}
                                {actionEdit}
                                {actionDelete}
                                {actions}
                            </div>";

            return new HtmlString($@"<td class='text-right'>{html}</td>");
        }

        public static HtmlString CommonActionGroup(this IHtmlHelper helper, int route, string defaultAction = "Detail", string descriptionAction = "Detalhes", bool editable = true, bool removible = true, params Tuple<string, string>[] customActions)
        {
            string actions = string.Empty;

            if (editable)
            {
                actions += CreateActionLink(helper, action: "Edit", route: route, description: "Editar");
            }

            if (removible)
            {
                actions += CreateActionLink(helper, action: "Delete", route: route, description: "Remover");
            }

            if (customActions.Length > 0)
            {
                actions += "<li role=\"separator\" class=\"divider\"></li>";

                foreach (Tuple<string, string> action in customActions)
                {
                    actions += CreateActionLink(helper, action.Item1, route, action.Item2);
                }
            }

            string html = $@"<div class='btn-group'>
                                {helper.ActionLink(descriptionAction, defaultAction, new { id = route }, new { @class = "btn btn-default btn-sm" }).ToHtmlString()}
                                <button type='button' class='btn btn-default btn-sm dropdown-toggle' data-toggle='dropdown'>
                                    <span class='caret'></span>
                                </button>
                                    <ul class='dropdown-menu'>{actions}</ul>
                            </div>";

            return new HtmlString(string.Concat("<td> ", html, "</td> "));
        }

        public static HtmlString CommonDisplayFor<TModel, TResult>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression)
        {
            return new HtmlString($@"<td>{helper.DisplayFor(expression).ToHtmlString()}</td>");
        }

        public static HtmlString CommonEditFor<TModel, TResult>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string labelText = null, bool editable = true)
        {
            return new HtmlString(helper.EditFor(expression, labelText, editable));
        }

        #region String

        public static string EditFor<TModel, TResult>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string labelText = null, bool editable = true)
        {
            var readOnly = new { htmlAttributes = new { @class = "form-control", disabled = "disabled", @readonly = "readonly" } };
            IHtmlContent labelContent = string.IsNullOrEmpty(labelText) ? helper.LabelFor(expression) : helper.LabelFor(expression, labelText);

            return $@"<div class='editor-label'>
                            {labelContent.ToHtmlString()}
                        </div>
                        <div class='editor-field'>
                            {helper.EditorFor(expression, editable ? null : readOnly).ToHtmlString()}
                            {helper.ValidationMessageFor(expression).ToHtmlString()}
                        </div>";
        }

        public static string DisplayName<TModelItem, TResult>(this IHtmlHelper<IEnumerable<TModelItem>> helper, Expression<Func<TModelItem, TResult>> expression)
        {
            return $@"<th>{helper.DisplayNameFor(expression)}</th>";
        }

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (string.IsNullOrEmpty(cssClass))
            {
                cssClass = "active";
            }

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }

            if (string.IsNullOrEmpty(action))
            {
                action = currentAction;
            }

            return controller == currentController && action == currentAction ?
                cssClass : string.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static string ToHtmlString(this IHtmlContent content)
        {
            StringWriter writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        private static string CreateActionLink(IHtmlHelper helper, string action, int route, string description = null)
        {
            description = description ?? action;
            return $@"<li>{helper.ActionLink(description, action, new { id = route }).ToHtmlString()}</li>";
        }

        #endregion
    }
}
