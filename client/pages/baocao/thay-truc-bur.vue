<template>
    <div class="container">
      <ThayTrucBurComponent />
    </div>
  
  </template>
  
  <script>
import ThayTrucBurComponent from '../../components/baocao/ThayTrucBurComponent.vue';
  
  export default {
    name: "homepage",
    // middleware: "auth",
    components: { ThayTrucBurComponent }
}
  </script>
  