$(document).on("submit", ".master-comment-form", function (event) {
    event.preventDefault();
    var form = $(this);
    var commentTextBox = form.children(".comment-input").eq(0);
    var id = $("#post-id").val();
    var commentInput = commentTextBox.val();
    $.post("/ViewPost/MasterComment", { comment: commentInput, postId: id }, function (result) {
        commentTextBox.val("");
        var newCommentContainer = $("<div class='comment-container'>" +
            "<div class='master-comment-container'>" +
            "<p class='comment-user-container'>" +
            "<strong class='comment-user'>" + result.username + "</strong>" +
            "<button class='show-child-comment-btn'><span class='glyphicon glyphicon-list'></span></button>" +
            "</p>" +
            "<p class='comment-body'>" + commentInput + "</p>" +
            "</div>" +
            "<form class='comment-form child-comment-form' action='/ViewPost' method='post'>" +
            "<input type='text' class='comment-input' placeholder='Comment' />" +
            "<input type='hidden' value='" + result.commentId + "' class='comment-id' />" +
            "<button type='submit' class='submit-comment-btn'><span class='glyphicon glyphicon-send'></span></button>" +
            "</form>" +
            "<div class='children-comment-container'>" +
            "</div>");
        newCommentContainer.insertAfter(form);
    });
});

$(document).on("submit", ".child-comment-form", function (event) {
    event.preventDefault();
    var form = $(this);
    var commentTextBox = form.children(".comment-input").eq(0);
    var idInput = form.children(".comment-id").eq(0);
    var id = idInput.val();
    var commentInput = commentTextBox.val();
    var childrenCommentContainer = form.siblings(".children-comment-container").eq(0);
    $.post("/ViewPost/ChildComment", { comment: commentInput, commentId: id }, function (result) {
        commentTextBox.val("");
        var newCommentContainer = "<div class='child-comment-container'>" +
            "<p class='comment-user-section'><strong class='comment-user'>" + result.username + "</strong></p>" +
            "<p class='comment-body'>" + commentInput + "</p>" +
            "</div>";
        childrenCommentContainer.prepend(newCommentContainer);
        childrenCommentContainer.show();
    });
});

$(document).on("click", ".show-child-comment-btn", function () {
    var commentContainer = $(this).parent().parent().siblings(".children-comment-container").eq(0);
    if (commentContainer.children().length > 0) {

        commentContainer.toggle();
    }
});