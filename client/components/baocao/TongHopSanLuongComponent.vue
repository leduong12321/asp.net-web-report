<template>
  <div>
    <CommonReportComponent :API_URL="API_URL" :title="title"/>
  </div>
</template>

<script>
import CommonReportComponent from './CommonReportComponent.vue';
export default {
    name: "tonghopsanluong",
    components: { CommonReportComponent },
    data() {
      return {
        API_URL: "https://localhost:44315/TongHopSanLuongHRC",
        title: 'Báo cáo tổng hợp sản lượng HRC'
      }
    }
}
</script>
