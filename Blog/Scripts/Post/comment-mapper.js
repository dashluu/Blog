function mapObjectToCommentModel(object) {
    var commentModel = {
        commentId: object["CommentId"],
        username: object["Username"],
        createdDate: object["CreatedDate"],
        content: object["Content"]
    };

    return commentModel;
}

function mapObjectToCommentPaginationModel(object) {
    var commentPaginationModel = {
        commentModels: object["Models"],
        hasNext: object["HasNext"]
    }

    return commentPaginationModel;
}