<template>
  <div>
    <div class="sidenav">
      <div class="login-main-text">
        <h2>Đăng nhập</h2>
        <p>
          Liên hệ với chung tôi nếu không đăng nhập được thông qua địa chỉ:
          cntt.qsp@gmail.com
        </p>
      </div>
    </div>
    <div class="main">
      <div class="col-md-6 col-sm-12">
        <div class="login-form">
          <form @submit.prevent="handleSubmit">
            <div class="form-group">
              <label>Tài khoản</label>
              <input
                type="text"
                :class="{ 'input-error': errorMessage?.length > 0 }"
                v-model="username"
                @focus="removeError()"
                class="form-control"
                placeholder="Tài khoản"
              />
            </div>
            <div class="form-group mb-2">
              <label>Mật khẩu</label>
              <input
                type="password"
                v-model="password"
                class="form-control"
                @focus="removeError()"
                :class="{ 'input-error': errorMessage?.length > 0 }"
                placeholder="Mật khẩu"
              />
            </div>
            <span class="text-error" v-if="errorMessage?.length > 0">{{
              errorMessage
            }}</span>
            <button class="btn btn-secondary mt-2">Đăng nhập</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "login",
  data() {
    return {
      username: null,
      password: null,
      errorMessage: "",
    };
  },
  mounted() {},
  methods: {
    async handleSubmit() {
      let info = {
        Username: this.username,
        Password: this.password,
      };
      try {
        // const user = await this.$axios.get('/api/user');
        const user = await this.$axios.post("/api/user", info);
        this.$store.dispatch("setUser", user.data);
        if (user.data) {
          this.$store.dispatch("setToast", true);
          this.errorMessage = null;
          this.$router.push({ path: "/" });
        } else {
          this.errorMessage = "Tài khoản hoặc mật khẩu không đúng.";
        }
      } catch (error) {
        console.log("error", error);
      }

      console.log("store -user", this.$store.getters.user);
    },
    removeError() {
      this.errorMessage = '';
    },
  },
};
</script>
<style lang="scss">
body {
  font-family: "Lato", sans-serif;
}
.main-head {
  height: 150px;
  background: #fff;
}
.sidenav {
  height: 100%;
  background-color: #000;
  overflow-x: hidden;
  padding-top: 20px;
}
.main {
  padding: 0px 10px;
}

@media screen and (max-height: 450px) {
  .sidenav {
    padding-top: 15px;
  }
}

@media screen and (max-width: 450px) {
  .login-form {
    margin-top: 10%;
  }
  .register-form {
    margin-top: 10%;
  }
}
@media screen and (min-width: 768px) {
  .main {
    margin-left: 40%;
  }

  .sidenav {
    width: 40%;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
  }

  .login-form {
    margin-top: 80%;
  }

  .register-form {
    margin-top: 20%;
  }
}

.login-main-text {
  margin-top: 20%;
  padding: 60px;
  color: #fff;
}

.login-main-text h2 {
  font-weight: 300;
}

.btn-black {
  background-color: #000 !important;
  color: #fff;
}
</style>
