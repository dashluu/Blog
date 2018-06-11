$(".post-img").click(function () {
    $(this).parent().siblings().eq(0).children("form").eq(0).submit();
});