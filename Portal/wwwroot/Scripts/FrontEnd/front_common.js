/*----------------------------------------------------:navigation--------------------------------------------*/
function show_global_menu(pMenuGroup) {
    $(".sub-menu-section").not($(pMenuGroup).next()).slideUp("slow", function () {
        $(pMenuGroup).next().slideDown("slow");
    });
}

/*----------------------------------------------------:edit-------------------------------------------------*/
function SavePage(id, Object, DisableDynamicControlsOnSave) {
    if (!$(".loading").is(":visible")) {
        if (form_validation($(".inner-content-area"), true)) {
            $(".loading").show();
            $(".result-box-container").fadeOut();
            $(".error-message").fadeOut();
            $(".input").removeClass("input-error", 100);

            if (DisableDynamicControlsOnSave)
                $(".inner-content-area").find(".dynamic-section").find("input").attr("disabled", true);

            $.post($(".inner-content-area").attr("action"), $(".inner-content-area").serialize(), function (data) {

                if (DisableDynamicControlsOnSave)
                    $(".inner-content-area").find(".dynamic-section").find("input").attr("disabled", false);

                eval("if (window." + Object + "_SavePage_result_process) " + Object + "_SavePage_result_process(data);");

                if (data["ErrorFields"]) {
                    $.each(data["ErrorFields"], function (i, item) {
                        $(item).addClass("input-error", "slow");
                        if ($(item).next().size() > 0 && $(item).next().hasClass("error-message")) {
                            $(item).next().find(".error-message-text").html($(item).attr("data-req-mess"));
                            $(item).next().show();
                        }
                        else {
                            $(item).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                            $(item).next().find(".error-message-text").html($(item).attr("data-req-mess"));
                        }
                    });
                }
                if (data["result"] == 2) {
                    ShowMessage(data["Message"], "success", true);
                    window.setTimeout(function () { window.location = data["redirectURL"]; }, 500);
                    return;
                } else if (data["result"] == 3) {
                    ShowMessage(data["Message"], "error", true);
                } else if (data["result"] == 0) {
                    ShowMessage(data["Message"], "error", true);
                } else {
                    ShowMessage(data["Message"], "success", true);
                }

                eval("if (window." + Object + "_SavePage_result_process_after) " + Object + "_SavePage_result_process_after(data);");
            });
        }
    }

    return false;
}

function DeletePage(id, Object) {
    if (!$(".loading").is(":visible")) {
        
        ShowConfirmMessage("Confirm delete?", function () {
            $(".loading").show();
            $(".result-box-container").fadeOut();
            $(".error-message").fadeOut();
            $.post($(".inner-content-area").attr("action").replace("/Save", "/Delete"), { __RequestVerificationToken: $("[name=__RequestVerificationToken]").val(), Id: $("[name=Id]").val(), Namespace: $("[name=Namespace]").val() }, function (data) {

                if (data["Result"] == 2) {
                    ShowMessage(data["Message"], "success",false);
                    window.setTimeout(function () { $(".loading").hide(); window.location = data["RedirectURL"]; }, 500);
                    return;
                } else if (data["Result"] == 0) {
                    ShowMessage(data["Message"], "error", true);
                } else {
                    if (data["ErrorFields"]) {
                        $.each(data["ErrorFields"], function (i, item) {
                            $("" + item).addClass("input-error", "slow");
                            if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                            }
                            else {
                                $("" + item).after("<div class='error-message'></div>");
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                            }
                        });
                    }
                    ShowMessage(data["Message"], "success", true);
                }
            });
            return true;
        });
    }
    return false;
}

function ShowMessage(Message, Type, hideLoading) {
    if (hideLoading)
        $(".loading").hide();
    if (Message)
        $(".result-box").html(Message);
    else
        $(".result-box").html("Record was strored in database!");

    $(".result-box").removeClass("result-error").addClass("result-" + Type);

    $(".result-box-container").fadeIn("slow");

    if (Type == "success") {
        window.setTimeout(function () { $(".result-box-container").fadeOut("slow"); }, 2000);
    } else if (Type == "error") {
        window.setTimeout(function () { $(".result-box-container").fadeOut("slow"); }, 2000);
    }
}

function PasswordEqual(pInut) {
    if ($(pInut).val() != $("[name=PasswordConfirm]").val()) {
        $("[name=PasswordConfirm]").addClass("input-error", "slow");
        return true;
    }
    return false;
}

function save_popUpHtml() {
    if (gHtmlPopupControl) {
        var val = $("#popUpHtml").val();
        gHtmlPopupControl.val($(val).text());
        gHtmlPopupControl.next().next().val(val);
    }
    if ($("#PopUpSaveFunction").size()>0) {
        eval($("#PopUpSaveFunction").val());
    }
    else {
        $.fancybox.close();
        gHtmlPopupControl.focus();
        gHtmlPopupControl = null;
    }
    return false;
}

function clear_popUpHtml() {
    $("#popUpHtml").val("");
    return false;
}
/*----------------------------------------------------:laborator------------------------------------------------*/
function change_laboratory(laboratorId) {
    if (!$(".loading").is(":visible")) {
        $(".loading").show();
        $.ajax({
            type: 'POST',
            url: gRootUrl + "LaboratoryHelper/UpdateLaboratory/" + laboratorId,
            success: function (responce) {
                window.location.reload();
            }
        });
    }
}
/*----------------------------------------------------:search---------------------------------------------------*/
function doSearch(report) {
    window.location = gRootUrl + "Report/"+report+"/?s=" + $("[name=global_searh]").val();
    return false;
}

