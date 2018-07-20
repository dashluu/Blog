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