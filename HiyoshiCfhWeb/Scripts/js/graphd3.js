"use strict";

var basedata = {
    expand: function () {
        var before = this.getBefore();
        this.fetch(before.year, before.month, function () {
            this.range.start = before;
        });
    },
    range: {
        start: {
            year: 2016,
            month: 12
        },
        end: {
            year: 2016,
            month: 12
        }
    },
    collection: {},
    add: function (year, month, array) {
        this.collection[String(year) + String(month)] = array;
    },
    fetch: function (year, month, callback) {
        if (this.lock)
            return;
        this.lock = true;
        d3.json("Materials?type=json&target=main&range=ym" + year + month, function (error, data) {
            if (!error) {
                add(year, month, data);
                this.lock = false;
                callback();
            }
            this.lock = false;
        });
        // TODO: URL generation
        // add
    },
    getBefore: function () {
        if (this.range.start !== null) {
            var start = this.range.start;
            if (start.month === 1) {
                return {
                    year: start.year - 1,
                    month: 12
                };
            } else {
                return {
                    year: start.year,
                    month: start.month - 1
                };
            }
        } else {
            console.log('range.start is null');
            return null;
        }
    },
    getAfter: function () {
        if (this.range.end !== null) {
            var end = this.range.end;
            if (end.month === 12) {
                return {
                    year: end.year + 1,
                    month: 1
                };
            } else {
                return {
                    year: end.year,
                    month: end.month + 1
                };
            }
        } else {
            console.log('range.end is null');
            return null;
        }
    },
    lock: false,
};

function create_graph(data, selector) {
    var svg;
    svg = d3.select(selector);

    var legendHeight = 30;
    var series = [
        { name: '燃料', color: 'green' },
        { name: '弾薬', color: 'chocolate' },
        { name: '鋼材', color: 'gray' },
        { name: 'ボーキサイト', color: 'orange' }
    ];
    var legendG = svg.append('g').attr('class', 'legend-g')
        .attr('transform', 'translate(0, ' + legendHeight / 2.0 + ')');
    var legendOffset = 0;
    for (var i = 0; i < series.length; i++) {
        var sg = legendG.append('g')
            .attr('transform', 'translate(' + legendOffset + ',0)');
        sg.append('circle').attr('r', 8).style('fill', series[i].color);
        sg.append('text')
            .attr('x', 10)
            .attr('dominant-baseline', 'middle').text(series[i].name);
        legendOffset += sg.node().getBBox().width + 10.0;
    }

    var graphG = svg.append('g').attr('class', 'graph-g')
        .attr('transform', 'translate(0, ' + legendHeight + ')');
    var svgWidth = Number(svg.attr('width'));
    var svgHeight = Number(svg.attr('height'));
    var width = svgWidth;
    var height = svgHeight - legendHeight;
    var x = d3.scaleTime().range([0, width]);
    var y = d3.scaleLinear().range([height, 0]);
    var maxValue = Math.max(
        d3.max(data[0]["values"], function (d) { return d["value"]; }),
        d3.max(data[1]["values"], function (d) { return d["value"]; }),
        d3.max(data[2]["values"], function (d) { return d["value"]; }),
        d3.max(data[3]["values"], function (d) { return d["value"]; })
    );
    var latestTime = Math.max(
        d3.max(data[0]["values"], function (d) { return new Date(d["time"]); }),
        d3.max(data[1]["values"], function (d) { return new Date(d["time"]); }),
        d3.max(data[2]["values"], function (d) { return new Date(d["time"]); }),
        d3.max(data[3]["values"], function (d) { return new Date(d["time"]); })
    );
    x.domain([new Date(data[0]["values"][0]["time"]), latestTime]);
    y.domain([0, maxValue]);
    var xAxis = d3.axisBottom(x).tickSize(height).tickPadding(6).tickFormat(
        function (d) {
            return d3.timeFormat("%Y-%m-%d %H:%M:%S")(new Date(d))
        });
    var yAxis = d3.axisRight(y).tickSize(width).tickPadding(6);
    var gX = graphG.append("g")
        .attr("class", "axis axis-x")
        .call(xAxis);
    var gY = graphG.append("g")
        .attr("class", "axis axis-y")
        .call(yAxis);
    var x2 = x.copy()
    var line = d3.line()
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
        pathFuel.attr("d", line);
        pathBull.attr("d", line);
        pathSteel.attr("d", line);
        pathBauxite.attr("d", line);
    }

    var pathFuel = graphG.append("path");
    var pathBull = graphG.append("path");
    var pathSteel = graphG.append("path");
    var pathBauxite = graphG.append("path");
    pathFuel.datum(data[0]["values"]).attr("d", line)
        .attr("class", "line line-fuel");
    pathBull.datum(data[1]["values"]).attr("d", line)
        .attr("class", "line line-bull");
    pathSteel.datum(data[2]["values"]).attr("d", line)
        .attr("class", "line line-steel");
    pathBauxite.datum(data[3]["values"]).attr("d", line)
        .attr("class", "line line-bauxite");

    var zoom = d3.zoom().on("zoom", zoomed).scaleExtent([1, 5]);
    svg.call(zoom);
}
