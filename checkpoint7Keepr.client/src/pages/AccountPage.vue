<script setup>
import { ref, watchEffect, onMounted, computed } from 'vue';
import { profilesService } from '../services/ProfilesService.js';
import { useRoute } from 'vue-router';
import KeepCard from '../components/KeepCard.vue';
import VaultCard from '../components/VaultCard.vue';
import { AppState } from '../AppState.js';
import { accountService } from '../services/AccountService.js';
import Pop from '../utils/Pop.js';


const route = useRoute()
const profile = computed(() => AppState.account)
const vaults = ref([])
const keeps = ref([])
const editModalOpen = ref(false)
onMounted(async () => {
  console.log('mounted')
  await Promise.all([ // nice speed up
    accountService.getMyKeeps(AppState.account.id),
    accountService.getMyVaults(AppState.account.id)
  ]).then(([keepData, vaultData]) => {
    keeps.value = keepData
    vaults.value = vaultData
  })

})

async function editAccount(e) {
  try {
    profile.value = await accountService.editAccount(
      {
        name: e.target.name.value,
        picture: e.target.picture.value,
        coverImg: e.target.coverImg.value
      }
    )

    Pop.success('Account Updated')
    editModalOpen.value = false
  } catch (error) {
    Pop.error(error)
  }
}

</script>

<template>
  <div class="d-flex flex-column pt-3">
    <img v-if="profile.coverImg" :src="profile.coverImg" alt="CoverIMG" class="coverImg">
    <div class="editContainer">
      <i class="mdi mdi-dots-horizontal fs-3 editButton" title="Edit Account" @click="editModalOpen = true"></i>
    </div>
    <div class="d-flex flex-column align-items-center">
      <img :src="profile.picture" class="rounded-circle">
      <h1>{{ profile.name }}</h1>
      <div>{{ vaults.length }} Vaults | {{ keeps.length }} Keeps</div>
    </div>

    <div class="p-3">
      <h2>Vaults</h2>
      <div class="vaults">
        <VaultCard v-for="vault in vaults" :key="vault.id" :vault="vault" />
      </div>
    </div>

    <div class="p-3">
      <h2>Keeps</h2>
      <div class="keeps">
        <KeepCard v-for="keep in keeps" :key="keep.id" :keep="keep" />
      </div>
    </div>

  </div>

  <div class="modal" v-if="editModalOpen">
    <div class="body">
      <form @submit.prevent="editAccount">
        <h2>Edit Account</h2>
        <label for="name">
          Name
          <input type="text" id="name" required name="name" :value="profile.name">
        </label>
        <label for="picture">
          Picture
          <input type="picture" id="picture" required name="picture" :value="profile.picture">
        </label>
        <label for="coverImg">
          Cover Image
          <input type="coverImg" id="coverImg" name="coverImg" :value="profile.coverImg">
        </label>
        <div class="d-flex gap-2">
          <button type="button" @click="editModalOpen = false">Cancel</button>
          <button type="submit">Save</button>
        </div>


      </form>
    </div>
  </div>
</template>

<style scoped lang="scss">
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  z-index: 100;
  display: flex;
  justify-content: center;
  align-items: center;

  .body {
    background-color: white;
    border-radius: 1rem;
    padding: 1rem;
    max-width: 80%;

    form {
      display: flex;
      flex-direction: column;
      gap: 1rem;
      align-items: center;
      max-width: 100%;

      button {
        border: 1px solid black;
        border-radius: .5rem;
        padding: .25rem .5rem;
      }

      label {
        display: flex;
        flex-direction: column;
        gap: .4rem;
        width: 100%;

        input {
          width: 100%;
        }
      }
    }
  }
}

.keeps {
  column-count: 3;
  column-gap: 1rem;
  row-gap: 1rem;
}

.coverImg {
  width: 40rem;
  height: 200px;
  object-fit: cover;
  margin: auto;
  margin-bottom: -3rem;
}

@media (max-width: 768px) {
  .keeps {
    column-count: 1;
  }

  .coverImg {
    width: 100%;
  }
}



.vaults {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.editButton {
  translate: 0rem 3rem;
  cursor: pointer;
}

.editContainer {
  display: flex;
  justify-content: flex-end;
  height: 0;
}

@media (min-width: 768px) {
  .editContainer {
    width: 35rem;
  }
}
</style>