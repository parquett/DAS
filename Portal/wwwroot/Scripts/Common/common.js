/*----------------------------------------------------:Error Handling--------------------------------------------*/
function show_error_message(messsage) {
    $(".message-box").fadeOut();
    $(".error_msg").html(messsage);
    $(".message-box").show();
    $(".message-box").fadeIn("slow");
}

/*----------------------------------------------------:validation-----------------------------------------------*/

var gValidationResult = true;
var gErrorMessage = "";
function form_validation(form_to_validate, global_page) {
    form_to_validate.find(".input").removeClass("input-error", 100);
    form_to_validate.find(".select").removeClass("input-error", 100);
    $(".error-message").fadeOut();
    gValidationResult = true;
    form_to_validate.find(".input:visible").each(function (i) {
        if (!global_page || $(this).closest(".dynamic-section").size() == 0) {
            var ballownew = $(this).attr("data-autocomplete-allownew") == "1";
            if ($(this).attr("data-req") == "1" && ($.trim($(this).val()) == "" || ($(this).hasClass("autocomplete-input") && !ballownew && $.trim($(this).parent().find("[name=" + $(this).attr("name") + "_id]").val()) == ""))) {
                $(this).addClass("input-error", "slow");
                $(this).focus();
                if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                    $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    $(this).next().fadeIn("slow");
                }
                else {
                    if (global_page) {
                        $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    }
                }
                gValidationResult = false;
            }
            else if (($(this).attr("data-req") == "2" && $.trim($(this).val()) != "") || $(this).attr("data-req") == "5") {
                var patt = new RegExp($(this).attr("data-regexp"));
                if (!patt.test($.trim($(this).val()))) {
                    $(this).addClass("input-error", "slow");
                    $(this).focus();
                    if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                        $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                        $(this).next().fadeIn("slow");
                    }
                    else {
                        if (global_page) {
                            $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                            $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                        }
                    }
                    gValidationResult = false;
                }
            }
            else if ($(this).attr("data-minlength") != null && $.trim($(this).val()).length > 0 && Number($(this).attr("data-minlength")) > $.trim($(this).val()).length) {
                $(this).addClass("input-error", "slow");

                if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                    $(this).next().find(".error-message-text").html("Cimpul tre sa fie cel putin " + $(this).attr("data-minlength") + " charactere");
                    $(this).next().fadeIn("slow");
                }
                else {
                    if (global_page) {
                        $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $(this).next().find(".error-message-text").html("Cimpul tre sa fie cel putin " + $(this).attr("data-minlength") + " chractere");
                    }
                }
                $(this).focus();
                gValidationResult = false;
            }
            else if ($(this).attr("data-validationfunc") != null && eval($(this).attr("data-validationfunc") + "(this)")) {
                $(this).addClass("input-error", "slow");

                if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                    $(this).next().find(".error-message-text").html(gErrorMessage != "" ? gErrorMessage : $(this).attr("data-req-mess"));
                    $(this).next().fadeIn("slow");
                }
                else {
                    if (global_page) {
                        $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $(this).next().find(".error-message-text").html(gErrorMessage != "" ? gErrorMessage : $(this).attr("data-req-mess"));
                    }
                }
                $(this).focus();
                gValidationResult = false;
                gErrorMessage = "";
            }
        }
    });

    form_to_validate.find(".select:visible").each(function (i) {
        if (!global_page || $(this).closest(".dynamic-section").size() == 0) {
            if ($(this).attr("data-req") == "1" && $.trim($(this).val()) == "") {
                $(this).addClass("input-error", "slow");
                $(this).focus();
                if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                    $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    $(this).next().fadeIn("slow");
                }
                else {
                    if (global_page) {
                        $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    }
                }
                gValidationResult = false;
            }
            else if ($(this).attr("data-validationfunc") != null && eval($(this).attr("data-validationfunc") + "(this)")) {
                $(this).addClass("input-error", "slow");

                if ($(this).next().size() > 0 && $(this).next().hasClass("error-message")) {
                    $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    $(this).next().fadeIn("slow");
                }
                else {
                    if (global_page) {
                        $(this).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $(this).next().find(".error-message-text").html($(this).attr("data-req-mess"));
                    }
                }
                $(this).focus();
                gValidationResult = false;
            }
        }
    });
    return gValidationResult;
}

