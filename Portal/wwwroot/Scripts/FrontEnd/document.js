$(document).ready(function () {
});


function change_document_mode(pInput) {
    if (!$("[name=DocumentMode_in]").is(":checked") && $(pInput).attr("name") == "DocumentMode_in") {
        $("[name=DocumentMode_in]").prop("checked", true);
        return;
    }

    if (!$("[name=DocumentMode_out]").is(":checked") && $(pInput).attr("name") == "DocumentMode_out") {
        $("[name=DocumentMode_out]").prop("checked", true);
        return;
    }

    if ($(pInput).attr("name") == "DocumentMode_out") {
        $("[name=DocumentMode_in]").prop("checked", false);
    }

    if ($(pInput).attr("name") == "DocumentMode_in") {
        $("[name=DocumentMode_out]").prop("checked", false);
    }
}

/*----------------------------------------------------:upload-----------------------------------------------*/
function initUploadDocumentFile(purl, pUniqueId) {
    $("#UploadFile").ajaxUpload({
        url: purl,
        name: "file",
        dataType: "JSON",
        onSubmit: function () {  
            if (!$(".loading").is(":visible")) {
                $(".loading").show();
                return true;
            }
            return false;
        },
        onComplete: function (responce) {
            jsonResponce = JSON.parse(responce);
            if (jsonResponce.Result == 0) {
                alert(jsonResponce.Message);
                $(".loading").hide();
                return true;
            }
            //tbd...
            $("[name=" + pUniqueId + "]").val(jsonResponce.data.Id);
            $("[name=Name]").val(jsonResponce.data.name);

            $("#" + pUniqueId + "_link").attr("href", jsonResponce.data.file);
            $("#" + pUniqueId + "_link").html(jsonResponce.data.name);
            $(".loading").hide();
            return true;
        }
    });
}