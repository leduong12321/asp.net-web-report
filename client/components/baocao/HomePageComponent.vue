<template>
  <div>
    TRANG CHỦ <button @click="getUser()">Click</button>
    <button @click="getUser2()">Click2</button>
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
  },
  methods: {
    async getUser() {
      const token = await this.$axios.post("/api/user/getname1");
      console.log('token', token);
    },
    async getUser2() {
      const token = await this.$axios.post("/api/user/getname2");
      console.log('token', token);
    }
  }
}
</script>
