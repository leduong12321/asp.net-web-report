<template>
    <div>
      <div class="box d-flex">
        <div class="col-4 align-items-center pl-0 pt-2 mt-1 fs-13">Báo cáo > {{title}}</div>
        <div class="col-8 pr-0">
        <div class="d-flex align-items-center jf-content-end mb-2">
          <div class="mr-2 text-date">Ngày bắt đầu:</div>
          <date-picker
            v-model="fromDay"
            type="datetime"
            placeholder="Ngày bắt đầu"
            value-type="timestamp"
            :show-time-panel="showTimePanel"
            @close="handleOpenChange"
            format="DD-MM-YYYY   HH:mm"
            :class="checkError"
            class="date-ui"
          >
            <template v-slot:footer>
              <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
                {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
              </button>
            </template>
          </date-picker>
          <div class="mr-2 text-date">Ngày kết thúc:</div>
          <date-picker
            v-model="toDay"
            type="datetime"
            placeholder="Ngày kết thúc"
            value-type="timestamp"
            :show-time-panel="showTimePanel"
            @close="handleOpenChange"
            format="DD-MM-YYYY   HH:mm"
            :class="checkError"
            class="date-ui"
          >
            <template v-slot:footer>
              <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
                {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
              </button>
            </template>
          </date-picker>
  
          <b-button class="btn btn-submit submit" @click="handSubmit"
            >Tìm kiếm</b-button
          >
        </div>
        <p class="text-error-input-date" v-if="errorInputDate.length > 0">
          {{ errorInputDate }}
        </p>
      </div>
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
  export default {
    name: "common-report-component",
    props: {
        API_URL: String,
        title: String,
    },
    data() {
      return {
        showTimePanel: false,
        fromDay: null,
        toDay: null,
        errorInputDate: "",
        url: null,
      };
    },
    computed: {
      checkError() {
        if (this.fromDay > this.toDay) {
          this.errorInputDate = "Có lỗi khi nhập thời gian";
          return "error-input-date";
        } else {
          this.errorInputDate = "";
          return "";
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
        this.url = this.API_URL + '?from='+ this.fromDay + '&to=' + this.toDay;
      },
      handSubmit() {
        if (this.fromDay > this.toDay) {
          return;
        }
        this.url = this.API_URL + '?from='+ this.fromDay + '&to=' + this.toDay;
        document.getElementById("iframe").src =
          document.getElementById("iframe").src;
      },
    },
  };
  </script>
  
  <style lang="scss">
  .text-error-input-date {
    color: #cc0033;
    font-size: 13px;
    padding-right: 140px;
    text-align: right;
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
      width: 180px;
    }
    input {
      height: 28px;
    }
  }
  </style>
  