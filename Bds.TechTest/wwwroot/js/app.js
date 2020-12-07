$(document).ready(function () {
    // Creates the HTML for a single search result.
    var createResultRowHtml = function (searchResult, index) {
        let html = '<div class="row card card-body searchResultRow">';
        html += '<a href="' + searchResult.uri + '">';
        html += '<h5>' + searchResult.pageTitle + '</h5>';
        html += '<small class="text-muted">' + searchResult.uri + '</small>';
        html += '</a>';
        html += '<p>' + '[DESC PLACEHOLDER]' + '</p>';
        html += '<div class="accordion" id="rawResultsAccordion-' + index.toString() + '">';
        html += '<div class="card">';
        html += '<div class="card-header" id="headingOne">';
        html += '<h2 class="mb-0">';
        html += '<button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#rawResults-' + index.toString() + '" aria-expanded="false" aria-controls="rawResults-' + index.toString() + '">';
        html += 'Raw Search Results...';
        html += '</button>';
        html += '</h2>';
        html += '</div>';
        html += '<div id="rawResults-' + index.toString() + '" class="collapse" aria-labelledby="headingOne" data-parent="#rawResultsAccordion-' + index.toString() + '">';
        html += '<div class="card-body">';
        html += '<div class="container">';
        html += '<div class="row"><div class="col-3">Provider</div><div class="col-7">Title</div><div class="col-2">Rank</div></div>';

        searchResult.rawSearchResults.forEach(function (rawSearchResult, index) {
            html += '<div class="row"><div class="col-3">' + rawSearchResult.providerName + '</div><div class="col-7">' + rawSearchResult.pageTitle + '</div><div class="col-2">' + rawSearchResult.rank.toString() + '</div></div>';
        });

        html += '</div>'; // container
        html += '</div>'; // card-body
        html += '</div>'; // rawResults-X
        html += '</div>'; // card
        html += '</div>'; // accordion
        html += '</div>'; // searchResultRow
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

        // Print the results we found.
        if (searchResults.searchResults) {
            let resultsHtml = "";

            searchResults["searchResults"].forEach(function (value, index) {
                let singleResultHtml = createResultRowHtml(value, index);
                resultsHtml += singleResultHtml;
            });

            $("#searchResultsPlaceholder").append(resultsHtml);
        }        

        // Flip the spinner / results.
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