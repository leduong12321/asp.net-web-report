
<template>
  <div class="top-menu">
    <div class="d-flex justify-content-center align-items-center">
      <nuxt-link to="/baocaosanxuat">Bao Cao San Xuat</nuxt-link>
      <span class="pr-4"><i class="bi bi-circle-fill"></i></span>
      <nuxt-link to="/department">Bao Cao Tong Hop San Xuat HSM</nuxt-link>
      <span class="pr-4"><i class="bi bi-circle-fill"></i></span>
      <nuxt-link to="/employee">Chat Luong Thanh Pham HRC</nuxt-link>
      <span class="dot ms-4 me-4"><i class="bi bi-circle-fill"></i></span>
    </div>
  </div>
</template>

<script>
export default {}
</script>

<style lang="scss">
.top-menu {
  margin: 42px auto;
  .golden-lines {
    border-bottom: 1px solid $primary;
    &::after {
      display: block;
      content: '';
      height: 1px;
      width: 80%;
      background-color: $primary;
      position: relative;
      top: 8px;
      margin: 0px auto;
    }
  }
  a {
    color: $gray-700;
    text-transform: uppercase;
    font-size: 22px;
    font-weight: 300;
    text-decoration: none;
    border-bottom: 1px solid transparent;
    &::after {
      display: block;
      content: '';
      height: 1px;
      background-color: transparent;
    }
    &.nuxt-link-exact-active {
      @extend .golden-lines;
    }
    &:hover {
      @extend .golden-lines;
    }
  }
  .dot {
    display: inline-block;
    color: $primary;
    font-size: 8px;
  }
}
</style>

