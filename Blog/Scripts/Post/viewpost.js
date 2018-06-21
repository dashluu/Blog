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

    var commentTextBox = form.children(".comment-input").eq(0);
    var commentInput = commentTextBox.val();

    var postInput = form.children(".post-id").eq(0);
    var id = postInput.val();

    $.post("/Post/MasterComment", { comment: commentInput, postId: id }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");
            var newCommentContainerSelector = createCommentContainerSelector(result.username, commentInput, result.commentId, id);
            var newCommentContainer = $(newCommentContainerSelector);
            newCommentContainer.insertAfter(form).show("fast");
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

    var commentTextBox = form.children(".comment-input").eq(0);
    var commentInput = commentTextBox.val();

    var commentIdInput = form.children(".comment-id").eq(0);
    var postIdInput = form.children(".post-id").eq(0);
    var commentId = commentIdInput.val();
    var postId = postIdInput.val();
    var childrenCommentContainer = form.siblings(".children-comment-container").eq(0);

    $.post("/Post/ChildComment", { comment: commentInput, commentId: commentId, postId: postId }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");

            var newCommentContainerSelector = createChildCommentContainerSelector(result.username, commentInput);

            childrenCommentContainer.prepend(newCommentContainerSelector);
            var newCommentSelector = childrenCommentContainer.children().eq(0);

            if (childrenCommentContainer.css("display") === "none") {
                childrenCommentContainer.show("fast");
                newCommentSelector.show();
            } else {
                newCommentSelector.show("fast");
            }
        }
    });
});

$(document).on("click", ".show-child-comment-btn", function () {
    var thisBtn = $(this);

    var childrenCommentContainer = thisBtn.parent()
        .parent()
        .siblings(".children-comment-container")
        .eq(0);

    var loadChildrenCommentInput = thisBtn.parent()
        .siblings(".load-children-comment")
        .eq(0);

    var loadChildrenComment = loadChildrenCommentInput.val();

    if (loadChildrenComment === "1") {
        childrenCommentContainer.toggle("fast");
        return;
    }

    var commentIdInput = $(this).parent()
        .parent()
        .siblings(".child-comment-form").eq(0)
        .children(".comment-id").eq(0);

    var commentId = commentIdInput.val();
    var skip = childrenCommentContainer.children().length;

    $.post("/Post/ShowChildComments", { commentId: commentId, skip: skip }, function (result) {
        if (result.status === 200) {
            loadChildrenCommentInput.val("1");
            var childComments = result.data;
            var nChildComment = childComments.length;
            childrenCommentContainer.show("fast");

            for (var i = 0; i < nChildComment; i++) {
                var childComment = childComments[i];
                var newChildCommentContainerSelector = createChildCommentContainerSelector(childComment.Username, childComment.Content);
                var newChildCommentContainer = $(newChildCommentContainerSelector);
                newChildCommentContainer.appendTo(childrenCommentContainer);
                newChildCommentContainer.show("fast");
            }
        }
    });
});

$(document).on("click", ".add-child-comment-btn", function () {
    var commentForm = $(this).parent()
        .parent()
        .siblings(".child-comment-form")
        .eq(0);
    commentForm.toggle("fast");
});

function createCommentContainerSelector(username, content, commentId, postId) {
    var commentContainerSelector = "<div class='comment-container' style='display: none'>" +
        "<div class='master-comment-container'>" +
        "<p class='comment-user-container'>" +
        "<strong class='comment-user'>" + username + "</strong>" +
        "<button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>" +
        "<button class='add-child-comment-btn'><span class='glyphicon glyphicon-edit'></span></button>" +
        "</p>" +
        "<p class='comment-body'>" + content + "</p>" +
        "<input type='hidden' class='load-children-comment' value='0' />" +
        "</div>" +
        "<form class='comment-form child-comment-form' action='/ViewPost' method='post' style='display: none'>" +
        "<textarea class='comment-input' placeholder='Comment'></textarea>" +
        "<input type='hidden' value='" + commentId + "' class='comment-id' />" +
        "<input type='hidden' value='" + postId + "' class='post-id'/>" +
        "<button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>" +
        "</form>" +
        "<div class='children-comment-container'>" +
        "</div>";

    return commentContainerSelector;
}

function createChildCommentContainerSelector(username, content) {
    var childCommentContainerSelector = "<div class='child-comment-container' style='display: none'>" +
        "<p class='comment-user-section'><strong class='comment-user'>" + username + "</strong></p>" +
        "<p class='comment-body'>" + content + "</p>" +
        "</div>";

    return childCommentContainerSelector;
} 

$("#expand-master-comment-btn").click(function () {
    var postId = $("#post-id").val();
    var skip = $(".comment-container").length;

    $.post("/Post/PaginateComment", { postId: postId, skip: skip }, function (result) {
        if (result.status === 200) {
            var comments = result.data;
            var commentSection = $(".comment-section").eq(0);
            var nComment = comments.length;

            for (var i = 0; i < nComment; i++) {
                var comment = comments[i];
                var newCommentContainerSelector = createCommentContainerSelector(comment.Username, comment.Content, comment.CommentId, postId);
                var newCommentContainer = $(newCommentContainerSelector);
                newCommentContainer.appendTo(commentSection);
                newCommentContainer.show("fast");
            }

            if (!result.hasNext) {
                $("#expand-master-comment-btn").hide();
            }
        }
    });
});