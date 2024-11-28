/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function checkbox_on_after_update_function(pControl, gpostArray) {
    gpostArray[pControl.find("input").attr("name")] = (pControl.find("input").is(":checked") ? "1" : "0");
    if (pControl.prev().hasClass("control-view")) {
        pControl.prev().html(pControl.find("input").is(":checked") ? "Yes" : "No");
    }
    return { "name": pControl.find("input").attr("name"), "value": (pControl.find("input").is(":checked") ? "1" : "0") };
}
