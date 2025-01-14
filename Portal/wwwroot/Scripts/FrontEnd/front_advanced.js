var gIDNP = "";
var gIDNO = "";
var gCompanyId = "";
var gAnimalNumber = "";

var gDiagnosticControl = null;
var gHtmlPopupControl = null;

var gCkEditorConfig;
var gCkEditorConfigTabs;

$(document).ready(function () {
    //$.datepicker.setDefaults($.datepicker.regional['']);
    $(".calendar-input").each(function() {
        $currentDatepicker = $(this);
        $currentDatepicker.datepicker({
            dateFormat: "dd/mm/yy",
            changeYear: true,
            yearRange: "1900:" + $currentDatepicker.attr("data-maxyear"),
            showButtonPanel: true,
            closeText: 'Clear',
            onClose: function (dateText, inst) {
                if ($(window.event.srcElement).hasClass('ui-datepicker-close')) {
                    document.getElementById(this.id).value = '';
                }
            }
        });
    });

    if ($(".content-request").size() > 0) {
        gIDNP = $("[name=IDNP]").val();
        $("[name=IDNP]").focus()
        gIDNO = $("[name=IDNO]").val();
        gCompanyId = $("[name=Name_id]").val();
        $("[name=Name]").focus()
        window.setTimeout(function () { checkIDNP() }, 500);
    }

    if ($(".diagnostic-input").size() > 0) {
        $.ajax({
            url: gRootUrl + "Scripts/FrontEnd/lib/diagnostic_asterisk.js",
            dataType: "script",
            beforeSend: function (xhr) { xhr.overrideMimeType('text/plain; charset=UTF-16'); },
        });
        $.ajax({
            url: gRootUrl + "Scripts/FrontEnd/lib/diagnostic_all.js",
            dataType: "script",
            beforeSend: function (xhr) { xhr.overrideMimeType('text/plain; charset=UTF-16'); },
        });
        $.ajax({
          url: gRootUrl + "Scripts/FrontEnd/lib/diagnostic.js",
          dataType: "script",
          success: function (data, textStatus, jqxhr) {
            $(".diagnostic-input").toArray().map(setupDiagnosisAutocomplete);
          },
          beforeSend: function (xhr) { xhr.overrideMimeType('text/plain; charset=UTF-16'); },
        });
    }
    
    if ($(".edit-section-table-row").size() == 1) {
        $(".edit-section-table-add").css("display", "table-row");
    }

    if ($(".autocomplete-input").size() > 0) {
        $.each($(".autocomplete-input"), function (i, item) {
            load_autocomplete(item);
        });
    }

    if ($("input[name=Street]").size() > 0) {
        if (gdisrtictID != null) {
            $.getScript(gRootUrl + "JsLoader/Streets/" + gdisrtictID + "/", function (data, textStatus, jqxhr) {
                $("input[name=Street]").autocomplete({
                    source: street_arr,
                    minLength: 2,
                    open: function (e, ui) {
                        var input_container_height = $(e.target).parents(".edit-section-row").outerHeight();//$(e.target).parents(".dynamic-section").height();
                        var input_container_top_offset = $(e.target).parents(".edit-section-row").position().top;
                        var autocomplete_max_height = $(window).height() - input_container_height - input_container_top_offset;
                        $(".ui-autocomplete:visible").css({
                            "overflow-y": "auto",
                            "max-height": autocomplete_max_height + "px", "z-index": 1000,
                            "max-width": $(e.target).parents(".edit-section-row").outerWidth(),
                            "margin-left": $(e.target).parents(".edit-section-row").css("padding-left"),
                        });
                    }
                });
            });
        }
    }

    if ($(".control-multyselect").size() > 0) {
        $.each($(".control-multyselect select"), function (i, item) {
            if ($.trim($(item).html()) == "") {

                gpostArray = {};
                gpostArray["values"] = $(item).attr("data-values");

                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "MultySelect/" + $(item).attr("data-namespace"),
                    async: true,
                    data: gpostArray,
                    success: function (responce) {
                        $(item).html(responce);
                        $(item).select2();
                    }
                });
            }
            else
                $(item).select2();
        });
    }

    //$(".select-multyselect").select2();

    init_htmlpopup_link();

    update_minimu_page_height();

    update_reporst_menu_height();

});

