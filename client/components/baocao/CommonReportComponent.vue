<template>
  <div>
    <div class="box d-flex jf-content-space-between">
      <div class="align-items-center pl-0 pt-1 fs-13 mb-3">
        Báo cáo > {{ title }}
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
                :button-variant="danger"
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
</template>

<script>
import moment from "moment";

export default {
  name: "common-report-component",
  props: {
    API_URL: String,
    title: String,
  },
  data() {
    return {
      isHide: true,
      showTimePanel: false,
      isManually: false,
      keySubmit: "",
      count: 0,
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
  },
  watch: {
    selected(value) {
      value == "set-manually"
        ? (this.isManually = true)
        : (this.isManually = false);
    },
    count() {
      if (this.keySubmit != "") {
        this.selected = this.keySubmit;
      } else {
        this.selected = "today";
      }
    },
  },
  mounted() {
    this.loadDataCurent();
  },
  methods: {
    toggleTimePanel() {
      this.showTimePanel = !this.showTimePanel;
    },
    handleOpenChange() {
      this.showTimePanel = false;
    },
    loadDataCurent() {
      this.fromDay = new Date().setHours(0, 0, 0).valueOf();
      this.toDay = new Date().setHours(23, 59, 59).valueOf();
      this.url = this.API_URL + "?from=" + this.fromDay + "&to=" + this.toDay;
    },
    showTime() {
      let currentHour = moment().hour();
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
            if (currentHour >= 8 && currentHour < 20) {
              this.fromDay = moment().startOf("day").valueOf() + 28800000;
              this.toDay = moment().endOf("day").valueOf() - 14400000;
            } else {
              this.fromDay =
                moment().subtract(1, "days").endOf("day").valueOf() - 14399000;
              this.toDay = moment().startOf("day").valueOf() + 28799000;
            }
            break;
          case "last-shift":
            if (currentHour >= 8 && currentHour < 20) {
              this.fromDay =
                moment().subtract(1, "days").endOf("day").valueOf() - 14399000;
              this.toDay = moment().startOf("day").valueOf() + 28799000;
            } else {
              this.fromDay = moment().subtract(1, "days").startOf("day").valueOf() + 28800000;
              this.toDay = moment().subtract(1, "days").endOf("day").valueOf() - 14400000;
            }
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
      // console.log("start", new Date(this.fromDay));
      // console.log("to", new Date(this.toDay));
      if (this.fromDay > this.toDay) {
        return;
      }
      this.url = this.API_URL + "?from=" + this.fromDay + "&to=" + this.toDay;
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
</style>
