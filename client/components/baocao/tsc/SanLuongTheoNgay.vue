<template>
    <div>
      <div>
        <b-form-select v-model="selectedMonth" :options="months" class="mb-3"></b-form-select>
      </div>
      <div
        class="chart-div"
        v-if="userLocal?.Name == 'Admin' || userLocal?.Name == 'TSC'"
      >
        <Chart
          class="chart"
          v-if="load"
          :chartData="chartData"
          :options="chartOptions"
        />
      </div>
    </div>
  </template>
  
  <script>
  import moment from "moment";
  import Chart from "../../chart/Chart_Mix.vue";
  import ChartDataLabels from "chartjs-plugin-datalabels";
  
  export default {
    name: "tsc-sequence",
    components: { Chart },
    props: {
      API_URL: String,
      title: String,
    },
    data() {
      return {
        isHideTextShowChart: false,
        userLocal: null,
        fromDay: null,
        toDay: null,
        chartDataAPI: [],
        listTotalWeight: [],
        listSequence: [],
        dataTSC1: null,
        dataTSC2: null,
        load: false,
        selectedMonth: null,
        listDayInMonth: [],
        months: [],
        chartData: {
            datasets: [{
                label: 'Sản lượng thép lỏng TSC1',
                data: [4005595, 1948272, 3685970],
                yAxisID: 'y1',
                stack: '1',
                backgroundColor: '#4472c4',
                order: 2,
            }, {
                label: 'Sản lượng thép lỏng TSC2',
                data: [2005595, 948272, 2685970],
                yAxisID: 'y1',
                stack: '1',
                backgroundColor: '#ed7d31',
                order: 2,
            },
            {
                label: 'Lũy kế tháng (Tấn)',
                data: [],
                type: 'line',
                yAxisID: 'y2',
                backgroundColor: 'rgba(255, 26, 104, 0)',
                borderColor: '#f6bb1a',
                order: 1,
            }],
            labels: []
        },
        plugins: [ChartDataLabels],
        chartOptions: {
            plugins: {
            datalabels: {
                anchor: 'end',
                align: 'top',
                font: {
                weight: 'bold',
                },
                formatter: (value, context) => {
                const datasetArray = [];
                for (let i = 0; i <= 2; i++) {
                    const dataset = context.chart.data.datasets[i]
                    if (dataset.data[context.dataIndex] != undefined) {
                    datasetArray.push(dataset.data[context.dataIndex]);
                    }
                }

                function totalSum(total, datapoint) {
                    return total + datapoint;
                }

                let sum = datasetArray.reduce(totalSum, 0);

                if (context.datasetIndex == datasetArray.length - 1) {
                    return sum.toLocaleString('en-IN', { minimumFractionDigits: 1, maximumFractionDigits: 1 });
                } else if (context.datasetIndex == 3 && (context.dataIndex == (context.chart.data.datasets[3].data.length - 1))) {
                    return value.toLocaleString('en-IN', { minimumFractionDigits: 1, maximumFractionDigits: 1 });
                } else {
                    return '';
                }
                }
            }
            },
            title: {
                display: true,
                text: 'SẢN LƯỢNG THEO NGÀY VÀ LŨY KẾ THÁNG',
                fontSize: 15,
            },
            tooltips: {
                mode: 'index'
            },
            responsive: true,
            maintainAspectRatio: false,
            scales: {
            xAxes: [{
                position: 'bottom',
                tooltips: {
                mode: 'index'
                },
                title: {
                display: true,
                text: 'Date'
                },
                gridLines: {
                lineWidth: 0
                }
            }],
            yAxes: [{
                id: 'y1',
                stacked: true,
                type: 'linear',
                position: 'left',
                ticks: {
                beginAtZero: true,
                // max: 12000,
                // callback: function(value, index, values) {
                //           return value + ' tấn';
                //       }
                },
                gridLines: {
                // lineWidth: 0,
                // display: false
                }
            },
            {
                id: 'y2',
                type: 'linear',
                position: 'right',
                ticks: {
                beginAtZero: true,
                // max: 240000,
                // stepSize: 20000
                },
                gridLines: {
                // lineWidth: 0
                // display: false
                }
            }],
            },
            legend: {
            position: 'bottom',
            labels: {
                fontSize: 15,
                usePointStyle: true,
                boxWidth: 6,
                padding: window.innerWidth / 45,
            },
            },
        },
      };
    },
    watch: {
      selectedMonth(value) {
        this.load = false;
        if(value) {
          this.fromDay = moment(value).startOf("month").valueOf();
          this.toDay = moment(value).endOf("month").valueOf();
          this.getDataChart();
        }
      }
    },
    mounted() {
      this.userLocal = JSON.parse(localStorage.getItem("user"));
      this.getMonths();
    },
    methods: {
      unique(arr) {
        return Array.from(new Set(arr)); 
      },
      getMonths() {
        for (let i = 0; i < 6; i++) {
          this.months.push(
            {
              text: 'Tháng ' + moment().subtract(i, "months").format('MM - YYYY'),
              value: moment().subtract(i, "months").startOf("day")
            }
          )
        }
        this.selectedMonth = this.months[0].value;
        this.fromDay = moment(this.selectedMonth).startOf("month").valueOf();
        this.toDay = moment(this.selectedMonth).endOf("month").valueOf();
      },
      async getDataChart() {
        this.dataTSC1 = [];
        this.dataTSC2 = [];
        this.listTotalWeight =[];
        let weight1 = [];
        let weight2 = [];
        this.listDayInMonth = new Array(moment(this.fromDay).daysInMonth()).fill(null).map((x, i) => moment(this.fromDay).startOf('month').add(i, 'days').format('YYYY-MM-DD'));

        const { data } = await this.$axios.get("/api/sanluongtheongay/get?from=" + this.fromDay + "&to=" + this.toDay);

        let tsc1 = data.filter(res =>  res.SEQ_COUNTER  / 1000 >= 2);
        let tsc2 = data.filter(res =>  res.SEQ_COUNTER / 1000 < 2);

        this.listDayInMonth.forEach((item) => {
        let dataFilter = tsc1.filter((res) => moment(res.PRODUCTION_DATE).format('YYYY-MM-DD') == item).reduce((total, currentValue) => {
              return (
                  parseInt(total + (currentValue.LADLE_OPENING_WGT - currentValue.LADLE_CLOSE_WGT - currentValue.TUNDISH_SKULL_WGT))
              );
            }, 0);
            weight1.push(dataFilter);
        });

        this.listDayInMonth.forEach((item) => {
        let dataFilter = tsc2.filter((res) => moment(res.PRODUCTION_DATE).format('YYYY-MM-DD') == item).reduce((total, currentValue) => {
              return (
                  parseInt(total + (currentValue.LADLE_OPENING_WGT - currentValue.LADLE_CLOSE_WGT - currentValue.TUNDISH_SKULL_WGT))
              );
            }, 0);
            weight2.push(dataFilter);
        });
        this.changeDataChart(weight1, weight2);
      },
      changeDataChart(weight1, weight2) {
        this.chartData.datasets[0].data = [];
        this.chartData.datasets[0].data = weight1;
  
        this.chartData.datasets[1].data = [];
        this.chartData.datasets[1].data = weight2;
  
        this.chartData.labels = [];
        this.chartData.labels = this.listDayInMonth;
        console.log('chartData', this.chartData);
  
        // this.chartOptions.scales.yAxes[0].ticks.max = (total[total.length - 1])*1.5
        this.load = true;
      },
    },
  };
  </script>
  
  <style lang="scss">
  .chart-div {
    padding-top: 20px;
  }
  </style>
  