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

    var commentTextBox = form.children(".comment-input").first();
    var commentInput = commentTextBox.val();

    var postInput = form.children(".post-id").first();
    var postId = postInput.val();

    var commentCountInput = $(".comment-count").first();
    var commentCount = commentCountInput.html();

    var masterCommentListContainer = form.siblings(".master-comment-list-container").first();

    $.post("/Post/AddMasterComment", { comment: commentInput, postId: postId }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");

            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInput.html(updatedCommentCount.toString());

            var object = result.data;
            var commentModel = mapObjectToCommentModel(object);

            var newCommentContainerSelector = createCommentContainerSelector(commentModel, postId);
            masterCommentListContainer.prepend(newCommentContainerSelector);
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

    var commentTextBox = form.children(".comment-input").first();
    var commentInput = commentTextBox.val();

    var commentIdInput = form.children(".comment-id").first();
    var commentId = commentIdInput.val();

    var postIdInput = form.children(".post-id").first();
    var postId = postIdInput.val();

    var commentCountInput = $(".comment-count").first();
    var commentCount = commentCountInput.html();

    var childCommentListContainer = form.siblings(".child-comment-list-container").first();

    $.post("/Post/AddChildComment", { comment: commentInput, commentId: commentId, postId: postId }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");

            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInput.html(updatedCommentCount.toString());

            var object = result.data;
            var childCommentModel = mapObjectToCommentModel(object);

            var newCommentContainerSelector = createChildCommentContainerSelector(childCommentModel);

            childCommentListContainer.prepend(newCommentContainerSelector);
            var newCommentSelector = childCommentListContainer.children().first();

            if (childCommentListContainer.css("display") === "none") {
                childCommentListContainer.show("fast");
                newCommentSelector.show();
            } else {
                newCommentSelector.show("fast");
            }
        }
    });
});

$(document).on("click", ".show-child-comment-btn", function () {
    var thisBtn = $(this);

    var childCommentListContainer = thisBtn
        .parent()
        .parent()
        .siblings(".child-comment-list-container")
        .first();

    var loadChildrenCommentInput = thisBtn
        .parent()
        .siblings(".load-children-comment")
        .first();

    var loadChildrenComment = loadChildrenCommentInput.val();

    if (loadChildrenComment === "1") {
        if (childCommentListContainer.children().length > 0) {
            childCommentListContainer.toggle("fast");
        }
        return;
    }

    var commentIdInput = $(this)
        .parent()
        .parent()
        .siblings(".child-comment-form")
        .first()
        .children(".comment-id")
        .first();

    var commentId = commentIdInput.val();
    var skip = childCommentListContainer.children().length;

    $.post("/Post/ShowChildComments", { commentId: commentId, skip: skip }, function (result) {
        if (result.status === 200) {
            loadChildrenCommentInput.val("1");
            var objects = result.data;
            var count = objects.length;
            childCommentListContainer.show("fast");

            for (var i = 0; i < count; i++) {
                var object = objects[i];
                var childCommentModel = mapObjectToCommentModel(object);

                var newChildCommentContainerSelector = createChildCommentContainerSelector(childCommentModel);
                var newChildCommentContainer = $(newChildCommentContainerSelector);
                newChildCommentContainer.appendTo(childCommentListContainer);
                newChildCommentContainer.show("fast");
            }
        }
    });
});

$(document).on("click", ".add-child-comment-btn", function () {
    var commentForm = $(this)
        .parent()
        .parent()
        .siblings(".child-comment-form")
        .first();
    commentForm.toggle("fast");
});

function createCommentContainerSelector(commentModel, postId) {
    var commentContainerSelector = "<div class='comment-container' style='display: none'>" +
        "<div class='master-comment-container'>" +
        "<div class='comment-user-container'>" +
        "<strong class='comment-user'>" + commentModel.username + "</strong>" +
        "<button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>" +
        "<button class='add-child-comment-btn'><span class='glyphicon glyphicon-edit'></span></button>" +
        "</div>" +
        "<div class='comment-body'>" + commentModel.content + "</div>" +
        "<input type='hidden' class='load-children-comment' value='0' />" +
        "</div>" +
        "<form class='comment-form child-comment-form' action='/ViewPost' method='post' style='display: none'>" +
        "<textarea class='comment-input' placeholder='Comment'></textarea>" +
        "<input type='hidden' value='" + commentModel.commentId + "' class='comment-id' />" +
        "<input type='hidden' value='" + postId + "' class='post-id'/>" +
        "<input type='hidden' value='" + commentModel.createdDate + "'class='comment-created-date' />" +
        "<button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>" +
        "</form>" +
        "<div class='child-comment-list-container'>" +
        "</div>";

    return commentContainerSelector;
}

function createChildCommentContainerSelector(commentModel) {
    var childCommentContainerSelector = "<div class='child-comment-container' style='display: none'>" +
        "<div class='comment-user-section'><strong class='comment-user'>" + commentModel.username + "</strong></div>" +
        "<div class='comment-body'>" + commentModel.content + "</div>" +
        "</div>";

    return childCommentContainerSelector;
} 

$("#expand-master-comment-btn").click(function () {
    var postId = $("#post-id").val();
    var createdDateString = $(".comment-created-date").last().val();

    $.post("/Post/ShowMoreComments", { postId: postId, createdDateString: createdDateString }, function (result) {
        if (result.status === 200) {
            var commentPaginationObject = result.data;
            var commentPaginationModel = mapObjectToCommentPaginationModel(commentPaginationObject);
            var commentModels = commentPaginationModel.commentModels;
            var commentModelCount = commentModels.length;

            var masterCommentListContainer = $(".master-comment-list-container").first();

            for (var i = 0; i < commentModelCount; i++) {
                var commentModel = mapObjectToCommentModel(commentModels[i]);

                var newCommentContainerSelector = createCommentContainerSelector(commentModel, postId);
                masterCommentListContainer.append(newCommentContainerSelector);

                var newCommentContainer = masterCommentListContainer.children().last();
                newCommentContainer.show("fast");
            }

            if (!commentPaginationModel.hasNext) {
                $("#expand-master-comment-btn").hide();
            }
        }
    });
});