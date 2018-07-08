function mapObjectToPostCardModel(object) {
    var postCardModel = {
        postId: object["PostId"],
        title: object["Title"],
        createdDate: object["CreatedDate"],
        updatedDate: object["UpdatedDate"],
        shortDescription: object["ShortDescription"],
        thumbnailImageSrc: object["ThumbnailImageSrc"]
    };

    return postCardModel;
}

function mapObjectToPostCardPaginationModel(object) {
    var postCardPaginationModel = {
        postCardModels: object["Models"],
        hasNext: object["HasNext"],
        hasPrevious: object["HasPrevious"],
        pageNumber: object["PageNumber"],
        pages: object["Pages"]
    }

    return postCardPaginationModel;
}

$(document).on("click", ".post-img", function () {
    $(this).parent().siblings().first().children("form").first().submit();
});

$(document).on("click", ".next-page-btn", function () {
    navigatePage($(this), true);
});

$(document).on("click", ".previous-page-btn", function () {
    navigatePage($(this), false);
});

function navigatePage(button, nextPage) {
    var pageIndicator = button.siblings(".page-indicator").first();
    var pageCountInput = pageIndicator.children(".page-count").first();

    var pageNumberInput = pageIndicator.children(".page-number").first();
    var pageNumber = parseInt(pageNumberInput.html());

    var categoryInput = button.parent().siblings(".post-category").first();
    var category = categoryInput.html().toLowerCase();

    var pageSize = button.siblings(".post-page-size").first().val();

    var row = button.parent().parent();

    if (nextPage) {
        pageNumber++;
    }
    else {
        pageNumber--;
    }

    $.post("/Home/CategoryPartial", { pageNumber: pageNumber, pageSize: pageSize, category: category }, function (result) {    
        if (result.status === 200) {
            var postCardPaginationObject = result.data;
            var postCardPaginationModel = mapObjectToPostCardPaginationModel(postCardPaginationObject);
            row.children(".post-card").remove();

            if (postCardPaginationModel.pages == 0) {
                row.parent().hide();
                return;
            }

            pageNumberInput.html(postCardPaginationModel.pageNumber);
            pageCountInput.html(postCardPaginationModel.pages);
            var postCardModels = postCardPaginationModel.postCardModels;
            var postCardModelCount = postCardModels.length;

            if (nextPage) {
                if (!postCardPaginationModel.hasNext) {
                    button.hide();
                }
                else {
                    button.show();
                }

                var previousButton = button.siblings(".previous-page-btn").first();

                if (!postCardPaginationModel.hasPrevious) {
                    previousButton.hide();
                }
                else {
                    previousButton.show();
                }
            }
            else {
                var nextButton = button.siblings(".next-page-btn").first();

                if (!postCardPaginationModel.hasNext) {
                    nextButton.hide();
                }
                else {
                    nextButton.show();
                }

                if (!postCardPaginationModel.hasPrevious) {
                    button.hide();
                }
                else {
                    button.show();
                }
            }

            for (var i = 0; i < postCardModelCount; i++) {
                var postCardModel = mapObjectToPostCardModel(postCardModels[i]);
                var postCardSelector = createPostCardSelector(postCardModel);
                row.append(postCardSelector);
            }
        }
    });
}

function createPostCardSelector(postCardModel) {
    var postCardSelector = "<div class='post-card container col-xs-12 col-sm-12 col-md-12 col-lg-6'>" +
        "<div class='row'>" +
        "<div class='col-xs-12 col-sm-3 col-md-3 col-lg-4 post-col-1'>" +
        "<img class='post-img' src='" + postCardModel.thumbnailImageSrc + "'/>" +
        "</div>" +
        "<div class='col-xs-12 col-sm-9 col-md-9 col-lg-8 post-col-2'>" +
        "<div class='post-title'>" + postCardModel.title + "</div>" +
        "<div class='post-date'>On <strong>" + postCardModel.createdDate + "</strong></div>" +
        "<div class='post-date'>Updated " + postCardModel.updatedDate + "</div>" +
        "<div class='post-summary'>" + postCardModel.shortDescription + "</div>" +
        "<form action='/Home/CategoryPost' method='post'>" +
        "<input type='hidden' value='" + postCardModel.postId + "' name='postId' />" +
        "</form>" +
        "</div>" +
        "</div>" +
        "</div>";

    return postCardSelector;
}