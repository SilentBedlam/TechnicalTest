using HtmlAgilityPack;
using System.Collections.Generic;

namespace Bds.TechTest.Lib
{
    /// <summary>
    /// Contract for a class which can scrape search results from an HTML document.
    /// </summary>
    /// <typeparam name="T">The Type of the object representing a single search result.</typeparam>
    public interface ISearchResultScraper<T>
    {
        /// <summary>
        /// Scrapes search results from the specified HTML page.
        /// </summary>
        /// <param name="argumentTBD">TBD.</param>
        /// <returns>A collection of <see cref="T"/> representing the search results.</returns>
        IEnumerable<T> ScrapeResults(HtmlDocument htmlDocument);
    }
}
