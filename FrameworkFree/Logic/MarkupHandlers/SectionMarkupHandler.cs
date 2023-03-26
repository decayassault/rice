using Data;
namespace MarkupHandlers
{
    internal sealed class SectionMarkupHandler
    {
        internal string GetArrows
            (int pageNumber, int pagesCount, int number)
        {
            string result = Constants.SE;
            string section = (number + Constants.One).ToString();
            const string a = "<span id='w'><a onClick='n(&quot;/s/";
            const string b = "?p=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/";

            if ((pageNumber >= 2)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat(a,
                            section,
                            b,
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber >= 2)
                    && (pageNumber + 2 == pagesCount))
            {
                result = string.Concat(a,
                            section,
                            b,
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber >= 2)
                    && (pageNumber + Constants.One == pagesCount))
            {
                result = string.Concat(a,
                            section,
                            b,
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == Constants.Zero)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == Constants.One) && (pagesCount == 3))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.One) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.Zero) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/s/",
                            section,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }

            return result;
        }
        internal string GetThreadLinkText(int id_, string name)
        {
            return string.Concat("<p onClick='n(&quot;/f/",
                              id_,
                              "?p=1&quot;);'>",
                              name,
                              "</p><br /><br />");
        }
        internal string GetEndpointHidden(int sectionNum)
        {
            return string.Concat(Constants.indic,
                                  sectionNum,
                                  Constants.indicEnd);
        }
        internal string GetNav()
        {
            return string.Concat(Constants.navMarker,
                                  Constants.brMarker);
        }
        internal string SetNavigation
            (int pageNumber, int pagesCount, int number)
        {
            return string.Concat(GetArrows(pageNumber, pagesCount, number),
                        Constants.brMarker);
        }
        internal string SetNavigationWithEndpoint
            (int pageNumber, int pagesCount, int number, string endpointHidden)
        {
            return string.Concat(SetNavigation
                                  (pageNumber, pagesCount, number),
                                  Constants.endNavMarker,
                                  endpointHidden);
        }
        internal string GetNavWithEndpoint(string endpointHidden)
        {
            return string.Concat(Constants.endNavMarker,
                                                  endpointHidden);
        }
    }
}