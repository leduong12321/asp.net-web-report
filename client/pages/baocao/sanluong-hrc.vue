<template>
  <div class="container">
    <TongHopSanLuongComponent/>
  </div>

</template>

<script>
import TongHopSanLuongComponent from '../../components/baocao/TongHopSanLuongComponent.vue';
export default {
    name: "homepage",
    // middleware: 'auth',
    components: { TongHopSanLuongComponent }
}
</script>
