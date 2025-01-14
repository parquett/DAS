function load_procedures_link(pLink, pType) {
    var procs = undefined;

    try{
        procs = eval("Procedures_" + pType);
    }
    catch (exception_var) {

    }

    if (typeof (procs) + "" != "undefined") {

    }
    else {
        if (!$(".loading").is(":visible")) {
            $('.ajax-loading-overlay').fadeIn("slow");
            $.getScript(gRootUrl + "AutoComplete/Procedure/" + $(pLink).attr("data-namespace") + "/" + pType)
              .done(function (script, textStatus) {
                  $(pLink).attr("onclick", "");
                  $(pLink).unbind("click");
                  $(pLink).fancybox({
                      width: 704,
                      autoSize: false,
                      padding: 7,
                      afterShow: function () {
                          procedure_loaded(pType);
                          fix_save_btn_positioning();
                      }
                  });
                  $(".edit-section-body-procedures-" + pType).find(".control-edit").fancybox({
                      width: 704,
                      autoSize: false,
                      padding: 7,
                      afterShow: function () {
                          procedure_loaded(pType);
                          procedure_show_footer_content();
                          fix_save_btn_positioning();
                      }
                  });
                  $('.ajax-loading-overlay').fadeOut("slow");
                  $(pLink).trigger('click');
              });
        }
    }

    return false;
}

function procedure_loaded(pType) {
    $("[name=procedure_searh]").focus();
    $("[name=procedure_searh]").autocomplete({
        source: eval("Procedures_" + pType),
        width: 620,
        minLength: 4,
        select: function (event, ui) {
            var name = ui.item.label;
            var postArray = "CheckinID=" + $("[name=Id]").val() + "&Procedure=" + name;
            if ($("[name=ProcedureExec]").size()>0) {
                postArray += "&ProcedureExec=" + $("[name=ProcedureExec]").val();
            }
            $.ajax({
                type: 'POST',
                url: gRootUrl + "Procedure/LoadFooter/",
                async: true,
                data: postArray,
                success: function (responce) {
                    procedure_load_footer(responce);
                }
            });
        }
    });
}

function procedure_load_footer(responce) {
    if ($.trim($(".procedure-widget-footer").html) != "") {
        $(".procedure-widget-footer").fadeOut("slow", function () {
            procedure_load_footer_content(responce);
        })
    }
    else {
        $(".procedure-widget-footer").hide();
        procedure_load_footer_content(responce);
    }
}

function procedure_load_footer_content(responce) {
    $(".procedure-widget-footer").html(responce);
    procedure_show_footer_content();
}

function procedure_show_footer_content() {
    $(".popup-content-proc").show();
    $(".procedure-widget-footer").fadeIn("slow", function () {
        $(".popup-controls").fadeIn("slow");
        //$(".popup-controls").css("marginTop", $(".fancybox-inner").height() - 119 + "px");
    });
    $(".procedure-widget-footer").find(".calendar-input").datepicker({
        dateFormat: "dd/mm/yy"
    });/*
    $(".fancybox-inner").resize(function () {
        $(".popup-controls").css("marginTop", $(".fancybox-inner").height() - 119 + "px");
    });*/
}

function save_Procedure(pType) {

    if (form_validation($(".popup-content-proc"), false)) {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Procedure/Save/?type=" + pType,
            async: true,
            data: $(".popup-content-proc").find(".edit-section-body").serialize(),
            success: function (data) {
                if (data["ErrorFields"]) {
                    $.each(data["ErrorFields"], function (i, item) {
                        $(".popup-content-proc").find("" + item).addClass("input-error", "slow");
                        if ($(".popup-content-proc").find("" + item).next().size() > 0 && $(".popup-content-proc").find("" + item).next().hasClass("error-message")) {
                            $(".popup-content-proc").find("" + item).next().find(".error-message-text").html($(".popup-content-proc").find("" + item).attr("data-req-mess"));
                            $(".popup-content-proc").find("" + item).next().show();
                        }
                        else {
                            $(".popup-content-proc").find("" + item).after("<div class='error-message'></div>");
                            $(".popup-content-proc").find("" + item).next().find(".error-message-text").html($(".popup-content-proc").find("" + item).attr("data-req-mess"));
                        }
                    });
                }
                if (data["Result"] == 0) {
                    ShowAlertMessage(data["Message"], function () {
                        return true;
                    });
                }
                else {
                    $(".edit-section-body-procedures-" + pType).html(data);
                    if (!$(".edit-section-body-procedures-" + pType).is(":visible")) {
                        $(".edit-section-body-procedures-" + pType).prev().addClass("edit-section-header-expanded", 300);
                        $(".edit-section-body-procedures-" + pType).prev().addClass("edit-section-header-expanded-noanimation");
                        $(".edit-section-body-procedures-" + pType).slideDown("slow");
                    }
                    $.fancybox.close();
                }
            }
        });
    }

    return false;
}

