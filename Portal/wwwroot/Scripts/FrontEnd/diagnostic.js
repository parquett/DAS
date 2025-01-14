function toggleDiagClass(pLink, diagnosticClassId) {
    if (!$(pLink).next().hasClass("diagnostics-container-opened")) {
        var postArray = "diagnosticClassId=" + diagnosticClassId;

        $.ajax({
            type: 'POST',
            url: gRootUrl + "Diagnostic/FindClass/",
            async: true,
            data: postArray,
            success: function (responce) {
                $(pLink).next().html(responce);
                $(pLink).next().slideDown("slow", function () {
                    $(pLink).next().addClass("diagnostics-container-opened");
                })
            }
        });
    }
    else {
        $(pLink).next().slideUp("slow", function () {
            $(pLink).next().removeClass("diagnostics-container-opened");
        })
    }

    return false;
}
function search_diagnostic_on_enter(pLink, event, filter) {
    if (event == null)
        event = window.event;

    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {

        var postArray = "diagnostickey=" + $("[name=diagnostic_searh]").val();

        $.ajax({
            type: 'POST',
            url: gRootUrl + "Diagnostic/FindDiagnostic/" + filter,
            async: true,
            data: postArray,
            success: function (responce) {
                $(".diagnostic-search-results").fadeOut("slow", function () {
                    $(".diagnostic-search-results").html(responce);
                    $(".diagnostic-search-results").fadeIn("slow");
                });
            }
        });

    }
}

function diagnostic_set_fav(pLink, event, diagnosticId, filter) {

    var postArray = "diagnosticId=" + diagnosticId;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Diagnostic/AddDiagnosticToFav/",
        async: true,
        data: postArray
    });

    $(pLink).toggleClass("fav-selected");
    diagnostic_reload_fav(filter);
    clickCancel(event);
}

function diagnostic_reload_fav(filter) {

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Diagnostic/LoadFav/" + filter,
        async: true,
        success: function (responce) {
            $(".popup-content-fav").html(responce);
        }
    });

}

function selectPopUpTab(pLink, tab) {

    $(".popup-content-tab").removeClass("popup-content-tab-selected");

    $(".popup-content-body-inner").not(".popup-content-" + tab).fadeOut("slow", function () {
        $(".popup-content-body-inner").not(".popup-content-" + tab).hide();
        if (!$(".popup-content-" + tab).is(":visible")) {
            $(".popup-content-" + tab).fadeIn("slow");
        }
    });

    $(pLink).addClass("popup-content-tab-selected");
    return false;
}

function diagnostic_select(pName, pId) {

    gDiagnosticControl.val(pName);

    var favs = readCookie("diagnosticvavs");

    if (favs == null)
        favs = pId
    else
        favs += "," + pId

    createCookie("diagnosticvavs", favs, 100);

    $.fancybox.close();
    gDiagnosticControl.focus();
    return false;
}