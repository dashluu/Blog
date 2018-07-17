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

$(document).on("submit", ".master-comment-form", function (event) {
    event.preventDefault();

    var form = $(this);
    form.validate();

    jQuery.validator.addMethod("commentRequired",
        jQuery.validator.methods.required,
        "Comment field is empty.");

    jQuery.validator.addClassRules("comment-input", {
        commentRequired: true,
        normalizer: function (value) {
            return $.trim(value);
        }
    });

    if (!form.valid()) {
        return;
    }

    var commentInputElement = form
        .children(".comment-input")
        .first();

    var postIdInputElement = form
        .children(".post-id")
        .first();

    var commentCountInputElement = $(".comment-count").first();

    var masterCommentListContainer = form
        .siblings(".master-comment-list-container")
        .first();

    var values = {
        comment: commentInputElement.val(),
        postId: postIdInputElement.val()
    };

    $.post("/Post/AddMasterComment", values, function (result) {
        if (result.status === 200) {
            commentInputElement.val("");

            var commentCount = commentCountInputElement.html();
            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInputElement.html(updatedCommentCount.toString());

            var object = result.data;
            var commentModel = mapObjectToCommentModel(object);

            var newCommentContainerHtml = createCommentContainerHtml(commentModel, values.postId);
            masterCommentListContainer.prepend(newCommentContainerHtml);
            var newCommentContainer = masterCommentListContainer.children().first();
            newCommentContainer.show("fast");
        }
    });
});

$(document).on("submit", ".child-comment-form", function (event) {
    event.preventDefault();

    var form = $(this);
    form.validate();

    jQuery.validator.addMethod("commentRequired",
        jQuery.validator.methods.required,
        "Comment field is empty.");

    jQuery.validator.addClassRules("comment-input", {
        commentRequired: true,
        normalizer: function (value) {
            return $.trim(value);
        }
    });

    if (!form.valid()) {
        return;
    }

    var commentInputElement = form
        .children(".comment-input")
        .first();

    var commentIdInputElement = form
        .children(".comment-id")
        .first();

    var postIdInputElement = form
        .children(".post-id")
        .first();

    var commentCountInputElement = $(".comment-count").first();

    var childCommentListContainer = form
        .siblings(".child-comment-list-container")
        .first();

    var values = {
        comment: commentInputElement.val(),
        commentId: commentIdInputElement.val(),
        postId: postIdInputElement.val()
    };

    $.post("/Post/AddChildComment", values, function (result) {
        if (result.status === 200) {
            commentInputElement.val("");

            var commentCount = commentCountInputElement.html();
            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInputElement.html(updatedCommentCount.toString());

            var object = result.data;
            var childCommentModel = mapObjectToCommentModel(object);

            var newCommentContainerHtml = createChildCommentContainerHtml(childCommentModel);

            childCommentListContainer.prepend(newCommentContainerHtml);
            var newCommentHtml = childCommentListContainer.children().first();

            if (childCommentListContainer.css("display") === "none") {
                childCommentListContainer.show("fast");
                newCommentHtml.show();
            } else {
                newCommentHtml.show("fast");
            }
        }
    });
});

$(document).on("click", ".show-child-comment-btn", function () {
    var button = $(this);

    var childCommentListContainer = button
        .parent()
        .parent()
        .siblings(".child-comment-list-container")
        .first();

    var loadChildrenCommentInputElement = button
        .parent()
        .siblings(".load-children-comment")
        .first();

    var loadChildrenComment = loadChildrenCommentInputElement.val();

    if (loadChildrenComment === "1") {
        if (childCommentListContainer.children().length > 0) {
            childCommentListContainer.toggle("fast");
        }
        return;
    }

    var commentIdInputElement = button
        .parent()
        .parent()
        .siblings(".child-comment-form")
        .first()
        .children(".comment-id")
        .first();

    var values = {
        commentId: commentIdInputElement.val(),
        skip: childCommentListContainer.children().length
    };

    $.post("/Post/ShowChildComments", values, function (result) {
        if (result.status === 200) {
            loadChildrenCommentInputElement.val("1");
            var objects = result.data;
            var count = objects.length;
            childCommentListContainer.show("fast");

            for (var i = 0; i < count; i++) {
                var object = objects[i];
                var childCommentModel = mapObjectToCommentModel(object);

                var newChildCommentContainerHtml = createChildCommentContainerHtml(childCommentModel);
                var newChildCommentContainer = $(newChildCommentContainerHtml);
                newChildCommentContainer.appendTo(childCommentListContainer);
                newChildCommentContainer.show("fast");
            }
        }
    });
});

$(document).on("click", ".add-child-comment-btn", function () {
    var button = $(this);

    var childCommentForm = button
        .parent()
        .parent()
        .siblings(".child-comment-form")
        .first();

    childCommentForm.toggle("fast");
});

function createCommentContainerHtml(commentModel, postId) {
    var commentContainerHtml =
        `<div class='comment-container' style='display: none'>
            <div class='master-comment-container'>
                <div class='comment-user-container'>
                    <strong class='comment-user'>${commentModel.username}</strong>
                    <button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>
                    <button class='add-child-comment-btn'><span class='glyphicon glyphicon-edit'></span></button>
                </div>
            <div class='comment-body'>${commentModel.content}</div>
            <input type='hidden' class='load-children-comment' value='0' />
        </div>
        <form class='comment-form child-comment-form' style='display: none'>
            <textarea class='comment-input' placeholder='Comment'></textarea>
            <input type='hidden' value=${commentModel.commentId} class='comment-id' />
            <input type='hidden' value=${postId} class='post-id' />
            <input type='hidden' value=${commentModel.createdDate} class='comment-created-date' />
            <button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>
        </form>
        <div class='child-comment-list-container'></div>`;

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

$("#expand-master-comment-btn").click(function () {
    var values = {
        postId: $("#post-id").val(),
        createdDateString: $(".comment-created-date").last().val()
    };

    $.post("/Post/ShowMoreComments", values, function (result) {
        if (result.status === 200) {
            var commentPaginationObject = result.data;
            var commentPaginationModel = mapObjectToCommentPaginationModel(commentPaginationObject);
            var commentModels = commentPaginationModel.commentModels;
            var commentModelCount = commentModels.length;

            var masterCommentListContainer = $(".master-comment-list-container").first();

            for (var i = 0; i < commentModelCount; i++) {
                var commentModel = mapObjectToCommentModel(commentModels[i]);

                var newCommentContainerHtml = createCommentContainerHtml(commentModel, values.postId);
                masterCommentListContainer.append(newCommentContainerHtml);

                var newCommentContainer = masterCommentListContainer.children().last();
                newCommentContainer.show("fast");
            }

            if (!commentPaginationModel.hasNext) {
                $("#expand-master-comment-btn").hide();
            }
        }
    });
});