<template>
  <div>
    <div class="box">
      <section>
        <date-picker
          v-model="fromDay"
          type="datetime"
          placeholder="Ngày bắt đầu"
          value-type="timestamp"
          :show-time-panel="showTimePanel"
          @close="handleOpenChange"
          format="DD-MM-YYYY   HH:mm"
          :class="checkError"
        >
          <template v-slot:footer>
            <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
              {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
            </button>
          </template>
        </date-picker>

        <date-picker
          v-model="toDay"
          type="datetime"
          placeholder="Ngày kết thúc"
          value-type="timestamp"
          :show-time-panel="showTimePanel"
          @close="handleOpenChange"
          format="DD-MM-YYYY   HH:mm"
          :class="checkError"
        >
          <template v-slot:footer>
            <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
              {{ showTimePanel ? "Chọn ngày" : "Chọn thời gian" }}
            </button>
          </template>
        </date-picker>

        <b-button variant="outline-primary btn-submit ml-2" @click="handSubmit"
          >Tìm kiếm</b-button
        >
      </section>
      <p class="text-error-input-date">{{ errorInputDate }}</p>
    </div>
    <iframe
      id="iframe"
      v-if="API_URL"
      :src="API_URL"
      width="100%"
      height="700px"
      allowfullscreen
      style="background-color: white"
    ></iframe>
  </div>
</template>

<script>
export default {
  name: "baocaosanxuat-component",
  data() {
    return {
      showTimePanel: false,
      API_URL: "https://localhost:44315/ReportViewer",
      fromDay: null,
      toDay: null,
      errorInputDate: "",
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
      this.fromDay = new Date(2021, 9, 24).setHours(0, 0, 0).valueOf();
      this.toDay = new Date(2021, 11, 24).setHours(23, 59, 59).valueOf();
      this.API_URL =
        "https://localhost:44315/ReportViewer?from=" +
        this.fromDay +
        "&to=" +
        this.toDay;
    },
    handSubmit() {
      if (this.fromDay > this.toDay) {
        return;
      }
      this.API_URL =
        "https://localhost:44315/ReportViewer?from=" +
        this.fromDay +
        "&to=" +
        this.toDay;
      document.getElementById("iframe").src =
        document.getElementById("iframe").src;
    },
  },
};
</script>

<style lang="scss">

.error-input-date {
  input {
    border: 2px solid red;
    color: white;
    background-color: #fa5b4c;
  }
}
.text-error-input-date {
  color: red;
  font-size: 13px;
}
</style>
