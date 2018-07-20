function createCommentContainerHtml(commentModel) {
    var commentContainerHtml =
        `<div class='comment-container' style='display: none'>
            <div class='comment-info-container'>
                <input type='hidden' value=${commentModel.commentId} class='comment-id' />
                <input type='hidden' value=${commentModel.createdDate} class='comment-created-date' />
            </div>
            <div class='master-comment-container'>
                <div class='comment-user-container'>
                    <strong class='comment-user'>${commentModel.username}</strong>
                    <button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>
                    <button class='add-child-comment-btn'><span class='glyphicon glyphicon-edit'></span></button>
                </div>
                <div class='comment-body'>${commentModel.content}</div>
            </div>
            <form class='comment-form child-comment-form' style='display: none'>
                <textarea class='comment-input' placeholder='Comment'></textarea>
                <button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>
            </form>
            <div class='child-comment-list-container'></div>
        </div>`;

    return commentContainerHtml;
}

function createChildCommentContainerHtml(commentModel) {
    var childCommentContainerHtml =
        `<div class='child-comment-container' style='display: none'>
            <div class='comment-user-section'><strong class='comment-user'>${commentModel.username}</strong></div>
            <div class='comment-body'>${commentModel.content}</div>
        </div>`;

    return childCommentContainerHtml;
}