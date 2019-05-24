"use strict";
class YearAndMonth {
    constructor(year, month) {
        this.year = year;
        this.month = month;
    }
}
class Graph {
}
var main_graph = {
    svg: undefined,
    line: undefined,
    series: [
        { name: '燃料', color: 'green', data: [], path: undefined },
        { name: '弾薬', color: 'chocolate', data: [], path: undefined },
        { name: '鋼材', color: 'gray', data: [], path: undefined },
        { name: 'ボーキサイト', color: 'orange', data: [], path: undefined }
    ],
};
var screw_graph = {
    svg: undefined,
    line: undefined,
    series: [
        { name: '改修資材', color: 'gray', data: [], path: undefined }
    ],
};
class Basedata {
    constructor() {
        this.range = {
            start: new YearAndMonth((new Date()).getFullYear(), (new Date()).getMonth() + 1),
            end: new YearAndMonth((new Date()).getFullYear(), (new Date()).getMonth() + 1)
        };
        this.lock = false;
        this.lock2 = false;
        this.collection = {};
        this.collection2 = {};
    }
    expand() {
        var before = this.getBefore();
        this.fetch(before.year, before.month, function () {
            basedata.range.start = before;
            update_from_base_data();
        });
        this.fetch2(before.year, before.month, function () {
            basedata.range.start = before;
            update_from_base_data2();
        });
    }
    ;
    add(year, month, array) {
        this.collection[String(year) + ('00' + month).slice(-2)] = array;
    }
    add2(year, month, array) {
        this.collection2[String(year) + ('00' + month).slice(-2)] = array;
    }
    getUri(year, month) {
        return "Materials?type=json&target=main&range=ym" + year + ('00' + month).slice(-2);
    }
    getUri2(year, month) {
        return "Materials?type=json&target=screw&range=ym" + year + ('00' + month).slice(-2);
    }
    fetch(year, month, callback) {
        if (this.lock)
            return;
        this.lock = true;
        d3.json(this.getUri(year, month)).then(function (data) {
            basedata.add(year, month, data);
            callback();
            basedata.lock = false;
        }).catch(function (error) {
            console.log("there was an error loading the data: " + error);
            basedata.lock = false;
        });
        // TODO: URL generation
        // add
    }
    fetch2(year, month, callback) {
        if (this.lock2)
            return;
        this.lock2 = true;
        d3.json(this.getUri2(year, month)).then(function (data) {
            basedata.add2(year, month, data);
            callback();
            basedata.lock2 = false;
        }).catch(function (error) {
            console.log("there was an error loading the data: " + error);
            basedata.lock2 = false;
        });
        // TODO: URL generation
        // add
    }
    getBefore() {
        if (this.range.start !== null) {
            var start = this.range.start;
            if (start.month === 1) {
                return new YearAndMonth(start.year - 1, 12);
            }
            else {
                return new YearAndMonth(start.year, start.month - 1);
            }
        }
        else {
            console.log('range.start is null');
            return null;
        }
    }
    getAfter() {
        if (this.range.end !== null) {
            var end = this.range.end;
            if (end.month === 12) {
                return {
                    year: end.year + 1,
                    month: 1
                };
            }
            else {
                return {
                    year: end.year,
                    month: end.month + 1
                };
            }
        }
        else {
            console.log('range.end is null');
            return null;
        }
    }
}
var basedata = new Basedata();
function update_from_base_data() {
    var increment = function (ym) {
        if (ym.month === 12) {
            return {
                year: ym.year + 1,
                month: 1
            };
        }
        else {
            return {
                year: ym.year,
                month: ym.month + 1
            };
        }
    };
    { // Main Graph
        var series = main_graph.series;
        var datatemp = [];
        for (var i = 0; i < series.length; i++) {
            series[i].data = [];
            datatemp[i] = [];
        }
        for (var target = basedata.range.start; target.year < basedata.range.end.year ||
            (target.year === basedata.range.end.year && target.month <= basedata.range.end.month); target = increment(target)) {
            var base = basedata.collection[String(target.year) + ('00' + target.month).slice(-2)];
            for (var i = 0; i < series.length; i++) {
                datatemp[i].push(base[i]["values"]);
            }
        }
        for (var i = 0; i < series.length; i++) {
            series[i].data = d3.merge(datatemp[i]);
            series[i].path.datum(series[i].data).attr("d", main_graph.line);
        }
    }
}
function update_from_base_data2() {
    var increment = function (ym) {
        if (ym.month === 12) {
            return {
                year: ym.year + 1,
                month: 1
            };
        }
        else {
            return {
                year: ym.year,
                month: ym.month + 1
            };
        }
    };
    { // Screw Graph
        var series = screw_graph.series;
        var datatemp = [];
        for (var i = 0; i < series.length; i++) {
            datatemp[i] = [];
        }
        for (var target = basedata.range.start; target.year < basedata.range.end.year ||
            (target.year === basedata.range.end.year && target.month <= basedata.range.end.month); target = increment(target)) {
            var base = basedata.collection2[String(target.year) + ('00' + target.month).slice(-2)];
            for (var i = 0; i < series.length; i++) {
                datatemp[i].push(base[i]["values"]);
            }
        }
        for (var i = 0; i < series.length; i++) {
            series[i].data = d3.merge(datatemp[i]);
            series[i].path.datum(series[i].data).attr("d", screw_graph.line);
        }
    }
}
function create_graph(data, selector, graph) {
    graph.svg = d3.select(selector);
    var legendHeight = 30;
    var series = graph.series;
    var legendG = graph.svg.append('g').attr('class', 'legend-g')
        .attr('transform', 'translate(0, ' + legendHeight / 2.0 + ')');
    var legendOffset = 10;
    for (var i = 0; i < series.length; i++) {
        var sg = legendG.append('g')
            .attr('transform', 'translate(' + legendOffset + ',0)');
        sg.append('circle').attr('r', 8).style('fill', series[i].color);
        sg.append('text')
            .attr('x', 10)
            .attr('dominant-baseline', 'middle').text(series[i].name);
        legendOffset += sg.node().getBBox().width + 10.0;
    }
    var graphG = graph.svg.append('g').attr('class', 'graph-g')
        .attr('transform', 'translate(0, ' + legendHeight + ')');
    var svgWidth = Number(graph.svg.attr('width'));
    var svgHeight = Number(graph.svg.attr('height'));
    var width = svgWidth;
    var height = svgHeight - legendHeight;
    var x = d3.scaleTime().range([0, width]);
    var y = d3.scaleLinear().range([height, 0]);
    var maxValue = 0;
    var latestTime = 0;
    for (var i = 0; i < series.length; i++) {
        maxValue = Math.max(maxValue, d3.max(data[i]["values"], function (d) { return d["value"]; }));
        latestTime = Math.max(latestTime, d3.max(data[i]["values"], function (d) { return new Date(d["time"]); }).getTime());
    }
    x.domain([new Date(data[0]["values"][0]["time"]), new Date(latestTime)]);
    y.domain([0, maxValue]);
    var formatMillisecond = d3.timeFormat(".%L"), formatSecond = d3.timeFormat(":%S"), formatMinute = d3.timeFormat("%H:%M"), formatHour = d3.timeFormat("%H"), formatDay = d3.timeFormat("%a %d"), formatWeek = d3.timeFormat("%m/%d"), formatMonth = d3.timeFormat("%m月"), formatYear = d3.timeFormat("%Y年");
    function multiFormat(date) {
        return (d3.timeSecond(date) < date ? formatMillisecond
            : d3.timeMinute(date) < date ? formatSecond
                : d3.timeHour(date) < date ? formatMinute
                    : d3.timeDay(date) < date ? formatHour
                        : d3.timeMonth(date) < date ? (d3.timeWeek(date) < date ? formatDay : formatWeek)
                            : d3.timeYear(date) < date ? formatMonth
                                : formatYear)(date);
    }
    var xAxis = d3.axisBottom(x).tickSize(height).tickPadding(6).tickFormat(multiFormat);
    var yAxis = d3.axisRight(y).tickSize(width).tickPadding(6);
    var gX = graphG.append("g")
        .attr("class", "axis axis-x")
        .call(xAxis);
    var gY = graphG.append("g")
        .attr("class", "axis axis-y")
        .call(yAxis);
    var x2 = x.copy();
    graph.line = d3.line()
        .x(function (d) {
        return x2(new Date(d["time"]));
    })
        .y(function (d) {
        return y(d["value"]);
    })
        .curve(d3.curveStepAfter);
    function zoomed() {
        x2 = d3.event.transform.rescaleX(x);
        gX.call(xAxis.scale(x2));
        for (var i = 0; i < graph.series.length; i++) {
            graph.series[i].path.attr("d", graph.line);
        }
        if (d3.timeDay.count(new Date(basedata.range.start.year, basedata.range.start.month - 1), x2.domain()[0]) < 3) {
            basedata.expand();
        }
    }
    function zoomend() {
        var maxValue = 0;
        var domain = x2.domain();
        for (var i = 0; i < series.length; i++) {
            var vdata = series[i].data.filter(function (value) {
                var date = new Date(value["time"]);
                return domain[0] <= date && date <= domain[1];
            });
            maxValue = Math.max(maxValue, d3.max(vdata, function (d) { return d["value"]; }));
        }
        y.domain([0, maxValue]);
        zoomed();
    }
    if (selector == "#main_chart") {
        basedata.add(basedata.range.start.year, basedata.range.start.month, data);
    }
    else {
        basedata.add2(basedata.range.start.year, basedata.range.start.month, data);
    }
    for (var i = 0; i < graph.series.length; i++) {
        graph.series[i].path = graphG.append("path");
        var path = graph.series[i].path;
        path.attr("class", "line line-" + String(i));
        path.attr("stroke", graph.series[i].color);
        path.datum(data[i]["values"]).attr("d", graph.line);
    }
    var zoom = d3.zoom().on("zoom", zoomed)
        .on("end", zoomend)
        .scaleExtent([0.2, 10])
        .translateExtent([[-Infinity, 0], [width, 0]]);
    graph.svg.call(zoom).on("wheel", function () { d3.event.preventDefault(); });
}
//# sourceMappingURL=graph.js.map