function submit_on_enter(input, event) {
    if (event == null)
        event = window.event;

    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {
        eval($(input).closest('.form-controls').find(".button:first").attr("onclick").replace("return ", ""));
    }
}

/*----------------------------------------------------:table---------------------------------------------------*/
var gpostArray;

function save_row_item(id) {

    if (form_validation($("#dataGrid"), false)) {

        gpostArray = {};
        gpostArray["bo_type"] = $("#dataGrid").attr("data-type");
        gpostArray["id"] = id;
        gpostArray["simple"] = 1;
        $(".data-grid-data-row-" + id).find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $(".data-grid-data-row-" + id).find(".cp-action-save").fadeOut("slow", function () {
            $(".data-grid-data-row-" + id).find(".cp-action-edit").css("display", "inline-block");
        });

        $(".data-grid-data-row-" + id).find(".control-edit").fadeOut("slow", function () {
            $(".data-grid-data-row-" + id).find(".control-view").fadeIn("slow");
        });

        save_item_generic(gpostArray, null);
    }
    return false;
}

function save_item(id) {

    if (form_validation($(".data-item-container"), true)) {

        gpostArray = {};
        gpostArray["bo_type"] = $(".data-item-container").attr("data-type");
        gpostArray["id"] = id;

        $(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $(".btn-save").fadeOut("slow", function () {
            $(".btn-info").css("display", "inline-block");
        });

        $(".control-edit").fadeOut("slow", function () {
            $(".control-view").fadeIn("slow");
        });

        save_item_generic(gpostArray, null);

    }
    return false;
}

function save_and_close_item(id, redirectUrl) {

    if (form_validation($(".data-item-container"), true)) {

        gpostArray = {};
        gpostArray["bo_type"] = $(".data-item-container").attr("data-type");
        gpostArray["id"] = id;

        $(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $(".btn-save").attr("disabled", true);
        $(".control-edit").attr("disabled", true);

        save_item_generic(gpostArray, redirectUrl);
    }
    return false;
}

function save_item_generic(gpostArray, redirectUrl) {

    $.ajax({
        type: 'POST',
        url: gRootUrl + "DataProcessor/Update/",
        async: true,
        dataType: "JSON",
        data: gpostArray,
        success: function (responce) {
            if (responce["Result"] == 2) {
                window.location.reload();
                return;
            }
            if (responce["Result"] == 1) {
                if (redirectUrl != null) {
                    window.location = redirectUrl;
                    return;
                }
            }
            if (responce["Result"] == 0) {
                var y = ($(window).height() / 2) - 180;
                $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                $(".modal-warning").find(".modal-body").find("p").html("Operation Fail");
                $(".modal-warning").fadeIn("slow");
                return;
            }
        }
    });
}

function resize_datagrid(pBtn) {
    $(pBtn).closest('.wrapper').find('.SecurityCRM_buttons_section').toggleClass('SecurityCRM_buttons_section_collapsed')
    if (window.drawTable) {

        window.setTimeout(function () {
            $('#dataGrid').DataTable().destroy();
            drawTable(gaoSearchData);
        }, 500);
    }
    return false;
}

var gaoSearchData = Array();
function do_search() {
    gpostArray = {};
    gaoSearchData = Array();
    gaoSearchData.push({ "name": "isFileter", "value": "1" });

    $(".seach-body").find(".control-edit").each(function (i) {
        var control = $(this).attr("data-control");
        gaoSearchData.push(eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);"));
    });

    $('#dataGrid').DataTable().destroy();
    drawTable(gaoSearchData);
}

function do_clear_search() {
    gaoSearchData = Array();

    $(".seach-body").find(".control-edit").each(function (i) {
        var control = $(this).attr("data-control");
        eval("if (window." + control + "_on_clear) " + control + "_on_clear($(this));");
    });

    $('#dataGrid').DataTable().destroy();
    drawTable(gaoSearchData);
}

function save_new_item(sDataType) {

    if (form_validation($(".data-item-container"), true)) {

        gpostArray = {};
        gpostArray["bo_type"] = $(".data-item-container").attr("data-type");

        $(".data-item-container").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $.ajax({
            type: 'POST',
            url: gRootUrl + "DataProcessor/Insert/",
            async: true,
            dataType: "JSON",
            data: gpostArray,
            success: function (responce) {
                if (responce["result"] == 2) {
                    window.location.reload();
                    return;
                }
                if (responce["result"] == 0) {
                    var y = ($(window).height() / 2) - 180;
                    $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                    $(".modal-warning").find(".modal-body").find("p").html("Operation Fail");
                    $(".modal-warning").fadeIn("slow");
                    return;
                }
            }
        });
    }
    return false;
}

function delete_row_item(id, name) {
    var y = ($(window).height() / 2) - 180;
    $(".modal-delete-confirmation").find(".modal-dialog").css("marginTop", y);
    $(".modal-delete-confirmation").find(".btn-outline").attr("data-id", id);
    $(".modal-delete-confirmation").find(".modal-body").html($(".modal-delete-confirmation").find(".modal-body").html().replace("{0}", name));
    $(".modal-delete-confirmation").fadeIn("slow");
    return false;
}

function delete_all_from_grid(name) {
    var y = ($(window).height() / 2) - 180;
    $(".modal-deleteall-confirmation").find(".modal-dialog").css("marginTop", y);
    $(".modal-deleteall-confirmation").find(".modal-body").html($(".modal-deleteall-confirmation").find(".modal-body").html().replace("{0}", name));
    $(".modal-deleteall-confirmation").fadeIn("slow");
    return false;
}

function do_delete_all_from_grid() {

    gpostArray = {};
    gpostArray["bo_type"] = ($("#dataGrid").size() > 0 ? $("#dataGrid").attr("data-type") : $(".data-item-container").attr("data-type"));
    gpostArray["id"] = "all";

    var redirectUrl = null;

    delete_item_generic(gpostArray, null, redirectUrl);
}

function do_delete_item(pBtn) {

    var id = $(pBtn).attr("data-id");
    gpostArray = {};
    gpostArray["bo_type"] = ($("#dataGrid").size() > 0 ? $("#dataGrid").attr("data-type") : $(".data-item-container").attr("data-type"));
    gpostArray["id"] = $(pBtn).attr("data-id");

    if ($(".data-grid-data-row-" + id).size() > 0) {
        $(".data-grid-data-row-" + id).find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });
    }
    else {
        $(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });
    }

    var redirectUrl = null;

    if ($(pBtn).attr("data-redirectUrl") != "") {
        redirectUrl = $(pBtn).attr("data-redirectUrl");
    }

    delete_item_generic(gpostArray, pBtn, redirectUrl);
}

