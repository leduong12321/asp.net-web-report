<template>
  <div class="wrapper" v-if="isData">
    <nav
      id="sidebar"
      :class="[isCollapse ? 'active' : 'none-active']"
      v-click-outside="externalClick"
    >
      <div class="sidebar-header d-flex">
        <img :src="logo" alt="logo-main" class="logo-main" />
        <span class="logo_name" v-if="!isCollapse">NM.CT QSP</span>
      </div>
      <ul class="list-unstyled list-parent components">
        <li v-for="(parent, index) in menu" :key="index">
          <div v-if="parent.role.includes(userLocal?.Role)">
            <nuxt-link
              class="link_name"
              :to="`${parent.url}`"
              v-if="parent.subMenus?.length == 0"
            >
              <i :class="parent.icon"></i>
              <span class="link_name" v-if="!isCollapse">{{
                parent.name
              }}</span>
            </nuxt-link>
            <a
              v-else
              @click="isCollapse = false"
              :href="`#${parent.url}`"
              data-toggle="collapse"
              aria-expanded="false"
              :class="[
                parent.subMenus?.length > 0 && isCollapse
                  ? 'dropdown-toggle'
                  : null,
                isCollapse == false ? 'parent' : 'child',
              ]"
            >
              <i :class="parent.icon"></i>
              <span v-if="!isCollapse">{{ parent.name }}</span>
            </a>
          </div>
          <ul
            class="collapse list-unstyled list-child1"
            :id="parent.url"
            :class="{ 'hide': isCollapse }"
          >
            <div>
              <li v-for="(child1, index) in parent?.subMenus" :key="index">
                <div v-if="child1.role.includes(userLocal?.Role)">
                  <span
                    v-if="
                      child1.subChildMenu?.length == 0 ||
                      !child1.url.includes('ID-')
                    "
                    @click="handleCollapse()"
                  >
                    <nuxt-link class="link_name" :to="`${child1.url}`">
                      <i :class="child1.icon"></i>
                      <span class="link_name">{{ child1.name }}</span>
                    </nuxt-link>
                  </span>
                  <a
                    v-else
                    :href="`#${child1.url}`"
                    data-toggle="collapse"
                    :aria-expanded="isCollapse ? 'true' : 'false',  "
                    :class="[
                      child1.subChildMenu?.length > 0 && isCollapse
                        ? 'dropdown-toggle'
                        : null,
                      isCollapse == false ? 'parent' : 'child',
                    ]"
                  >
                  <i :class="child1.icon"></i>
                    {{ child1.name }}
                  </a>
                </div>
                <ul
                  class="collapse list-unstyled list-child2"
                  :id="child1.url"
                  :class="{ 'hide': isCollapse }"
                >
                  <li
                    v-for="(child2, index) in child1?.subChildMenu"
                    :key="index"
                  >
                    <div v-if="child2.role.includes(userLocal?.Role)">
                      <nuxt-link
                        class="link_name last-of-child"
                        :to="`${child2.url}`"
                      >
                        <i class="fas fa-tachometer-alt"></i>
                        <span class="link_name" @click="handleCollapse('3')">{{ child2.name }}</span>
                      </nuxt-link>
                    </div>
                  </li>
                </ul>
              </li>
            </div>
          </ul>
        </li>
      </ul>
    </nav>
    <div class="header-section">
      <div class="right-header">
        <img :src="vnFlag" alt="viet-nam-flag" class="mr-3" />
        <i class="bx bxs-bell-ring mr-3"></i>
        <div class="user d-flex">
          <span class="mr-2 mt-1"
            >Xin chào, {{ userLocal.Name }}</span
          >
          <img :src="userImage" alt="user" class="user-image" />
        </div>
        <i
          class="bx bx-log-out logout-icon pr-3"
          v-b-tooltip.hover.bottomleft="{ customClass: 'my-tooltip-class' }"
          title="Đăng xuất"
          @click="handLogout()"
        ></i>
      </div>
    </div>
    <section class="home-section">
      <Nuxt class="nuxt-body" style="max-width: 100%; padding: 4px" />
    </section>
    <PopupLogout
      v-if="isPopupLogout"
      :isPopupLogout="isPopupLogout"
      @close-popup="isPopupLogout = false"
    />
  </div>
