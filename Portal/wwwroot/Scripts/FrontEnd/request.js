//var gProtolCode = "";

//const { post } = require("request");

$(document).ready(function () {
    if ($(".dynamic-section-sample").size() > 0) {
        $("#dynamic_section_samples").click();
    }

    //checkAnimal();

    if ($(".producer-input").size() > 0) {
        $.each($(".producer-input"), function (i, item) {
            load_producers(item);
        });
    }

    if ($(".origin-input").size() > 0) {
        $.each($(".origin-input"), function (i, item) {
            load_origins(item);
        });
    }

    if ($(".company-input").size() > 0) {
        $.each($(".company-input"), function (i, item) {
            load_companies(item);
        });
        if ($(".company-input").val()=="")
            $(".company-input").autocomplete("search");
    }

    if ($(".packing-input").size() > 0) {
        $.each($(".packing-input"), function (i, item) {
            load_packings(item);
        });
    }

    if ($(".samplingresponsable-input").size() > 0) {
        $.each($(".samplingresponsable-input"), function (i, item) {
            load_SamplingResponsable(item);
        });
    }

    if ($(".method-input").size() > 0) {
        $.each($(".method-input"), function (i, item) {
            load_method(item);
        });
    }

    if ($(".sample-type-input").size() > 0) {
        $.each($(".sample-type-input"), function (i, item) {
            load_sample_type(item);
        });
    }

    if ($(".animalkind-input").size() > 0) {
        $.each($(".animalkind-input"), function (i, item) {
            load_animalkind(item);
        });
    }
    if ($(".animalpecies-input").size() > 0) {
        $.each($(".animalpecies-input"), function (i, item) {
            load_animalpecies(item);
        });
    }

    if ($(".investigationdefaultvalues-input").size() > 0) {
        $.each($(".investigationdefaultvalues-input"), function (i, item) {
            load_InvestigationDefaultValues(item);
        });
    }

    if ($(".investigation-responsable-input").size() > 0) {
            $.each($(".investigation-responsable-input"), function (i, item) {
                load_InvestigationResponsables(item);
        });
    }


    if ($(".select-examinations").size() > 0) {
        $(".select-examinations").select2();
    }
});

function load_companies(item) {
    $(item).autocomplete({
        source: Company,
        minLength: 3,
        select: function (event, ui) {
            var name = ui.item.label;
            $(this).val(name);
            $(this).parent().find("input[type=hidden]").val(ui.item.value);
            if (gCompanyId != ui.item.value) {
                gCompanyId = ui.item.value;
                findIDNP();
            }
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
        },
        change: function (event, ui) {
            if (ui == null || ui.item == null) {
                $(this).parent().find("input[type=hidden]").val("");
                if (gCompanyId != "") {
                    gCompanyId = "";
                    findIDNP();
                }
            }
        }
    }).focus(function () {
        $(this).autocomplete("search");
    });
}

function load_gosts(item) {
    if (typeof GOST == 'undefined') {
        $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=GOST&cond=" + $(item).attr("data-cond"))
               .done(function (script, textStatus) {
                   while (script.indexOf("&quot;") > -1) {
                       script = script.replace("&quot;", '\"');
                   }
                   while (script.indexOf("&gt;") > -1) {
                       script = script.replace("&gt;", '>');
                   }
                   while (script.indexOf("&lt;") > -1) {
                       script = script.replace("&lt;", '<');
                   }
                   while (script.indexOf("&#171;") > -1) {
                       script = script.replace("&#171;", '«');
                   }
                   while (script.indexOf("&#187;") > -1) {
                       script = script.replace("&#187;", '»');
                   }
                   eval(latinizeDecode(script));
                   $(item).autocomplete({
                       source: eval($(item).attr("name")),
                       minLength: 0,
                       select: function (event, ui) {
                           var name = ui.item.label;
                           $(this).val(name);
                           $(this).parent().find("input[type=hidden]").val(ui.item.value);
                           return false;
                       },
                       focus: function (event, ui) {
                           event.preventDefault();
                       },
                       change: function (event, ui) {
                           if (ui == null || ui.item == null) {
                               $(this).parent().find("input[type=hidden]").val("");
                           }
                       }
                   }).focus(function () {
                       $(this).autocomplete("search");
                   });
               });
    }
    else {
        $(item).autocomplete({
            source: GOST,
            minLength: 0,
            select: function (event, ui) {
                var name = ui.item.label;
                $(this).val(name);
                $(this).parent().find("input[type=hidden]").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
            },
            change: function (event, ui) {
                if (ui == null || ui.item == null) {
                    $(this).parent().find("input[type=hidden]").val("");
                }
            }
        }).focus(function () {
            $(this).autocomplete("search");
        });
    }
}

function load_InvestigationDefaultValues(item) {
    if (typeof InvestigationDefaultValue == 'undefined') {
        $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
               .done(function (script, textStatus) {
                   while (script.indexOf("&quot;") > -1) {
                       script = script.replace("&quot;", '\"');
                   }
                   while (script.indexOf("&gt;") > -1) {
                       script = script.replace("&gt;", '>');
                   }
                   while (script.indexOf("&lt;") > -1) {
                       script = script.replace("&lt;", '<');
                   }
                   while (script.indexOf("\\u003c") > -1) {
                       script = script.replace("\\u003c", '<');
                   }
                   eval(latinizeDecode(script));
                   $(item).autocomplete({
                       source: eval($(item).attr("name")),
                       minLength: 0,
                       select: function (event, ui) {
                           var name = ui.item.label;
                           $(this).val(name);
                           $(this).parent().find("input[type=hidden]").val(ui.item.value);
                           return false;
                       },
                       focus: function (event, ui) {
                           event.preventDefault();
                       },
                       change: function (event, ui) {
                           if (ui == null || ui.item == null) {
                               $(this).parent().find("input[type=hidden]").val("");
                           }
                       }
                   }).focus(function () {
                       $(this).autocomplete("search");
                   });
               });
    }
    else {
        $(item).autocomplete({
            source: InvestigationDefaultValue,
            minLength: 0,
            select: function (event, ui) {
                var name = ui.item.label;
                $(this).val(name);
                $(this).parent().find("input[type=hidden]").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
            },
            change: function (event, ui) {
                if (ui == null || ui.item == null) {
                    $(this).parent().find("input[type=hidden]").val("");
                }
            }
        }).focus(function () {
            $(this).autocomplete("search");
        });
    }
}

function load_origins(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function load_InvestigationResponsables(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name") ),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function load_InvestigationAdditionalParameters(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function load_SamplingResponsable(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function load_method(item) {
    var InvestigationId = $("#" + $(item).attr("name").replace("_method", "_invId")).val();
    var lModel = "";

    if (typeof InvestigationId === 'undefined')
        lModel = "InvestigationMethod";
    else
        lModel = "InvestigationMethod_" + InvestigationId;

    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + lModel+"&cond=" + InvestigationId)
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval(lModel),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function load_packings(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

var gProducer = 0;
function load_producers(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       if (gProducer != ui.item.value) {
                           gProducer = ui.item.value;
                           LoadProducerAddress();
                       }
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                           if (gProducer != "") {
                               gProducer = "";
                               LoadProducerAddress();
                           }
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function LoadProducerAddress() {
    var postArray = "producerId=" + gProducer;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/LoadProducerAddress/",
        async: true,
        dataType: "JSON",
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {                
                $("[name=ProducerAddress]").val(responce["Message"]);
            }
        },
        error: function () {
            //TBD
        },
    });
}

function load_animalkind(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                       }
                   }
               }).focus(function () {
               $(this).autocomplete("search");
           });
           });
}

function load_animalpecies(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name") +"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));
               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       var prevVal = $(this).parent().find("input[type=hidden]").val();
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       if (prevVal != ui.item.value)
                            load_animalkind($(".animalkind-input"));
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui == null || ui.item == null) {
                           var prevVal = $(this).parent().find("input[type=hidden]").val();
                           $(this).parent().find("input[type=hidden]").val("");
                           if (prevVal != ui.item.value)
                                load_animalkind($(".animalkind-input"));
                       }
                   }
               }).focus(function () {
               $(this).autocomplete("search");
                });
           });
}

function load_sample_type(item) {
    $.getScript(gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?model=" + $(item).attr("name")+"&cond=" + $(item).attr("data-cond"))
           .done(function (script, textStatus) {
               while (script.indexOf("&quot;") > -1) {
                   script = script.replace("&quot;", '\"');
               }
               eval(latinizeDecode(script));

               $(item).autocomplete({
                   source: eval($(item).attr("name")),
                   minLength: 0,
                   select: function (event, ui) {
                       var name = ui.item.label;
                       $(this).val(name);
                       $(this).parent().find("input[type=hidden]").val(ui.item.value);
                       reloadInvestidationResultsChoise($("#sampleId").val());
                       return false;
                   },
                   focus: function (event, ui) {
                       event.preventDefault();
                   },
                   change: function (event, ui) {
                       if (ui==null || ui.item == null) {
                           $(this).parent().find("input[type=hidden]").val("");
                           reloadInvestidationResultsChoise($("#sampleId").val());
                       }
                   }
               }).focus(function () {
                   $(this).autocomplete("search");
               });
           });
}

function Reload_Street_Options() {
    if ($("[name=Districts]").val() != null && $("[name=Districts]").val() != "") {
        $.getScript(gRootUrl + "JsLoader/Streets/" + $("[name=Districts]").val() + "/", function (data, textStatus, jqxhr) {
            $("input[name=Street]").autocomplete({
                source: street_arr,
                minLength: 2
            });
        });
// TBD filter Towns
    }
}

function toggle_Requester_Details(pLink) {
    var show = ($(pLink).attr('data-state')=="0");
 
    if (show) {
        $(pLink).attr('data-state', '1');
        $(pLink).html("mbyll");

        if ($("[name=RequestType_jur]").is(":checked")) {
            $(".edit-section-row-details:not(.edit-section-row-phiz)").fadeIn("slow");
        }

        if ($("[name=RequestType_phiz]").is(":checked")) {
            $(".edit-section-row-details:not(.edit-section-row-jur)").fadeIn("slow");
        }
    }
    else {
        $(".edit-section-row-details").fadeOut("slow");
        $(pLink).attr('data-state', '0');
        $(pLink).html("details");
    }
}

function change_request_type(pInput) {
    if (!$("[name=RequestType_phiz]").is(":checked") && $(pInput).attr("name") == "RequestType_phiz") {
        $("[name=RequestType_phiz]").prop("checked", true);
        return;
    }

    if (!$("[name=RequestType_jur]").is(":checked") && $(pInput).attr("name") == "RequestType_jur") {
        $("[name=RequestType_jur]").prop("checked", true);
        return;
    }

    if ($(pInput).attr("name") == "RequestType_jur") {

        $("[name=RequestType_phiz]").prop("checked", false);
        $(".edit-section-row-phiz").fadeOut("slow", function () {
            $(".edit-section-row-jur").fadeIn("slow");
        });
    }

    if ($(pInput).attr("name") == "RequestType_phiz") {

        $("[name=RequestType_jur]").prop("checked", false);
        $(".edit-section-row-jur").fadeOut("slow", function () {
            $(".edit-section-row-phiz").fadeIn("slow");
        });
    }
}

function checkAnimal() {
    if ($("[name=EarTag]").size() > 0 && gAnimalNumber != $("[name=EarTag]").val() && $("[name=EarTag]").val().length >= 10 && $("[name=EarTag]").val().length <= 15) {
        gAnimalNumber = $("[name=EarTag]").val();
        findAnimal();
    }
    window.setTimeout(function () { checkAnimal() }, 1000);
}

function findAnimal() {

    var postArray = "";

    postArray = "sAnimalNumber=" + gAnimalNumber;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Animal/LoadAnimal/",
        async: true,
        dataType: "JSON",
        timeout: 4000,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {

                $(".edit-section-samples").hide();
                $(".edit-section-samples").html(responce["Data"]["Samples"]);
                $(".edit-section-samples").slideDown("slow");

                $("[name=AnimalName]").val(responce["Data"]["Name"]);
                $("[name=Species]").val(responce["Data"]["Species"]);
                $("[name=Species_id]").val(responce["Data"]["Species_id"]);
                $("[name=Kind]").val(responce["Data"]["Kind"]);
                $("[name=Kind_id]").val(responce["Data"]["Kind_id"]);
                $("[name=Sex]").val(responce["Data"]["Sex"]);
                $("[name=Age]").val(responce["Data"]["Age"]);
                $("[name=DeathDate]").val(responce["Data"]["DeathDate"]);

                load_animalkind($(".animalkind-input"));
            }
        },
        error: function () {
            //TBD
        },
    });
}

