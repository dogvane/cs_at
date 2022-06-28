// 绘制K线的数据

function drawKBar(jsonData) {
    var chartDom = document.getElementById('echart_kbar');
    console.log(chartDom)
    console.log(echarts)
    var myChart = echarts.init(chartDom);

    console.log(jsonData);
    let tradeDays = jsonData.map(o => o.tradingDay);
    let bars = jsonData.map(o => [o.o, o.c, o.l, o.h]); // 开盘价,收盘价,最低价,最高价

    let barMin = Math.min(...jsonData.filter(o => o.l != null).map(o => o.l)) * 0.9;  // 展示的区域按照价再取 10% 的范围
    let barMax = Math.max(...jsonData.filter(o => o.h != null).map(o => o.h)) * 1.1;

    console.log(barMin, barMax);

    var option;

    option = {
        xAxis: {
            data: tradeDays,
        },
        yAxis: {
            scale: true,
            splitArea: {
                show: true
            }
        },
        series: [
            {
                type: 'candlestick',
                data: bars
            }
        ]
    };

    option && myChart.setOption(option);
}

