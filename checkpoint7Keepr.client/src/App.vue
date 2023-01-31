<template>
  <header>
    <Navbar />
  </header>

  <router-view />


</template>

<script setup>

import Navbar from './components/Navbar.vue'


</script>
<style lang="scss">
@import "./assets/scss/main.scss";
</style>

<style scoped>
header {
  z-index: 2;
}

main {
  background: #FEF6F0;

}
</style>