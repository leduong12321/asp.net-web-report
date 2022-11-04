<template>
  <div>
    <div class="box d-flex jf-content-space-between">
      <div class="align-items-center pl-0 pt-1 fs-13 mb-3">
        Báo cáo > {{ title }}
      </div>
      <div v-if="isHideTextShowChart">
        <div v-if="selected == 'set-manually' && this.toDay - this.fromDay > 2678400000"></div>
        <b-form-checkbox v-model="checked" class="fs-13" name="check-button" switch v-else>
          <span class="view-chart-txt">Xem biểu đồ</span>
        </b-form-checkbox>
      </div>
      <div
        v-b-toggle.sidebar-right
        class="align-items-center pl-0 pt-1 fs-13 mb-3 search-text"
        @click="(isHide = true), count++"
      >
        Tìm kiếm nâng cao
      </div>
      <b-sidebar
        v-if="isHide"
        id="sidebar-right"
        title="Tìm kiếm nâng cao"
        right
        shadow
        backdrop
        class="sidebar-right fs-13"
      >
        <template header-close>
          <div class="p-3 body-head">
            <b-form-group label="Chọn thời gian" v-slot="{ ariaDescribedby }">
              <b-form-radio-group
                v-model="selected"
                :options="options"
                :aria-describedby="ariaDescribedby"
                name="plain-stacked"
                plain
                stacked
              ></b-form-radio-group>
            </b-form-group>

            <div class="pr-0" v-if="isManually">
              <div class="mr-2 text-date mb-1">Ngày bắt đầu:</div>
              <date-picker
                v-model="fromDay"
                type="datetime"
                placeholder="Ngày bắt đầu"
                value-type="timestamp"
                :show-time-panel="showTimePanel"
                @close="handleOpenChange"
                format="DD-MM-YYYY    HH:mm"
                :class="checkError"
                class="date-ui mb-2"
              >
                <template v-slot:footer>
                  <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
                    {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
                  </button>
                </template>
              </date-picker>
              <div class="mr-2 text-date mb-1">Ngày kết thúc:</div>
              <date-picker
                v-model="toDay"
                type="datetime"
                placeholder="Ngày kết thúc"
                value-type="timestamp"
                :show-time-panel="showTimePanel"
                @close="handleOpenChange"
                format="DD-MM-YYYY    HH:mm"
                :class="checkError"
                class="date-ui mb-2"
              >
                <template v-slot:footer>
                  <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
                    {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
                  </button>
                </template>
              </date-picker>
              <p class="text-error-input-date" v-if="errorInputDate.length > 0">
                {{ errorInputDate }}
              </p>
            </div>

            <b-button
              class="mt-4 btn btn-secondary btn-sm btn-search"
              :disabled="errorInputDate.length > 0"
              block
              @click="showTime(), (isHide = false)"
              >Tìm kiếm</b-button
            >
          </div>
        </template>
        <template #footer="{ hide }">
       <div class="d-flex bg-dark text-light align-items-center px-3 py-2">
        <strong class="mr-auto">NM.CT QSP</strong>
        <b-button size="sm" @click="hide">Đóng</b-button>
       </div>
      </template>
      </b-sidebar>
    </div>
    <div :class="{'hidden' : checked}">
      <iframe
      id="iframe"
      v-if="url"
      :src="url"
      width="100%"
      height="700px"
      allowfullscreen
      style="background-color: white"
      class="mb-5 iframe"
    ></iframe>
    </div>
    <div class="chart-div" v-if="checked">
      <div class="time-chart d-flex">
        <div class="txt-date">
          <div>Từ ngày:</div>
          <div>Đến ngày:</div>
        </div>
        <div class="txt-time">
          <div>{{formatDate(this.fromDay)}}</div>
          <div>{{formatDate(this.toDay)}}</div>
        </div>
      </div>
      <Chart v-if="load" :chartData="chartData" :options="chartOptions" />
    </div>
  </div>
</template>

<script>
import moment from "moment";
import Chart from "../chart/Chart.vue";
import { FAKE_DATA} from "../../js/fakeData";

export default {
  name: "common-report-component",
  components: { Chart },
  props: {
    API_URL: String,
    title: String,
  },
  data() {
    return {
      isHideTextShowChart: false,
      checked: false,
      isHide: true,
      showTimePanel: false,
      isManually: false,
      keySubmit: "",
      count: 0,
      countChecked: 0,
      fromDay: null,
      toDay: null,
      date: new Date(),
      errorInputDate: "",
      url: null,
      selected: "today",
      options: [
        { text: "Hôm nay", value: "today" },
        { text: "Tháng này", value: "this-month" },
        { text: "Hôm qua", value: "last-day" },
        { text: "Tháng trước", value: "last-month" },
        { text: "Tuần này", value: "this-week" },
        { text: "Ca này", value: "this-shift" },
        { text: "Tuần trước", value: "last-week" },
        { text: "Ca trước", value: "last-shift" },
        { text: "Tuỳ chỉnh", value: "set-manually" },
      ],

      fakeData: FAKE_DATA,
      chartDataAPI: [],
      load: false,
      chartData: {
        datasets: []
      },
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                type: 'time',
                position: 'bottom',
                ticks: {
                    autoSkip: true,
                    maxTicksLimit: 10
                }
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        legend: {
          position: 'bottom',
          labels: {
            usePointStyle: true,
            boxWidth: 6,
            padding: 20,
          }
        },
      }
    };
  },
  computed: {
    checkError() {
      if (this.fromDay > this.toDay) {
        this.errorInputDate = "Có lỗi khi nhập thời gian";
        return "error-input-date";
      } else if (this.toDay - this.fromDay > 8208000000) {
        this.errorInputDate =
          "Để đảm bảo hệ thống hoạt động ổn định, khoảng cách giữa 2 khoảng thời gian không được quá 90 ngày";
        return "error-input-date";
      } else {
        this.errorInputDate = "";
        return "";
      }
    },
    formatDate() {
      return (value) => {
        return moment(value).format('DD.MM.YYYY   HH:mm')
      }
    },
  },
  watch: {
    selected(value) {
      if(value == "set-manually") {
        this.isManually = true;
        // this.isHideTextShowChart = false;
      } else {
        // this.isHideTextShowChart = true;
        this.isManually = false
      }
    },
    count() {
      if (this.keySubmit != "") {
        this.selected = this.keySubmit;
      } else {
        this.selected = "this-shift";
      }
    },
    checked() {
      this.countChecked++;
      if(this.countChecked == 1) {
        this.getDataChart();
      }
    },
    // watch: {
    //   '$route'(to, from) {
    //     console.log('to', to);
    //     console.log('from', from);
    //   }
    // },
  },
  mounted() {
    this.selected = "this-shift";
    if(this.title == 'Báo cáo sản xuất') {
      this.isHideTextShowChart = true;
    } else {
      this.isHideTextShowChart = false;
    }
    this.showTime();
  },
  methods: {
    toggleTimePanel() {
      this.showTimePanel = !this.showTimePanel;
    },
    handleOpenChange() {
      this.showTimePanel = false;
    },
    
    showTime() {
      let curentDate = parseInt((Date.now() - 3600000)/12/3600000)*12*3600000 + 3600000;
      console.log('curentDate', curentDate);
      if (this.selected != "set-manually") {
        switch (this.selected) {
          case "today":
            this.fromDay = moment().startOf("day").valueOf();
            this.toDay = moment().endOf("day").valueOf();
            break;
          case "last-day":
            this.fromDay = moment()
              .subtract(1, "days")
              .startOf("day")
              .valueOf();
            this.toDay = moment().subtract(1, "days").endOf("day").valueOf();
            break;
          case "this-week":
            this.fromDay = moment().startOf("isoWeek").valueOf();
            this.toDay = moment().endOf("isoWeek").valueOf();
            break;
          case "last-week":
            this.fromDay = moment()
              .subtract(1, "week")
              .startOf("isoWeek")
              .valueOf();
            this.toDay = moment()
              .subtract(1, "week")
              .endOf("isoWeek")
              .valueOf();
            break;
          case "this-month":
            this.fromDay = moment().startOf("month").valueOf();
            this.toDay = moment().endOf("month").valueOf();
            break;
          case "last-month":
            this.fromDay = moment()
              .subtract(1, "month")
              .startOf("month")
              .valueOf();
            this.toDay = moment().subtract(1, "month").endOf("month").valueOf();
            break;
          case "this-shift":
            this.fromDay = curentDate;
            this.toDay = curentDate + 12*3600*1000 - 1;
            break;
          case "last-shift":
            this.fromDay = curentDate - 12*3600*1000;
            this.toDay = curentDate - 1;
            break;
          default:
            this.fromDay = moment().startOf("day").valueOf();
            this.toDay = moment().endOf("day").valueOf();
            break;
        }
      }
      this.handSubmit();
    },
    handSubmit() {
      this.keySubmit = this.selected;
      if (this.fromDay > this.toDay) {
        return;
      }
      this.url = this.API_URL + "?from=" + this.fromDay + "&to=" + this.toDay;
      this.checked = false;
      // this.isHideTextShowChart = true;
      this.countChecked = 0;
    },
    async getDataChart() {
      const {data} = await this.$axios.get("/api/baocaosanxuat/get?from=" + this.fromDay + "&to=" + this.toDay);
      
      if(data) {
        let tsc1 = data.filter(res => res.SLAB_ID.charAt() == '1');
        let tsc2 = data.filter(res => res.SLAB_ID.charAt() == '2');
        this.changeDataChart(tsc1, tsc2);
      }
    },
    changeDataChart(tsc1, tsc2) {
      let chartTargThk1 = {
        label: 'Target Thick 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.TARGET_THICK
          }
        )),
        borderColor: "#FF0000",
        backgroundColor: "#FF0000",
      };

      let chartCrown1 = {
        label: 'Crown 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.AVG_CROWN
          }
        )),
        borderColor: "#11538C",
        backgroundColor: "#11538C",
        hidden: true,
      };

      let chartWedge1 = {
        label: 'Wedge 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.AVG_WEDGE
          }
        )),
        borderColor: "#BCA8EF",
        backgroundColor: "#BCA8EF",
        hidden: true,
      };

      let chartFlatness1 = {
        label: 'Flatnees 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.FLATNEES
          }
        )),
        borderColor: "#00FFFF",
        backgroundColor: "#00FFFF",
        hidden: true,
      };

      let chartSymHeadFlatness1 = {
        label: 'Sym Head Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymHeadFlatness
          }
        )),
        borderColor: "#454B1B",
        backgroundColor: "#454B1B",
        hidden: true,
      };

      let chartSymBodyFlatness1 = {
        label: 'Sym Body Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymBodyFlatness
          }
        )),
        borderColor: "#DF60EB",
        backgroundColor: "#DF60EB",
        hidden: true,
      };

      let chartSymTailFlatness1 = {
        label: 'Sym Tail Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymTailFlatness
          }
        )),
        borderColor: "#117191",
        backgroundColor: "#117191",
        hidden: true,
      };

      let chartASymHeadFlatness1 = {
        label: 'ASym Head Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymHeadFlatness
          }
        )),
        borderColor: "#E0C837",
        backgroundColor: "#E0C837",
        hidden: true,
      };

      let chartASymBodyFlatness1 = {
        label: 'ASym Body Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymBodyFlatness
          }
        )),
        borderColor: "#CADE71",
        backgroundColor: "#CADE71",
        hidden: true,
      };

      let chartASymTailFlatness1 = {
        label: 'ASym Tail Flatness 1',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymTailFlatness
          }
        )),
        borderColor: "#AB483E",
        backgroundColor: "#AB483E",
        hidden: true,
      };

      let chartDSC1 = {
        label: 'DSC1 (bar)',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.DSC1_RAMP1
          }
        )),
        borderColor: "#19FA3A",
        backgroundColor: "#19FA3A",
        hidden: true,
      };

      let chartDSC2 = {
        label: 'DSC2 (bar)',
        data: tsc1.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.DSC2_RAMP1
          }
        )),
        borderColor: "#EB7B07",
        backgroundColor: "#EB7B07",
        hidden: true,
      };
      // TSC 2
      let chartTargThk2 = {
        label: 'Target Thick 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.TARGET_THICK
          }
        )),
        borderColor: 'rgba(255, 0, 0, 1)',
        backgroundColor: 'rgba(255, 255, 255, 1)',
      };

      let chartCrown2 = {
        label: 'Crown 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.AVG_CROWN
          }
        )),
        borderColor: "#11538C",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartWedge2 = {
        label: 'Wedge 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.AVG_WEDGE
          }
        )),
        borderColor: "#BCA8EF",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartFlatness2 = {
        label: 'Flatnees 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.FLATNEES
          }
        )),
        borderColor: "#00FFFF",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartSymHeadFlatness2 = {
        label: 'Sym Head Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymHeadFlatness
          }
        )),
        borderColor: "#454B1B",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartSymBodyFlatness2 = {
        label: 'Sym Body Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymBodyFlatness
          }
        )),
        borderColor: "#DF60EB",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartSymTailFlatness2 = {
        label: 'Sym Tail Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.SymTailFlatness
          }
        )),
        borderColor: "#117191",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartASymHeadFlatness2 = {
        label: 'ASym Head Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymHeadFlatness
          }
        )),
        borderColor: "#E0C837",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartASymBodyFlatness2 = {
        label: 'ASym Body Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymBodyFlatness
          }
        )),
        borderColor: "#CADE71",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      let chartASymTailFlatness2 = {
        label: 'ASym Tail Flatness 2',
        data: tsc2.map(res  => (
          {
            x: res.ROLLING_STOP,
            y: res.ASymTailFlatness
          }
        )),
        borderColor: "#AB483E",
        backgroundColor: 'rgba(255, 255, 255, 1)',
        hidden: true,
      };

      this.chartData.datasets = [];
      this.chartData.datasets.push(
        chartTargThk1, 
        chartCrown1, 
        chartWedge1, 
        chartFlatness1, 
        chartSymHeadFlatness1, 
        chartSymBodyFlatness1, 
        chartSymTailFlatness1, 
        chartASymHeadFlatness1, 
        chartASymBodyFlatness1, 
        chartASymTailFlatness1, 
        chartDSC1, 
        chartDSC2,
        chartTargThk2,
        chartCrown2, 
        chartWedge2, 
        chartFlatness2, 
        chartSymHeadFlatness2, 
        chartSymBodyFlatness2, 
        chartSymTailFlatness2, 
        chartASymHeadFlatness2, 
        chartASymBodyFlatness2, 
        chartASymTailFlatness2, 
        );
      this.load = true;
    },
  },
};
</script>

