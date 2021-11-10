using System;
using System.IO;
using System.Linq;

namespace Framework.Helper.Helpers
{
    public static class FileDialogHelper
    {
        private static readonly string imageExtesions = "Imagens (*.bmp; *.jpg; *.jpeg; *.jpe; *.gif; *.png) | *.bmp; *.jpg; *.jpeg; *.jpe; *.gif; *.png";

        private static FileInfo GetFileInfo(long? limitSizeMb, string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (limitSizeMb != null && (limitSizeMb.Value / 1024m) < ((fileInfo.Length / 1024m) / 1024m))
            {
                throw new Exception(string.Format("O tamanho do arquivo não pode ser superior a {0} MB", (limitSizeMb.Value / 1024)));
            }

            return fileInfo;
        }

        private static string GetExtensions(string name, string[] extensions)
        {
            if (extensions == null || !extensions.Any())
            {
                return imageExtesions;
            }
            else
            {
                string retorno = string.Join("; ", extensions.Select(c => "*." + c));
                return string.Format("{0}({1}) | {1}", !string.IsNullOrEmpty(name) ? name + " " : null, retorno);
            }
        }
    }
}