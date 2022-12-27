<template>
  <div>
    <CommonReportComponent :API_URL="API_URL" :title="title"/>
  </div>
</template>
<script>
import CommonReportComponent from './CommonReportComponent.vue';
export default {
  name: "equipment",
  components: { CommonReportComponent },
  data() {
    return {
      API_URL: process.env.baseUrl + "/Equipment",
      title: 'Báo cáo tình trạng sản xuất của thiết bị'
    }
  }
}
</script>

  