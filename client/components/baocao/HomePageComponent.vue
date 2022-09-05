<template>
  <div class="container">
    {{text}}
  </div>

</template>

<script>
const API_URL = 'https://localhost:44315/api/';
export default {
  name: 'homepage-component',
  data() {
    return {
      text: '',
      yourValue: null,
    }
  },
  mounted() {
    this.refreshData();
  },
  methods: {
    async refreshData() {
      const result = await this.$axios.get(API_URL+'baocaosanxuat');
      this.text = result.data;
    }
  }
}
</script>
