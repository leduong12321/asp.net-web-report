<template>
  <div class="container">
    {{text}}
  </div>

</template>

<script>
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
      const result = await this.$axios.get(process.env.baseApiUrl + 'baocaosanxuat');
      this.text = result.data;
    }
  }
}
</script>
