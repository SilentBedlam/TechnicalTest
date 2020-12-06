namespace Bds.TechTest.Lib.Results
{
    /// <summary>
    /// Contract for a class which represents a ranked search result.
    /// </summary>
    interface IRankedSearchResult : ISimpleSearchResult
    {
        /// <summary>
        /// The rank assigned to the search result by the original search engine.
        /// </summary>
        int Rank { get; }
    }
}
