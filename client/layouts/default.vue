<template>
  <div class="slidebar-container">
    <div class="sidebar" :class="{ 'close-sidebar': isClose }">
      <div class="logo-details">
        <i class="bx bx-color"></i>
        <span class="logo_name">NM.CT QSP</span>
      </div>
      <ul class="nav-links">
        <li v-for="(menu, index) in menus" :key="index" class="li">
          <div class="iocn-link">
            <nuxt-link
              class="link_name"
              :to="menu.url.length > 0 ? menu.url : ''"
            >
              <i :class="menu.icon"></i>
              <span class="link_name">{{ menu.name }}</span>
            </nuxt-link>
            <ul class="sub-menu blank" v-if="menu.subMenus.length == 0">
              <li>
                <nuxt-link class="link_name" :to="menu.url">{{
                  menu.name
                }}</nuxt-link>
              </li>
            </ul>
            <div v-else>
              <i class="bx bxs-chevron-down arrow"></i>
            </div>
          </div>
          <div v-if="menu.subMenus.length > 0">
            <ul class="sub-menu">
              <li>
                <nuxt-link class="link_name" :to="menu.url">{{
                  menu.name
                }}</nuxt-link>
              </li>
              <li v-for="(subMenu, index) in menu.subMenus" :key="index">
                <nuxt-link :to="subMenu.url">{{ subMenu.name }}</nuxt-link>
              </li>
            </ul>
          </div>
        </li>
        <li>
          <div class="profile-details">
            <div class="profile-content">
              <img :src="userImage" alt="profileImg" />
            </div>
            <div class="name-job">
              <div class="profile_name">Tài khoản</div>
              <div class="job">KTV CNTT</div>
            </div>
            <i class="bx bx-log-out" @click="handLogout()"></i>
          </div>
        </li>
      </ul>
    </div>
    <div class="header-section">
      <div class="home-content">
        <i
          class="bx bx-menu"
          v-if="isClose"
          @click="closeSideBar()"
        ></i>
        <i class="bx bx-x" v-else @click="closeSideBar()"></i>
      </div>
      <div class="right-header">
        <img :src="vnFlag" alt="viet-nam-flag" class="mr-3" />
        <i class="bx bxs-bell-ring mr-3"></i>
        <div class="user">
          <span class="mr-2 mt-1">Hi, Admin</span>
          <img :src="userImage" alt="user" class="user-image" />
        </div>
      </div>
    </div>
    <section class="home-section">
      <Nuxt class="nuxt-body" style="max-width: 100%; padding: 4px;" />
    </section>
  </div>
</template>

<script>
import userImage from "../assets/images/user.png";
import vnFlag from "../assets/images/vn-flag.png";
export default {
  data() {
    return {
      isClose: true,
      userImage,
      vnFlag,
      windowWidth: window.innerWidth,
      menus: [
        {
          icon: "bx bx-home",
          name: "Trang chủ",
          url: "/",
          subMenus: [],
        },
        {
          icon: "bx bxs-report",
          name: "Báo cáo",
          url: "",
          subMenus: [
            {
              icon: "",
              name: "Sản xuất",
              url: "/baocao/sanxuat",
            },
            {
              icon: "",
              name: "Sản lượng HRC",
              url: "/baocao/sanluong-hrc",
            },
            {
              icon: "",
              name: "Chất lượng HRC",
              url: "/baocao/chatluong-hrc",
            },
          ],
        },
        {
          icon: "bx bx-bar-chart-alt",
          name: "Phân tích",
          url: "/",
          subMenus: [
            {
              icon: "",
              name: "TSC",
              url: "tsc",
            },
            {
              icon: "",
              name: "TF",
              url: "tf",
            },
            {
              icon: "",
              name: "HSM",
              url: "hsm",
            },
          ],
        },
        {
          icon: "bx bx-help-circle",
          name: "Hỗ trợ",
          url: "/",
          subMenus: [],
        },
      ],
    };
  },
  mounted() {
    // if(this.windowWidth > 1900) {
    //   this.isClose = false;
    // }
    this.handleShowSubMenu();
  },
  // watch: {
  //   'isClose'() {
  //     let element = document.getElementsByClassName("nuxt-body");
  //     element[0].classList.remove('container');
  //     debugger;
  //     console.log('e');
  //   }
  // },
  methods: {
    handleShowSubMenu() {
      let arrow = document.querySelectorAll(".li");
      for (var i = 0; i < arrow.length; i++) {
        arrow[i].addEventListener("click", (e) => {
          let arrowParent = e.target.parentElement.parentElement.parentElement; //selecting main parent of arrow
          if (e.target.parentElement.parentElement.classList[0] == "li") {
            arrowParent = e.target.parentElement.parentElement;
          }
          arrowParent.classList.toggle("showMenu");
        });
      }
    },
    closeSideBar() {
      this.isClose = !this.isClose;
      // setTimeout(() => {
      //   let element = document.getElementsByClassName("nuxt-body");
      //   element[0].classList.remove('container');
      // }, 1);
    },
    handLogout() {
      alert("Do you want logout the website!");
    },
  },
};
</script>

