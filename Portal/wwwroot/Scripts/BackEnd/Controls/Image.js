
/*----------------------------------------------------:upload-----------------------------------------------*/
function initUpload(purl,pUniqueId,pAdminWidth,pAdminHeight,pWidth,pHeight,pBOName) {
    $("#upload_image_container_" + pUniqueId).find('a').ajaxUpload({
        url: purl + "/?AdminWidth=" + pAdminWidth + "&AdminHeight=" + pAdminHeight + "&Width=" + pWidth + "&Height=" + pHeight + "&BOName=" + pBOName,
        name: "file",
        dataType: "JSON",
        onSubmit: function () {
            $("#upload_image_container_" + pUniqueId).find('.upload-image-image-loading').show();
            return true;
        },
        onComplete: function (responce) {
            $("#upload_image_container_" + pUniqueId).find('.upload-image-image-loading').hide();

            jsonResponce = JSON.parse(responce);
            if (jsonResponce.Result == 0) {
                alert(jsonResponce.Message);
                return true;
            }
            $("#upload_image_container_" + pUniqueId).parent().attr("data-id", jsonResponce.data.Id);
            $("#upload_image_container_" + pUniqueId).find('img').attr("src", jsonResponce.data.thumb);
            return true;
        }
    });
}

/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function Image_on_after_update_function(pControl, gpostArray) {
    gpostArray[pControl.attr("data-name")] = pControl.attr("data-id");
    pControl.prev().find("img").attr("src",  pControl.find("img").attr("src"));
    return pControl.attr("data-id");
}