function delete_Procedure(pBlock, pId, pcheckinId, pNamespace) {
    var postArray = "Id=" + pId + "&CheckInId=" + pcheckinId + "&Namespace=" + pNamespace;
    return remove_item(postArray, "Confirmati Anularea Procedurii?", pBlock);
}

function edit_Procedure(pLink, pId, pType) {
    var procs = undefined;

    try {
        procs = eval("Procedures_" + pType);
    }
    catch (exception_var) {

    }

    if (typeof (procs) + "" != "undefined") {

    }
    else {
        if (!$(".loading").is(":visible")) {
            $('.ajax-loading-overlay').fadeIn("slow");
            $.getScript(gRootUrl + "AutoComplete/Procedure/" + $(pLink).attr("data-namespace") + "/" + pType)
              .done(function (script, textStatus) {
                  $(".edit-section-body-procedures-" + pType).find(".control-edit").attr("onclick", "");
                  $(".edit-section-body-procedures-" + pType).find(".control-edit").unbind("click");
                  $(".edit-section-body-procedures-" + pType).find(".control-edit").fancybox({
                      width: 704,
                      autoSize: false,
                      padding: 7,
                      afterShow: function () {
                          procedure_loaded(pType);
                          procedure_show_footer_content();
                          procedure_popup_tooltipster_html_preview_inputs();
                          fix_save_btn_positioning();
                      }
                  });
                  $(".add-procedure-" + pType).fancybox({
                      width: 704,
                      autoSize: false,
                      padding: 7,
                      afterShow: function () {
                          procedure_loaded(pType);
                          fix_save_btn_positioning();
                      }
                  });
                  $('.ajax-loading-overlay').fadeOut("slow");
                  $(pLink).trigger('click');
              });
        }
    }

    return false;
}

function procedure_popup_tooltipster_html_preview_inputs() {
  var $fancybox_visible_html_preview_inputs = $(".sample-popup-container:visible .html-preview-input").not(".tooltipstered");
  if ($fancybox_visible_html_preview_inputs.length) {
    $fancybox_visible_html_preview_inputs.tooltipster({
      contentAsHTML: true
    });
  }
}

function show_protocol(purl) {
    gCkEditorConfigNoTabs = {
        toolbar:
                [
                    ["Bold", "Italic", "Underline", '-', "P", "BulletedList", "NumberedList", "Indent", "Outdent", 'FontSize', "Font"],
                    ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
                    ["JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"],
                    ["Maximize", "ShowBlocks"]
                ],
        removePlugins: 'elementspath',
        resize_enabled: false,
        allowedContent: true,
        ignoreEmptyParagraph: true,
        width: $(".header").width() < 1100 ? 907 : 954,
        height: 299
    };
    if ($("[name=Protocol]").next().next().val() != "") {
        purl += "/1";
    }
    gHtmlPopupControl = null;
    $.ajax({
        type: 'POST',
        url: gRootUrl + purl,
        data: $(".popup-content-proc").find(".edit-section-body").serialize(),
        success: function (responce) {
            $(".sample-popup-container").hide();
            $(".procedure-protocol-container").html(responce);
            $(".procedure-protocol-container").show();
            if ($("[name=Protocol]").next().next().val() != "") {
                $("#popUpHtml").val($("[name=Protocol]").next().next().val());
            }
            $("#popUpHtml").ckeditor(gCkEditorConfigNoTabs);
            $(".fancybox-wrap").width(1014);
            $(".fancybox-wrap").css("left", Number($(".fancybox-wrap").css("left").replace("px","")) -148);
            $(".fancybox-inner").width(1000);
            $(".fancybox-inner").height(550);
            $(".fancybox-wrap").addClass("htmlpopup");
            if ($(".header").width() < 1100) {
                $(".fancybox-wrap").addClass("htmlpopup-limited");
            }
        }
    });
    return false
}

function confirm_SurgeryProtocol() {
    
    ShowConfirmMessage("Confirmati schimabarea Protocolului?", function () {

        var val = $("#popUpHtml").val();
        $("[name=Protocol]").val($(val).text());
        $("[name=Protocol]").next().next().val(val);

        $(".sample-popup-container").show();
        $(".procedure-protocol-container").hide();
        $(".fancybox-wrap").removeClass("htmlpopup");
        $(".fancybox-wrap").removeClass("htmlpopup-limited");
        $.fancybox.update();
        return true;
    });

}

function print_procedure(procexecId, type, Namespace) {
    var postArray = "procexecId=" + procexecId + "&type="+type+"&Namespace=" + Namespace;
    return print(postArray);
}

function fix_save_btn_positioning() {
  var controls = $(".popup-controls-samples");
  var fancybox_window = $(".popup-controls-samples").parents(".fancybox-opened");
  if(controls.length && fancybox_window.length) {
    controls.css("top",fancybox_window.position().top+fancybox_window.height()-52);
  }
}