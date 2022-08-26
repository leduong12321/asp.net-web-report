<template>
  <div class="container">
    <div v-for="(item, index) in departments" :key="index">
    Id: {{item.DepartmentId}}
    Name: {{item.DepartmentName}}
    </div>
  </div>

</template>

<script>
const API_URL = 'http://localhost:5101/api/';
export default {
  name: 'homepage-component',
  data() {
    return {
      departments: []
    }
  },
  mounted() {
    this.refreshData();
  },
  methods: {
    async refreshData() {
      const result = await this.$axios.get(API_URL+'Department');
      this.departments = result.data;
      console.log(this.departments);
    }
  }
}
</script>
