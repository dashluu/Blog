function createPostCardHtml(postCardModel) {
    var postCardHtml =
        `<div class='post-card container col-xs-12 col-sm-12 col-md-12 col-lg-6'>
                <div class='row'>
                    <div class='col-xs-12 col-sm-3 col-md-3 col-lg-4 post-col-1'>
                        <img class='post-img' src=${postCardModel.thumbnailImageSrc} />
                        <input type="hidden" class="post-id" value=${postCardModel.postId} />
                    </div>
                    <div class='col-xs-12 col-sm-9 col-md-9 col-lg-8 post-col-2'>
                        <div class='post-title'>${postCardModel.title}</div>
                        <div class='post-date'>On <strong>${postCardModel.createdDate}</strong></div>
                        <div class='post-date'>Updated ${postCardModel.updatedDate}</div>
                        <div class='post-summary'>${postCardModel.shortDescription}</div>
                    </div>
                </div>
            </div>`;

    return postCardHtml;
}