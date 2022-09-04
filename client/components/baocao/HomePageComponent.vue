<template>
  <div class="container">
    TRANG CHỦ
  </div>

</template>

<script>
const API_URL = 'https://localhost:44315/api/';
export default {
  name: 'homepage-component',
  data() {
    return {
      product: []
    }
  },
  mounted() {
    this.refreshData();
  },
  methods: {
    async refreshData() {
      const result = await this.$axios.get(API_URL+'baocaosanxuat');
      this.product = result.data;
    }
  }
}
</script>
