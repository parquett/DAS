$(document).ready(function () {    
    if ($(".consumable-input").size() > 0) {
        $.each($(".consumable-input"), function (i, item) {
            load_consumables(item);
        });
    }

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
        },
        _renderMenu: function (ul, items) {
            var that = this,
              currentCategory = "";
            $.each(items, function (index, item) {
                var li;
                if (item.category != currentCategory) {
                    ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                    currentCategory = item.category;
                }
                li = that._renderItemData(ul, item);
                if (item.category) {
                    li.attr("aria-label", item.category + " : " + item.label);
                }
            });
        }
    });
});

function load_consumables(item) {
    if (typeof (eval("window."+$(item).attr("name"))) !== 'undefined') {
        $(item).catcomplete({
            source: eval($(item).attr("name")),
            minLength: 1,
            select: function (event, ui) {
                event.preventDefault();
                var name = ui.item.label;
                $(this).val(name);
                $(this).parent().find("input[type=hidden]").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                var name = ui.item.label;
                while (name.indexOf("&quot;") > -1) {
                    name = name.replace("&quot;", '"');
                }
                $(this).val(name);
                $(this).parent().find("input[type=hidden]").val(ui.item.value);
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $(this).val('');
                    $(this).focus();
                    $(this).parent().find("input[type=hidden]").val("");
                }
            },
            open: function (e, ui) {
                var input_container_height = $(e.target).parents(".edit-section-row").outerHeight();//$(e.target).parents(".dynamic-section").height();
                var input_container_top_offset = $(e.target).parents(".edit-section-row").position() != null ? $(e.target).parents(".edit-section-row").position().top : 0;
                var autocomplete_max_height = $(window).height() - input_container_height - input_container_top_offset;
                $(".ui-autocomplete:visible").css({
                    "overflow-y": "auto",
                    "max-height": autocomplete_max_height + "px", "z-index": 1000,
                    "max-width": $(e.target).parents(".edit-section-row").outerWidth(),
                    "margin-left": "10px",
                });
            }
        });
    }
    else {
        $.getScript(gRootUrl + "AutoComplete/Consumable/" + $(item).attr("data-namespace") + ($(item).attr("data-param") ? "/" + $(item).attr("data-param") : ""))
               .done(function (script, textStatus) {
                   while (script.indexOf("&quot;") > -1) {
                       script = script.replace("&quot;", '\"');
                   }
                   eval(script);
                   $(item).catcomplete({
                       source: eval($(item).attr("name")),
                       minLength: 1,
                       select: function (event, ui) {
                           event.preventDefault();
                           var name = ui.item.label;
                           $(this).val(name);
                           $(this).parent().find("input[type=hidden]").val(ui.item.value);
                           return false;
                       },
                       focus: function (event, ui) {
                           event.preventDefault();
                           var name = ui.item.label;
                           while (name.indexOf("&quot;") > -1) {
                               name = name.replace("&quot;", '"');
                           }
                           $(this).val(name);
                           $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       },
                       change: function (event, ui) {
                           if (ui.item == null) {
                               $(this).val('');
                               $(this).focus();
                               $(this).parent().find("input[type=hidden]").val("");
                           }
                       },
                       open: function (e, ui) {
                           var input_container_height = $(e.target).parents(".edit-section-row").outerHeight();//$(e.target).parents(".dynamic-section").height();
                           var input_container_top_offset = $(e.target).parents(".edit-section-row").position() != null ? $(e.target).parents(".edit-section-row").position().top : 0;
                           var autocomplete_max_height = $(window).height() - input_container_height - input_container_top_offset;
                           $(".ui-autocomplete:visible").css({
                               "overflow-y": "auto",
                               "max-height": autocomplete_max_height + "px", "z-index": 1000,
                               "max-width": $(e.target).parents(".edit-section-row").outerWidth(),
                               "margin-left": "10px",
                           });
                       }
                   });
               });
    }
}

function DistributionBase_SavePage_result_process(data) {
    if (data["Data"]) {
        for (var i = 0; i < data["Data"]["errorcount"]; i++) {
            if (data["Data"][i]) {                
                $($("[name=Consumable]")[i]).addClass("input-error", "slow");
                $($("[name=Qty]")[i]).addClass("input-error", "slow");
                if ( $($("[name=Consumable]")[i]).next().size() > 0 &&  $($("[name=Consumable]")[i]).next().hasClass("error-message")) {
                    $($("[name=Consumable]")[i]).next().find(".error-message-text").html(data["Data"][i]);
                     $($("[name=Consumable]")[i]).next().show();
                }
                else {
                    $($("[name=Consumable]")[i]).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                    $($("[name=Consumable]")[i]).next().find(".error-message-text").html(data["Data"][i]);
                }
            }
        }
    }
}

function SampleInvestigationResult_SavePage_result_process(data) {
    if (data["Data"]) {
        for (var i = 0; i < data["Data"]["errorcount"]; i++) {
            if (data["Data"][i]) {
                $($("[name=Consumable]:visible")[i]).addClass("input-error", "slow");
                $($("[name=Qty]:visible")[i]).addClass("input-error", "slow");
                if ($($("[name=Consumable]:visible")[i]).next().size() > 0 && $($("[name=Consumable]:visible")[i]).next().hasClass("error-message")) {
                    $($("[name=Consumable]:visible")[i]).next().find(".error-message-text").html(data["Data"][i]);
                    $($("[name=Consumable]:visible")[i]).next().show();
                }
                else {
                    $($("[name=Consumable]:visible")[i]).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                    $($("[name=Consumable]:visible")[i]).next().find(".error-message-text").html(data["Data"][i]);
                }
            }
        }
    }
}

function SampleInvestigationConsumable_dynamic_section_array(pBlock) {
    gpostArray["SampleId"] = $("[name=SampleId]").val();
    gpostArray["InvestigationTypeId"] = $("[name=InvestigationTypeId]").val();
}


function ConsumableRequestConsumable_dynamic_section_open(pBlock, responce) {
    $.each($(pBlock).parent().next().find(".consumable-input"), function (i, item) {
        load_consumables(item);
    });
}

function Invoice_dynamic_section_open(pBlock, responce) {
    $.each($(pBlock).parent().next().find(".consumable-input"), function (i, item) {
        load_consumables(item);
    });
}

function SampleInvestigationConsumable_dynamic_section_open(pBlock, responce) {
    $.each($(pBlock).parent().next().find(".consumable-input"), function (i, item) {
        load_consumables(item);
    });
}