function checkIDNP() {
    if ($("[name=RequestType_phiz]").is(":checked")) {
        if (gIDNP != $("[name=IDNP]").val() && $("[name=IDNP]").val().length == 13) {
            gIDNP = $("[name=IDNP]").val();
            findIDNP();
        }
    }
    else {
        if (gIDNO != $("[name=IDNO]").val() && $("[name=IDNO]").val().length == 13 && gCompanyId=="") {
            gIDNO = $("[name=IDNO]").val();
            findIDNP();
        }
    }
    window.setTimeout(function () { checkIDNP() }, 500);
}

var gObjectType = 0;

// 1 - request
// 2 - protocol

function findIDNP() {

    var postArray ="";
       
    if ($("[name=RequestType_phiz]").is(":checked")) {
        postArray = "sIDNP=" + $("[name=IDNP]").val();
    }
    else {
        if ($("[name=Name_id]").val() != "") {
            postArray = "sID=" + $("[name=Name_id]").val();
        }
        else {
            postArray = "sIDNO=" + $("[name=IDNO]").val();
        }
    }
    postArray += "&ObjectType=" + gObjectType;
    $.ajax({
        type: 'POST',
        url: gRootUrl + "PersonHelper/LoadInfo/",
        async: true,
        dataType: "JSON",
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                if (gObjectType == 1) {
                    $(".edit-section-requests").hide();
                    $(".edit-section-requests").html(responce["Data"]["Requests"]);
                    $(".edit-section-requests").slideDown("slow");
                }
                
                if (gObjectType == 2) {
                    $(".edit-section-header-recent-protocols").hide();
                    $(".recent-protocols").hide();
                    $(".recent-protocols").html(responce["Data"]["Protocols"]);
                    if (responce["Data"]["Protocols"]!=null && responce["Data"]["Protocols"] != "") {
                        $(".recent-protocols").slideDown("slow");
                        $(".edit-section-header-recent-protocols").slideDown("slow");
                    }
                }

                $("[name=CompanyId]").val(responce["Data"]["CompanyId"]);
                $("[name=IDNO]").val(responce["Data"]["IDNO"]);
                gIDNO = responce["Data"]["IDNO"];
                $("[name=Name]").val(responce["Data"]["CompanyName"]);
                $("[name=FirstName]").val(responce["Data"]["FirstName"]);
                $("[name=LastName]").val(responce["Data"]["LastName"]);
                $("[name=RequesterId]").val(responce["Data"]["RequesterId"]);

                $("[name=Districts_id]").val(responce["Data"]["DistrictsId"]);
                $("[name=Districts]").val(responce["Data"]["DistrictsName"]);

                $("[name=Street]").val(responce["Data"]["Street"]);
                $("[name=StreetNumber]").val(responce["Data"]["StreetNumber"]);
                $("[name=Office]").val(responce["Data"]["Office"]);
                $("[name=CompanyPhone]").val(responce["Data"]["Phone"]);
                $("[name=RequesterPhone]").val(responce["Data"]["Phone"]);
                $("[name=Fax]").val(responce["Data"]["Fax"]);
                $("[name=Email]").val(responce["Data"]["Email"]);
                $("[name=Apartment]").val(responce["Data"]["Apartment"]);
                $("[name=Towns_id]").val(responce["Data"]["TownsId"]);
                $("[name=Towns]").val(responce["Data"]["TownsName"]);
                
                if ($("[name=Districts]").val() != null && $("[name=Districts]").val() != "") {
                    $.getScript(gRootUrl + "JsLoader/Streets/" + $("[name=Districts]").val() + "/", function (data, textStatus, jqxhr) {
                        $("input[name=Street]").autocomplete({
                            source: street_arr,
                            minLength: 2
                        });
                    });
                }
            }
        },
        error: function () {
            //TBD
        },
    });
}

function ChangeSamplingType(pSelect) {

    var lSamplingTypeID = $(pSelect).val();
    if (gSamplingTypeID != lSamplingTypeID) {
        gSamplingTypeID = lSamplingTypeID;
        var postArray = "SamplingTypeID=" + lSamplingTypeID;
        $.ajax({
            type: 'POST',
            url: gRootUrl + "ProtocolHelper/LoadSamplingType/",
            async: true,
            dataType: "JSON",
            data: postArray,
            success: function (responce) {
                if (responce["Result"] == 1) {
                    var lBudget = responce["Message"];
                    if (gBudget != lBudget) {
                        gBudget = lBudget;
                        LoadPlan();
                    }
                }
            },
            error: function () {
                //TBD
            },
        });
    }
}

function ChangeCountrySampleCode(pInput) {

    var lCountryID = $(pInput).parent().find("input[type=hidden]").val();
    if (gCountryID != lCountryID) {
        gCountryID = lCountryID;
        LoadPlan();
    }
}

function ChangeSampleCode(pInput) {

    var lSampleCodeID = $(pInput).parent().find("input[type=hidden]").val();
    if (gSampleCodeID != lSampleCodeID) {
        gSampleCodeID = lSampleCodeID;
        LoadPlan();
    }
}


function LoadPlan() {
    var IsFito = ($("[name=ProtocolType]").val()=="4");
    if (gCountryID != 0 && gSampleCodeID != 0 && gBudget == "1" && $("[name=CustomsPost]").val() != "") {
        var postArray = "CountryID=" + gCountryID + "&SampleCodeID=" + gSampleCodeID + "&CustomsPostID=" + $("[name=CustomsPost]").val() + "&Date=" + $.datepicker.formatDate('ddmmyy', StrToDate($("[name=SamplingDate]").val())) + "&IsFito=" + (IsFito?"1":"0");
        $.ajax({
            type: 'POST',
            url: gRootUrl + "ProtocolHelper/LoadPlan/",
            async: true,
            dataType: "JSON",
            timeout: 4000,
            data: postArray,
            success: function (responce) {
                if (responce["Result"] == 1) {
                    $(".current-plan").hide();
                    if (responce["Data"]["Plan"] != null) {
                        $(".current-plan").html(responce["Data"]["Plan"]);
                        $(".current-plan").slideDown("slow");
                    }
                    else {
                        $(".current-plan").html("");
                    }
                    $(".select-examinations").html(responce["Data"]["Examinations"]);
                    $(".select-examinations").select2();
                }
            },
            error: function () {
                //TBD
            },
        });
    }
    else if (gBudget == "0") {
        var postArray ="IsFito=" + (IsFito ? "1" : "0");
        $.ajax({
            type: 'POST',
            url: gRootUrl + "ProtocolHelper/LoadExaminations/",
            async: true,
            dataType: "JSON",
            timeout: 4000,
            data: postArray,
            success: function (responce) {
                if (responce["Result"] == 1) {
                    $(".current-plan").hide();
                    $(".current-plan").html("");
                    $(".select-examinations").html(responce["Data"]["Examinations"]);
                    $(".select-examinations").select2();
                }
            },
            error: function () {
                //TBD
            },
        });
    }
}

function RequestDocument_dynamic_section_array(pBlock) {
    gpostArray["RequestId"] = $("[name=Id]").val();
}

function Transfer_dynamic_section_array(pBlock) {
    gpostArray["RequestId"] = $("[name=Id]").val();
}

function SampleDocument_dynamic_section_array(pBlock) {
    gpostArray["SampleId"] = $("[name=SampleId]").val();
    gpostArray["InvestigationTypeId"] = $("[name=InvestigationTypeId]").val();
}

function Sample_dynamic_section_array(pBlock) {
    gpostArray["RequestId"] = $("[name=Id]").val();
}

function Transfer_dynamic_section_open(pBlock, responce) {
    var minDate = $.datepicker.parseDate('dd/mm/yy', $(pBlock).parent().next().find("#lastTransferDate").size() > 0 ? $(pBlock).parent().next().find("#lastTransferDate").val() : $("[name=SampleRegisterDate]").val());
    $(pBlock).parent().next().find(".calendar-input").datepicker({
        dateFormat: "dd/mm/yy",
        minDate: minDate,
        placeholder: 'Select Type'
    });
}

function Sample_dynamic_section_open(pBlock, responce) {

    if ($(pBlock).parent().next().find(".autocomplete-input").size() > 0) {
        $.each($(pBlock).parent().next().find(".autocomplete-input"), function (i, item) {
            load_autocomplete(item);
        });
    }

    $.each($(pBlock).parent().next().find(".producer-input"), function (i, item) {
        load_producers(item);
    });

    $.each($(pBlock).parent().next().find(".origin-input"), function (i, item) {
        load_origins(item);
    });

    $.each($(pBlock).parent().next().find(".packing-input"), function (i, item) {
        load_packings(item);
    });
    
    $.each($(pBlock).parent().next().find(".sample-type-input"), function (i, item) {
        load_sample_type(item);
    });

    $.each($(pBlock).parent().next().find(".samplingresponsable-input"), function (i, item) {
        load_SamplingResponsable(item);
    });

    $.each($(pBlock).parent().next().find(".animalkind-input"), function (i, item) {
        load_animalkind(item);
    });

    $.each($(pBlock).parent().next().find(".animalpecies-input"), function (i, item) {
        load_animalpecies(item);
    });

    $(".select-investigation-types").select2({
        templateSelection: formatInestigationType,
        templateselectionContainer: formatInestigationTypeStatus,
        placeholder: 'Select Type'
    });
}

