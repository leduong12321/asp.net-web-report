<template>
    <div class="container">
        <SanLuongTheoNgay />
    </div>
  
  </template>
  
  <script>
import SanLuongTheoNgay from '../../../components/baocao/tsc/SanLuongTheoNgay.vue';

  export default {
    name: "homepage",
    middleware: "auth",
    components: { SanLuongTheoNgay },
}
  </script>