function checklimits(pcontrol, ptag,ucod='') {
    var minimum = parseFloat($("#" + ptag + "_min").val());
    var maximum = parseFloat($("#" + ptag + "_max").val());

    var prenum = $(pcontrol).val().replace(",", ".");
    var num = Number(prenum.replace(/[^0-9\.]+/g, ""));
    if ($(pcontrol).val() == 'absent') {
        num = 1;
    }
    if ($(pcontrol).val() == 'lipsa') {
        num = 1;
    }

    $("[name=" + ptag + "_outofbounds]").val("");
  
        $(pcontrol).closest(".edit-section-row").removeClass("edit-section-row-alert");
        $("[name=" + ptag + "_Valid" + ucod+"]").prop("checked",false);
    

    if ($(pcontrol).val() != "") {
        var value = parseFloat(num);

        var smultiply = $(pcontrol).closest(".edit-section-row").find("[name=" + ptag + '_advancedval' + ucod+"] option:selected").attr("data-advanced");

        if (smultiply != "" && smultiply != undefined) {
            multiply = parseFloat(smultiply);
            value = value * multiply;
        }

        $("[name=" + ptag + "_numeric]").val(num);

        if (minimum == 0 && maximum == 0) {
            return;
        }
        if (value < minimum || value > maximum) {
            $("[name=" + ptag + "_outofbounds]").val("1");

                $(pcontrol).closest(".edit-section-row").addClass("edit-section-row-alert");
                    
                $("[name=" + ptag + "_Valid" + ucod+"]").prop("checked",true);
        }
    }
    else {
        $("[name=" + ptag + "_numeric]").val("");
    }
}

function changeValidInvestihationState(pcontrol, ptag) {
    $(pcontrol).closest(".edit-section-row").removeClass("edit-section-row-alert");
    if ($(pcontrol).is(":checked")){
        $(pcontrol).closest(".edit-section-row").addClass("edit-section-row-alert");
    }
}

function changeValidInvestihationLimitState(pcontrol, pId) {
    $(pcontrol).closest(".edit-section-table-row").removeClass("edit-section-table-row-invalid");
    if ($(pcontrol).is(":checked")) {
        $(pcontrol).closest(".edit-section-table-row").addClass("edit-section-table-row-invalid");
    }
}