</template>

<script>
import DatePicker from "vue2-datepicker";
import vClickOutside from "v-click-outside";
import userImage from "../assets/images/user.png";
import logo from "../assets/images/logo.png";
import vnFlag from "../assets/images/vn-flag.png";
import PopupLogout from "../components/common/PopupLogout.vue";
import "vue2-datepicker/index.css";

export default {
  components: { DatePicker, PopupLogout },
  directives: {
    clickOutside: vClickOutside.directive,
  },
  data() {
    return {
      isData: false,
      userLocal: null,
      isCollapse: true,
      isClose: true,
      userImage,
      vnFlag,
      logo,
      isPopupLogout: false,
      menu: [
        {
          icon: "bx bx-home",
          name: "Trang chủ",
          url: "/",
          subMenus: [],
          role: [0, 1, 2],
        },
        {
          icon: "bx bxs-report",
          name: "Báo cáo",
          url: "ID-01-BaoCao",
          role: [0, 1, 2, 3],
          subMenus: [
            {
              icon: "bx bx-folder-plus",
              name: "Khu vực Đúc",
              url: "ID-02-TSC1",
              role: [0, 3],
              subChildMenu: [
                {
                  icon: "",
                  name: "Tình trạng thiết bị",
                  url: "/baocao/tsc1/equipment",
                  role: [0, 3],
                }
              ],
            },
            {
              icon: "bx bx-folder-plus",
              name: "Khu vực Lò",
              url: "ID-02-LF",
              role: [0, 4],
              subChildMenu: [
                {
                  icon: "",
                  name: "Báo cáo LF",
                  url: "/baocao/lf",
                  role: [0, 1],
                },
              ],
            },
            {
              icon: "bx bx-folder-plus",
              name: "Khu vực Cán",
              url: "ID-02-HSM",
              role: [0, 1, 2],
              subChildMenu: [
                {
                  icon: "",
                  name: "Sản xuất",
                  url: "/baocao/sanxuat",
                  role: [0, 1],
                },
                {
                  icon: "",
                  name: "Tổng hợp sản lượng HRC",
                  url: "/baocao/sanluong-hrc",
                  role: [0],
                },
                {
                  icon: "",
                  name: "Chất lượng thành phẩm HRC",
                  url: "/baocao/chatluong-hrc",
                  role: [0],
                },
                {
                  icon: "",
                  name: "Thay trục BUR",
                  url: "/baocao/thay-truc-bur",
                  role: [0, 2],
                },
                {
                  icon: "",
                  name: "Thay trục WR",
                  url: "/baocao/thay-truc-wr",
                  role: [0, 2],
                },
              ],
            },
          ],
        },
        {
          icon: "bx bx-help-circle",
          name: "Hỗ trợ",
          url: "/ho-tro",
          subMenus: [],
          role: [0, 1, 2],
        },
      ],
    };
  },
  // watch: {
  //   'userLocal'(value) {
  //     console.log('value', value);
  //   }
  // },
  mounted() {
    this.$store.dispatch("refreshToken");
    if(localStorage.getItem('user')) {
      this.userLocal = JSON.parse(localStorage.getItem('user'));
    }
    if(!this.userLocal) {
      this.$router.push({ path: "/login" });
    }
    this.isData = true;
  },
  methods: {
    externalClick() {
      this.isCollapse = true;
    },
    handleCollapse(key) {
      this.isCollapse = true;
    },
    handLogout() {
      this.isPopupLogout = true;
    },
  },
};
</script>

