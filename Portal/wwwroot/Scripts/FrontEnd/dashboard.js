// request permission on page load
document.addEventListener('DOMContentLoaded', function () {
    if (Notification.permission !== "granted")
        Notification.requestPermission();
});

var gscreenwidth;
var gavailbale_width;
var gscreenheight;
var gavailbale_height;
var gchart_height;
var gsvg;
var gpie;
var garc;
var gouterArc;
var gChartkey;
var gChartRadius;
var gChartDataArray;
var gChartlabels;
var gheightoffset = 185;

$(document).ready(function () {
    if ($(".dashboard-diag").size() > 0) {
        gscreenheight = $(window).height();
        gscreenwidth = $(window).width();
        gavailbale_width = gscreenwidth - 810;
        if (gavailbale_width < 500) {
            gavailbale_width = gscreenwidth - 40;
            gavailbale_height = gscreenheight;
            $(".dashboard-diag").addClass("dashboard-diag-center");
            $("#dashboard_diag_separator").show();
            if ($(".table-widget").size() > 0) {
                $(".dashboard-diag").find(".dashboard-ico").hide();
                $(".dashboard-icons-top").show();
            }
        }
        else {
            $(".dashboard-diag").width(gscreenwidth - 715 - 57);
            gavailbale_height = gscreenheight - 115 - 105;
        }

        if (gavailbale_height > 440) {
            if ($(".table-widget").find(".data-grid-container").hasClass("data-grid-container-half"))
            {
                $(".table-widget").find(".data-grid-container").css('max-height', gavailbale_height/2 + 'px');
            }
            else
            {
                $(".table-widget").find(".data-grid-container").css('max-height', gavailbale_height + 'px');
            }
        }

        load_dashboardDiagramm();

        var gdashboardtype = 1;
        if (gdashboardtype != 1) {
            window.settimeout(function () { refreshdashboard(); }, 5 * 1000);
        }
    }
});

function refreshDashboard() {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "DashBoard/Refresh/" + gLatestDashboardId + "/" + gDashboardItemsCount,
        data: $(".table-widget").serialize(),
        async: true,
        dataType: "JSON",
        success: function (responce) {
            if ($.trim(responce["Data"]["List"]) != "") {
                $(responce["Data"]["List"]).insertBefore(".data-grid-data-row:first").hide().show('slow');
                gLatestDashboardId = responce["Data"]["ItemId"];
                gDashboardItemsCount = responce["Data"]["ItemCount"];
                if (!Notification) {
                    return;
                }
            }

            if (responce["Data"]["Notification"] != null) {
                if (Notification.permission !== "granted")
                    Notification.requestPermission();
                else {
                    var notification = new Notification(responce["Data"]["Notification"]["Title"], {
                        icon: responce["Data"]["Notification"]["Thumb"],
                        body: responce["Data"]["Notification"]["Body"],
                    });

                    notification.onclick = function () {
                        window.open(responce["Data"]["Notification"]["URL"]);
                        notification.close();
                    };

                }
            }

            if (responce["Data"]["ChangedDataList"] != null) {
                for (var i = 0; i < responce["Data"]["ChangedDataList"].length; i++) {
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-1");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-2");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-3");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-4");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-5");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-6");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-7");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-8");
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).removeClass("data-grid-data-row-type-9");

                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).addClass("data-grid-data-row-type-" + responce["Data"]["ChangedDataList"][i].StatusId);
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).find("[name=widgetitems]").val(responce["Data"]["ChangedDataList"][i].Id + ":" + responce["Data"]["ChangedDataList"][i].StatusId);
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).find("#samplename").html(responce["Data"]["ChangedDataList"][i].Sample);
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).find(".data-grid-data-invtypes").html(responce["Data"]["ChangedDataList"][i].Investigations);
                    $(".data-grid-data-row-" + responce["Data"]["ChangedDataList"][i].Id).find(".data-grid-data-status").html(responce["Data"]["ChangedDataList"][i].StatusName);
                }
            }

            if (responce["Data"]["RemoveDataList"] != null) {
                for (var i = 0; i < responce["Data"]["RemoveDataList"].length; i++) {
                    var pId=responce["Data"]["RemoveDataList"][i].Id;
                    $(".data-grid-data-row-" + pId).fadeOut("slow", function () {
                        $(this).remove();
                    })
                }
            }

            if (responce["Data"]["DataList"] != null) {
                for (var i = 0; i < responce["Data"]["DataList"].length; i++) {
                    $(".data-grid-data-row-" + responce["Data"]["DataList"][i].Id).find("#samplename").html(responce["Data"]["DataList"][i].Sample);
                    $(".data-grid-data-row-" + responce["Data"]["DataList"][i].Id).find(".data-grid-data-invtypes").html(responce["Data"]["DataList"][i].Investigations);
                }
            }
            if (responce["Data"]["ChartData"] != null) {
                eval(responce["Data"]["ChartData"]);
                change_dashboardDiagramm(load_dashboardDiagrammData());
            }

            window.setTimeout(function () { refreshDashboard(); }, 5 * 1000);
        },
        error: function () {
            //tbd
        },
    });

}

