function loadProtocolAnimal(pControl) {
    if (!$(".loading").is(":visible")) {
        $(".loading").show();
        gpostArray = {};
        gpostArray["sAnimalNumber"] = $(pControl).val();
        gpostArray["SpeciesId"] = $("[name=Species_id]").val();

        if (gpostArray["sAnimalNumber"] == "") {
            $(".loading").hide();
            return false;
        }
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Animal/LoadAnimalInfo/",
            async: true,
            data: gpostArray,
            success: function (data) {
                if (data["Result"] == 1) {
                    $(pControl).closest(".edit-section-table-row").find("#animal_id").val(data["Data"]["AnimalId"]);
                    $(pControl).closest(".edit-section-table-row").find(".edit-section-table-content-animalinfo").html(data["Data"]["AnimalSummary"]);
                    if (data["Data"]["SyckState_id"] == "2") {
                        $(pControl).closest(".edit-section-table-row").addClass("edit-section-table-row-invalid");
                    }
                    else {
                        $(pControl).closest(".edit-section-table-row").removeClass("edit-section-table-row-invalid");
                    }
                }
                if (data["Result"] == 0 && data["Message"] != null) {
                    ShowMessage(data["Message"], "error", false);
                    $(pControl).closest(".edit-section-table-row").addClass("edit-section-table-row-invalid");
                    $(pControl).closest(".edit-section-table-row").find(".edit-section-table-content-animalinfo").html("");
                }
                $(".loading").hide();
            }
        });
    }
    return false;
}

function save_ProtocolAnimal(pBtn, requestId, Namespace) {

    if (!$(".loading").is(":visible")) {
        if (form_validation($(pBtn).closest(".edit-section-table-row"), false)) {
            $(".loading").show();
            sample_nr = $(pBtn).closest(".edit-section-table-row").find("#ProtocolAnimal_nr").val();
            eval("gpostArrayAnimal_" + sample_nr + "['RequestId']=" + requestId);
            eval("gpostArrayAnimal_" + sample_nr + "['ProtocolAnimalId']=" + $("#ProtocolAnimal-row-" + sample_nr).find("#ProtocolAnimal_id").val());
            eval("gpostArrayAnimal_" + sample_nr + "['Namespace']='" + Namespace + "'");
            eval("gpostArrayAnimal_" + sample_nr + "['Species']=" + $("[name=Species_id]").val());
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=Quantity]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['Quantity']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=Quantity]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=SamplingLocation]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['SamplingLocation']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=SamplingLocation]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=EarTag]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['EarTag']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=EarTag]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=Code]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['Code']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=Code]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=Age]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['Age']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=Age]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=Sex]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['Sex']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=Sex]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=AnimalState]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['AnimalState']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=AnimalState]").val() + "'");
            }
            if ($("#ProtocolAnimal-row-" + sample_nr).find("[name=Kind]").size() > 0) {
                eval("gpostArrayAnimal_" + sample_nr + "['Kind']='" + $("#ProtocolAnimal-row-" + sample_nr).find("[name=Kind]").val() + "'");
            }
            console.log(eval("gpostArrayAnimal_" + sample_nr));
            $.ajax({
                type: 'POST',
                url: gRootUrl + "DynamicControl/Save/",
                async: true,
                data: eval("gpostArrayAnimal_" + sample_nr),
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

function save_Animal(sampleNr) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Animal/SaveAnimalProtocolPopUp/",
        async: true,
        data: $("form[name='animal-protocol-data']").serialize() + "&" + $("form[name='animal-data']").serialize(),
        success: function (response) {
            if (response.Result == 1)
                $.fancybox.close();
        }
    });
    console.log($("form[name='animal-protocol-data']").serialize() + "&" + $("form[name='animal-data']").serialize());
}

function delete_ProtocolAnimal(pBlock, paId, pNamespace) {
    var postArray = "Id=" + paId + "&Namespace=" + pNamespace;
    return remove_item(postArray, "Do you want to delete selected animal?", pBlock);
}

function showAnimalDetailPopup(pLink, pAnimalProtocolId) {
    if (form_validation($(pLink).closest(".edit-section-table-row"), false)) {
        if ($(pLink).hasClass("link-disabled"))
            return false;

        var AnimalId = $(pLink).closest(".edit-section-table-row").find("[name=animal_id]").val();

        if (AnimalId == "")
            return false;

        $.fancybox.open([
            {
                width: 970,
                height: 550,
                autoSize: false,
                padding: 7,
                title: $(pLink).closest(".edit-section-table-row").find(".edit-section-table-content-animalinfo").html(),
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

                    $(".calendar-input").datepicker({
                        dateFormat: "dd/mm/yy",
                        changeYear: true,
                        yearRange: "1900:" + (new Date()).getFullYear()
                    });

                    if ($(".popup-content-proc").find(".autocomplete-input").size() > 0) {
                        $.each($(".popup-content-proc").find(".autocomplete-input"), function (i, item) {
                            load_autocomplete(item);
                        });
                    }

                    $(".popup-content-proc").find(".select-multyselect").select2();

                    fix_save_btn_positioning();
                },
                type: 'ajax',
                href: gRootUrl + "Animal/PopUpLoad/" + pAnimalProtocolId + "/" + AnimalId
            }
        ]);
    }
    return false;
}

function count_per_RequestInvestigationType_page() {
    show_RequestInvestigationType_page(0);
}

function show_RequestInvestigationType_page(page) {
    if (!$(".loading").is(":visible")) {
        $('.ajax-loading-overlay').fadeIn("slow");
        var postArray = "RequestInvestigationTypeId=" + $("[name=Id]").val() + "&SearchStr=" + $("[name=EarTagSearch]").val()
            + "&InvestigationsStr=" + $("[name=InvestigationsStr]").val() + "&Page=" + page + "&CountPerPage="
        if ($("#dpdwn_count_per_page").length > 0) {
            postArray += $("#dpdwn_count_per_page").val();
        } else {
            postArray += 0;
        }
        $.ajax({
            type: 'POST',
            url: gRootUrl + "Sample/LoadProtocolAnimalInvestigations/",
            async: true,
            data: postArray,
            success: function (responce) {
                $('.ajax-loading-overlay').fadeOut();
                $('#ReloadAnimalInvestigationContainer').html(responce);
            },
        });
    }
    return false;
}

function RequestInvestigationTypeViewSettings(RequestInvestigationTypeId) {
    var postArray = "RequestInvestigationTypeId=" + RequestInvestigationTypeId

    $.ajax({
        type: 'POST',
        url: gRootUrl + "Sample/RequestInvestigationTypeViewSettings/",
        data: postArray,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                var invs = $("[name=Investigations]").val() == null ? '0' : $("[name=Investigations]").val();
                window.open(gRootUrl + "LabControl/RequestInvestigationType/" + RequestInvestigationTypeId + "/" + invs);
                return true;
            }, function () {
                return true;
            }, "OK", "Close");
        }
    });
    return false;
}