function ProtocolSample_dynamic_section_open(pBlock, responce) {
    if ($(pBlock).parent().next().find(".autocomplete-input").size() > 0) {
        $.each($(pBlock).parent().next().find(".autocomplete-input"), function (i, item) {
            load_autocomplete(item);
        });
    }

    $.each($(pBlock).parent().next().find(".producer-input"), function (i, item) {
        load_producers(item);
    });

    $.each($(pBlock).parent().next().find(".origin-input"), function (i, item) {
        load_origins(item);
    });

    $.each($(pBlock).parent().next().find(".packing-input"), function (i, item) {
        load_packings(item);
    });

    $.each($(pBlock).parent().next().find(".sample-type-input"), function (i, item) {
        load_sample_type(item);
    });

    $.each($(pBlock).parent().next().find(".samplingresponsable-input"), function (i, item) {
        load_SamplingResponsable(item);
    });

    $.each($(pBlock).parent().next().find(".animalkind-input"), function (i, item) {
        load_animalkind(item);
    });

    $.each($(pBlock).parent().next().find(".animalpecies-input"), function (i, item) {
        load_animalpecies(item);
    });

    $(".select-investigation-types").select2({
        templateSelection: formatInestigationType,
        templateselectionContainer: formatInestigationTypeStatus
    });
}
function formatInestigationType(InestigationType) {

    sample_nr = $(InestigationType.element).closest(".edit-section-table-row").find("#sample_nr").val();
    var sample_id =$("#sample-row-" + sample_nr).find("#sample_id").val();
    if (!InestigationType.id) { return InestigationType.text; }
    if (sample_id == 0) { return InestigationType.text; }
    var $state = $(
      "<A onclick='return InestigationTypeLink(\"" + gRootUrl + "LabControl/Sample/" + sample_id + "/" + InestigationType.id + "\",event);' class='multy-item-clickable' href='#'>" + InestigationType.text + "</A>"
    );
    return $state;
};

function formatInestigationTypeStatus(InestigationType) {
    sample_nr = $(InestigationType.element).closest(".edit-section-table-row").find("#sample_nr").val();
    var sample_id = $("#sample-row-" + sample_nr).find("#sample_id").val();
    var htmlcontainer =
      '<li class="select2-selection__choice multy-select-state-' + $(InestigationType.element).attr("data-status") + '">' +
        '<span class="select2-selection__choice__remove" role="presentation">' +
          '&times;' +
        '</span>' +
      '</li>';
    return htmlcontainer;
};

function InestigationTypeLink(url, event) {
    window.open(url);
    return clickCancel(event);
}

function change_InvestigationType(pSelect) {
    if ($(pSelect).val() != "") {
        $(pSelect).closest(".edit-section-table-row").find("[id*='investigation_type_details_']").prop("disabled", false);
        $(pSelect).closest(".edit-section-table-row").find("[id*='investigation_type_details_']").removeClass("link-disabled");
        var values = $(pSelect).val() + "";
        if (values.indexOf(",") == -1) {
            $(pSelect).closest(".edit-section-table-row").find("[id*='investigation_type_details_']").click();
        }
    }
    else {
        $(pSelect).closest(".edit-section-table-row").find("[id*='investigation_type_details_']").prop("disabled", true);
        $(pSelect).closest(".edit-section-table-row").find("[id*='investigation_type_details_']").addClass("link-disabled");
    }
}

function showInvestigationTypePopup(pLink, pSampleId) {
    if (form_validation($(pLink).closest(".edit-section-table-row"), false)) {
        if ($(pLink).hasClass("link-disabled"))
            return false;

        if ($(pLink).closest(".edit-section-table-row").find("[name=InvestigationType]").val() == null)
            return false;

        var SampleNameId = $(pLink).closest(".edit-section-table-row").find("[name=SampleName_id]").val();

        if (SampleNameId == "")
            SampleNameId = "0";

        $.fancybox.open([
            {
                width: 970,
                height: 550,
                autoSize: false,
                padding: 7,
                title: $(pLink).closest(".edit-section-table-row").find("[name=SampleName]").val(),
                wrapCSS: "sample-popup",
                helpers: {
                    overlay: {
                        speedIn: 0,
                        speedOut: 300,
                        opacity: 0.8,
                        css: {
                            cursor: 'default'
                        },
                        closeClick: false
                    }
                },
                afterShow: function () {
                    if ($("[name=EarTag]").size() > 0) {
                        gAnimalNumber = $("[name=EarTag]").val();
                    }

                    if ($(".producer-input").size() > 0) {
                        $.each($(".producer-input"), function (i, item) {
                            load_producers(item);
                        });
                    }

                    if ($(".origin-input").size() > 0) {
                        $.each($(".origin-input"), function (i, item) {
                            load_origins(item);
                        });
                    }

                    if ($(".packing-input").size() > 0) {
                        $.each($(".packing-input"), function (i, item) {
                            load_packings(item);
                        });
                    }

                    if ($(".sample-type-input").size() > 0) {
                        $.each($(".sample-type-input"), function (i, item) {
                            load_sample_type(item);
                        });
                    }

                    if ($(".samplingresponsable-input").size() > 0) {
                        $.each($(".samplingresponsable-input"), function (i, item) {
                            load_SamplingResponsable(item);
                        });
                    }

                    if ($(".animalkind-input").size() > 0) {
                        $.each($(".animalkind-input"), function (i, item) {
                            load_animalkind(item);
                        });
                    }

                    if ($(".animalpecies-input").size() > 0) {
                        $.each($(".animalpecies-input"), function (i, item) {
                            load_animalpecies(item);
                        });
                    }

                    $(".calendar-input").datepicker({
                        dateFormat: "dd/mm/yy",
                        changeYear: true,
                        yearRange: "1900:" + (new Date()).getFullYear()
                    });


                    if ($(".gost-input").size() > 0) {
                        $.each($(".gost-input"), function (i, item) {
                            load_gosts(item);
                        });
                    }

                    if ($(".popup-content-proc").find(".autocomplete-input").size() > 0) {
                        $.each($(".popup-content-proc").find(".autocomplete-input"), function (i, item) {
                            load_autocomplete(item);
                        });
                    }

                    $(".select-multyselect").select2();

                    fix_save_btn_positioning();
                },
                type: 'ajax',
                href: gRootUrl + "Sample/PopUpLoad/" + $("[name=Id]").val() + "/" + pSampleId + "/" + $(pLink).closest(".edit-section-table-row").find("[name=InvestigationType]").val() + "/" + $(pLink).closest(".edit-section-table-row").find("#sample_nr").val() + "/" + SampleNameId
            }
        ]);
    }
    return false;
}

function fix_save_btn_positioning() {
    var controls = $(".popup-controls-samples");
    var fancybox_window = $(".popup-controls-samples").parents(".fancybox-opened");
    if (controls.length && fancybox_window.length) {
        controls.css("top", fancybox_window.position().top + fancybox_window.height() - 52);
    }
}

function save_sample_details(sample_nr) {

    if (form_validation($("#sample_form_" + sample_nr), true) && inv_validation()) {

        if (gNeedSampleAddConfirmation) {
            ShowConfirmMessage("Confirm the Sample Registration? <i>(Completion is added New Request)</i><br/><input type='checkbox' value='1' id='printreport' " + (sample_nr == 0 ? "checked" : "") + "><label for='printreport'>Print the TEST RESULT<label>", function () {

                do_prepare_save_sample_details(sample_nr);

                var Namespace = 'SecurityCRM.Models.Objects.Sample';

                save_Sample($("#sample-row-" + sample_nr).find(".control_save")[0], $("[name=Id]").val(), Namespace, function () {
                    post_save_Sample(sample_nr, Namespace, "new");
                });

                return true;
            }, function () {
                return true;
            }, "Yes", "Cancel", "Finalizing", function () {

                do_prepare_save_sample_details(sample_nr);

                var Namespace = 'SecurityCRM.Models.Objects.Sample';

                save_Sample($("#sample-row-" + sample_nr).find(".control_save")[0], $("[name=Id]").val(), Namespace, function () {
                    post_save_Sample(sample_nr, Namespace, "reload");
                });

                return true;
            });
        }
        else {
            do_prepare_save_sample_details(sample_nr);

            var Namespace = 'SecurityCRM.Models.Objects.Sample';

            save_Sample($("#sample-row-" + sample_nr).find(".control_save")[0], $("[name=Id]").val(), Namespace, function () {
                post_save_Sample(sample_nr, Namespace, "");
            });
        }
    }
    return false;
}


function do_prepare_save_sample_details(sample_nr) {

    $("#sample_form_" + sample_nr).find("input:not(:checkbox)").each(function (i) {
        eval("gpostArray_" + sample_nr + "['" + $(this).attr("name") + "']='" + $(this).val() + "'");
    });

    $("#sample_form_" + sample_nr).find('input:checked').each(function () {
        eval("gpostArray_" + sample_nr + "['" + $(this).attr("name") + "']='" + $(this).val() + "'");
    });

    $("#sample_form_" + sample_nr).find("select").each(function (i) {
        eval("gpostArray_" + sample_nr + "['" + $(this).attr("name") + "']='" + $(this).val() + "'");
    });

    $("#sample_form_" + sample_nr).find("textarea").each(function (i) {
        eval("gpostArray_" + sample_nr + "['" + $(this).attr("name") + "']='" + $(this).val() + "'");
    });

    $.fancybox.close();
}


function post_save_Sample(sample_nr, Namespace,action) {
    var sampleId = 0;
    if (sample_nr == 0) {
        sampleId = $("#NewItemId").val();
    }
    else {
        sampleId = $("#sample-row-" + sample_nr).find("#sample_id").val();
    }

    $(".loading").hide();
    if ($("#printreport").is(":checked")) {
        print_request_document_for_section(sampleId, Namespace);
    }

    if (action=="reload") {
        window.location = gRootUrl + "LabControl/Request";
    }
    else if (action == "new") {
        $("#addSample").click();
    }
}

function print_request_document_for_section(sampleId, Namespace) {
    var type = "fisa_de_insosire_pentru_incercari";
    var postArray = "SampleID=" + sampleId + "&type=" + type + "&Namespace=" + Namespace;
    postArray += "&PrintResults=1";
    print(postArray);
}

