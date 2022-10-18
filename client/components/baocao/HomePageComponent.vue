<template>
  <div>
    TRANG CHỦ
  </div>

</template>

<script>
export default {
  name: 'homepage-component',
  data() {
    return {
      
    }
  },
  mounted() {
    this.getData();
  },
  methods: {
    async getData() {
      const results = await this.$axios.get("/api/department/get");
      console.log('result', results);
    }
  }
}
</script>