function search_on_enter(input, event) {
    if (event == null)
        event = window.event;

    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {
        eval($(input).closest('.header-control').find("#search-btn").attr("onclick").replace("return ", ""));
    }
}
/*----------------------------------------------------:lang---------------------------------------------------*/
function toggleLang() {
    if ($("#lang-list").is(":visible")) {
        $("#lang-list").fadeOut("slow");
    }
    else {
        $("#lang-list").fadeIn("slow");
    }
    return false;
}

function changeLanguage(langId) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Language/Change/" + langId,
        async: true,
        success: function (responce) {
            window.location.reload();
        }
    });
    return false;
}

/*----------------------------------------------------:print---------------------------------------------------*/
function intit_print_dialog(responce) {
    $("#printArea").html(responce);

    var timer = setInterval(function () {
        if ($("#printArea").find(".print-end").size() > 0) {
            $(".loading").hide();
            $("#printArea").printArea();
            clearInterval(timer);
        }
    }, 200);
}

function print(postArray) {

    if (!$(".loading").is(":visible")) {
        $(".loading").show();
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Print/Print/",
            async: true,
            data: postArray,
            success: function (responce) {
                intit_print_dialog(responce);
            }
        });
    }

    return false;
}
function ExportToWord(postArray) {

    if (!$(".loading").is(":visible")) {
        $(".loading").show();
        $.fileDownload(gRootUrl + "Print/ExportWord/", {
            httpMethod: "POST",
            data: postArray,
            successCallback: function (url) {
                $(".loading").hide();
            },
            failCallback: function (html, url) {

                $(".loading").hide();

                ShowMessage(html, "error", true);

            }
        });

        $(".loading").hide();
    }

    return false;
}
/*-------------------------------------------------------------------------------------------------------------*/

function load_autocomplete(item) {
    if ($(item).attr("data-autocomplete-server") == "1") {
        var control = $(item).autocomplete({
            source: function (request, response) {
                this.element.parent().find(".clear-link").addClass("autocolmplete-loading");
                var lSender = this.element;
                $.ajax({
                    dataType: "json",
                    type: 'Get',
                    url: gRootUrl + "AutoComplete/" + this.element.attr("data-namespace") + "/?term=" + request.term + "&cond=" + this.element.attr("data-cond"),
                    success: function (data) {
                        lSender.parent().find(".clear-link").removeClass("autocolmplete-loading");
                        lSender.removeClass('ui-autocomplete-loading');
                        response($.map(data, function (item) {
                            item.label = latinizeDecode(item.label);
                            return item;
                        }));
                    },
                    error: function (data) {
                        lSender.parent().find(".clear-link").removeClass("autocolmplete-loading");
                    }
                });
            },
            minLength: $(item).attr("data-AutocompleteMinLen"),
            _renderItem: function (ul, item) {
                return $("<li>")
                    .append(decodeURI(item.label))
                    .appendTo(ul);
            },
            select: function (event, ui) {
                var name = $("<li></li>").html(ui.item.label).text();
                $(this).val(name);
                $(this).parent().find("input[type=hidden]").val(ui.item.value);
                if ($(this).attr("onchange") != null && $(this).attr("onchange") != "") {
                    eval($(this).attr("onchange"));
                }
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
            },
            change: function (event, ui) {
                if (ui != null && ui.item == null) {
                    $(this).parent().find("input[type=hidden]").val("");
                }
            },
            open: function (e, ui) {
                var input_container_height = $(e.target).outerHeight();//$(e.target).parents(".dynamic-section").height();
                var input_container_top_offset = $(e.target).position().top;
                if ($(e.target).parents(".edit-section-row").size() > 0) {
                    input_container_height = $(e.target).parents(".edit-section-row").outerHeight();//$(e.target).parents(".dynamic-section").height();
                    input_container_top_offset = $(e.target).parents(".edit-section-row").position().top;
                }
                var autocomplete_max_height = $(window).height() - input_container_height - input_container_top_offset;
                $(".ui-autocomplete:visible").css({
                    "overflow-y": "auto",
                    "max-height": autocomplete_max_height + "px", "z-index": 1000,
                    "max-width": $(e.target).parents(".edit-section-row").outerWidth(),
                    "margin-left": $(e.target).parents(".edit-section-row").css("padding-left"),
                });
            }
        })
        control.focus(function () {
            $(this).autocomplete("search");
        })
        control.data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li></li>").data("item.autocomplete", item)
                .append($('<div/>').html(item.label).text())
                .appendTo(ul);
        };
    }
    else if (eval("window." + $(item).attr("data-AutocompleteName")) != undefined) {
        attach_autocomplete(item);
    }
    else {
        $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-AutocompleteName") + "/" + $(item).attr("data-namespace"))
            .done(function (script, textStatus) {
                eval(script);
                attach_autocomplete(item);
            });
    }
}

function attach_autocomplete(item) {
    var control = $(item).autocomplete({
        source: function (request, response) {
            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
            response($.grep(eval($(item).attr("data-AutocompleteName")), function (item) {
                item.label = latinizeDecode(item.label);
                return matcher.test(item.label);
            }));
        },
        minLength: $(item).attr("data-AutocompleteMinLen"),
        select: function (event, ui) {
            var name = $("<li></li>").html(ui.item.label).text();
            $(this).val(name);
            $(this).parent().find("input[type=hidden]").val(ui.item.value);
            if ($(item).attr("onchange") != null && $(item).attr("onchange") != "") {
                eval($(item).attr("onchange"));
            }
            return false;
        },
        change: function (event, ui) {
            if (ui != null && ui.item == null) {
                $(this).parent().find("input[type=hidden]").val("");
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
        }
    });
    control.focus(function () {
        $(this).autocomplete("search");
    });
}