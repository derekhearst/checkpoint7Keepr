<template>
  <div class="page">
    <div class="keeps">
      <KeepCard v-for="keep in AppState.keeps" key="keep.id" :keep="keep" />
    </div>
  </div>
</template>

<script setup>
import { ref, watchEffect, onMounted } from 'vue';
import { AppState } from '../AppState.js';
import KeepCard from '../components/KeepCard.vue';
import { keepsService } from '../services/KeepsService.js';
import Pop from '../utils/Pop.js';


onMounted(async () => {
  try {
    await keepsService.getKeeps()
  }
  catch {
    Pop.error("Couldn't load keeps")
  }
})

</script>

<style scoped lang="scss">
.page {
  padding: 1rem;
}

.keeps {
  column-count: 3;
  column-gap: 1rem;
  row-gap: 1rem;

}
</style>
