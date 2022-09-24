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
              <input type="text" v-model="username" class="form-control" placeholder="Tài khoản" />
            </div>
            <div class="form-group">
              <label>Mật khẩu</label>
              <input
                type="password"
                v-model="password"
                class="form-control"
                placeholder="Mật khẩu"
              />
            </div>
            <button class="btn btn-secondary" >Đăng nhập</button>
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
        password: null
    }
  },
  mounted() {
    // this.getUser();
  },
  methods: {
    // async getUser() {
    //   const result = await this.$axios.get(process.env.baseApiUrl +'user').then((res) => {
    //     if(res.data) {
    //         console.log('2', res);
    //     }
    //   });
    // },
    async handleSubmit() {
        let info = {
            Username: this.username,
            Password: this.password
        }
        try {
          // const user = await this.$axios.get('/api/user');
          const user = await this.$axios.post('/api/user', info);
          this.$store.dispatch('setUser', user.data);
          console.log(user);
          if(user.data) {
            this.$router.push({ path: '/'} );
          }
        } catch (error) {
          console.log('error', error);
        }
        
        console.log('store -user', this.$store.getters.user);
    }
  }
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
