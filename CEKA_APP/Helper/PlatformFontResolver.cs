using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Helper
{
    public class PlatformFontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string name = familyName;
            if (isBold && isItalic)
                name = $"{familyName}-BoldItalic";
            else if (isBold)
                name = $"{familyName}-Bold";
            else if (isItalic)
                name = $"{familyName}-Italic";

            return new FontResolverInfo(name);
        }

        public byte[] GetFont(string faceName)
        {
            using (var fontCollection = new System.Drawing.Text.PrivateFontCollection())
            {
                try
                {
                    string fontPath = GetFontFile(faceName);
                    if (!string.IsNullOrEmpty(fontPath) && File.Exists(fontPath))
                    {
                        fontCollection.AddFontFile(fontPath);
                        return File.ReadAllBytes(fontPath);
                    }
                }
                catch
                {
                    // Varsayılan font olarak Arial döner
                    string arialPath = GetFontFile("Arial");
                    if (!string.IsNullOrEmpty(arialPath) && File.Exists(arialPath))
                    {
                        fontCollection.AddFontFile(arialPath);
                        return File.ReadAllBytes(arialPath);
                    }
                }
            }
            return null;
        }

        private string GetFontFile(string faceName)
        {
            string windowsFonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string[] fontFiles = Directory.GetFiles(windowsFonts, "*.ttf");

            foreach (var file in fontFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Replace("-", "").Equals(faceName.Replace("-", ""), StringComparison.OrdinalIgnoreCase))
                    return file;
            }
            return null;
        }
    }
}