function inv_validation() {
    var count = 0;
    $("#InvestidationResultsChoise").find(".caption").find("a").removeClass("input-error");
    $("#InvestidationResultsChoise").find(".caption").removeClass("input-error");
    $('[name=InvestigationTypePopUP] :selected').each(function (i, selected) {
        if ($("#investigations_list_" + $(selected).val()).find(".checkbox:checked").size() == 0) {
            count++;
            var section = $("#investigations_list_" + $(selected).val()).prev().find(".caption").find("a").size() > 0 ? $("#investigations_list_" + $(selected).val()).prev().find(".caption").find("a") : $("#investigations_list_" + $(selected).val()).prev().find(".caption");
            section.addClass("input-error", "slow");
            if (section.next().size() > 0 && section.next().hasClass("error-message")) {
                section.next().find(".error-message-text").html("Select Tests");
                section.next().show();
            }
            else {
                section.after("<div class='error-message'><div class='error-message-text'></div></div>");
                section.next().find(".error-message-text").html("Select Tests");
            }
        }
    });

    return count==0;
}

function save_ProtocolSample(pBtn, protocolId, Namespace) {

    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();
            sample_nr = $(pBtn).closest(".edit-section-table-row").find("#sample_nr").val()
            eval("gpostArray_" + sample_nr + "['ProtocolId']=" + protocolId);
            eval("gpostArray_" + sample_nr + "['ProtocolSampleId']=" + $("#sample-row-" + sample_nr).find("#sample_id").val());
            eval("gpostArray_" + sample_nr + "['Namespace']='" + Namespace + "'");
            if ($("#sample-row-" + sample_nr).find("[name=SampleName]").size() > 0) {
                var SampleName = $("#sample-row-" + sample_nr).find("[name=SampleName]").val();

                while (SampleName.indexOf("''") > -1) {
                    SampleName = SampleName.replace("''", '"');
                }
                while (SampleName.indexOf("'") > -1) {
                    SampleName = SampleName.replace("'", '"');
                }
                eval("gpostArray_" + sample_nr + "['SampleName']='" + SampleName + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=SampleName_id]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['SampleNameId']='" + $("#sample-row-" + sample_nr).find("[name=SampleName_id]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Quantity]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Quantity']='" + $("#sample-row-" + sample_nr).find("[name=Quantity]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Unit]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Unit']='" + $("#sample-row-" + sample_nr).find("[name=Unit]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Lot]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Lot']='" + $("#sample-row-" + sample_nr).find("[name=Lot]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=LotUnit]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['LotUnit']='" + $("#sample-row-" + sample_nr).find("[name=LotUnit]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=SealNr]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['SealNr']='" + $("#sample-row-" + sample_nr).find("[name=SealNr]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=ControlQuantity]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['ControlQuantity']='" + $("#sample-row-" + sample_nr).find("[name=ControlQuantity]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=DateProduction]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['DateProduction']='" + $("#sample-row-" + sample_nr).find("[name=DateProduction]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Packing]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Packing']='" + $("#sample-row-" + sample_nr).find("[name=Packing]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Packing_id]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Packing_id']='" + $("#sample-row-" + sample_nr).find("[name=Packing_id]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=EarTag]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['EarTag']='" + $("#sample-row-" + sample_nr).find("[name=EarTag]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=DateValid]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['DateValid']='" + $("#sample-row-" + sample_nr).find("[name=DateValid]").val() + "'");
            }

            if ($("#sample-row-" + sample_nr).find("[name=InvestigationType]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['InvestigationType']='" + $("#sample-row-" + sample_nr).find("[name=InvestigationType]").val() + "'");
            }

            if ($("#sample-row-" + sample_nr).find("[name=UniqueCod]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['UniqueCod']='" + $("#sample-row-" + sample_nr).find("[name=UniqueCod]").val() + "'");
            }

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: eval("gpostArray_" + sample_nr),
                success: function (responce) {
                    $(pBtn).closest(".edit-section-body").html(responce);
                    Sample_dynamic_section_open(pBtn, responce);
                    $(".loading").hide();
                }
            });
        }
    }
    return false;
}


function save_Sample(pBtn, requestId, Namespace, on_load_function) {
    
    if (!$(".loading").is(":visible")) {        
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();
            sample_nr = $(pBtn).closest(".edit-section-table-row").find("#sample_nr").val()
            eval("gpostArray_" + sample_nr + "['RequestId']=" + requestId);
            eval("gpostArray_" + sample_nr + "['SampleId']=" + $("#sample-row-" + sample_nr).find("#sample_id").val());
            eval("gpostArray_" + sample_nr + "['Namespace']='" + Namespace + "'");
            if ($("#sample-row-" + sample_nr).find("[name=SampleName]").size() > 0) {
                var SampleName = $("#sample-row-" + sample_nr).find("[name=SampleName]").val();

                while (SampleName.indexOf("''") > -1) {
                    SampleName = SampleName.replace("''", '"');
                }
                while (SampleName.indexOf("'") > -1) {
                    SampleName = SampleName.replace("'", '"');
                }
                eval("gpostArray_" + sample_nr + "['SampleName']='" + SampleName + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=SampleName_id]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['SampleNameId']='" + $("#sample-row-" + sample_nr).find("[name=SampleName_id]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Quantity]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Quantity']='" + $("#sample-row-" + sample_nr).find("[name=Quantity]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Unit] option:selected").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Unit']='" + $("#sample-row-" + sample_nr).find("[name=Unit] option:selected").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Lot]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Lot']='" + $("#sample-row-" + sample_nr).find("[name=Lot]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=LotUnit]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['LotUnit']='" + $("#sample-row-" + sample_nr).find("[name=LotUnit]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=DateProduction]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['DateProduction']='" + $("#sample-row-" + sample_nr).find("[name=DateProduction]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Packing]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Packing']='" + $("#sample-row-" + sample_nr).find("[name=Packing]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=Packing_id]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['Packing_id']='" + $("#sample-row-" + sample_nr).find("[name=Packing_id]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=SealNr]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['SealNr']='" + $("#sample-row-" + sample_nr).find("[name=SealNr]").val() + "'");
            }
            if ($("#sample-row-" + sample_nr).find("[name=DateValid]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['DateValid']='" + $("#sample-row-" + sample_nr).find("[name=DateValid]").val() + "'");
            }

            if ($("#sample-row-" + sample_nr).find("[name=InvestigationType]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['InvestigationType']='" + $("#sample-row-" + sample_nr).find("[name=InvestigationType]").val() + "'");
            }

            if ($("#sample-row-" + sample_nr).find("[name=UniqueCod]").size() > 0) {
                eval("gpostArray_" + sample_nr + "['UniqueCod']='" + $("#sample-row-" + sample_nr).find("[name=UniqueCod]").val() + "'");
            }

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: eval("gpostArray_" + sample_nr),
                success: function (responce) {
                    $(pBtn).closest(".edit-section-body").html(responce);
                    Sample_dynamic_section_open(pBtn, responce);
                    if (on_load_function != undefined) {
                        on_load_function();
                    }
                    else {
                        $(".loading").hide();
                    }
                }
            });
        }
    }
    return false;
}

function reloadInvestidationResultsChoise(sample_id) {
    var InvestigationTypeIds = $("[name=InvestigationTypePopUP]").val();
    $("[name=SampleType]").attr("data-cond","InvestigationTypes|" + InvestigationTypeIds);
    //var GOSTId = $("[name=GOST_id]").size()>0?$("[name=GOST_id]").val():"";
    var SampleTypeId = $("[name=SampleType_id]").size() > 0 ? $("[name=SampleType_id]").val() : "";

    var lInvestigationsArray = Array();
    var lInvestigationsCountChecked = 0;
    $("#InvestidationResultsChoise").find("input:checked").each(function (i) {
        lInvestigationsArray[lInvestigationsCountChecked] = $(this).attr("name");
        lInvestigationsCountChecked++;
    });
    
    var lGostsArray = Array();
    var lGostsCount = 0;

    if (InvestigationTypeIds != null) {
        var postArray = "InvestigationTypeIds=" + InvestigationTypeIds + /*"&GOSTId=" + GOSTId +*/ "&SampleTypeId=" + SampleTypeId;
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadInvestigationResults/" + sample_id,
            async: true,
            data: postArray,
            success: function (responce) {
                $("#InvestidationResultsChoise").html(responce);
                for (var i = 0; i < lInvestigationsCountChecked; i++) {
                    $("#" + lInvestigationsArray[i]).prop('checked', true);
                }
            }
        });

        $("#InvestidationGostsChoise").find(".select-multyselect").each(function (i) {
            lGostsArray[lGostsCount] = Array();
            lGostsArray[lGostsCount]["values"] = $(this).val();
            lGostsArray[lGostsCount]["name"] = $(this).attr("name");
            lGostsArray[lGostsCount]["textname"] = $($("#InvestidationGostsChoise").find(".gost-input")[i]).attr("name");
            lGostsArray[lGostsCount]["text"] = $($("#InvestidationGostsChoise").find(".gost-input")[i]).val();

            lGostsArray[lGostsCount]["quantityname"] = $($("#InvestidationGostsChoise").find(".decimal-input")[i]).attr("name");
            lGostsArray[lGostsCount]["quantity"] = $($("#InvestidationGostsChoise").find(".decimal-input")[i]).val();

            lGostsArray[lGostsCount]["unitname"] = $($("#InvestidationGostsChoise").find(".select")[i]).attr("name");
            lGostsArray[lGostsCount]["unit"] = $($("#InvestidationGostsChoise").find(".select")[i]).val();

            lGostsCount++;
        });
    }
    else {
        $("#InvestidationResultsChoise").html("Investigations");
    }

    if (InvestigationTypeIds != null) {
        var postArray = "InvestigationTypeIds=" + InvestigationTypeIds + "&SampleTypeId=" + SampleTypeId;
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadGOSTS/" + sample_id,
            async: true,
            data: postArray,
            success: function (responce) {
                $("#InvestidationGostsChoise").html(responce);

                for (var i = 0; i < lGostsCount; i++) {
                    $("[name=" + lGostsArray[i]["name"] + "]").val(lGostsArray[i]["values"]);
                    $("[name=" + lGostsArray[i]["textname"] + "]").val(lGostsArray[i]["text"]);
                    $("[name=" + lGostsArray[i]["unitname"] + "]").val(lGostsArray[i]["unit"]);
                    $("[name=" + lGostsArray[i]["quantityname"] + "]").val(lGostsArray[i]["quantity"]);
                }

                $("#InvestidationGostsChoise").find(".select-multyselect").select2();

                if ($(".gost-input").size() > 0) {
                    $.each($(".gost-input"), function (i, item) {
                        load_gosts(item);
                    });
                }
            }
        });
    }
    else {
        $("#InvestidationGostsChoise").html("");
    }
}

function ConfirmRequest(sample_id,investigation_id) {
    ShowConfirmMessage("Confirm Results?", function () {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/Confrim/" + sample_id + "/" + investigation_id,
            async: true,
            success: function (responce) {
                window.location.reload();
            }
        });
        return true;
    });
    return false;
}

