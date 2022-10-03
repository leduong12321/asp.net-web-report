<template>
  <div></div>
</template>

<script>
export default {
  data() {
    return {};
  },
  props: {
    isPopupLogout: Boolean,
  },
  mounted() {
    if (this.isPopupLogout) {
      this.showMsgBoxTwo();
    }
  },
  methods: {
    showMsgBoxTwo() {
      this.$bvModal
        .msgBoxConfirm("Bạn có chắc muốn đăng xuất không?", {
          size: "md",
          buttonSize: "md",
          okVariant: "danger",
          okTitle: "Đăng xuất",
          cancelTitle: "Ở Lại",
          footerClass: "footer-calss",
          modalClass: "custom-popup",
          bodyClass: "body-class",
          contentClass: "content-class",
          hideHeaderClose: false,
          centered: true,
        })
        .then((value) => {
          if (value) {
            localStorage.removeItem('token');
            localStorage.removeItem('tokenExpiration');
            this.$store.dispatch("setLogoutTimer", 0);
            this.$store.dispatch("setUser", null);
            this.$router.push({ path: "/login" });
          } else {
            this.$emit("close-popup", false);
          }
        })
        .catch((err) => {
          // An error occurred
        });
    },
  },
};
</script>
<style scoped lang="scss">
:deep() {
  .custom-popup {
    .modal-dialog {
      justify-content: center;
    }
  }
  .content-class {
    max-width: 450px;
    padding: 1rem 1.7rem;
    font-size: 18px;
    box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  }
  .body-class {
    padding-left: 0px;
    padding-right: 0px;
  }
  .modal-content {
    max-width: 350px;
  }
  .footer-calss {
    justify-content: space-between;
    padding-left: 0px;
    padding-right: 0px;
    border-top: 0px;
    button {
        width: 130px;
        box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;
    }
  }
}
</style>
