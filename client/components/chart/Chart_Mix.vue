
<script>
import VueChart from "vue-chartjs";
import Chart from "chart.js";
import ChartDataLabels from "chartjs-plugin-datalabels"

Chart.plugins.register(ChartDataLabels);

const { Line, Bar } = VueChart;

export default {
  extends: Line,
  extends: Bar,
  props: ["chartData", "options"],
  mounted() {
    this.renderChart(this.chartData, this.options);
  },
  watch: {
    'chartData'(to, from) {
      this.renderChart(this.chartData, this.options)
    },
  },
}
</script>>