function update_reporst_menu_height() {//asdsada
    $("#report_menu").css("marginTop", "-" + $("#report_menu").height() - 22 + "px");
}

function update_minimu_page_height() {
    var height = 0;
    height += $(".header").height();
    height += $(".content").height();
    height += $(".footer").height();
    var screenheight = $(window).height();
    $(".menu-sections").css("max-height", $(".content").height() + screenheight - height - 120);
    // $(".content").height($(".content").height() + screenheight - height + 14);
    var paddingTop = 40;
    $(".content").height(screenheight - $(".header").height() - $(".footer").height() - paddingTop);
    $(".content").css("margin-top", $(".header").height());
    $(".content").css("padding-top", paddingTop + "px");
    //$(".content").css("padding-bottom", "60px");
    if ($(".content-control").height() - 173 < screenheight) {
        $(".content-control").css("min-height", screenheight - 173);
    }
    if ($(".ajax-loading-overlay").size() > 0) {
        $(".ajax-loading-overlay").height($(".content").height());
    }
    if ($(".page-controls").size() > 0) {
        var dif = 71;//71
        $(".page-controls").css("marginTop", $(".content").height() - dif);
        $(".result-box-container").css("marginTop", $(".content").height() - dif - 81);
        $(".loading").css("marginTop", $(".content").height() - dif - 31);
    }
    if ($(".header-breadcrumbs").size() > 0) {
        if ($(".header").width() < 1100) {
            $(".header-breadcrumbs").css("marginLeft", 8);
            $(".header-breadcrumbs").width(311);
        }
    }

    $(".confirmation-overlay").height($("body").height());

}

$(window).resize(function () {
    update_minimu_page_height();
});

function filterDiagnozeNoAsterisk(diagnose) {
  var regexp = /\(.*\*.*\)/;
  if(!regexp.test(diagnose)) {
    return diagnose;
  }
}

function filterDiagnozeOnlyAsterisk(diagnose) {
  var regexp = /\(.*\*.*\)/;
  if(regexp.test(diagnose)) {
    return diagnose;
  }
}

function filterDiagnozeOnlyPlus(diagnose) {
  var regexp = /\(.*\+.*\)/;
  if(regexp.test(diagnose)) {
    return diagnose;
  }
}

function setupDiagnosisAutocomplete(diagnostic_input) {
  var filtered_diagnosis = Diagnostics;
  var filter = $(diagnostic_input).attr("data-cond");

  if(filter && filter.length) {
      switch (filter) {
        case "all":
            filtered_diagnosis = Diagnostics_All;
            break;
      case "only-asterisk":
            filtered_diagnosis = Diagnostics_Asterisk;
        break;
      case "only-plus":
          filtered_diagnosis = Diagnostics_Plus;
        break;
    }
  }

  $(diagnostic_input).autocomplete({
      source: filtered_diagnosis,
      minLength: 2,
      open: function(e, ui) {
          var input_container_height = $(e.target).parents(".edit-section-row").outerHeight();//$(e.target).parents(".dynamic-section").height();
          var input_container_top_offset = $(e.target).parents(".edit-section-row").position().top;
          var autocomplete_max_height = $(window).height() - input_container_height - input_container_top_offset;
          $(".ui-autocomplete:visible").css({
              "overflow-y": "auto",
              "max-height": autocomplete_max_height + "px", "z-index": 1000,
              "max-width": $(e.target).parents(".edit-section-row").outerWidth(),
              "margin-left": $(e.target).parents(".edit-section-row").css("padding-left"),
          });
      },
      select: function (event, ui) {
          var attr = $(this).attr('data-onchange');

          if (typeof attr !== typeof undefined && attr !== false) {
              var name = ui.item.label;
              eval(attr.replace("{1}", ui.item.label));
          }          
      }
  });
}

