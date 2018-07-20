$(document).on("click", ".post-img", function () {
    var postId = $(this).siblings(".post-id").first().val();
    window.location.href = `/Posts/${postId}`;
});

$(document).on("click", ".next-page-btn", function () {
    var targetRow = $(this).parent().parent();
    navigatePage(targetRow, true);
});

$(document).on("click", ".previous-page-btn", function () {
    var targetRow = $(this).parent().parent();
    navigatePage(targetRow, false);
});

var searchQuery = "";

function navigatePage(targetRow, nextPage) {
    var postNavigationInputElement = targetRow
        .children(".post-navigation-controls")
        .first();

    var categoryInputElement = targetRow
        .children(".post-category")
        .first();

    var pageIndicatorInputElement = postNavigationInputElement
        .children(".page-indicator")
        .first();

    var pageNumberInputElement = pageIndicatorInputElement
        .children(".page-number")
        .first();

    var pageCountInputElement = pageIndicatorInputElement
        .children(".page-count")
        .first();

    var nextBtnInputElement = postNavigationInputElement
        .children(".next-page-btn")
        .first();

    var previousBtnInputElement = postNavigationInputElement
        .children(".previous-page-btn")
        .first();

    var category = categoryInputElement.html().toLowerCase();

    var values = {
        pageNumber: parseInt(pageNumberInputElement.html()) + (nextPage ? 1 : -1),
        searchQuery: searchQuery
    };

    var url = `/Categories/${category}`;

    $.post(url, values, function (result) {
        if (result.status === 200) {
            var postCardPaginationObject = result.data;

            var postCardPaginationModel = mapObjectToPostCardPaginationModel(postCardPaginationObject);
            targetRow.children(".post-card").remove();

            if (postCardPaginationModel.pages == 0) {
                targetRow.parent().hide();
                return;
            }

            pageNumberInputElement.html(postCardPaginationModel.pageNumber);
            pageCountInputElement.html(postCardPaginationModel.pages);
            var postCardModels = postCardPaginationModel.postCardModels;
            var postCardModelCount = postCardModels.length;

            if (!postCardPaginationModel.hasNext) {
                nextBtnInputElement.hide();
            }
            else {
                nextBtnInputElement.show();
            }

            if (!postCardPaginationModel.hasPrevious) {
                previousBtnInputElement.hide();
            }
            else {
                previousBtnInputElement.show();
            }

            for (var i = 0; i < postCardModelCount; i++) {
                var postCardModel = mapObjectToPostCardModel(postCardModels[i]);
                var postCardHtml = createPostCardHtml(postCardModel);
                targetRow.append(postCardHtml);
            }
        }
    });
}