function delete_item(id, name, redirectUrl) {
    var y = ($(window).height() / 2) - 180;
    $(".modal-delete-confirmation").find(".modal-dialog").css("marginTop", y);
    $(".modal-delete-confirmation").find(".btn-outline").attr("data-id", id);
    $(".modal-delete-confirmation").find(".btn-outline").attr("data-redirectUrl", redirectUrl);
    $(".modal-delete-confirmation").find(".modal-body").html($(".modal-delete-confirmation").find(".modal-body").html().replace("{0}", name));
    $(".modal-delete-confirmation").fadeIn("slow");
    return false;
}

function closeModalDelete() {
    $(".modal-delete-confirmation").fadeOut("slow");
    $(".modal-deleteall-confirmation").fadeOut("slow");
}

function closeModalAlert() {
    $(".modal-warning").fadeOut("slow");
}

function closeModalClone() {
    $(".modal-clone-confirmation").fadeOut("slow");
}

function delete_item_generic(gpostArray, pBtn, redirectUrl) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "DataProcessor/Delete/",
        async: true,
        dataType: "JSON",
        data: gpostArray,
        success: function (responce) {
            if (responce["result"] == 3) {
                var y = ($(window).height() / 2) - 180;
                $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                $(".modal-warning").find(".modal-body").find("p").html(responce["Message"]);
                $(".modal-warning").fadeIn("slow");
                return;
            }
            if (responce["result"] == 2) {
                window.location.reload();
                return;
            }
            if (responce["result"] == 1) {
                $(".modal-delete-confirmation").fadeOut("slow");
                $(".data-grid-data-row-" + $(pBtn).attr("data-id")).fadeOut("slow");
                if (redirectUrl != null) {
                    window.location = redirectUrl;
                    return;
                }
            }
            if (responce["result"] == 0) {
                var y = ($(window).height() / 2) - 180;
                $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                $(".modal-warning").find(".modal-body").find("p").html("Operation Fail");
                $(".modal-warning").fadeIn("slow");
                return;
            }
        }
    });
}


