$("#add-category-btn").click(function () {

    var nameInput = $("#category-name");
    var name = nameInput.val();
    var descriptionInput = $("#category-description");
    var description = descriptionInput.val();
    var form = $("#category-form");

    form.validate({
        rules: {
            CategoryName: {
                required: true,
                normalizer: function (value) {
                    return $.trim(value);
                }
            },
            CategoryDescription: {
                required: true,
                normalizer: function (value) {
                    return $.trim(value);
                }
            }
        },
        messages: {
            CategoryName: "Category name is required.",
            CategoryDescription: "Category description is required."
        }
    });

    if (!form.valid()) {
        return;
    }

    $.post("/Post/CreateCategory", { name: name, description: description }, function (result) {
        if (result.status === 200) {
            nameInput.val("");
            descriptionInput.val("");

            var titleCaseName = result.name.charAt(0).toUpperCase() + result.name.slice(1);

            $('#post-category').append($('<option>', {
                value: result.name,
                text: titleCaseName
            }));
        }
    }); 

});