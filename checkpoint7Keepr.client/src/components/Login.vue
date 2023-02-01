<template>
  <div class="">
    <button @click="login" v-if="!user.isAuthenticated">Login</button>
    <div v-else class="bits">
      <button class="" @click="logout">Logout</button>
      <div v-if="account.id">
        <RouterLink :to="{ name: 'Account' }">
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

.bits {
  display: flex;
  align-items: center;
  flex-wrap: wrap-reverse;
  gap: .5rem;
  justify-content: center;
}
</style>