function copy_row_item(id, name) {
    var y = ($(window).height() / 2) - 180;
    $(".modal-clone-confirmation").find(".modal-dialog").css("marginTop", y);
    $(".modal-clone-confirmation").find(".btn-outline").attr("data-id", id);
    $(".modal-clone-confirmation").find("#name_input").val("Copy - " + name);
    $(".modal-clone-confirmation").fadeIn("slow");
    return false;
}

function copy_item(id, name) {
    var y = ($(window).height() / 2) - 180;
    $(".modal-clone-confirmation").find(".modal-dialog").css("marginTop", y);
    $(".modal-clone-confirmation").find(".btn-outline").attr("data-id", id);
    $(".modal-clone-confirmation").find("#name_input").val("Copy - " + name);
    $(".modal-clone-confirmation").fadeIn("slow");
    $(".modal-clone-confirmation").find(".btn-outline").attr("data-redirectUrl", "1");
    return false;
}

function do_clone_item(pBtn) {

    var id = $(pBtn).attr("data-id");
    gpostArray = {};
    gpostArray["bo_type"] = ($("#dataGrid").size() > 0 ? $("#dataGrid").attr("data-type") : $(".data-item-container").attr("data-type"));
    gpostArray["id"] = $(pBtn).attr("data-id");
    gpostArray["name"] = $(pBtn).closest(".modal-content").find("#name_input").val();

    var redirectUrl = null;

    if ($(pBtn).attr("data-redirectUrl") != "") {
        redirectUrl = $(pBtn).attr("data-redirectUrl");
    }

    clone_item_generic(gpostArray, pBtn, redirectUrl);
}

function clone_item_generic(gpostArray, pBtn, redirectUrl) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "DataProcessor/Clone/",
        async: true,
        dataType: "JSON",
        data: gpostArray,
        success: function (responce) {
            if (responce["Result"] == 3) {
                var y = ($(window).height() / 2) - 180;
                $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                $(".modal-warning").find(".modal-body").find("p").html(responce["Message"]);
                $(".modal-warning").fadeIn("slow");
                return;
            }
            if (responce["Result"] == 2) {
                window.location.reload();
                return;
            }
            if (responce["Result"] == 1) {
                $(".modal-clone-confirmation").fadeOut("slow");

                if (redirectUrl != null) {
                    window.location = responce["Message"];
                } else {
                    window.location.reload();
                }
            }
            if (responce["Result"] == 0) {
                var y = ($(window).height() / 2) - 180;
                $(".modal-warning").find(".modal-dialog").css("marginTop", y);
                $(".modal-warning").find(".modal-body").find("p").html("Operation Fail");
                $(".modal-warning").fadeIn("slow");
                return;
            }
        }
    });
}

function edit_item() {

    $(".btn-info").fadeOut("slow", function () {
        $(".btn-save").css("display", "inline-block");
    });

    // do not hide read only properties
    $(".control-edit").parent().find(".control-view").fadeOut("slow", function () {
        $(".control-edit").fadeIn("slow");
    });

    return false;
}