<style lang="scss">
.wrapper {
  max-height: 100vh;
  overflow: hidden;
}
.navbar {
  padding: 15px 10px;
  background: #fff;
  border: none;
  border-radius: 0;
  margin-bottom: 20px;
  box-shadow: 1px 1px 3px rgba(0, 0, 0, 0.1);
}

.navbar-btn {
  box-shadow: none;
  outline: none !important;
  border: none;
}

.line {
  width: 100%;
  height: 1px;
  border-bottom: 1px dashed #ddd;
  margin: 40px 0;
}

i,
span {
  display: inline-block;
}

/* ---------------------------------------------------
    SIDEBAR STYLE
----------------------------------------------------- */

#sidebar {
  min-width: 300px;
  max-width: 300px;
  background: $primary;
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  width: 280px;
  background: #11101d;
  z-index: 100;
  ul li a {
    color: white;
  }
  ul li a:hover {
    text-decoration: none;
  }
}

#sidebar.active {
  min-width: 50px;
  max-width: 50px;
  text-align: center;
}

#sidebar.active .sidebar-header h3,
#sidebar.active .CTAs {
  display: none;
}

#sidebar.active .sidebar-header strong {
  display: block;
}

#sidebar ul li a {
  text-align: left;
}

#sidebar.active ul li a {
  padding: 16px 10px;
  text-align: center;
  font-size: 0.6em;
}

#sidebar.active ul li a i {
  margin-right: 0;
  display: block;
  font-size: 2em;
}

#sidebar.active ul ul a {
  padding: 10px !important;
  color: #fff;
  background: $primary;
}

#sidebar.active .dropdown-toggle::after {
  top: auto;
  bottom: 10px;
  right: 50%;
  -webkit-transform: translateX(50%);
  -ms-transform: translateX(50%);
  transform: translateX(50%);
}

#sidebar .sidebar-header {
  background-color: #11101d;
  .logo-main {
    max-height: 34px;
    padding: 5px;
    margin-top: 24px;
    background-color: #11101d;
  }
  .logo_name {
    color: white;
    margin-top: 24px;
    font-size: 1.5rem;
    padding-left: 6px;
    font-weight: bold;
  }
  i {
    font-size: 2rem;
  }
}

#sidebar .sidebar-header strong {
  display: none;
  font-size: 1.8em;
}

#sidebar ul.components {
  padding: 20px 0;
}

#sidebar.active ul.components li a.child,
#sidebar ul.components li a.parent {
  display: block;
}

#sidebar ul.components li a.child,
#sidebar.active ul.components li a.parent {
  display: none;
}

#sidebar ul li a {
  padding: 10px;
  font-size: 0.9em;
  display: block;
  &.nuxt-link-exact-active {
    opacity: 1;
    color: aqua !important;
    // font-weight: bold;
  }
}

#sidebar ul li a:hover {
  color: white;
  // background: #cdcdcd;
}

#sidebar ul li a i {
  margin-right: 10px;
}

#sidebar ul li.active > a,
a[aria-expanded="true"] {
  color: #fff;
  background: $primary;
}

#sidebar ul li.active > a:hover,
a[aria-expanded="true"] {
  color: $primary;
  background: #fff;
}

#sidebar ul li.subactive > a,
a[aria-expanded="true"] {
  color: $primary;
  background: #fff;
}

#sidebar ul li.subactive > a:hover,
a[aria-expanded="true"] {
  color: #fff;
  background: $primary;
}

a[data-toggle="collapse"] {
  position: relative;
}

.dropdown-toggle::after {
  display: block;
  position: absolute;
  top: 50%;
  right: 20px;
  transform: translateY(-50%);
}

ul ul a {
  font-size: 0.8em !important;
  padding-left: 30px !important;
  /*background: #6d7fcc;*/
}

ul.CTAs {
  padding: 20px;
}

ul.CTAs a {
  text-align: center;
  font-size: 0.8em !important;
  display: block;
  border-radius: 5px;
  margin-bottom: 5px;
}