function outputDiagnosticsChanged(pDiagstrs){
    
    var code = pDiagstrs.substring(1, pDiagstrs.indexOf(")"));
    if (code.indexOf("+") > 0) {
        $("#diagdrg2").fadeIn("slow");
    }
    else {
        $("#diagdrg2").fadeOut("slow");
    }
}

function ToggleColumn(pControl, pModule, pColumnId) {

    if ($(pControl).is(":checked")) {
        $(".col-" + pModule + '-' + pColumnId).fadeIn("slow");
    }
    else {
        $(".col-" + pModule + '-' + pColumnId).fadeOut("slow");
        if ($("#all_fields").prop('checked')) {
            $("#all_fields").prop('checked', false);
            $("#all_fields").closest(".icheckbox_flat-blue").removeClass("checked");
            $("#all_fields").closest(".icheckbox_flat-blue").attr("aria-checked", "false");
        }
    }
    if ($(".fields-col").find("input[id^=col_]").not(":checked").length == 0) {
        $("#all_fields").prop('checked', true);
        $("#all_fields").closest(".icheckbox_flat-blue").addClass("checked");
        $("#all_fields").closest(".icheckbox_flat-blue").attr("aria-checked", "true");
    }
}

/*----------------------------------------------------:dynamic_section-----------------------------------------*/
function toggle_dynamic_section(pBlock, pNamespace, pType, bforceaddNew) {
    if (!$(pBlock).parent().hasClass("edit-section-header-expanded")) {
        load_dynamic_section(pBlock, pNamespace, pType, bforceaddNew);
    }
    else {
        $(pBlock).parent().removeClass("edit-section-header-expanded", 300);
        $(pBlock).parent().removeClass("edit-section-header-expanded-noanimation");
        $(pBlock).parent().next().slideUp("slow");
    }
    return false;
}
function load_dynamic_section(pBlock, pNamespace, pType, bforceaddNew) {
    $(pBlock).parent().addClass("edit-section-header-expanded", 300);
    $(pBlock).parent().addClass("edit-section-header-expanded-noanimation");
    $(pBlock).parent().next().fadeIn("slow");

    gpostArray = {};
    gpostArray["Namespace"] = pNamespace;
    gpostArray["ParentId"] = $("[name=Id]").val();

    eval("if (window." + pType + "_dynamic_section_array) " + pType + "_dynamic_section_array(pBlock);");

    $.ajax({
        type: 'POST',
        url: gRootUrl + "DynamicControl/Load/",
        async: true,
        data: gpostArray,
        success: function (responce) {
            $(pBlock).parent().next().html(responce);
            $(pBlock).parent().next().hide();
            $(pBlock).parent().next().slideDown("slow");
            eval("if (window." + pType + "_dynamic_section_open) " + pType + "_dynamic_section_open(pBlock,responce);");
            if ($(pBlock).parent().next().find(".edit-section-table-row ").size() == 1 || bforceaddNew) {
                $(pBlock).parent().next().find(".edit-section-table-add").css("opacity", 0);
                $(pBlock).parent().next().find(".edit-section-table-add").slideDown("slow", function () { $(pBlock).parent().next().find(".edit-section-table-add").css("display", "table-row"); });
                $(pBlock).parent().next().find(".edit-section-table-add").animate({ opacity: 1 }, "slow");
            }

        }
    });
}

function add_new_dynamic_section(pBlock, pNamespace, pType) {
    if (!$(pBlock).parent().hasClass("edit-section-header-expanded")) {
        toggle_dynamic_section(pBlock, pNamespace, pType, true);
    }
    else {
        if ($(pBlock).parent().next().find(".edit-section-table-add").css("display") == "table-row") {

            gpostArray = {};
            gpostArray["Namespace"] = pNamespace;

            eval("if (window." + pType + "_dynamic_section_array) " + pType + "_dynamic_section_array(pBlock);");

            if ($(".control_save").size() == 0) { // allow multiple rows adding 
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "DynamicControl/LoadNewItem/"+$("[name=Id]").val(),
                    async: true,
                    data: gpostArray,
                    success: function (responce) {
                        var firstRow = $(pBlock).parent().next().find(".edit-section-table-add:first");
                        firstRow.before(responce);
                        firstRow.prev().css("opacity", 0);
                        firstRow.prev().css("display", "table-row");
                        firstRow.prev().animate({ opacity: 1 }, "slow");

                        eval("if (window." + pType + "_dynamic_section_open) " + pType + "_dynamic_section_open(pBlock,responce);");
                    }
                });
            }
        }
        else {

            $(pBlock).parent().next().find(".edit-section-table-add").css("display", "table-row");
        }
        eval("if (window." + pType + "_dynamic_section_open) " + pType + "_dynamic_section_open(pBlock,null);");
    }
    return false;
}