function edit_row_item(id) {

    $(".data-grid-data-row-" + id).find(".cp-action-edit").fadeOut("slow", function () {
        $(".data-grid-data-row-" + id).find(".cp-action-save").css("display", "inline-block");
    });

    // do not hide read only properties
    $(".data-grid-data-row-" + id).find(".control-edit").parent().find(".control-view").fadeOut("slow", function () {
        $(".data-grid-data-row-" + id).find(".control-edit").fadeIn("slow");
    });

    return false;
}

function do_print_class() {

    gpostArray = {};
    gpostArray["bo_type"] = $("#dataGrid").attr("data-type");

    if ($(".seach-body").size() > 0) {
        gpostArray["isFileter"] = "1";
        $(".seach-body").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });
    }

    $.ajax({
        type: 'POST',
        url: gRootUrl + "DataProcessor/Print/",
        async: true,
        data: gpostArray,
        success: function (responce) {
            if (responce == "") {
                window.location.reload();
            }
            else {
                $("#printArea").html(responce);

                var timer = setInterval(function () {
                    if ($("#printArea").find(".print-end").size() > 0) {
                        $("#printArea").printArea();
                        clearInterval(timer);
                    }
                }, 200);
            }
        }
    });
    return false;
}
function print_item(id) {

    gpostArray = {};
    gpostArray["bo_type"] = $(".data-item-container").attr("data-type");
    gpostArray["id"] = id;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "DataProcessor/Print/",
        async: true,
        data: gpostArray,
        success: function (responce) {
            if (responce == "") {
                window.location.reload();
            }
            else {
                $("#printArea").html(responce);

                var timer = setInterval(function () {
                    if ($("#printArea").find(".print-end").size() > 0) {
                        $("#printArea").printArea();
                        clearInterval(timer);
                    }
                }, 200);
            }
        }
    });
    return false;
}

function do_export_excell_class() {

    gpostArray = {};
    gpostArray["bo_type"] = $("#dataGrid").attr("data-type");

    if ($(".seach-body").size() > 0) {
        gpostArray["isFileter"] = "1";
        $(".seach-body").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });
    }

    $.fileDownload(gRootUrl + "DataProcessor/ExportExcell/", {
        httpMethod: "POST",
        data: gpostArray,
        successCallback: function (url) {
            //TBD
        },
        failCallback: function (html, url) {

            //TBD

            alert(html);

        }
    });

    return false;
}

function export_excell_item(id) {

    gpostArray = {};
    gpostArray["bo_type"] = $(".data-item-container").attr("data-type");
    gpostArray["id"] = id;

    $.fileDownload(gRootUrl + "DataProcessor/ExportExcell/", {
        httpMethod: "POST",
        data: gpostArray,
        successCallback: function (url) {
            //TBD
        },
        failCallback: function (html, url) {

            //TBD

            alert(html);

        }
    });

    return false;
}

function do_export_csv_class() {

    gpostArray = {};
    gpostArray["bo_type"] = $("#dataGrid").attr("data-type");

    if ($(".seach-body").size() > 0) {
        gpostArray["isFileter"] = "1";
        $(".seach-body").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });
    }

    $.fileDownload(gRootUrl + "DataProcessor/ExportCSV/", {
        httpMethod: "POST",
        data: gpostArray,
        successCallback: function (url) {
            //TBD
        },
        failCallback: function (html, url) {

            //TBD

            alert(html);

        }
    });

    return false;
}


function export_csv_item(id) {

    gpostArray = {};
    gpostArray["bo_type"] = $(".data-item-container").attr("data-type");
    gpostArray["id"] = id;

    $.fileDownload(gRootUrl + "DataProcessor/ExportCSV/", {
        httpMethod: "POST",
        data: gpostArray,
        successCallback: function (url) {
            //TBD
        },
        failCallback: function (html, url) {

            //TBD

            alert(html);

        }
    });

    return false;
}


function clear_autocomplete(pLink) {
    $(pLink).prev().val("0");
    $(pLink).prev().prev().val("");
    $(pLink).prev().prev().autocomplete('option', 'change').call($(pLink).prev().prev());
    return false;
}