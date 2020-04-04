$('#readme-modal').on('show.bs.modal', (event) => {
    var button = $(event.relatedTarget);
    var htmlStr = button.data('src');
    var link = button.data('link');

    $('#body-content').html(htmlStr);
    $('#github-link').attr('href', link);
});