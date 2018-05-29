tinymce.init({
    selector: 'textarea#blog-editor',
    resize: false,
    plugins: "lists",
    toolbar1: 'undo redo | bold italic underline | alignleft aligncenter alignright alignjustify',
    toolbar2: 'cut copy paste | numlist bullist',
    menubar: false,
    height: 350
});
