<template>
  <div class="container">
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
      >
        <template v-slot:footer>
          <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
            {{ showTimePanel ? 'Chọn ngày' : 'Chọn thời gian' }}
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
      >
        <template v-slot:footer>
          <button class="mx-btn mx-btn-text" @click="toggleTimePanel">
            {{ showTimePanel ? 'Chọn ngày' : 'Chọn thời gian' }}
          </button>
        </template>
      </date-picker>

      <b-button variant="outline-primary" @click="handSubmit">Submit</b-button>
    </section>
    </div>
    <iframe
      id="iframe"
      v-if="API_URL"
      :src="API_URL"
      width="100%"
      height="700px"
      allowfullscreen
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
    };
  },
  mounted() {
    // if(this.$route?.query.fromDay) {
    //   this.API_URL = 'https://localhost:44315/ReportViewer?fromDay='+ this.fromDay ;
    //   console.log('API_URL', this.API_URL);
    // }
  },
  methods: {
    toggleTimePanel() {
      this.showTimePanel = !this.showTimePanel;
    },
    handleOpenChange() {
      this.showTimePanel = false;
    },
    handSubmit() {
      this.API_URL =
        "https://localhost:44315/ReportViewer?from=" + this.fromDay + "&to=" + this.toDay;
      document.getElementById("iframe").src = document.getElementById("iframe").src;
      console.log("from", this.fromDay);
      console.log("to", this.toDay);
      console.log("to", this.API_URL);
    },
  },
};
</script>
