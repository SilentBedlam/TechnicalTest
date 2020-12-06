$(document).ready(function () {

    // Handle the search button's click event: submit a request to the server and retrieve the data from multiple search engines.
    $('#search').on("click", function () {
        // TBC: Add validation of the input.
        let searchTerm = $('#searchTerm').val();

        $.ajax({
            url: "./api/CombinedSearch",
            contentType: "application/json",
            method: "POST",            
            data: JSON.stringify({ "SearchTerm": searchTerm }),
            error: function () {
                alert("Failed");
            },
            success: function (result) {
                alert(JSON.stringify(result));
            }
        })
    })
});