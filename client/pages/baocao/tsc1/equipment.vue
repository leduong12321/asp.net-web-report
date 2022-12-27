<template>
    <div class="container">
        <EquipmentComponent />
    </div>
  
  </template>
  
  <script>
import EquipmentComponent from '../../../components/baocao/EquipmentComponent.vue';
  export default {
    name: "homepage",
    middleware: "auth",
    components: { EquipmentComponent },
}
  </script>