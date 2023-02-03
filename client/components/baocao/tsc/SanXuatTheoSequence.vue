<template>
  <div>
    <div>
      <div>Xem trên: </div>
      <b-form-group v-slot="{ ariaDescribedby }">
        <b-form-radio-group v-model="selected" :options="options" :aria-describedby="ariaDescribedby"
        name="radio-inline"></b-form-radio-group>
      </b-form-group>
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
      tscName: 'tsc1',
      userLocal: null,
      fromDay: null,
      toDay: null,
      chartDataAPI: [],
      listTotalWeight: [],
      listSequence: [],
      customData: null,
      load: false,
      selected: "tsc1",
      options: [
        { text: "TSC 1", value: "tsc1" },
        { text: "TSC 2", value: "tsc2" },
      ],
      selectedMonth: null,
      months: [],
      chartData: {
        datasets: [
          {
            label: "Sản lượng (Nghìn tấn)",
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
                // return (value/10000).toString().replace(/\B(?=(\d{2})+(?!\d))/g, ",");
                let str = (value/100).toString().replace(/\B(?=(\d{2})+(?!\d))/g, ",");
                return str.replace(str, str.slice(0,4));
              }
              if (context.datasetIndex == datasetArray.length - 1) {
                return (value/10).toFixed().toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
              } else if (context.datasetIndex == 1 && (context.dataIndex == (context.chart.data.datasets[1].data.length - 1))) {
                return (value/10).toFixed().toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
              } else {
                return '';
              }
            },
          },
        },
        title: {
          display: true,
          text: "SẢN LƯỢNG SEQUENCE THEO THÁNG" ,
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
  watch: {
    selected(value) {
      this.load = false;
      if (value) {
        this.tscName = value;
        this.getDataChart(value);
      } 
    },
    selectedMonth(value) {
      this.load = false;
      if(value) {
        this.fromDay = moment(value).startOf("month").valueOf();
        this.toDay = moment(value).endOf("month").valueOf();
        this.getDataChart(this.tscName);
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
    async getDataChart(tscName) {
      this.listSequence = [];
      this.customData = [];
      this.listTotalWeight =[];

      const { data } = await this.$axios.get("/api/sanxuattheosequence/get?from=" + this.fromDay + "&to=" + this.toDay);
      
      data.forEach(e => {
        if(e.SEQ_HEAT_COUNTER == 1 
            && moment(e.LADLE_OPENING_DATE).valueOf() >= this.fromDay + 8*3600*1000
            && moment(e.LADLE_OPENING_DATE).valueOf() < this.toDay + 8*3600*1000
          ) {
          this.listSequence.push(e.SEQ_COUNTER);
        }
      });

      if(tscName == 'tsc2') {
        this.customData = data.filter(res =>  res.SEQ_COUNTER / 1000 < 2);
        this.listSequence = this.listSequence.filter(res =>  res / 1000 < 2);
      } else {
        this.customData = data.filter(res =>  res.SEQ_COUNTER  / 1000 >= 2);
        this.listSequence = this.listSequence.filter(res =>  res / 1000 >= 2);
      }
      
      this.listSequence.forEach((item) => {
        let dataFilter = this.customData
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

      this.chartData.datasets[1].data = [];
      this.chartData.datasets[1].data = listTotal;

      this.chartData.labels = [];
      this.chartData.labels = this.listSequence;

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
