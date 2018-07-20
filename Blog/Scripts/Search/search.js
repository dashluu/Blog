$("#search-box").keyup(function (event) {
    searchQuery = $(this).val();

    if (event.which !== 13 || !searchQuery) {
        return;
    }

    var searchResultContainer = $(".search-result-container").first();

    var values = {
        searchQuery: searchQuery
    };

    var url = "/Search";

    $.post(url, values, function (result) {
        searchResultContainer.children().remove();
        searchResultContainer.append(result);
    });
})