<template>
    <div class="container">
      <iframe v-if="API_URL" :src="API_URL" width="100%" height="700px" allowfullscreen></iframe>
    </div>
  
  </template>
  
  <script>
  export default {
    name: 'baocaosanxuat-component',
    data() {
      return {
        API_URL: 'https://localhost:44315/ReportViewer'
      }
    },
    mounted() {
      if(this.$route?.query.downcoiler) {
        this.API_URL = 'https://localhost:44315/ReportViewer?downcoiler='+ this.$route?.query.downcoiler;
        console.log('API_URL', this.API_URL);
      }
    },
    methods: {

    }
  }
  </script>
  