a.download {
  background: #fff;
  color: #7386d5;
}

a.article,
a.article:hover {
  background: #6d7fcc !important;
  color: #fff !important;
}
.list-unstyled {
  &.list-parent {
    li {
      border-bottom: 1px solid #464a4e;
      &:first-child {
        border-top: 1px solid #464a4e;
      }
    }
  }
  &.list-child1 {
    background-color: #464a4e;
    .parent {
      background-color: #464a4e;
    }
  }
  .last-of-child {
    padding-left: 60px !important;
    background-color: rgba(205, 205, 205, 0.1);
  }
}
.active {
  a[aria-expanded="true"]::after {
    background: none;
  }
  a[aria-expanded="false"]::after {
    background: none;
  }
}
.none-active {
  a[aria-expanded="true"]::after {
    content: "";
    position: absolute;
    right: 0;
    display: inline-block;
    height: 12px;
    width: 12px;
    margin-right: 12px;
    margin-top: 4px;
    background: url(../assets/images/arrow-down.png) no-repeat 0 0;
    background-size: 12px 12px;
    transform: rotate(180deg);
  }
  a[aria-expanded="false"]::after {
    content: "";
    position: absolute;
    right: 0;
    display: inline-block;
    height: 12px;
    width: 12px;
    margin-right: 12px;
    margin-top: 4px;
    background: url(../assets/images/arrow-down.png) no-repeat 0 0;
    background-size: 12px 12px;
  }
}

/* ---------------------------------------------------
    CONTENT STYLE
----------------------------------------------------- */

#content {
  width: 100%;
  padding: 0px;
  min-height: 100vh;
  transition: all 0.3s;
}
.header-section {
  position: relative;
  height: 40px;
  box-shadow: 0 10px 30px 0 rgb(47 60 74 / 8%);
  display: flex;
  align-items: center;
  justify-content: flex-end;
}
.right-header {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  font-style: italic;
  i {
    font-size: 20px;
    margin-top: 3px;
    &.logout-icon {
      font-size: 26px;
      margin-top: 0px;
      cursor: pointer;
    }
    &.my-tooltip-class {
      .tooltip-inner {
        padding: 0.25rem 0.75rem;
        font-size: 13px;
      }
    }
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
    margin-right: 30px;
    font-size: 13px;
  }
}
.home-section {
  margin-left: 50px;
  margin-right: 16px;
  width: calc(100% - 50px);
  padding: 12px 24px;
  overflow-y: scroll;
  position: relative;
}
.sidebar.close-sidebar ~ .home-section,
.sidebar.close-sidebar ~ .header-section {
  left: 60px;
  width: calc(100% - 60px);
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

/* ---------------------------------------------------
    MEDIAQUERIES
----------------------------------------------------- */

@media (max-width: 768px) {
  #sidebar {
    min-width: 80px;
    max-width: 80px;
    text-align: center;
    margin-left: -80px !important;
  }
  .dropdown-toggle::after {
    top: auto;
    bottom: 10px;
    right: 50%;
    -webkit-transform: translateX(50%);
    -ms-transform: translateX(50%);
    transform: translateX(50%);
  }
  #sidebar.active {
    margin-left: 0 !important;
  }
  #sidebar .sidebar-header h3,
  #sidebar .CTAs {
    display: none;
  }
  #sidebar .sidebar-header strong {
    display: block;
  }
  #sidebar ul li a {
    padding: 20px 10px;
  }
  #sidebar ul li a span {
    font-size: 0.85em;
  }
  #sidebar ul li a i {
    margin-right: 0;
    display: block;
  }
  #sidebar ul ul a {
    padding: 10px !important;
  }
  #sidebar ul li a i {
    font-size: 1.3em;
  }
  #sidebar {
    margin-left: 0;
  }
  #sidebarCollapse span {
    display: none;
  }
}
</style>
