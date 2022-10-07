<template>
  <div class="container">
    <ChatLuongThanhPhamComponent/>
  </div>

</template>

<script>

import ChatLuongThanhPhamComponent from '../../components/baocao/ChatLuongThanhPhamComponent.vue';
export default {
    name: "homepage",
    // middleware: 'auth',
    components: { ChatLuongThanhPhamComponent }
}
</script>
