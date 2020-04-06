using CommonMark;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Util
{
    public static class MarkdownParser
    {

        public static HtmlString Parse(string markdown)
        {
            if (!String.IsNullOrEmpty(markdown))
            {
                return new HtmlString(CommonMarkConverter.Convert(markdown));
            }

            return null;
        }

    }
}
