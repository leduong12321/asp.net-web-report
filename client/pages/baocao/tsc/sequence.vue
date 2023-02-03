<template>
    <div class="container">
        <SanXuatTheoSequence />
    </div>
  
  </template>
  
  <script>
import SanXuatTheoSequence from '../../../components/baocao/tsc/SanXuatTheoSequence.vue';

  export default {
    name: "homepage",
    middleware: "auth",
    components: { SanXuatTheoSequence },
}
  </script>