function ArcRequest(request_id, labcode) {

    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/ArcRequest/" + request_id,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var postArray = "&DateIssued=" + $("[name=DateIssued]").val() + "&DateIssued_Hours=" + $("[name=DateIssued_Hours]").val() + "&DateIssued_Minutes=" + $("[name=DateIssued_Minutes]").val() + "&LaboratoryCod=" + labcode;
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "RequestHelper/Archive/" + request_id,
                    async: true,
                    dataType: "JSON",
                    data: postArray,
                    success: function (responce) {
                        window.location.reload();
                        if (responce.Message != "" && responce.Message != null) {
                            alert(responce.Message);
                        }
                    }
                });
                return true;
            });
        }
    });
    return false;
}

function StartWork(sample_id, investigationtype_id,section_id) {

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/StartWork/",
        data: "ResponsableName=" + $("[name=ResponsableName]").val() + "&SectionId=" + section_id,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var postArray = "SampleId=" + sample_id + "&InvestigationTypeId=" + investigationtype_id + "&ResponsableName=" + $("[name=ResponsableName]").val() + "&DateInvestigationStart=" + $("[name=DateInvestigationStart]").val();
                    //+ "&DateInvestigationStart_Hours=" + $("[name=DateInvestigationStart_Hours]").val() + "&DateInvestigationStart_Minutes=" + $("[name=DateInvestigationStart_Minutes]").val();
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "Sample/ChangeStatus/",
                    async: true,
                    dataType: "JSON",
                    data: postArray,
                    success: function (responce) {
                        window.location.reload();
                    }
                });
                return true;
            }, undefined, undefined, undefined, undefined, undefined, undefined, 370);

            $.each($(".investigation-responsable-input"), function (i, item) {
                load_InvestigationResponsables(item);
            });
        }
    });
    return false;
}


function LoadMMSVSRequest() {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/MMSVSForm/",
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "RequestHelper/ImportMMSVS/" + $("[name=MMSVSId]").val(),
                    async: true,
                    dataType: "JSON",
                    success: function (responce) {
                        if (responce["Result"] == 2) {
                            window.location.reload();
                            return;
                        }
                        if (responce["Result"] == 1) {
                            window.location = responce["RedirectURL"];
                            return;
                        }
                        if (responce["Result"] == 0) {
                            ShowAlertMessage(responce["Message"])
                            return;
                        }
                    }
                });
                return true;
            });
        }
    });
    return false;
}

function PayRequest(pRequest) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/PayForm/" + pRequest,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                if(form_validation($(".popup-section"), true)) {
                    gpostArray = {};

                    $(".popup-section").find("input:not(:checkbox)").each(function (i) {
                        gpostArray[$(this).attr("name")] = $(this).val();
                    });

                    $(".popup-section").find('input:checked').each(function () {
                        gpostArray[$(this).attr("name")] = $(this).val();
                    });

                    $(".popup-section").find("select").each(function (i) {
                        gpostArray[$(this).attr("name")] = $(this).val();
                    });

                    $(".popup-section").find("textarea").each(function (i) {
                        gpostArray[$(this).attr("name")] = $(this).val();
                    });

                    $.ajax({
                        type: 'POST',
                        url: gRootUrl + "RequestHelper/Pay/" + pRequest,
                        async: true,
                        dataType: "JSON",
                        data: gpostArray,
                        success: function (responce) {
                            if (responce["Result"] == 2) {
                                window.location.reload();
                                return;
                            }
                            if (responce["Result"] == 1) {
                                window.location = responce["RedirectURL"];
                                return;
                            }
                            if (responce["Result"] == 0) {
                                ShowAlertMessage(responce["Message"])
                                return;
                            }
                        }
                    });
                    return true;
                }
            });
        }
    });
    return false;
}

function DeleteRequest(pRequestId, pNamespace) {
    var postArray = "Id=" + pRequestId + "&Namespace=" + pNamespace;
    ShowConfirmMessage("Confirm deleting request?", function () {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "LabControl/Delete/",
            async: true,
            data: postArray,
            success: function (data) {
                if (data["Result"] == 2) {
                    ShowMessage(data["Message"], "success", true);
                    window.setTimeout(function () { window.location = data["RedirectURL"]; }, 1000);
                    return;
                } else if (data["Result"] == 0) {
                    ShowMessage(data["Message"], "error", true);
                } else {
                    if (data["ErrorFields"]) {
                        $.each(data["ErrorFields"], function (i, item) {
                            $("" + item).addClass("input-error", "slow");
                            if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                                $("" + item).next().show();
                            }
                            else {
                                $("" + item).after("<div class='error-message'></div>");
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                            }
                        });
                    }
                    ShowMessage(data["Message"], "success", true);
                }
            }
        });
        return true;
    });
    return false;
}

function DeleteDocumentManagemet(pDmId, pNamespace) {
    var postArray = "Id=" + pDmId + "&Namespace=" + pNamespace;
    ShowConfirmMessage("Confirm deleting Document?", function () {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "LabControl/Delete/",
            async: true,
            data: postArray,
            success: function (data) {
                if (data["Result"] == 2) {
                    ShowMessage(data["Message"], "success", true);
                    window.setTimeout(function () { window.location = data["RedirectURL"]; }, 1000);
                    return;
                } else if (data["Result"] == 0) {
                    ShowMessage(data["Message"], "error", true);
                } else {
                    if (data["ErrorFields"]) {
                        $.each(data["ErrorFields"], function (i, item) {
                            $("" + item).addClass("input-error", "slow");
                            if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                                $("" + item).next().show();
                            }
                            else {
                                $("" + item).after("<div class='error-message'></div>");
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                            }
                        });
                    }
                    ShowMessage(data["Message"], "success", true);
                }
            }
        });
        return true;
    });
    return false;
}
function delete_Sample(pBlock, psampleId, pRequestId, pNamespace) {
    var postArray = "Id=" + psampleId + "&RequestId=" + pRequestId + "&Namespace=" + pNamespace;
    return remove_item(postArray, "Confirm record delete?", pBlock);
}

function delete_ProtocolSample(pBlock, psampleId, pProtocolId, pNamespace) {
    var postArray = "Id=" + psampleId + "&ProtocolId=" + pProtocolId + "&Namespace=" + pNamespace;
    return remove_item(postArray, "Confirm record delete?", pBlock);
}

function PrintSampleResultsReport(sampleId, investigationTypeId, type, Namespace) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/ResultsReportPrintSettings/",
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var postArray = "SampleID=" + sampleId + "&InvestigationTypeID=" + investigationTypeId + "&type=" + type + "&Namespace=" + Namespace;
                if ($("[name=PrintResults]").is(":checked")) {
                    postArray += "&PrintResults=1";
                }
                print(postArray);
                return true;
            }, function () {
                return true;
            }, "Print", "Close");
        }
    });
    return false;
}

function PrintRequestResultsReport(requestId, type, Namespace) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/ResultsReportPrintSettings/",
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var postArray = "RequestID=" + requestId + "&type=" + type + "&Namespace=" + Namespace;
                if ($("[name=Certificated]").is(":checked")) {
                    postArray += "&Certificated=1";
                }
                if ($("[name=PrintEachSample]").is(":checked")) {
                    postArray += "&PrintEachSample=1";
                }
                if ($("[name=PrintFooterText]").is(":checked")) {
                    postArray += "&PrintFooterText=1";
                }
                if ($("[name=PrintWord]").is(":checked")) {
                    ExportToWord(postArray);
                }
                else {
                    print(postArray);
                }
                return true;
            }, function () {
                return true;
            }, "Print", "Close");
        }
    });
    return false;
}

function PrintProtocol(protocolId, type, protocolTypeId, Namespace) {
    var postArray = "ProtocolID=" + protocolId + "&type=" + type + "&protocolTypeId=" + protocolTypeId + "&Namespace=" + Namespace;
    print(postArray);
}

function PrintRequest(requestId, Namespace) {
    var postArray = "RequestID=" + requestId + "&Namespace=" + Namespace;
    return print(postArray);
}

function PrintRequestReport(requestId, type, Namespace) {
    var postArray = "RequestID=" + requestId + "&type=" + type + "&Namespace=" + Namespace;
    return print(postArray);
}

function ShowInvestigationAdvancedRegProperties(sampleId, investigationId, sampleInvestigationId, tag) {

    if (!$(".loading").is(":visible")) {
        $(".loading").show();

        var sampleTypeId = $("[name=SampleType_id]").val() != "" ? $("[name=SampleType_id]").val() : "0";

        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/AdvancedRegProperties/" + sampleId + "/" + investigationId + "/" + sampleInvestigationId + "/" + sampleTypeId,
            async: true,
            success: function (data) {
                    ShowConfirmMessage(data, function () {
                        var txtProperties = "";
                        if ($("[name=" + tag + "_CountOfInvestigations_Settings]").val() != "0" && $("[name=" + tag + "_CountOfInvestigations_Settings]").size() != 0) {
                            txtProperties += "samples:" + $("[name=" + tag + "_CountOfInvestigations_Settings]").val();
                        }
                        $("[name=" + tag + "_CountOfInvestigations]").val($("[name=" + tag + "_CountOfInvestigations_Settings]").val());
                        if ($("[name=" + tag + "_InvestigationAdditionalParameters_Settings]").val() != "") {
                            if (txtProperties != "")
                                txtProperties += ",";
                            txtProperties += $("[name=" + tag + "_InvestigationAdditionalParameters_Settings]").val();
                            $("[name=" + tag + "_InvestigationAdditionalParameters]").val($("[name=" + tag + "_InvestigationAdditionalParameters_Settings]").val());
                        }
                        if ($("[name=" + tag + "_Unit_Settings]").val() != "") {
                            if (txtProperties != "")
                                txtProperties += ",";
                            txtProperties += $("[name=" + tag + "_Unit_Settings]").find(":selected").text();
                        }
                        if ($("[name=" + tag + "_InvestigationAdditionalLimit_Settings]").val() != "") {
                            if (txtProperties != "")
                                txtProperties += ",";
                            txtProperties += $("[name=" + tag + "_InvestigationAdditionalLimit_Settings]").val();
                            $("[name=" + tag + "_InvestigationAdditionalLimit]").val($("[name=" + tag + "_InvestigationAdditionalLimit_Settings]").val());
                        }
                        $("[name=" + tag + "_AdditionalNotes]").val($("[name=" + tag + "_AdditionalNotes_Settings]").val());
                        $("[name=" + tag + "_Unit]").val($("[name=" + tag + "_Unit_Settings]").val());
                        $("[name=" + tag + "_Price]").val($("[name=" + tag + "_Price_Settings]").val());
                        $("[name=" + tag + "_Minimum]").val($("[name=" + tag + "_Minimum_Settings]").val());
                        $("[name=" + tag + "_MinimumAdvancedValue]").val($("[name=" + tag + "_MinimumAdvancedValue_Settings]").val());
                        $("[name=" + tag + "_Maximum]").val($("[name=" + tag + "_Maximum_Settings]").val());
                        $("[name=" + tag + "_MaximumAdvancedValue]").val($("[name=" + tag + "_MaximumAdvancedValue_Settings]").val());
                        $("[name=" + tag + "_TextLimits]").val($("[name=" + tag + "_TextLimits_Settings]").val());
                        if (txtProperties != "") {
                            txtProperties = "(" + txtProperties + ")";
                            $("[name=" + tag + "]").prop("checked", true);
                        }
                        $("#properties_container_" + tag).html(txtProperties);

                        return true;
                    }, function () {
                    return true;
                    }, getTranslation("Save"), getTranslation("Back"), getTranslation("Cancel"), function () {

                        $("[name=" + tag + "_AdditionalNotes]").val("");
                        $("[name=" + tag + "_Unit]").val("");
                        $("[name=" + tag + "_Price]").val("");
                        $("[name=" + tag + "_Minimum]").val("");
                        $("[name=" + tag + "_MinimumAdvancedValue]").val("");
                        $("[name=" + tag + "_Maximum]").val("");
                        $("[name=" + tag + "_MaximumAdvancedValue]").val("");
                        $("[name=" + tag + "_TextLimits]").val("");
                        $("#properties_container_" + tag).html("");

                        if (sampleInvestigationId > 0) {
                            var postArray = "sampleInvestigationId=" + sampleInvestigationId;

                            $.ajax({
                                type: 'POST',
                                url: gRootUrl + "Sample/ClearAdvancedRegProperties/",
                                data: postArray
                            });
                        }

                        return true;
                    }, function () {
                    if ($(".investigationsdditionalparameters-input").size() > 0) {
                        $.each($(".investigationsdditionalparameters-input"), function (i, item) {
                            load_InvestigationAdditionalParameters(item);
                        });
                    }
                    if ($(".confirmation-message").find(".autocomplete-input").size() > 0) {
                        $.each($(".confirmation-message").find(".autocomplete-input"), function (i, item) {
                            load_autocomplete(item);
                        });
                    }
                    $(".loading").hide();
                });
            }
        });
    }
    return false;
}

