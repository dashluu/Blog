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

function createPostCardHtml(postCardModel) {
    var postCardHtml =
        `<div class='post-card container col-xs-12 col-sm-12 col-md-12 col-lg-6'>
                <div class='row'>
                    <div class='col-xs-12 col-sm-3 col-md-3 col-lg-4 post-col-1'>
                        <img class='post-img' src=${postCardModel.thumbnailImageSrc} />
                    </div>
                    <div class='col-xs-12 col-sm-9 col-md-9 col-lg-8 post-col-2'>
                        <div class='post-title'>${postCardModel.title}</div>
                        <div class='post-date'>On <strong>${postCardModel.createdDate}</strong></div>
                        <div class='post-date'>Updated ${postCardModel.updatedDate}</div>
                        <div class='post-summary'>${postCardModel.shortDescription}</div>
                        <form action='/Home/ViewPost' method='post'>
                            <input type='hidden' value=${postCardModel.postId} name='postId' />
                        </form>
                    </div>
                </div>
            </div>`;

    return postCardHtml;
}

$(document).on("click", ".post-img", function () {
    $(this).parent().siblings().first().children("form").first().submit();
});

$(document).on("click", ".next-page-btn", function () {
    var targetRow = $(this).parent().parent();
    navigatePage(targetRow, true);
});

$(document).on("click", ".previous-page-btn", function () {
    var targetRow = $(this).parent().parent();
    navigatePage(targetRow, false);
});

var searchQuery = null;

function navigatePage(targetRow, nextPage) {
    var postNavigationElement = targetRow
        .children(".post-navigation-controls")
        .first();

    var categoryElement = targetRow
        .children(".post-category")
        .first();

    var pageIndicatorElement = postNavigationElement
        .children(".page-indicator")
        .first();

    var pageNumberElement = pageIndicatorElement
        .children(".page-number")
        .first();

    var pageCountElement = pageIndicatorElement
        .children(".page-count")
        .first();

    var pageSizeElement = postNavigationElement
        .children(".post-page-size")
        .first();

    var nextBtnElement = postNavigationElement
        .children(".next-page-btn")
        .first();

    var previousBtnElement = postNavigationElement
        .children(".previous-page-btn")
        .first();

    var values = {
        pageNumber: parseInt(pageNumberElement.html()) + (nextPage ? 1 : -1),
        category: categoryElement.html().toLowerCase(),
        pageSize: pageSizeElement.val(),
        searchQuery: searchQuery
    };

    $.post("/Home/CategoryPartial", values, function (result) {
        if (result.status === 200) {
            var postCardPaginationObject = result.data;

            var postCardPaginationModel = mapObjectToPostCardPaginationModel(postCardPaginationObject);
            targetRow.children(".post-card").remove();

            if (postCardPaginationModel.pages == 0) {
                targetRow.parent().hide();
                return;
            }

            pageNumberElement.html(postCardPaginationModel.pageNumber);
            pageCountElement.html(postCardPaginationModel.pages);
            var postCardModels = postCardPaginationModel.postCardModels;
            var postCardModelCount = postCardModels.length;

            if (!postCardPaginationModel.hasNext) {
                nextBtnElement.hide();
            }
            else {
                nextBtnElement.show();
            }

            if (!postCardPaginationModel.hasPrevious) {
                previousBtnElement.hide();
            }
            else {
                previousBtnElement.show();
            }

            for (var i = 0; i < postCardModelCount; i++) {
                var postCardModel = mapObjectToPostCardModel(postCardModels[i]);
                var postCardHtml = createPostCardHtml(postCardModel);
                targetRow.append(postCardHtml);
            }
        }
    });
}

