$(document).on("click", ".show-child-comment-btn", function () {
    var button = $(this);

    var childCommentListContainer = button
        .parent()
        .parent()
        .siblings(".child-comment-list-container")
        .first();

    if (childCommentListContainer.children().length > 0) {
        childCommentListContainer.toggle("fast");
        return;
    }

    var parentCommentIdInputElement = button
        .parent()
        .parent()
        .siblings(".comment-info-container")
        .first()
        .children(".comment-id")
        .first();

    var parentCommentId = parentCommentIdInputElement.val();

    var url = `/Comments/${parentCommentId}/ChildComments`;

    $.post(url, {}, function (result) {
        if (result.status === 200) {
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

$("#expand-master-comment-btn").click(function () {
    var postId = $(".post-id").first().val();

    var values = {
        createdDateString: $(".comment-created-date").last().val()
    };

    var url = `/${postId}/Comments/More`;

    $.post(url, values, function (result) {
        if (result.status === 200) {
            var commentPaginationObject = result.data;
            var commentPaginationModel = mapObjectToCommentPaginationModel(commentPaginationObject);
            var commentModels = commentPaginationModel.commentModels;
            var commentModelCount = commentModels.length;

            var masterCommentListContainer = $(".master-comment-list-container").first();

            for (var i = 0; i < commentModelCount; i++) {
                var commentModel = mapObjectToCommentModel(commentModels[i]);

                var newCommentContainerHtml = createCommentContainerHtml(commentModel);
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