function load_dashboardDiagramm() {
    gchart_height = gavailbale_height - 62 - gheightoffset;
    gsvg = d3.select(".dashboard-diag-container")
    .append("svg").attr("width", gavailbale_width).attr("height", gchart_height)
    .append("g")

    gsvg.append("g")
        .attr("class", "slices");
    gsvg.append("g")
        .attr("class", "labels");
    gsvg.append("g")
        .attr("class", "lines");

    var width = gavailbale_width,
        height = gchart_height;

    gChartRadius = Math.min(width, height) / 2;

    gpie = d3.layout.pie()
        .sort(null)
        .value(function (d) {
            return d.value;
        });

    garc = d3.svg.arc()
        .outerRadius(gChartRadius * 0.8)
        .innerRadius(gChartRadius * 0.4);

    gouterArc = d3.svg.arc()
        .innerRadius(gChartRadius * 0.9)
        .outerRadius(gChartRadius * 0.9);

    gsvg.attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    gChartkey = function (d) { return d.data.Id; };

    change_dashboardDiagramm(load_dashboardDiagrammData());
}

function change_dashboardDiagramm(data) {
    /* ------- PIE SLICES -------*/
    var slice = gsvg.select(".slices").selectAll("path.slice").data(gpie(data), gChartkey);

    slice.enter()
        .insert("path")
        .style("fill", function (d) { return gChartDataArray(d.data.Id); })
        .attr("onclick", function (d) { return "drill_down('" + d.data.Id + "')"; })
        .attr("class", "slice diagramm-link-slice");

    slice
        .transition().duration(1000)
        .attrTween("d", function (d) {
            this._current = this._current || d;
            var interpolate = d3.interpolate(this._current, d);
            this._current = interpolate(0);
            return function (t) {
                return garc(interpolate(t));
            };
        });

    /* ------- TEXT LABELS -------*/

    var text = gsvg.select(".labels").selectAll("text").data(gpie(data), gChartkey);

    text.enter()
        .append("text")
        .attr("onclick", "drill_down($(this).attr('value'))")
        .attr("class", "diagramm-link-label")
        .attr("dy", ".35em")
        .attr("value", function (d) {
            return d.data.Id;
        });

    text.text(function (d) { return latinizeDecode(d.data.label); });

    function midAngle(d) {
        return d.startAngle + (d.endAngle - d.startAngle) / 2;
    }

    text.transition().duration(1000)
        .attrTween("transform", function (d) {
            this._current = this._current || d;
            var interpolate = d3.interpolate(this._current, d);
            this._current = interpolate(0);
            return function (t) {
                var d2 = interpolate(t);
                var pos = gouterArc.centroid(d2);
                pos[0] = gChartRadius * (midAngle(d2) < Math.PI ? 1 : -1);
                return "translate(" + pos + ")";
            };
        })
        .styleTween("text-anchor", function (d) {
            this._current = this._current || d;
            var interpolate = d3.interpolate(this._current, d);
            this._current = interpolate(0);
            return function (t) {
                var d2 = interpolate(t);
                return midAngle(d2) < Math.PI ? "start" : "end";
            };
        });

    slice.exit()
        .remove();
    text.exit()
        .remove();

    /* ------- SLICE TO TEXT POLYLINES -------*/

    var polyline = gsvg.select(".lines").selectAll("polyline")
        .data(gpie(data), gChartkey);

    polyline.enter()
        .append("polyline");

    polyline.transition().duration(1000)
        .attrTween("points", function (d) {
            this._current = this._current || d;
            var interpolate = d3.interpolate(this._current, d);
            this._current = interpolate(0);
            return function (t) {
                var d2 = interpolate(t);
                var pos = gouterArc.centroid(d2);
                pos[0] = gChartRadius * 0.95 * (midAngle(d2) < Math.PI ? 1 : -1);
                return [garc.centroid(d2), gouterArc.centroid(d2), pos];
            };
        });

    polyline.exit()
        .remove();
};

function load_dashboardDiagrammData() {
    gChartlabels = gChartDataArray.domain();
    return gChartlabels.map(function (label) {
        var label_arr = label.split(":");
        return { label: label_arr[0] + "(" + label_arr[1] + ")", value: label_arr[1], Id: label_arr[2] }
    });
}

function drill_down(id) {

    if (gDashboardType == 1) {
        window.open(gRootUrl + 'Report/UserList/Role/Lib.BusinessObjects/' + id);
    }

    if (gDashboardType == 2) {
        window.open(gRootUrl + 'Report/RequestRegister/RequestStatus/SecurityCRMLib.BusinessObjects/' + id);
    }

    //if (gDashboardType == 3) {
    //    window.open(gRootUrl + 'Report/SectionPage/RequestStatus/SecurityCRMLib.BusinessObjects/' + id);
    //}

    if (gDashboardType == 5) {
        window.open(gRootUrl + 'Report/ANSARequestRegister/NotValid/null/' + (id == 0 ? "false" : "true"));
    }

    if (gDashboardType == 6) {
        window.open(gRootUrl + 'Report/ProtocolRegister/ProtocolStatus/SecurityCRMLib.BusinessObjects/' + id);
    }

    if (gDashboardType == 7) {
        window.open(gRootUrl + 'Report/RequestInvestigationTypeList/RequestStatus/SecurityCRMLib.BusinessObjects/' + id);
    }
}

function show_dashboard_tab(ind) {
    $(".wiget-tab-container").hide();
    $(".wiget-tab-container-" + ind).show();
}