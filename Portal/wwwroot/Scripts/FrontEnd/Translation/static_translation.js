$(document).ready(function () {
    if ($("#TranslationGrid").length > 0) {
        ReinitDGtranslationQuickSearch('Translation', 'translation');
    }
});

function ReinitDGtranslationQuickSearch(pController, pObject) {
    gGlobalSearch = $("[name=GlobalSearch]").val();
    $("[name=GlobalSearch]").on("change paste keyup", function () {
        var globalSearch = $("[name=GlobalSearch]").val();
        if (gGlobalSearch != globalSearch) {
            gGlobalSearch = globalSearch;
            window.setTimeout(function () {
                if (globalSearch == $("[name=GlobalSearch]").val()) {
                    researchTranslation();
                }
            }, 1000);
        }
    });
    gTranslationKey = $("[name=TranslationKey]").val();
    $("[name=TranslationKey]").on("change paste keyup", function () {
        var TranslationKey = $("[name=TranslationKey]").val();
        if (gTranslationKey != TranslationKey) {
            gTranslationKey = TranslationKey;
            window.setTimeout(function () {
                if (TranslationKey == $("[name=TranslationKey]").val()) {
                    researchTranslation();
                }
            }, 1000);
        }
    });
}


function researchTranslation(pageNum) {
    if (typeof pageNum != "undefined" && pageNum != "0") {
        $("#page_num").val(pageNum);
    }
    else {
        $("#page_num").val(0);
    }
    var GlobalSearch = $("[name=GlobalSearch]").val();
    var Key = $("[name=TranslationKey]").val();

    gpostArray = {};
    gpostArray["CountPerPage"] = $("#dpdwn_count_per_page").val();
    gpostArray["PageNum"] = $("#page_num").val();
    gpostArray["Assembly"] = $("[name=Assembly]").val();
    gpostArray["GlobalSearch"] = GlobalSearch;
    gpostArray["Key"] = Key;

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Translation/LoadGrid/",
        async: true,
        dataType: "JSON",
        data: gpostArray,
        success: function (responce) {
            if (responce["Result"] == 1) {
                $("#TranslationGrid").fadeOut("slow", function () {
                    $("#TranslationGrid").html(responce["Data"]["View"]);
                    window.setTimeout(function () { $("#TranslationGrid").fadeIn("slow") }, 1);
                })
            }
            if (responce["Result"] == 2) {
                window.location.reload();
                return;
            }
        }
    });
    return false;

}

function show_Translation_page(pageNum) {
    researchTranslation(pageNum);
}


function removeTranslationRow(prefix, Id) {
    if (typeof Id != "undefined" && Id != "0") {
        postArray = {};
        postArray["TranslationId"] = Id;
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Translation/Delete",
            async: true,
            data: postArray
        });

    }
    $(".translation-row-" + prefix).remove();
}


function loadTranslation(Id, prefix) {
    if (typeof Id == "undefined") {
        prefix = $("div[class*='translation-row-']").length;
    }
    postArray = {};
    postArray["TranslationId"] = Id;
    postArray["Prefix"] = prefix;
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Translation/LoadRow",
        async: true,
        data: postArray,
        success: function (responce) {
            if (Id > 0) {
                $(".translation-row-" + prefix).replaceWith(responce);
            } else {
                $("#TranslationGridHeader").after(responce);
            }
        }
    });
}

function saveTranslation(prefix, Id) {
    var postArray = {};
    postArray["TranslationId"] = $("[name=TranslationId_" + prefix + "]").val();
    postArray["Key"] = $("[name=Key_" + prefix + "]").val();
    postArray["AssemblyId"] = $("[name=Assembly]").val();
    postArray["Languages"] = $("[name^=Language_" + prefix + "]").map(function () {
        if ($(this).val() != "") {
            return $(this).attr("data-language-id");
        }
    }).get();
    postArray["LanguageValues"] = $("[name^=Language_" + prefix + "]").map(function () {
        if ($(this).val() != "") {
            return $(this).val().replaceAll(",", "|");
        }
    }).get();
    postArray["Prefix"] = prefix;
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Translation/Save",
        async: true,
        data: postArray,
        success: function (responce) {
            if (responce.Result == 3) {
                alert(responce["Message"])
            }
            else {
                $(".translation-row-" + prefix).replaceWith(responce);
            }
        }
        
    });
}

function reset_translation_filters() {
    $("#GlobalSearch").val("");
    $("#TranslationKey").val("");
}
