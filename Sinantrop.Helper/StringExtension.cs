using System.Text.RegularExpressions;

namespace Sinantrop.Helper
{
    public static class StringExtension
    {
        public static string GetUrl(this string text, bool additionalCheck = true)
        {
            string str = string.Empty;
            //string str =  new Regex("<(?<Tag_Name>(a)|img)\\b[^>]*?\\b(?<URL_Type>(?(1)href|src))\\s*=\\s*(?:\"(?<URL>(?:\\\\\"|[^\"])*)\"|'(?<URL>(?:\\\\'|[^'])*)')").Match(bodyXml).Value.Replace("<a href=\"", "").Replace("\"", "");


            Regex urlRx = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

            str = urlRx.Match(text).Value;


            return str;
        }

    }
}
