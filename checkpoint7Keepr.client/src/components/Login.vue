<template>
  <div class="px-3">
    <button @click="login" v-if="!user.isAuthenticated">Login</button>
    <div v-else class="d-flex gap-2 align-items-center">
      <button class="" @click="logout">Logout</button>

      <div v-if="account.picture || user.picture">
        <RouterLink :to="{ name: 'Profile', params: { id: account.id } }">

          <img :src="account.picture || user.picture" alt="account photo" />
        </RouterLink>
      </div>

    </div>
  </div>

</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState'
import { AuthService } from '../services/AuthService'
export default {
  setup() {
    return {
      user: computed(() => AppState.user),
      account: computed(() => AppState.account),
      async login() {
        AuthService.loginWithPopup()
      },
      async logout() {
        AuthService.logout({ returnTo: window.location.origin })
      }
    }
  }
}
</script>

<style lang="scss" scoped>
button {
  background: #E9D8D6;
  border: none;
  border-radius: 10px;
  padding: .25rem;
  padding-left: .5rem;
  padding-right: .5rem;
  font-weight: bold;
}

img {
  height: 75px;
  border-radius: 50%;
}
</style>
