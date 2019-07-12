$(function () {
    $(".nav_link").click(function () {
        //Custom attribute data_id is used to store the ID
        //Get the ID
        alert("sdfsdfd");
        var id = $(this).attr("data_id");
        alert(id);
        $.ajax({
            url: 'https://localhost:44302/Project/DisplaYEmployees/3',
            type: "post",
            dataType: "html",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ tabId: id, projectId: 1 } ), //add parameter
            success: function (data) {
                alert(data);
                //success
                $('#ProductsDiv' + id).html(data); //populate the tab content.
            },
            error: function () {
//                var err = eval("(" + xhr.responseText + ")");
  //              alert(err.Message);
                alert("error");
            }
        });
    });
});