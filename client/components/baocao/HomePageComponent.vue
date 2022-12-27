<template>
  <div>
    <div>
      TRANG CHỦ
    </div>
    <div class="chart-div">
      <Chart class="chart" v-if="load" :chartData="chartData" :options="chartOptions" />
    </div>
  </div>

</template>

<script>
import Chart from "../chart/Chart_Mix.vue";
import ChartDataLabels from "chartjs-plugin-datalabels";

export default {
  name: 'homepage-component',
  components: { Chart },
  props: {
    API_URL: String,
    title: String,
  },
  data() {
    return {
      chartDataAPI: [],
      load: false,
      chartData: {
        datasets: [{
          label: 'Kíp A',
          data: [],
          yAxisID: 'y1',
          stack: '1',
          backgroundColor: '#4472c4',
          order: 2,
        }, {
          label: 'Kíp B',
          data: [],
          yAxisID: 'y1',
          stack: '1',
          backgroundColor: '#ed7d31',
          order: 2,
        },
        {
          label: 'Kíp C',
          data: [],
          yAxisID: 'y1',
          stack: '1',
          backgroundColor: '#a6a6a6',
          order: 2
        },
        {
          label: 'Lũy kế tháng',
          data: [],
          type: 'line',
          yAxisID: 'y2',
          backgroundColor: ['rgba(255, 26, 104, 0)'],
          borderColor: ['#f6bb1a'],
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
                return value
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
              max: 12000,
              // callback: function(value, index, values) {
              //           return value + ' tấn';
              //       }
            },
            gridLines: {
              offsetGridLines: true
            }
          },
          {
            id: 'y2',
            type: 'linear',
            position: 'right',
            ticks: {
              beginAtZero: true,
              max: 240000,
              stepSize: 20000
            },
            grid: {
              display: false,
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
  mounted() {
    this.getDataChart();
  },
  methods: {
    async getDataChart() {
      const { data } = await this.$axios.get("/api/Accumulated/get/");

      this.changeDataChart(data);
    },
    changeDataChart(data) {
      let crewA = data.map(res =>
        res.A == null ? 0 : res.A
      );
      let crewB = data.map(res =>
        res.B == null ? 0 : res.B
      );
      let crewC = data.map(res =>
        res.C == null ? 0 : res.C
      );
      let monthTotal = data.map(res =>
        res.MONTH_TOTAL
      );
      let archivedDate = data.map(res =>
        res.ARCHIVED_DATE_FORMAT
      );

      this.chartData.datasets[0].data = [];
      this.chartData.datasets[0].data = crewA;

      this.chartData.datasets[1].data = [];
      this.chartData.datasets[1].data = crewB;

      this.chartData.datasets[2].data = [];
      this.chartData.datasets[2].data = crewC;

      this.chartData.datasets[3].data = [];
      this.chartData.datasets[3].data = monthTotal;

      this.chartData.labels = [];
      this.chartData.labels = archivedDate;

      console.log('chart', this.chartData);
      this.load = true;
    },
  }
}
</script>

<style lang="scss">
.chart-div {
  padding-top: 20px;

}
.chart {
}
</style>