<style lang="scss">
.text-error-input-date {
  color: #cc0033;
  font-size: 13px;
  text-align: left;
}
.date-ui {
  input {
    border: 1px solid #555;
  }
  &.error-input-date {
    input {
      border: 1px solid #cc0033;
      background-color: #fce4e4;
    }
  }
}
.text-date {
  font-size: 13px;
}
.search-text {
  text-decoration: underline;
  font-style: italic;
  font-weight: bold;
}
.submit {
  color: #11101d;
}
.iframe {
  border: 0px;
  min-height: 800px;
  max-height: calc(100% - 100px);
}
.date-ui {
  .mx-input-wrapper {
    width: 250px;
  }
  input {
    height: 28px;
  }
}
.mx-datepicker-popup {
  left: unset !important;
  right: 54px;
}
.sidebar-right {
  header {
    background-color: #343a40 ;
    height: 60px;
    color: white;
    box-shadow: 0 10px 30px 0 rgb(47 60 74 / 15%);
    button {
      opacity: 1;
    }
    svg {
      color: white;
      font-size: 1.8rem;
    }
    strong {
      font-size: 1rem;
      font-weight: inherit;
    }
  }
  .b-sidebar-body {
    margin-top: 10px;
    .body-head {
      legend {
        font-size: 14px;
      }
    }
  }
  .form-check {
    float: left;
    width: 50%;
    margin-bottom: 6px;
    input,
    label {
      cursor: pointer;
    }
  }
  .active {
    color: #fff !important;
    background-color: #28a745 !important;
    border-color: #28a745 !important;
  }
  .btn-search {
    width: 50%;
    font-size: 14px;
  }
}
.chart-div {
  padding-bottom: 100px;
  background-color: white;
}
.hidden {
  display: none;
}
.time-chart {
  font-size: 11px;
  padding: 20px;
  font-style: italic;
  .txt-date {
    width: 70px;
  }
  .txt-time {
    font-weight: bold;
  }
}
.view-chart-txt {
  padding-top: 3px;
}
</style>