function Request_SavePage_result_process_after(data) {
    /*if (data["Result"] == 1) {
        ShowConfirmMessage("Adaugam următoare Cerere?", function () {
            window.location = data["RedirectURL"];
            return true;
        }, function () {
            return true;
        }, "Yes", "No");
    }*/
}


function DeleteProtocol(pProtocolId, pNamespace) {
    var postArray = "Id=" + pProtocolId + "&Namespace=" + pNamespace;
    ShowConfirmMessage("Confirmati Anularea Procesului Verbal?", function () {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "LabControl/Delete/",
            async: true,
            data: postArray,
            success: function (data) {
                if (data["Result"] == 2) {
                    ShowMessage(data["Message"], "success", true);
                    window.setTimeout(function () { window.location = data["RedirectURL"]; }, 1000);
                    return;
                } else if (data["Result"] == 0) {
                    ShowMessage(data["Message"], "error", true);
                } else {
                    if (data["ErrorFields"]) {
                        $.each(data["ErrorFields"], function (i, item) {
                            $("" + item).addClass("input-error", "slow");
                            if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                                $("" + item).next().show();
                            }
                            else {
                                $("" + item).after("<div class='error-message'></div>");
                                $("" + item).next().find(".error-message-text").html($("" + item).attr("data-req-mess"));
                            }
                        });
                    }
                    ShowMessage(data["Message"], "success", true);
                }
            }
        });
        return true;
    });
    return false;
}
/*----------------------------------------------------:delete-----------------------------------------------------*/
function remove_item(postArray, message, pBlock, bClientReload) {
    ShowConfirmMessage(message, function () {
        $.ajax({
            type: 'POST',
            url: gRootUrl + "DynamicControl/Delete/",
            async: true,
            data: postArray,
            success: function (responce) {
                $(pBlock).closest(".edit-section-table-row").fadeOut("slow", function () {
                    $(pBlock).closest(".edit-section-table-row").remove();
                });
            }
        });
        return true;
    });
    return false;
}
/*----------------------------------------------------:upload-----------------------------------------------*/
function initUploadFile(purl, pNamespace, pType) {
    $(".edit-section-new-document").ajaxUpload({
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
            load_dynamic_section($("#dynamic_section_documents")[0], pNamespace, pType, false);
            initUploadFile(purl, pNamespace, pType);
            $(".loading").hide();
            return true;
        }
    });
}

function save_RequestDocument(pBtn, requestId, Namespace,documentId) {

    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();

            gpostArray = {};

            $(pBtn).closest(".edit-section-table-row").find("input:not(:checkbox)").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find('input:checked').each(function () {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("select").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("textarea").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            gpostArray["ParentId"] = requestId;
            gpostArray["documentId"] = documentId;
            gpostArray["Namespace"] = Namespace;

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: gpostArray,
                success: function (responce) {
                    $(".loading").hide();
                }
            });
        }
    }
    return false;
}

function save_PatientDocument(pBtn, patientId, Namespace, documentId) {

    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();

            gpostArray = {};

            $(pBtn).closest(".edit-section-table-row").find("input:not(:checkbox)").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find('input:checked').each(function () {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("select").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("textarea").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            gpostArray["ParentId"] = patientId;
            gpostArray["documentId"] = documentId;
            gpostArray["Namespace"] = Namespace;

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: gpostArray,
                success: function (responce) {
                    $(".loading").hide();
                }
            });
        }
    }
    return false;
}

function save_SampleDocument(pBtn, sampleId, InvestigationIdTypeId, Namespace, documentId) {

    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();

            gpostArray = {};

            $(pBtn).closest(".edit-section-table-row").find("input:not(:checkbox)").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find('input:checked').each(function () {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("select").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            $(pBtn).closest(".edit-section-table-row").find("textarea").each(function (i) {
                gpostArray[$(this).attr("name")] = $(this).val();
            });

            gpostArray["SampleId"] = sampleId;
            gpostArray["InvestigationTypeId"] = InvestigationIdTypeId;
            gpostArray["documentId"] = documentId;
            gpostArray["Namespace"] = Namespace;

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: gpostArray,
                success: function (responce) {
                    $(".loading").hide();
                }
            });
        }
    }
    return false;
}


/*----------------------------------------------------:transfer-------------------------------------------------*/
function save_Transfer(pBlock, pRequestId, pNamespace) {
    if (form_validation($(pBlock).closest(".edit-section-body").find(".edit-section-table-add"), false)) {
        var postArray = "RequestId=" + pRequestId + "&Namespace=" + pNamespace;

        postArray += "&ToLaboratory=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=ToLaboratory]").val();
        postArray += "&DateStart=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateStart]").val();
        postArray += "&DateStart_Hours=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateStart_Hours]").val();
        postArray += "&DateStart_Minutes=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateStart_Minutes]").val();
        postArray += "&DateEnd=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateEnd]").val();
        postArray += "&DateEnd_Hours=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateEnd_Hours]").val();
        postArray += "&DateEnd_Minutes=" + $(pBlock).closest(".edit-section-body").find(".edit-section-table-add").find("[name=DateEnd_Minutes]").val();

        $.ajax({
            type: 'POST',
            url: gRootUrl + "DynamicControl/Save/",
            async: true,
            data: postArray,
            success: function (responce) {
                $(pBlock).closest(".edit-section-body").html(responce);
                var minDate = $.datepicker.parseDate('dd/mm/yy', $(pBlock).parent().next().find("#lastTransferDate").size() > 0 ? $(pBlock).parent().next().find("#lastTransferDate").val() : $("[name=SampleRegisterDate]").val());
                $(pBlock).closest(".edit-section-body").find(".calendar-input").datepicker({
                    dateFormat: "dd/mm/yy",
                    minDate: minDate
                });
            }
        });
    }
    return false;
}

function delete_Transfer(pBlock, pTransferId, pcheckinId, pNamespace, allowDelete) {
    if (allowDelete) {
        ShowConfirmMessage("Confirmati Anularea Transferului?", function () {
            var postArray = "Id=" + pTransferId + "&RequestId=" + pcheckinId + "&Namespace=" + pNamespace;

            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Delete/",
                async: true,
                data: postArray,
                success: function (responce) {
                    $(pBlock).closest(".edit-section-table-row").fadeOut("slow", function () {
                        $(pBlock).closest(".edit-section-body").html(responce);
                        var minDate = $.datepicker.parseDate('dd/mm/yy', $(pBlock).parent().next().find("#lastTransferDate").size() > 0 ? $(pBlock).parent().next().find("#lastTransferDate").val() : $("[name=Checkin_Date]").val());
                        $(pBlock).closest(".edit-section-body").find(".calendar-input").datepicker({
                            dateFormat: "dd/mm/yy",
                            minDate: minDate
                        });
                    });
                }
            });
            return true;
        });
    }
    return false;
}

function select_all_investigations(pInvestigationTypeId) {
    $("#investigations_list_" + pInvestigationTypeId).find(".checkbox").prop("checked", $("[name=selectall_" + pInvestigationTypeId + "]").is(":checked"));
}
function UpdateLimitsLable(pControl, tag) {
    var TextLimits = $("[name=" + tag + "_TextLimits_Settings]").val();
    if (tag == "") {
        TextLimits = $(pControl).closest(".edit-section-table-row").find("[name=Text]").val();
    }
    var labellimits = $("#limits_label");
    if (tag == "") { labellimits = $(pControl).closest(".edit-section-table-row").find("#limits_label"); }

    if (TextLimits != "") {
        labellimits.html(TextLimits);
    }
    else{
        var Min = $("[name=" + tag + "_Minimum_Settings]").val();
        if (tag == "") {
            Min = $(pControl).closest(".edit-section-table-row").find("[name=Minimum]").val();
        }
        var Max = $("[name=" + tag + "_Maximum_Settings]").val();
        if (tag == "") {
            Max = $(pControl).closest(".edit-section-table-row").find("[name=Maximum]").val();
        }
        var MinAdv = $("[name=" + tag + "_MinimumAdvancedValue_Settings]").find(":selected").text();
        if (tag == "") {
            MinAdv = $(pControl).closest(".edit-section-table-row").find("[name=MinimumAdvancedValue]").find(":selected").text();
        }
        var MaxAdv = $("[name=" + tag + "_MaximumAdvancedValue_Settings]").find(":selected").text();
        if (tag == "") {
            MaxAdv = $(pControl).closest(".edit-section-table-row").find("[name=MaximumAdvancedValue]").find(":selected").text();
        }
        if (Min != "0.00" && Min != "0") {
            Min += MinAdv;
        } else {
            Min = "";
        }

        if (Max != "0.00" && Max != "0") {
            Max += MaxAdv;
        } else {
            Max = "";
        }

        if (Min != "" && Max != "") {
            labellimits.html(Min+" - "+Max);
        }
        else if (Min != ""){
            labellimits.html(Min);
        }
        else if (Max != "") {
            labellimits.html(Max);
        }
    }
}

