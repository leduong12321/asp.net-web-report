<template>
  <div class="container">
    <SanXuatComponent />
  </div>
</template>

<script>
import SanXuatComponent from "../../components/baocao/SanXuatComponent.vue";

export default {
  name: "homepage",
  middleware: 'auth',
  components: { SanXuatComponent },
};
</script>
