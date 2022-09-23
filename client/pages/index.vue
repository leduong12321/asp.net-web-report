<template>
  <div class="container">
    <HomePageComponent />
  </div>

</template>

<script>
import HomePageComponent from "../components/baocao/HomePageComponent.vue";

export default {
    name: "homepage",
    middleware: 'auth',
    components: { HomePageComponent }
}
</script>
