<template>
  <div class="container">
    <TongHopSanLuongComponent/>
  </div>

</template>

<script>
import TongHopSanLuongComponent from '../../components/baocao/TongHopSanLuongComponent.vue';
export default {
    name: "homepage",
    middleware: ['check-auth', 'auth'],
    components: { TongHopSanLuongComponent }
}
</script>
