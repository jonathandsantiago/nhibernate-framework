using Apresentacao.Models.Helpers;
using Dominio.Models;
using Dominio.ViewMode;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ApresentacaoFramework.Models.Helpers.Testes
{
    public static class TesteHelper
    {
        public static HtmlString GetHtmlHeaderModel(this IHtmlHelper<IEnumerable<TesteViewModel>> helper)
        {
            string html = string.Concat(HtmlHelpers.DisplayName(helper, model => model.Id),
                                        HtmlHelpers.DisplayName(helper, model => model.Nome));

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlRowsModel(this IHtmlHelper<IEnumerable<Teste>> helper, IEnumerable<TesteViewModel> model)
        {
            string html = string.Empty;

            foreach (TesteViewModel item in model.ToList())
            {
                html += string.Concat("<tr> ",
                                        helper.CommonDisplayFor(modelItem => item.Id),
                                        helper.CommonDisplayFor(modelItem => item.Nome),
                                        helper.CommonActionButtons(route: item.Id).ToHtmlString(),
                                    "</tr> ");
            }

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlFildsModel(this IHtmlHelper<TesteViewModel> helper, bool editable = true)
        {
            string html = string.Concat(helper.EditFor(model => model.Nome, "Nome", editable));

            return new HtmlString(html);
        }
    }
}
