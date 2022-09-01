<template>
  <div class="container">
    <div v-for="(item, index) in product" :key="index">
    STT: {{item.Sothutu}}
    Name: {{item.SanPham}}
    Mac thep: {{item.STEEL_GRADE}}
    So cuon: {{item.SoCuon}}
    Tong KL PDI: {{item.TongKhoiLuongPDI}}
    Tong KL: {{item.TongKhoiLuong}}
    </div>
  </div>

</template>

<script>
const API_URL = 'http://localhost:22954/api/';
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
      const result = await this.$axios.get(API_URL+'BaoCaoSanXuat');
      this.product = result.data;
      console.log(this.product);
    }
  }
}
</script>
