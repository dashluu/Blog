$(document).on("click", ".post-img", function () {
    $(this).parent().siblings().eq(0).children("form").eq(0).submit();
});

$(document).on("click", ".next-page-btn", function () {
    navigatePage($(this), true);
});

$(document).on("click", ".previous-page-btn", function () {
    navigatePage($(this), false);
});

function navigatePage(button, nextPage) {
    var pageIndicator = button.siblings(".page-indicator").eq(0);
    var pageNumberInput = pageIndicator.children(".page-number").eq(0);
    var pageNumber = pageNumberInput.html();
    var pageCountInput = pageIndicator.children(".page-count").eq(0);
    var categoryInput = button.siblings(".post-category");
    var category = categoryInput.val();

    var row = button.parent().parent();

    $.post("/Home/CategoryPartial", { pageNumber: pageNumber, nextPage: nextPage, category: category }, function (result) {    
        if (result.status === 200) {
            if (nextPage) {
                pageNumber++;
            }
            else {
                pageNumber--;
            }

            pageNumberInput.html(pageNumber);
            pageCountInput.html(result.pages);
            row.children(".post-card").remove();
            var postCards = result.data;
            var postCardsCount = postCards.length;

            if (nextPage) {
                if (!result.hasNext) {
                    button.hide();
                }
                else {
                    button.show();
                }

                var previousButton = button.siblings(".previous-page-btn");

                if (!result.hasPrevious) {
                    previousButton.hide();
                }
                else {
                    previousButton.show();
                }
            }
            else {
                var nextButton = button.siblings(".next-page-btn");

                if (!result.hasNext) {
                    nextButton.hide();
                }
                else {
                    nextButton.show();
                }

                if (!result.hasPrevious) {
                    button.hide();
                }
                else {
                    button.show();
                }
            }

            for (var i = 0; i < postCardsCount; i++) {
                var postCard = postCards[i];
                var postCardSelector = createPostCardSelector(postCard);
                $(postCardSelector).appendTo(row);
            }
        }
    });
}

function createPostCardSelector(postCard) {
    return "<div class='post-card container col-xs-12 col-sm-12 col-md-12 col-lg-6'>" +
        "<div class='row'>" +
        "<div class='col-xs-12 col-sm-3 col-md-3 col-lg-4 post-col-1'>" +
        "<img class='post-img' src='" + postCard.ThumbnailImageSrc + "'/>" +
        "</div>" +
        "<div class='col-xs-12 col-sm-9 col-md-9 col-lg-8 post-col-2'>" +
        "<p class='post-title'>" + postCard.Title + "</p>" +
        "<p class='post-date'>On <strong>" + postCard.CreatedDate + "</strong></p>" +
        "<p class='post-date'>Updated " + postCard.UpdatedDate + "</p>" +
        "<p class='post-summary'>" + postCard.ShortDescription + "</p>" +
        "<form action='/Home/CategoryPost' method='post'>" +
        "<input type='hidden' value='" + postCard.PostId + "' name='postId' />" +
        "</form>" +
        "</div>" +
        "</div>" +
        "</div>";
}