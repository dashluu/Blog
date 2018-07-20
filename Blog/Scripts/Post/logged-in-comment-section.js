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

    var postIdInputElement = $(".post-id").first();

    var commentCountInputElement = $(".comment-count").first();

    var masterCommentListContainer = form
        .siblings(".master-comment-list-container")
        .first();

    var values = {
        comment: commentInputElement.val(),
        postId: postIdInputElement.val()
    };

    var url = "/Comments";

    $.post(url, values, function (result) {
        if (result.status === 200) {
            commentInputElement.val("");

            var commentCount = commentCountInputElement.html();
            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInputElement.html(updatedCommentCount.toString());

            var object = result.data;
            var commentModel = mapObjectToCommentModel(object);

            var newCommentContainerHtml = createCommentContainerHtml(commentModel);
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

    var parentCommentIdInputElement = form
        .siblings(".comment-info-container")
        .first()
        .children(".comment-id")
        .first();

    var postIdInputElement = $(".post-id").first();

    var commentCountInputElement = $(".comment-count").first();

    var childCommentListContainer = form
        .siblings(".child-comment-list-container")
        .first();

    var parentCommentId = parentCommentIdInputElement.val();

    var values = {
        comment: commentInputElement.val(),
        postId: postIdInputElement.val(),
        loadChildComments: childCommentListContainer.children().length === 0
    };

    var url = `/Comments/${parentCommentId}/ChildComments/New`;

    $.post(url, values, function (result) {
        if (result.status === 200) {
            commentInputElement.val("");

            var commentCount = commentCountInputElement.html();
            var updatedCommentCount = parseInt(commentCount) + 1;
            commentCountInputElement.html(updatedCommentCount.toString());

            if (!values.loadChildComments) {
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
            } else {
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
        }
    });
});

$(document).on("click", ".add-child-comment-btn", function () {
    var childCommentForm = $(this)
        .parent()
        .parent()
        .siblings(".child-comment-form")
        .first();

    childCommentForm.toggle("fast");
});