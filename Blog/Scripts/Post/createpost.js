$("#add-category-btn").click(function () {

    var nameInput = $("#category-name");
    var name = nameInput.val();
    var descriptionInput = $("#category-description");
    var description = descriptionInput.val();

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