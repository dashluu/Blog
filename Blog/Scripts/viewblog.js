$(document).on("submit", ".master-comment-form", function (event) {
    event.preventDefault();
    var form = $(this);
    var commentTextBox = form.children(".comment-input").eq(0);
    var commentInput = commentTextBox.val();
    if (commentInput === "") {
        alert("Your comment is empty.");
        return;
    }
    var postInput = form.children(".post-id").eq(0);
    var id = postInput.val();
    $.post("/ViewPost/MasterComment", { comment: commentInput, postId: id }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");
            var newCommentContainer = $("<div class='comment-container' style='display: none'>" +
                "<div class='master-comment-container'>" +
                "<p class='comment-user-container'>" +
                "<strong class='comment-user'>" + result.username + "</strong>" +
                "<button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>" +
                "<button class='add-child-comment-btn'><span class='glyphicon glyphicon-edit'></span></button>" +
                "</p>" +
                "<p class='comment-body'>" + commentInput + "</p>" +
                "</div>" +
                "<form class='comment-form child-comment-form' action='/ViewPost' method='post' style='display: none'>" +
                "<textarea class='comment-input' placeholder='Comment'></textarea>" +
                "<input type='hidden' value='" + result.commentId + "' class='comment-id' />" +
                "<input type='hidden' value='" + id + "' class='post-id'/>" +
                "<button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>" +
                "</form>" +
                "<div class='children-comment-container'>" +
                "</div>");
            newCommentContainer.insertAfter(form).show("slow");
        }
    });
});

$(document).on("submit", ".child-comment-form", function (event) {
    event.preventDefault();
    var form = $(this);
    var commentTextBox = form.children(".comment-input").eq(0);
    var commentInput = commentTextBox.val();
    if (commentInput === "") {
        alert("Your comment is empty.");
        return;
    }
    var commentIdInput = form.children(".comment-id").eq(0);
    var postIdInput = form.children(".post-id").eq(0);
    var commentId = commentIdInput.val();
    var postId = postIdInput.val();
    var childrenCommentContainer = form.siblings(".children-comment-container").eq(0);
    $.post("/ViewPost/ChildComment", { comment: commentInput, commentId: commentId, postId: postId }, function (result) {
        if (result.status === 200) {
            commentTextBox.val("");
            var newCommentContainer = "<div class='child-comment-container' style='display: none'>" +
                "<p class='comment-user-section'><strong class='comment-user'>" + result.username + "</strong></p>" +
                "<p class='comment-body'>" + commentInput + "</p>" +
                "</div>";
            childrenCommentContainer.prepend(newCommentContainer);
            var newCommentSelector = childrenCommentContainer.children().eq(0);
            if (childrenCommentContainer.css("display") === "none") {
                childrenCommentContainer.show("slow");
                newCommentSelector.show();
            } else {
                newCommentSelector.show("slow");
            }
        }
    });
});

$(document).on("click", ".show-child-comment-btn", function () {
    var commentContainer = $(this).parent().parent().siblings(".children-comment-container").eq(0);
    if (commentContainer.children().length > 0) {
        commentContainer.toggle("slow");
    }
});

$(document).on("click", ".add-child-comment-btn", function () {
    var commentForm = $(this).parent().parent().siblings(".child-comment-form").eq(0);
    commentForm.toggle("slow");
});