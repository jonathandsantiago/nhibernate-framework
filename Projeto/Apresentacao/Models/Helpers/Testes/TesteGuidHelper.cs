using Apresentacao.Models.Helpers;
using Dominio.ViewMode;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ApresentacaoFramework.Models.Helpers.Testes
{
    public static class TesteGuidHelper
    {
        public static HtmlString GetHtmlHeaderModel(this IHtmlHelper<IEnumerable<TesteGuidViewModel>> helper)
        {
            string html = string.Concat(HtmlHelpers.DisplayName(helper, model => model.Id),
                                        HtmlHelpers.DisplayName(helper, model => model.Nome));

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlRowsModel(this IHtmlHelper<IEnumerable<TesteGuidViewModel>> helper, IEnumerable<TesteGuidViewModel> model)
        {
            string html = string.Empty;

            foreach (TesteGuidViewModel item in model.ToList())
            {
                html += string.Concat("<tr> ",
                                        helper.CommonDisplayFor(modelItem => item.Id),
                                        helper.CommonDisplayFor(modelItem => item.Nome),
                                        helper.CommonActionButtons(route: item.Id).ToHtmlString(),
                                    "</tr> ");
            }

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlFildsModel(this IHtmlHelper<TesteGuidViewModel> helper, bool editable = true)
        {
            string html = string.Concat(helper.EditFor(model => model.Nome, "Nome", editable));

            return new HtmlString(html);
        }
    }
}