<style lang="scss">
/* Google Fonts Import Link */
@import url("https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap");
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: "Poppins", sans-serif;
}
body {
  overflow: hidden;
}
.sidebar {
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  width: 260px;
  background: #11101d;
  z-index: 100;
  transition: all 0.5s ease;
}
.sidebar.close-sidebar {
  width: 60px;
}
.sidebar .logo-details {
  height: 60px;
  width: 100%;
  display: flex;
  align-items: center;
}
.sidebar .logo-details i {
  font-size: 30px;
  color: #fff;
  height: 50px;
  min-width: 60px;
  text-align: center;
  line-height: 50px;
}
.sidebar .logo-details .logo_name {
  font-size: 22px;
  color: #fff;
  font-weight: 600;
  transition: 0.3s ease;
  transition-delay: 0.1s;
}
.sidebar.close-sidebar .logo-details .logo_name {
  transition-delay: 0s;
  opacity: 0;
  pointer-events: none;
}
.sidebar .nav-links {
  height: 100%;
  padding: 30px 0 150px 0;
  overflow: auto;
}
.sidebar.close-sidebar .nav-links {
  overflow: visible;
}
.sidebar .nav-links::-webkit-scrollbar {
  display: none;
}
.sidebar .nav-links li {
  position: relative;
  list-style: none;
  transition: all 0.4s ease;
  cursor: pointer;
}
.sidebar .nav-links li:hover {
  background: #1d1b31;
}
.sidebar .nav-links li .iocn-link {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.sidebar .nav-links li .iocn-link a {
  width: 100%;
}
.sidebar.close-sidebar .nav-links li .iocn-link {
  display: block;
}
.sidebar .nav-links li i {
  height: 50px;
  min-width: 60px;
  text-align: center;
  line-height: 50px;
  color: #fff;
  font-size: 20px;
  transition: all 0.3s ease;
}
.sidebar .nav-links li.showMenu i.arrow {
  transform: rotate(-180deg);
}
.sidebar.close-sidebar .nav-links i.arrow {
  display: none;
}
.sidebar .nav-links li a {
  display: flex;
  align-items: center;
  text-decoration: none;
}
.sidebar .nav-links li a .link_name {
  font-size: 16px;
  font-weight: 400;
  color: #fff;
  transition: all 0.4s ease;
}
.sidebar.close-sidebar .nav-links li a .link_name {
  opacity: 0;
  pointer-events: none;
}
.sidebar .nav-links li .sub-menu {
  padding: 6px 6px 14px 80px;
  margin-top: -10px;
  background: #1d1b31;
  display: none;
}
.sidebar .nav-links li.showMenu .sub-menu {
  display: block;
}
.sidebar .nav-links li .sub-menu a {
  color: #fff;
  font-size: 14px;
  padding: 4px 0 0 2px;
  white-space: nowrap;
  opacity: 0.6;
  transition: all 0.3s ease;
  &.nuxt-link-exact-active {
    opacity: 1;
    font-weight: bold;
  }
}
.sidebar .nav-links li .sub-menu a:hover {
  opacity: 1;
}
.sidebar.close-sidebar .nav-links li .sub-menu {
  position: absolute;
  left: 100%;
  top: -10px;
  margin-top: 0;
  padding: 10px 20px;
  border-radius: 0 6px 6px 0;
  opacity: 0;
  display: block;
  pointer-events: none;
  transition: 0s;
}
.sidebar.close-sidebar .nav-links li:hover .sub-menu {
  top: 0;
  opacity: 1;
  pointer-events: auto;
  transition: all 0.4s ease;
}
.sidebar .nav-links li .sub-menu .link_name {
  display: none;
}
.sidebar.close-sidebar .nav-links li .sub-menu .link_name {
  font-size: 14px;
  opacity: 1;
  display: block;
}
.sidebar .nav-links li .sub-menu.blank {
  opacity: 1;
  pointer-events: auto;
  padding: 3px 20px 6px 16px;
  opacity: 0;
  pointer-events: none;
}
.sidebar .nav-links li:hover .sub-menu.blank {
  top: 50%;
  transform: translateY(-50%);
}
.sidebar .profile-details {
  position: fixed;
  bottom: 0;
  width: 260px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #1d1b31;
  padding: 12px 0;
  transition: all 0.5s ease;
}
.sidebar.close-sidebar .profile-details {
  background: none;
}
.sidebar.close-sidebar .profile-details {
  width: 60px;
}
.sidebar .profile-details .profile-content {
  display: flex;
  align-items: center;
}
.sidebar .profile-details img {
  height: 52px;
  width: 52px;
  object-fit: cover;
  border-radius: 16px;
  margin: 0 4px 0 2px;
  background: #1d1b31;
  transition: all 0.5s ease;
}
.sidebar.close-sidebar .profile-details img {
  padding: 10px;
}
.sidebar .profile-details .profile_name,
.sidebar .profile-details .job {
  color: #fff;
  font-size: 18px;
  font-weight: 500;
  white-space: nowrap;
}
.sidebar.close-sidebar .profile-details i,
.sidebar.close-sidebar .profile-details .profile_name,
.sidebar.close-sidebar .profile-details .job {
  display: none;
}
.sidebar .profile-details .job {
  font-size: 12px;
}
.header-section {
  left: 260px;
  width: calc(100% - 260px);
  transition: all 0.5s ease;
  position: relative;
  height: 40px;
  box-shadow: 0 10px 30px 0 rgb(47 60 74 / 8%);
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.right-header {
  display: flex;
  justify-content: center;
  align-items: center;
  font-style: italic;
  i {
    font-size: 20px;
    margin-top: 3px;
  }
  img {
    width: 18px;
    height: 18px;
    object-fit: cover;
    &.user-image {
      width: 24px;
      height: 24px;
    }
  }
  .user {
    margin-right: 46px;
    font-size: 13px;
  }
}
.home-section {
  // background: #e4e9f7;
  left: 260px;
  width: calc(100% - 260px);
  transition: all 0.5s ease;
  padding: 12px 24px;
  overflow-y: scroll;
  position: relative;
  // height: 100vh;
}
.sidebar.close-sidebar ~ .home-section,
.sidebar.close-sidebar ~ .header-section {
  left: 60px;
  width: calc(100% - 60px);
}
.home-content {
  padding-left: 4px;
  padding-top: 5px;
  background-color: #11101d;
  border-radius: 0px 8px 8px 0px;
  // display: flex;
  // align-items: center;
  // flex-wrap: wrap;
  // position: absolute;
}
.header-section .home-content .bx-menu,
.header-section .home-content .bx-x,
.header-section .home-content .text {
  color: white;
  font-size: 20px;
}
.header-section .home-content .bx-menu,
.header-section .home-content .bx-x {
  cursor: pointer;
  margin-right: 8px;
}
.header-section .home-content .text {
  font-size: 26px;
  font-weight: 600;
}

@media screen and (max-width: 400px) {
  .sidebar {
    width: 240px;
  }
  .sidebar.close-sidebar {
    width: 60px;
  }
  .sidebar .profile-details {
    width: 240px;
  }
  .sidebar.close-sidebar .profile-details {
    background: none;
  }
  .sidebar.close-sidebar .profile-details {
    width: 60px;
  }
  .home-section {
    left: 240px;
    width: calc(100% - 240px);
  }
  .sidebar.close-sidebar ~ .home-section {
    left: 60px;
    width: calc(100% - 60px);
  }
  .nuxt-body {
    
  }
}
</style>
