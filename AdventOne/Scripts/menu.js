$(function () {

    $(document).ready(function () {

        if (location.pathname.startsWith('/Project/Details/')) {

            var activeTab = jQuery("#ActiveTab").val();
            var projectId = jQuery("#ProjectId").val();
            var url = location.protocol + '//' + location.hostname + ':' + location.port + '/Project/DisplaySubView';

            $('#menu' + activeTab).addClass("in active");
            $('#tab' + activeTab).addClass("active");

            $.ajax({
                url: url,
                type: "post",
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ tabId: activeTab, projectId: projectId }), //add parameters
                success: function (data) {
                    $('#ProductsDiv' + activeTab).html(data);
                },
                error: function () {
                    alert("error");
                }
            });

        }

        if (location.pathname.startsWith('/Employee/Details/')) {

            var activeTab = jQuery("#ActiveTab").val();
            var employeeId = jQuery("#EmployeeId").val();
            var url = location.protocol + '//' + location.hostname + ':' + location.port + '/Employee/DisplaySubView';
            $('#menu' + activeTab).addClass("in active");
            $('#tab' + activeTab).addClass("active");

            $.ajax({
                url: url,
                type: "post",
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ tabId: activeTab, employeeId: employeeId }), //add parameters
                success: function (data) {
                    $('#ProductsDiv' + activeTab).html(data);
                },
                error: function () {
                    alert("error");
                }
            });

        }

    });

    $(".nav_link").click(function () {

        if (location.pathname.startsWith('/Project/Details/')) {

            var id = $(this).attr("data_id");
            var projectId = jQuery("#ProjectId").val();
            var url = location.protocol + '//' + location.hostname + ':' + location.port + '/Project/DisplaySubView';

            $.ajax({
                url: url,
                type: "post",
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ tabId: id, projectId: projectId }), //add parameters
                success: function (data) {
                    $('#ProductsDiv' + id).html(data);
                },
                error: function () {
                    alert("error");
                }
            });

        }

        if (location.pathname.startsWith('/Employee/Details/')) {

            var id = $(this).attr("data_id");
            var employeeId = jQuery("#EmployeeId").val();
            var url = location.protocol + '//' + location.hostname + ':' + location.port + '/Employee/DisplaySubView';

            $.ajax({
                url: url,
                type: "post",
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ tabId: id, employeeId: employeeId }), //add parameters
                success: function (data) {
                    $('#ProductsDiv' + id).html(data);
                },
                error: function () {
                    alert("error");
                }
            });

        }

    });

});