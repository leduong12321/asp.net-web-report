<template>
    <div class="container">
        <ThayTrucWr />
    </div>
  
  </template>
  
  <script>
import ThayTrucWr from '../../components/baocao/ThayTrucWr.vue';
  
  export default {
    name: "homepage",
    // middleware: "auth",
    components: { ThayTrucWr }
}
  </script>