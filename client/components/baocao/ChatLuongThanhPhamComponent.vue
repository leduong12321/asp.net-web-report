<template>
  <div>
    <CommonReportComponent :API_URL="API_URL" :title="title"/>
  </div>
</template>
<script>
import CommonReportComponent from './CommonReportComponent.vue';
export default {
    name: "cahtluongthanhpham",
    components: { CommonReportComponent },
    data() {
      return {
        API_URL: process.env.baseRPUrl + "/ChatLuongThanhPhamHRC",
        title: 'Báo cáo chất lượng thành phẩm HRC'
      }
    }
}
</script>

  