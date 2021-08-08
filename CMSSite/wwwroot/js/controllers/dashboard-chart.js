const randomColor = () => {
    const hex = Math.random().toString(16).substr(-6);
    return "#" + hex;
};

const generateArrayOfColor = (number) => {
    const res = [];
    if (number) {
        for (let i = 0; i < number; i++) {
            const color = randomColor();
            res.push(color);
        }
    }
    return res;
};

const donutColors = [
    "#4E6AFA",
    "#FE5959",
    "#8B96FF",
    "#17CA89",
    "#FFBE33",
];
// const donutColors = generateArrayOfColor(5);

const mixedColors = {
    bar: "#355E9A",
    line: "#15D79D",
};

const mixedLegendPlugin = {
    id: "mixed-legend",
    afterUpdate(chart, args, options) {
        const parent = document.getElementById("mixed-legend");
        while (parent.firstChild) {
            parent.firstChild.remove();
        }
        const items =
            chart.options.plugins.legend.labels.generateLabels(chart);
        items.forEach((item) => {
            const child = document.createElement("div");
            child.classList.add("legend-item");

            child.onclick = () => {
                const { type } = chart.config;
                if (type === "pie" || type === "doughnut") {
                    // Pie and doughnut charts only have a single dataset and visibility is per item
                    chart.toggleDataVisibility(item.index);
                } else {
                    chart.setDatasetVisibility(
                        item.datasetIndex,
                        !chart.isDatasetVisible(item.datasetIndex)
                    );
                }
                chart.update();
            };

            const boxColor = document.createElement("div");
            boxColor.classList.add("box-color");
            boxColor.style.background = item.fillStyle;

            const textContainer = document.createElement("p");
            const text = document.createTextNode(item.text);
            textContainer.style.textDecoration = item.hidden
                ? "line-through"
                : "";
            textContainer.appendChild(text);

            child.appendChild(boxColor);
            child.appendChild(textContainer);
            parent.appendChild(child);
        });
    },
};

const donutLegendPlugin = {
    id: "donut-legend",
    afterUpdate(chart, args, options) {
        const parent = document.getElementById("donut-legend");
        while (parent.firstChild) {
            parent.firstChild.remove();
        }
        const items =
            chart.options.plugins.legend.labels.generateLabels(chart);
        items.forEach((item, index) => {
            const child = document.createElement("div");
            child.classList.add("legend-item");

            child.onclick = () => {
                const { type } = chart.config;
                if (type === "pie" || type === "doughnut") {
                    // Pie and doughnut charts only have a single dataset and visibility is per item
                    chart.toggleDataVisibility(item.index);
                } else {
                    chart.setDatasetVisibility(
                        item.datasetIndex,
                        !chart.isDatasetVisible(item.datasetIndex)
                    );
                }
                chart.update();
            };

            const boxColor = document.createElement("div");
            boxColor.classList.add("box-color");
            boxColor.style.background = item.fillStyle || "black";

            const textContainer = document.createElement("p");
            textContainer.classList.add("text");
            const text = document.createTextNode(item.text);
            textContainer.style.textDecoration = item.hidden
                ? "line-through"
                : "";
            textContainer.appendChild(text);

            const valueContainer = document.createElement("p");
            const data = chart.data.datasets[0].data[index];
            valueContainer.classList.add("value");
            valueContainer.setAttribute("data-number", data);
            valueContainer.classList.add("counting");
            valueContainer.setAttribute("id", "donut" + index + "counting");
            valueContainer.setAttribute("data-suffix", "%");
            const value = document.createTextNode(
                chart.data.datasets[0].data[index] + "%"
            );
            valueContainer.appendChild(value);

            child.appendChild(boxColor);
            child.appendChild(textContainer);
            child.appendChild(valueContainer);
            parent.appendChild(child);
        });
    },
};

const donutTooltipHandler = (ctx) => {
    const { tooltip } = ctx;
    const tooltipEl = document.getElementById("donut-tooltip");
    while (tooltipEl.firstChild) {
        tooltipEl.firstChild.remove();
    }
    if (tooltip.opacity === 0) {
        tooltipEl.style.opacity = 0;
        return;
    }
    if (tooltip.body) {
        const valueContainer = document.createElement("span");
        const value = document.createTextNode(tooltip.dataPoints[0].parsed);
        valueContainer.appendChild(value);
        valueContainer.classList.add("number");

        const percentageSign = document.createElement("span");
        percentageSign.innerHTML = "%";
        percentageSign.classList.add("percentage");

        tooltipEl.appendChild(valueContainer);
        tooltipEl.appendChild(percentageSign);
        tooltipEl.style.opacity = 1;
    }
};

const horizontalBarOptions = {
    // update horizontal
    responsive: true,
    maintainAspectRatio: false,
    // end update horizontal
    indexAxis: "y",
    plugins: {
        legend: {
            display: false,
        },
        // update
        datalabels: {
            color: 'black',
            display: true,
            font: {
                weight: 400,
                size: 10,
                lineHeight: 1.3
            },
            align: 'end',
            anchor: 'start',
            offset: 10,
            textAlign: 'center'
        }
      // end update
    },
    scales: {
        x: {
            grid: {
                display: false,
            },
        },
        y: {
            grid: {
                display: false,
            },
            // update horizontal
            ticks: {
                autoSkip: false,
            },
            // end update horizontal
            reverse: true,
        },
    },
};

const donutOptions = (donutTooltipHandler) => {
    return {
        plugins: {
            tooltip: {
                enabled: false,
                position: "nearest",
                external: donutTooltipHandler,
            },
            legend: {
                display: false,
            },
            htmlLegend: {
                containerId: "donut-legend",
            },
        },
    };
};

const mixedOptions = {
    scales: {
        y: {
            beginAtZero: true,
            grid: {
                borderDash: [5, 5],
            },
        },
        x: {
            grid: {
                borderDash: [5, 5],
            },
        },
    },
    plugins: {
        legend: {
            display: false,
        },
        htmlLegend: {
            containerId: "mixed-legend",
        },
    },
};