<template>
  <div class="container">
    <HomePageComponent />
  </div>
</template>

<script>
import HomePageComponent from "../components/baocao/HomePageComponent.vue";

export default {
  name: "homepage",
  data() {
    return {
      count: 0,
    };
  },
  // middleware: "auth",
  components: { HomePageComponent },
  mounted() {
    if (this.$store.getters.isToast) {
      this.showToast();
      this.$store.dispatch("setToast", false);
    }
  },
  methods: {
    showToast() {
      // Use a shorter name for this.$createElement
      const h = this.$createElement;
      // Increment the toast count
      this.count++;
      // Create the message
      const vNodesMsg = h("p", { class: ["mb-0"] }, [
        ` Bạn đang đăng nhập dưới tài khoản ${this.$store.getters.user.Name} `,
      ]);
      // Create the title
      const vNodesTitle = h(
        "div",
        { class: ["d-flex", "flex-grow-1", "align-items-baseline", "mr-2"] },
        [
          h("strong", { class: "mr-2" }, "Đăng nhập thành công"),
          h("small", { class: "ml-auto text-italics" }),
        ]
      );
      // Pass the VNodes as an array for message and title
      this.$bvToast.toast([vNodesMsg], {
        title: [vNodesTitle],
        solid: true,
        variant: "info",
        toastClass: "toast-custom",
      });
    },
  },
};
</script>

<style scoped lang="scss">
:deep() {
  .toast-custom {
    margin-top: 50px;
  }
}
</style>
