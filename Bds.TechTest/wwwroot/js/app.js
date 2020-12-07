$(document).ready(function () {
    // Creates the HTML for a single search result.
    var createResultRowHtml = function (searchResult) {
        let html = '<div class="row">';
        html += '<a href="' + searchResult.uri + '">';
        html += '<h4>' + searchResult.pageTitle + '<small class="text-muted">' + searchResult.uri + '</small></h4>';
        html += '</a></div>';
        return html;
        //// "searchResults":[{"pageTitle":"Vitamin D - Health Professional Fact Sheet","uri":"https://ods.od.nih.gov/factsheets/VitaminD-HealthProfessional/","averageRank":1,"rawSearchResults":[{"providerName":"Google","rank":1,"pageTitle":"Vitamin D - Health Professional Fact Sheet","uri":"https://ods.od.nih.gov/factsheets/VitaminD-HealthProfessional/"}]}
    };

    // Resets the form to it's original appearance.
    var resetForm = function () {
        $("#searchTerm").val("");
        $("#searchResults, #searchError, #spinner").hide();
        $("#splash").show();
    };

    // Handles the results of a successful search.
    var handleSearchSuccess = function (searchResults) {
        // Hide the spash screen but keep the spinner until we've finished processing.
        $("#splash").hide();

        if (searchResults.searchResults) {
            let resultsHtml = "";
            searchResults["searchResults"].forEach(function (value) {
                let singleResultHtml = createResultRowHtml(value);
                resultsHtml += singleResultHtml;
            });
            $("#searchResultsPlaceholder").append(resultsHtml);
        }        

        $("#spinner").hide();
        $("#searchResults").show();
    };

    // Handles the results of a failed search.
    var handleSearchError = function () {
        $("#splash, #spinner").hide();
        $("#searchError").show();
    };

    // Handle the search button"s click event: submit a request to the server and retrieve the data from multiple search engines.
    $("#search").on("click", function () {
        $("#spinner").show();

        let searchTerm = $("#searchTerm").val().trim();

        if (searchTerm) {
            $.ajax({
                url: "./api/CombinedSearch",
                contentType: "application/json",
                method: "POST",
                data: JSON.stringify({ "SearchTerm": searchTerm }),
                error: handleSearchError,
                success: handleSearchSuccess
            });
        } else {
            alert("A search term must be provided and may not be composed entirely of white space.");
        }
    });

    // Handle the reset link's click event.
    // Reset the form by clearing the search text, and showing the page as it originally appeared.
    $("#resetFormLink1, #resetFormLink2").on("click", resetForm);
});