function save_Dynamic(pBtn, parentId, Namespace) {
    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();

            gpostArray = {};
            gpostArray["Namespace"] = Namespace;
            gpostArray["ParentId"] = parentId;

            $(pBtn).closest(".edit-section-table-row").find("input").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: gpostArray,
                success: function (responce) {
                    $(pBtn).closest(".edit-section-body").html(responce);
                    dynamic_section_open(pBtn, responce);
                    $(".loading").hide();
                }
            });
        }
    }
    return false;
}

function delete_Dynamic(pBlock, pItemId, pParentId, pNamespace,bClientReload) {
    var postArray = "Id=" + pItemId + "&ParentId=" + pParentId + "&Namespace=" + pNamespace + "&ClientReload=" + (bClientReload?1:0);
    return remove_item(postArray, "Confirm delete?", pBlock, bClientReload);
}

function dynamic_section_open(pBlock, responce) {
    $.each($(pBlock).parent().next().find(".consumable-input"), function (i, item) {
        load_consumables(item);
    });
}

function check_idnp(evt, val) {
    isIntegerKey(evt);
    if (val.value.length == 13) {

        gpostArray = {};
        gpostArray["IDNP"] = val.value;

        $.ajax({
            type: 'POST',
            url: gRootUrl + "RequestHelper/LoadPatientByIDNP/",
            async: true,
            data: gpostArray,
            success: function (responce) {
                if (responce["result"] == 1) {
                    $("[name=PatientId]")[0].value = responce.data["PatientId"];
                    $("[name=FirstName]")[0].value = responce.data["FirstName"];
                    $("[name=LastName]")[0].value = responce.data["LastName"];
                    $("[name=Adress]")[0].value = responce.data["Adress"];
                    $("[name=Seria]")[0].value = responce.data["Seria"];
                    $("[name=DocumentNumber]")[0].value = responce.data["DocumentNumber"];
                    $("[name=Sex]")[0].value = responce.data["SexId"];
                    $("[name=BirthDate]")[0].value = responce.data["BirthDate"];
                    $("[name=Phone]")[0].value = responce.data["Phone"];
                    $("[name=Email]")[0].value = responce.data["Email"];
                    if (responce.data["City"] != undefined) {
                        $("[name=City]").closest(".input-row").html(responce.data["City"]);
                    } else {
                        $("[name=City]")[0].value = 0;
                    }
                    $("[name=District]")[0].value = responce.data["DistrictId"];
                    if (responce.data["Requests"] != undefined) {
                        $("[name=Requests]").html(responce.data["Requests"]);
                    }
                }
                else {
                    if (val.value[0] == '1' || val.value[0] == '2') {
                        $("[name=Sex]")[0].value = val.value[0];
                    } else if (val.value[0] == '5') {
                        $("[name=Sex]")[0].value = 1;
                    } else if (val.value[0] == '6') {
                        $("[name=Sex]")[0].value = 2;
                    }

                    if (val.value[0] == '1' || val.value[0] == '2') {
                        let birthday = val.value[5] + val.value[6] + "/" + val.value[3] + val.value[4] + "/19" + val.value[1] + val.value[2];
                        $("[name=BirthDate]")[0].value = birthday;
                    }
                    else if (val.value[0] == '5' || val.value[0] == '6') {
                        let birthday = val.value[5] + val.value[6] + "/" + val.value[3] + val.value[4] + "/20" + val.value[1] + val.value[2];
                        $("[name=BirthDate]")[0].value = birthday;
                    }
                }
            }
        });
    }
}
/*-------------------------------------------------------------------------------------------------------------*/
