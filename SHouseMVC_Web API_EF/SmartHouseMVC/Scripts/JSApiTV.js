function Action(key) {

    $("#" + key + "-on").click(function(e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/on/" + key,
            type: "PUT",
            success: function(data) {
                var src;
                if ($("#" + key + "-chSetup").attr("value") === "00" || $("#" + key + "-chSetup").attr("value") === "10") {
                    src = "/Content/images/1.gif";
                    $("#" + key + "-chSetup").attr("value", "10");
                } else {
                    src = "/Content/images/2.gif";
                    $("#" + key + "-chSetup").attr("value", "11");
                }
                $("#" + key + "-imTV").attr("src", src);
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-off").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/off/" + key,
            type: "PUT",
            success: function(data) {
                var src = "/Content/images/offTV.png";
                if ($("#" + key + "-chSetup").attr("value") === "10") {
                    $("#" + key + "-chSetup").attr("value", "00");
                } else {
                    $("#" + key + "-chSetup").attr("value", "01");
                }
                $("#" + key + "-imTV").attr("src", src);
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-scan").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/scan/" + key,
            type: "PUT",
            success: function(data) {
                var src;
                if ($("#" + key + "-chSetup").attr("value") === "01" || $("#" + key + "-chSetup").attr("value") === "00") {
                    src = "/Content/images/offTV.png";
                    alert("Сначала надо включить телевизор");
                } else {
                    src = "/Content/images/2.gif";
                    $("#" + key + "-chSetup").attr("value", "11");
                }
                if ($("#" + key + "-chSetup").attr("value") === "01" || $("#" + key + "-chSetup").attr("value") === "11") {
                    $(".buttonScan").css("color", "#444");
                }
                $("#" + key + "-imTV").attr("src", src);
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-listChan").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/listChan/" + key,
            type: "PUT",
            success: function(data) {
                alert(data);
            }
        });
    });

    $("#" + key + "-nCh").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/nCh/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-eCh").click(function(e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/eCh/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-prevCh").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/prevCh/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-maxV").click(function(e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/maxV/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-minV").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/minV/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-mute").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/mute/" + key,
            type: "PUT",
            success: function(data) {
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-setChannel").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/" + key + "/chan/" + $("#chan-" + key).val(),
            type: "PUT",
            success: function(data) {
                var ch = $("#chan-" + key).val();
                if (ch <= 0 || ch > 100) {
                    alert("Ошибка. Такого канала не существует!");
                };
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-setVolume").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/" + key + "/vol/" + $("#vol-" + key).val(),
            type: "PUT",
            success: function(data) {
                var v = $("#vol-" + key).val();
                if (v < 0 || v > 100) {
                    alert("Ошибка! Недопустимое значение громкости!");
                };
                $("#" + key + "-status").html(data);
            }
        });
    });

    $("#" + key + "-Del").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/api/TVApi/" + key,
            type: "DELETE",
            success: function(data) {
                $("#" + key + "-device-div").remove();
                alert("Устройство удалено!");
            }
        });
    });
}