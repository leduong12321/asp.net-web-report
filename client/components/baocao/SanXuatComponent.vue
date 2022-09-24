<template>
  <div>
    <CommonReportComponent :API_URL="API_URL" :title="title"/>
  </div>
</template>
<script>
import CommonReportComponent from './CommonReportComponent.vue';
export default {
    name: "bao-cao-san-xuat",
    components: { CommonReportComponent },
    data() {
      return {
        API_URL: process.env.baseUrl + "/ReportViewer",
        title: 'Báo cáo sản xuất'
      }
    }
}
</script>
