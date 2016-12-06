"use strict";

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
    x.domain([new Date(data[0]["values"][0]["time"]), new Date()]);
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