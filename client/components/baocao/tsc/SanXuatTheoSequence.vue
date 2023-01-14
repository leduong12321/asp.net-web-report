<template>
  <div>
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
      chartDataAPI: [],
      listTotalWeight: [],
      listSequence: [],
      load: false,
      chartData: {
        datasets: [
          {
            label: "Sản lượng TSC1",
            data: [],
            yAxisID: "y1",
            backgroundColor: "#ed7d31",
            order: 2,
          },
          // {
          //     label: 'Sản lượng TSC2',
          //     data: [15, 19, 24, 34, 5],
          //     yAxisID: 'y1',
          //     backgroundColor: '#ed7d31',
          //     order: 2,
          // },
          {
            label: "Tổng sản lượng",
            data: [],
            type: "line",
            yAxisID: "y2",
            backgroundColor: "rgba(255, 26, 104, 0)",
            borderColor: "#f6bb1a",
            order: 1,
          },
        ],
        labels: [],
      },
      plugins: [ChartDataLabels],
      chartOptions: {
        plugins: {
          datalabels: {
            anchor: "end",
            align: "top",
            font: {
              weight: "bold",
            },
            formatter: (value, context) => {
              const datasetArray = [];
              const dataset = context.chart.data.datasets[0];
              if (dataset.data[context.dataIndex] != undefined) {
                datasetArray.push(dataset.data[context.dataIndex]);
              }

              if (context.datasetIndex == datasetArray.length - 1) {
                return value.toLocaleString("en-IN", {
                  minimumFractionDigits: 1,

                });
              }
            },
          },
        },
        title: {
          display: true,
          text: "SẢN LƯỢNG THEO SEQUENCE",
          fontSize: 15,
        },
        tooltips: {
          mode: "index",
        },
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          xAxes: [
            {
              position: "bottom",
              tooltips: {
                mode: "index",
              },
              title: {
                display: true,
                text: "Date",
              },
              gridLines: {
                lineWidth: 0
              },
            },
          ],
          yAxes: [
            {
              id: "y1",
              type: "linear",
              position: "left",
              ticks: {
                beginAtZero: true,
              },
              gridLines: {
                display: false,
              },
            },
            {
              id: "y2",
              type: "linear",
              position: "right",
              ticks: {
                beginAtZero: true,
              },
              gridLines: {},
            },
          ],
        },
        legend: {
          position: "bottom",
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
    this.userLocal = JSON.parse(localStorage.getItem("user"));
    this.getDataChart();
  },
  methods: {
    unique(arr) {
      return Array.from(new Set(arr)); //
    },
    async getDataChart() {
      const { data } = await this.$axios.get("/api/SanXuatTheoSequence/get/");
      
      data.forEach(e => {
        if(e.SEQ_HEAT_COUNTER == 1 
            && moment(e.LADLE_OPENING_DATE).valueOf() >= moment('2022-11-15T17:50:42').startOf("month").valueOf() - 4*3600*1000
            && moment(e.LADLE_OPENING_DATE).valueOf() < moment('2022-11-15T17:50:42').endOf("month").valueOf() - 20*3600*1000
          ) {
          this.listSequence.push(e.SEQ_COUNTER);
        }
      });

      this.listSequence.forEach((item) => {
        let dataFilter = data
          .filter((res) => res.SEQ_COUNTER == item)
          .reduce((total, currentValue) => {
            return (
                parseInt(total + (currentValue.LADLE_OPENING_WGT - currentValue.LADLE_CLOSE_WGT - currentValue.TUNDISH_SKULL_WGT))
            );
          }, 0);
        this.listTotalWeight.push(dataFilter);
      });

      this.changeDataChart();
    },
    changeDataChart() {
      let total = 0;
      let listTotal = [];
      for (let i = 0; i < this.listTotalWeight.length; i++) {
        total = total + this.listTotalWeight[i];
        listTotal.push(total);
      }

      this.chartData.datasets[0].data = [];
      this.chartData.datasets[0].data = this.listTotalWeight;

    //   this.chartData.datasets[1].data = [];
    //   this.chartData.datasets[1].data = listTotal;

      this.chartData.labels = [];
      this.chartData.labels = this.listSequence;

      // this.chartOptions.scales.yAxes[0].ticks.max = (total[total.length - 1])*1.5
      this.load = true;
      console.log("chartdata", this.chartData);
    },
  },
};
</script>

<style lang="scss">
.chart-div {
  padding-top: 20px;
}
</style>