function loadPv() {
    var postArray = "code=" + $("#ProtocolCode").val();
    $(".ajax-loading-overlay").show();
    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/loadPv/",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 0) {
                alert(responce["Message"]);
                $(".ajax-loading-overlay").hide();
            }
            if (responce["Result"] == 2) {
                window.location.reload();
                return;
            }
            if (responce["Result"] == 1) {            
                ShowConfirmMessage(responce["Data"]["Protocol"], function () {
                    if ($("input[name=Id]").val() == "0") {

                        postArray = "ProtocolId=" + responce["Data"]["ProtocolId"];

                        $.ajax({
                            type: 'POST',
                            url: gRootUrl + "RequestHelper/importPv/",
                            async: true,
                            data: postArray,
                            success: function (responce) {
                                if (responce["Result"] == 2) {
                                    window.location = responce["RedirectURL"];
                                    return;
                                }
                                $(".ajax-loading-overlay").hide();
                            }
                        });
                    }
                    else {
                        $("[name=Protocol]").val(responce["Data"]["ProtocolId"]);
                        $(".ajax-loading-overlay").hide();
                        eval($("#Save").attr("onclick").replace("return ",""));
                        window.location.reload();
                    }
                    return true;
                }, function () {
                    $(".ajax-loading-overlay").hide();
                    return true;
                }, ($("input[name=Id]").val() == "0" ? "Add Request" : "Save"), "Cancel");

                return;
            }
        }
    });
}

function EnableDisableExaminationsText(pCheckBox,pExamId) {
    if ($(pCheckBox).is(":checked")) {
        $("[name=ExaminationsText_" + pExamId + "]").prop("disabled", false); 
        $("[name=ExaminationsText_" + pExamId + "]").focus();
    }
    else {
        $("[name=ExaminationsText_" + pExamId + "]").prop("disabled", true);
    }
}

function showProtocolTypePopUp() {
    $.fancybox.open([
        {
            autoSize: true,
            padding: 7,
            title: "Alege Tipul Protocolului",
            closeBtn: false, // hide close button
            closeClick: false, // prevents closing when clicking INSIDE fancybox
            helpers: {
                overlay: {
                    css: {
                        'background': 'rgba(58, 42, 45, 0.95)'
                    },
                    locked: true,
                    closeClick: false
                }
            },
            keys: {
                // prevents closing when press ESC button
                close: null
            },
            type: 'ajax',
            href: gRootUrl + "ProtocolHelper/ProtocolTypePopUp/"
        }
    ]);
}
function checkSampleExists(pInput) {
    var result = false;
    var isAnimal = $("[name=IsAnimal]").val();

    if ($.trim($(pInput).val()) != "") {
        var postArray = "SampleId=" + $(pInput).val() + "&Investigation=" + $("[name=InvestigationId]").val()
        $.ajax({
            type: 'POST',
            url: gRootUrl + (isAnimal == "1" ? "Sample/CheckAnimalSampleExists/" : "Sample/CheckSampleExists/"),
            async: false,
            data: postArray,
            success: function (responce) {
                if (responce.Data["SampleExists"] == "0") {
                    result = true;
                }
                else {
                    result = false;
                }
            }
        });
    }
    return result;
}

function LoadInvestigationThreshold(pInput) {
    if ($.trim($(pInput).val()) != "") {
        $("[name=BOId]").val($(pInput).val());
        var postArray = "InvestigationId=" + $(pInput).val();
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadInvestigationThreshold/",
            async: false,
            data: postArray,
            success: function (response) {
                // Link to Investigation settings
                $("[name=toInvestigationSettings]").removeClass("sample-detail-link-disabled").addClass("sample-detail-link");

                // Control widget
                $(".equipment-data-controls-table").children(".edit-section-table-row").each(function () {
                    if ($(this).find("[name=AverageNegC]").length > 0) {
                        if (parseFloat($(this).find("[name=AverageNegC]").val()) > parseFloat(response.Data["NegativeThresholdValue"])) {
                            $(this).find("[name=Validity]").val("invalid");
                            $(this).find("[name=Validity]").removeClass("equipment-data-page-text").addClass("equipment-data-page-invalid");
                        }
                        else {
                            $(this).find("[name=Validity]").val("valid");
                            if ($(this).find("[name=Validity]").hasClass("equipment-data-page-invalid")) {
                                $(this).find("[name=Validity]").removeClass("equipment-data-page-invalid").addClass("equipment-data-page-text");
                            }
                        }
                    }
                    if ($(this).find("[name=AveragePosC]").length > 0) {
                        if (parseFloat($(this).find("[name=AveragePosC]").val()) > parseFloat(response.Data["PositiveThresholdValue"])) {
                            $(this).find("[name=Validity]").val("invalid");
                            $(this).find("[name=Validity]").removeClass("equipment-data-page-text").addClass("equipment-data-page-invalid");
                        }
                        else {
                            $(this).find("[name=Validity]").val("valid");
                            if ($(this).find("[name=Validity]").hasClass("equipment-data-page-invalid")) {
                                $(this).find("[name=Validity]").removeClass("equipment-data-page-invalid").addClass("equipment-data-page-text");
                            }
                        }
                    }
                });

                // Results widget
                $(".equipment-data-results-table").children(".edit-section-table-row").each(function () {
                    if ($(this).find("[name*=Percentage]").length > 0) {
                        if (parseFloat($(this).find("[name*=Percentage]").val()) > parseFloat(response.Data["ResultThresholdValue"])) {
                            $(this).find("[name*=Result_]").val("positive");
                            $(this).find("[name*=Result_]").removeClass("equipment-data-page-text").addClass("equipment-data-page-invalid");
                        }
                        else {
                            $(this).find("[name*=Result_]").val("negative");
                            if ($(this).find("[name*=Result_]").hasClass("equipment-data-page-invalid")) {
                                $(this).find("[name*=Result_]").removeClass("equipment-data-page-invalid").addClass("equipment-data-page-text");
                            }
                        }
                    }
                });

                // Warnings
                if (response.Data["NegativeThresholdValue"] == "0"
                    && response.Data["PositiveThresholdValue"] == "0"
                    && response.Data["ResultThresholdValue"] == "0") {
                    WarningAppendText("Calculation Formula Not Set");
                }
                else {
                    WarningAppendText("");
                }
            }
        });
    }
    else {
        $("[name=toInvestigationSettings]").addClass("sample-detail-link-disabled").removeClass("sample-detail-link");

        $(".equipment-data-controls-table").children(".edit-section-table-row").each(function () {
            $(this).find("[name=Validity]").val("-");
            if ($(this).find("[name=Validity]").hasClass("equipment-data-page-invalid")) {
                $(this).find("[name=Validity]").removeClass("equipment-data-page-invalid").addClass("equipment-data-page-text");
            }
        });
        $(".equipment-data-results-table").children(".edit-section-table-row").each(function () {
            $(this).find("[name*=Result_]").val("-");
            if ($(this).find("[name*=Result_]").hasClass("equipment-data-page-invalid")) {
                $(this).find("[name*=Result_]").removeClass("equipment-data-page-invalid").addClass("equipment-data-page-text");
            }
        });
    }
}

function autoFillSampleName(pInput) {
    if ($(pInput).closest('.edit-section-table-row').find("[name*=SampleId_]").val() != "") {

        var initialInput = $(pInput).closest('.edit-section-table-row').find("[name*=SampleId_]").val().split('/');
        if (initialInput.length > 3) {
            var insertNum = parseInt(initialInput[3]) + 1;
            var currentNum = parseInt($(pInput).closest('.edit-section-table-row').find("[name*=SampleId_]").attr('name').split('_')[1]);

            $("[name*=SampleId_]").each(function () {
                if (parseInt($(this).attr('name').split('_')[1]) > currentNum) {
                    $(this).val(initialInput[0] + '/' + initialInput[1] + '/' + initialInput[2] + '/' + insertNum);
                    insertNum++;
                }
            });
        }
    }
}

function LoadEquipmentResultsData(EquipmentDataId, pControl) {
    var postArray = "EquipmentDataId=" + EquipmentDataId;
    if (pControl != undefined) {
        if ($(pControl).val() == "0") {
            $(pControl).val("1");
        }
        else {
            $(pControl).val("0");
        }
    }
    postArray += "&ShowTwins=" + $("[name=ShowTwin]").val();
    postArray += "&InvestigationId=" + $("[name=InvestigationId]").val();
    if ($("[name=InvestigationFormulaId]").val() != '')
        postArray += "&InvestigationFormulaId=" + $("[name=InvestigationFormula]").val();

    if (pControl != undefined) {
        $("[name=ShowTwin]").val($(pControl).val());
    }

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/LoadEquipmentResultsData/",
        async: true,
        data: postArray,
        success: function (responce) {
            $('#ReloadDataResultsContainer').html(responce);
            LoadInvestigationThreshold($("[name=InvestigationId]"));
            $("[name=ShowTwins]").val($("[name=ShowTwin]").val());
        },
    });
}

function ChangeBSValue(pControl) {
    if ($(pControl).val() == "0") {
        $(pControl).val("1");
    }
    else {
        $(pControl).val("0");
    }
}

function LoadEquipmentResultsChemistry(EquipmentDataId) {

    var TestKit = $("[name=TestKit_id]").val();
    var CutOff = $("[name=CutOff]").val();
    var DilutionFactor = $("[name=DilutionFactor]").val();

    if (TestKit != "" && CutOff != "" && DilutionFactor != "") {
        var postArray = "EquipmentDataId=" + EquipmentDataId + "&TestKit=" + TestKit + "&CutOff=" + CutOff.replace('.', ',')
            + "&DilutionFactor=" + DilutionFactor.replace('.', ',') + "&IsBlankSample=" + $("[name=IsBlankSample]").val();
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadEquipmentResultsData/",
            async: true,
            data: postArray,
            success: function (responce) {
                $('#ReloadDataResultsContainer').html(responce);
            },
        });
    }
}

function toAdminSettings(pBObject) {
    var Id = $("[name=BOId]").val();
    if (Id != "" && Id != "0") {
        window.open(gRootUrl + "ControlPanel/EditItem/" + pBObject + "/SecurityCRMLib.BusinessObjects/" + Id);
    }
}

function LoadTestKitStandards(EquipmentDataId) {
    var testKitId = $("[name=TestKit_id]").val();
    $("[name=BOId]").val(testKitId);

    if (testKitId != "0" && testKitId != "") {
        var postArray = "EquipmentDataId=" + EquipmentDataId + "&TestKitId=" + testKitId;

        $("[name=toTestKitSettings]").removeClass("sample-detail-link-disabled").addClass("sample-detail-link");
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadTestKitStandards/",
            async: true,
            data: postArray,
            success: function (responce) {
                $('#reloadEquipmentDataControls').html(responce);
            },
        });
    }
    else {
        $("[name=toTestKitSettings]").removeClass("sample-detail-link").addClass("sample-detail-link-disabled");
    }
}

