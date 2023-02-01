<template>
  <header>
    <div class="header">

      <a href="#" class="buttonHome">Home</a>
      <div class="dropdown" v-if="AppState.account.id">
        <button class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">Create</button>
        <ul class="dropdown-menu">
          <li><button class="dropdown-item" @click="isKeepModalOpen = true">New Keep</button></li>
          <li><button class="dropdown-item" @click="isVaultModalOpen = true">New Vault</button></li>
        </ul>
      </div>

    </div>
    <img src="../assets/img/Keepr logo.png" alt="Keepr logo" />
    <Login />
  </header>

  <div v-if="isVaultModalOpen" class="modalBg" @click="isVaultModalOpen = !isVaultModalOpen">
    <div class="modalBody" @click.stop="">
      <h2>Create Vault</h2>
      <form @submit.prevent="createVault">
        <label for="name">
          Vault Name
          <input type="text" required name="name" id="name" />
        </label>
        <label for="description">
          Vault Description
          <input type="text" required name="description" id="description" />
        </label>
        <label for="img">
          Vault Image
          <input type="url" required name="img" id="img" />
        </label>
        <label for="isPrivate">
          Private
          <input type="checkbox" name="isPrivate" id="isPrivate" />
        </label>
        <div class="d-flex justify-content between">

          <button type="button" @click="isVaultModalOpen = false">Cancel</button>
          <button type="submit">Create Vault</button>
        </div>

      </form>
    </div>
  </div>

  <div v-if="isKeepModalOpen" class="modalBg" @click="isKeepModalOpen = !isKeepModalOpen">
    <div class="modalBody" @click.stop="">
      <h2>Create Keep</h2>
      <form @submit.prevent="createKeep">

        <label for="title">Keep title

          <input type="text" required name="title" id="title" />
        </label>



        <label for="img">Keep Image

          <input type="url" required name="img" id="img" />
        </label>


        <label for="description">Keep Description

          <textarea type="text" required name="description" id="description"></textarea>
        </label>

        <div class="d-flex justify-content-between">

          <button type="button" @click="isKeepModalOpen = false">Cancel</button>
          <button type="submit">Create Keep</button>
        </div>

      </form>
    </div>
  </div>


</template>

<script setup>

import { ref, watchEffect, onMounted } from 'vue';
import { keepsService } from '../services/KeepsService.js';
import { vaultsService } from '../services/VaultsService.js';
import Pop from '../utils/Pop.js';
import Login from './Login.vue'
import { AppState } from '../AppState.js';
async function createKeep(e) {
  try {
    keepsService.createKeep(
      {
        name: e.target.title.value,
        img: e.target.img.value,
        description: e.target.description.value
      }
    )
    isKeepModalOpen.value = false
    Pop.success('Keep created')
  } catch (error) {
    Pop.error(error)
  }

}

async function createVault(e) {
  try {
    vaultsService.createVault(
      {
        name: e.target.name.value,
        description: e.target.description.value,
        img: e.target.img.value,
        isPrivate: e.target.isPrivate.checked
      }
    )
    Pop.success('Vault created')
    isVaultModalOpen.value = false
  } catch (error) {
    Pop.error(error)
  }

}

const isVaultModalOpen = ref(false)
const isKeepModalOpen = ref(false)

watchEffect(() => {
  let mainBody = document.querySelector('#mainBody')
  if (isVaultModalOpen.value) {
    mainBody.style.overflow = 'hidden'
  } else {
    mainBody.style.overflow = 'visible'
  }
  if (isKeepModalOpen.value) {
    mainBody.style.overflow = 'hidden'
  } else {
    mainBody.style.overflow = 'visible'
  }
})


</script>


<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: .25rem;
  flex-wrap: wrap;

  align-self: stretch !important;

}

header {
  background: rgba(254, 246, 240, 1);
  box-shadow: 0px 0px 5px 5px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: .25rem;
}

img {
  height: 75px;
}

.buttonHome {
  background: #E9D8D6;
  border: none;
  border-radius: 10px;
  padding: .25rem;
  padding-left: .5rem;
  padding-right: .5rem;
  font-weight: bold;
  color: black;
}

.dropdown-toggle {
  background: inherit;
  border: none;
  border-radius: 10px;
  padding: .25rem;
  padding-left: .5rem;
  padding-right: .5rem;
  font-weight: bold;
}

.modalBg {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modalBody {
  background: #FEF6F0;
  padding: 1rem;
  font-size: 1.3rem;
  display: flex;
  flex-direction: column;
}

.modalBody label {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: .5rem;
}

.modalBody form {
  display: flex;
  flex-direction: column;
  gap: .5rem;
}

.modalBody button {
  background: #E9D8D6;
  border: none;
  border-radius: 10px;
  padding: .25rem;
  padding-left: .5rem;
  padding-right: .5rem;
  font-weight: bold;
  color: black;
  cursor: pointer;
  margin: auto;
}

@media (max-width: 600px) {
  .modalBody {
    width: 85%;

    padding: .25rem;
  }

  label {
    display: flex;
    flex-direction: column;
    gap: .1rem;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: .7rem;
  }
}
</style>