function WarningAppendText(text) {
    $("[name=ThresholdMissingWarning]").each(function () {
        $(this).empty();
        $(this).append(text);
    });
}

function SetEquipmentDataCancelStatus(EquipmentDataId) {
    var postArray = "EquipmentDataId=" + EquipmentDataId;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/SetEquipmentDataCancelStatus/",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                window.location = responce["RedirectURL"];
                return;
            }
        },
    });

}

function EquipmentDataAssignTo(EquipmentDataId, SectionId, ResponsibleId) {

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/EquipmentDataAssignTo/",
        data: "SectionId=" + SectionId + "&EquipmentDataId=" + EquipmentDataId + "&ResponsibleId=" + ResponsibleId,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var postArray = "EquipmentDataId=" + EquipmentDataId + "&ResponsibleId=" + $("[name=ResponsibleId]").val();
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "Sample/SetEquipmentDataResponsible/",
                    async: true,
                    dataType: "JSON",
                    data: postArray,
                    success: function (responce) {
                        if (responce["Result"] == 1) {
                            window.location = responce["RedirectURL"];
                            return;
                        }                        
                    }
                });
                return true;
            }, undefined, undefined, undefined, undefined, undefined, undefined, 370);
        }
    });
    return false;
}

function ShowSendToEmail(pLink) {
    if ($(pLink).prop("checked")) {
        $("#divSendToEmail").attr("style", "display:block");
    } else {
        $("#divSendToEmail").attr("style", "display:none")
    }
}

$(document).ready(function () {
    if ($("#excellanimal_btn").size() > 0) {
        $("#excellanimal_btn").ajaxUpload({
            url: gRootUrl + "RequestHelper/ImportAnimalsExcell/?RequestId=" + $("[name=Id]").val() + "&SpeciesId=" + $("[name=Species_id]").val(),
            accept: ['xlsx'],
            name: "file",
            dataType: "JSON",
            onSubmit: function () {
                $("#excellanimal").attr('checked', true);
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
                $("#dynamic_section_animals").click();
                $("#dynamic_section_documents").click();
                //initUploadFile(purl, pNamespace, pType);
                $(".loading").hide();
                return true;
            }
        });
    }

    if ($("#UploadSampleNumbers").length > 0) {
        $("#UploadSampleNumbers").ajaxUpload({
            url: gRootUrl + "RequestHelper/UploadSampleNumbers",
            accept: ['xlsx'],
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
                if (jsonResponce.Result == 1) {
                    var sampleArr = jsonResponce.Data["Samples"].split(',');
                    for (var i = 1; i <= sampleArr.length; i++) {
                        $("[name=SampleId_" + i + "]").val(sampleArr[i - 1]);
                    }
                    $(".loading").hide();
                    return true;
                }
                $(".loading").hide();
                return true;
            }
        });
    }
});

function ChangeInvestigationFormula(pControl) {
    var postArray = "InvestigationFormulaId=" + $(pControl).val();

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/ChangeInvestigationFormula/",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                $(pControl).closest(".edit-section-row").find(".edit-section-row-left").find(".input-row").find(".label").html(responce.Data["InvestigationFormulaDesc"]);
                LoadInvestigationThreshold($("[name=InvestigationId]"));
                return;
            }
            if (responce.Result == 3) {
                $(pControl).closest(".edit-section-row").find(".edit-section-row-left").find(".input-row").find(".label").html("");
                return;
            }
        },
    });

}

function SetInvestigationHasChanged(pControl) {
    $("[name=InvestigationHasChanged_" + $(pControl).attr('name').split('_')[1] + "]").val(1);
}

function SetValidHasChanged(pControl) {
    $(pControl).val($(pControl).val() == "0" ? "1" : "0");
    SetInvestigationHasChanged(pControl);
}

function ConfirmRequestInvestigationType(RequestInvestigationTypeId) {
    var postArray = "RequestInvestigationTypeId=" + RequestInvestigationTypeId;

    ShowConfirmMessage("Confirm Results?", function () {
        $.ajax({
            type: 'POST',
            data: postArray,
            url: gRootUrl + "Sample/ConfrimRequestInvestigationType/" + RequestInvestigationTypeId,
            async: true,
            success: function (responce) {
                window.location.reload();
            }
        });
        return true;
    });
    return false;
}

function multiplyAnimalState(pControl, ReqInvTypelId, pUpdatingItem) {
    var lMethod = $(pControl).parent().find($("[name^=" + pUpdatingItem + "_]")).val();
    var lInvestigation = $(pControl).parent().find($("[name=InvestigationId]")).val();

    var postArray = "RequestInvestigationTypeId=" + ReqInvTypelId + "&SearchId=" + lMethod +
        "&UpdatingItem=" + pUpdatingItem + "&InvestigationId=" + lInvestigation;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "RequestHelper/MultiplyInvestigationMethod/",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                $.each($(".edit-section-table-row"), function (i, item) {
                    if (lInvestigation == $(item).find($("[name=InvestigationId]")).val()) {
                        $(item).find($("[name^=" + pUpdatingItem + "_]")).val(lMethod);
                    }
                });
            }
        },
    });
    $.each($(".edit-section-table-row"), function (i, item) {
        if (lInvestigation == $(item).find($("[name=InvestigationId]")).val()) {
            $(item).find($("[name^=" + pUpdatingItem + "_]")).val(lMethod);
        }
    });
}

function GenerateMilkSMS(pSampleId) {
    if (!$(".ajax-loading-overlay").is(":visible")) {

        $('.ajax-loading-overlay').fadeIn("slow");

        gpostArray = {};
        gpostArray["bo_type"] = "SecurityCRM.Models.Reports.MilkSampleDetails";
        gpostArray["Sample"] = pSampleId;
        gpostArray["__RequestVerificationToken"] = $("[name=__RequestVerificationToken]").val();

        console.log(gpostArray);
        $.fileDownload(gRootUrl + "Report/ExportCSV/", {
            httpMethod: "POST",
            data: gpostArray,
            successCallback: function (url) {
                $('.ajax-loading-overlay').fadeOut();
            },
            failCallback: function (html, url) {

                $('.ajax-loading-overlay').fadeOut();

                ShowMessage(html, "error", true);

            }
        });
    }

    return false;
}



function process_barcode_on_enter(input, event) {

    if (event == null)
        event = window.event;

    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {
        $(input).closest(".edit-section-table-row").next().find(".input-barcode-focus").focus();

    }
}

function select_all_investigations_group(GroupId) {
    $(".inv_group_" + GroupId).prop("checked", $("[name=selectall_group_" + GroupId + "]").is(":checked"));
}

function calculateResultByFormula(pSampleId, pInvestigationId, pInvestigationTag) {
    $.fancybox.open([
        {
            width: 1155,
            height: 450,
            autoSize: false,
            padding: 7,
            title: "Calculation",
            wrapCSS: "sample-popup",
            helpers: {
                overlay: {
                    speedIn: 0,
                    speedOut: 300,
                    opacity: 0.8,
                    css: {
                        cursor: 'default'
                    },
                    closeClick: false
                }
            },
            afterShow: function () {

                if ($("[name=EarTag]").size() > 0) {
                    gAnimalNumber = $("[name=EarTag]").val();
                }

                fix_save_btn_positioning();
            },
            type: 'ajax',
            data: gpostArray,
            href: gRootUrl + "Sample/PopUpLoadFormulaWidget?SampleId=" + pSampleId + "&InvestigationId=" + pInvestigationId + "&InvestigationTag=" + pInvestigationTag
        }
    ]);
    return false;
}

function calculateByFormula(pInvestigationCnt,pInvestigationTag) {
    var postArray = {};
    //postArray["InvestigationFormulaId"] = $("[name=InvestigationFormula]").val();
    postArray["InvestigationCnt"] = pInvestigationCnt;
    postArray["InvestigationTag"] = pInvestigationTag;

    $.each($(".edit-section-table-row"), function (i, item) {
        postArray["Dilutions_" + (i + 1)] = "";
        for (var j = 1; j <= 4; j++){
            if ($(item).find($("[name=Dilution" + j + "_" + (i + 1) + "]")).val() != "" &&
                $(item).find($("[name=Dilution" + j + "_" + (i + 1) + "]")).val() != "undefined") {
                postArray["Dilutions_" + (i + 1)] += (postArray["Dilutions_" + (i + 1)] == "" ? "" : ",") + $(item).find($("[name=Dilution" + j + "_" + (i + 1) + "]")).val();
            }
            postArray["Value_" + (i + 1)] = $(item).find($("[name=Value_" + (i + 1) + "]")).val();
            postArray["Tested_" + (i + 1)] = $(item).find($("[name=Tested_" + (i + 1) + "]")).val();
            postArray["Typic_" + (i + 1)] = $(item).find($("[name=Typic_" + (i + 1) + "]")).val();
            postArray["InvestigationFormulaId_" + (i + 1)] = $("[name=InvestigationFormula_" + (i + 1) + "]").val();
        }
        i++;
    });

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/FormulaCalculator/",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                var cnt = responce["Data"]["InvestigationCnt"];
                for (var i = 1; i <= cnt; i++) {
                    $("[name=" + responce["Data"]["InvestigationTag"] + (cnt > 0 ? "_" + i + "]" : "]")).val(responce["Data"]["Result_" + i]);
                    $("[name=" + responce["Data"]["InvestigationTag"] + "_advancedval" + (cnt > 0 ? "_" + i + "]" : "]")).val(responce["Data"]["AdvancedValueId_" + i]);
                }
                $.fancybox.close();
            }
        },
    });

}

function DistrictsReload() {
    if (typeof $("[name=District]").val() != "undefined") {
        //$.getScript(gRootUrl + "JsLoader/District/" + $("[name=District]").val() + "/", function (data, textStatus, jqxhr) {
        //    $("input[name=City]").autocomplete({
        //        source: street_arr,
        //        minLength: 2
        //    });
        //});

        var postArray = "DistrictId=" + $("[name=District]").val();

        $.ajax({
            type: 'POST',
            url: gRootUrl + "RequestHelper/UpdateCities/",
            async: true,
            dataType: "JSON",
            data: postArray,
            success: function (responce) {
                $("[name=City]").closest(".input-row").html(responce["data"]["City"]);
            }
        });
    }
}

function changeResponsable(pSelect) {
    console.log()
    if ($(pSelect, "option:selected").val() == 1) {
        $("[name=Responsable]").val($("[name=LastName]").val() + " " + $("[name=FirstName]").val());
    } else if ($(pSelect, "option:selected").val() == 4) {
        $("[name=Responsable]").val($("[name=Company]").val());
    } else if($(pSelect, "option:selected").val() == 6) {
        $("[name=Responsable]").val($("[name=Laboratory] option:selected").html());
    } 
}