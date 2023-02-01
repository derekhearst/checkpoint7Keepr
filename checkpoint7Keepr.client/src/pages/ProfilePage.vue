<script setup>
import { ref, watchEffect, onMounted } from 'vue';
import { profilesService } from '../services/ProfilesService.js';
import { useRoute } from 'vue-router';
import KeepCard from '../components/KeepCard.vue';
import VaultCard from '../components/VaultCard.vue';


const route = useRoute()
const profile = ref({})
const vaults = ref([])
const keeps = ref([])
onMounted(async () => {
	console.log('mounted')
	watchEffect(async () => {
		if (route.params.id) {
			await Promise.all([ // nice speed up
				profilesService.getById(route.params.id),
				profilesService.getVaultsByProfileId(route.params.id),
				profilesService.getKeepsByProfileId(route.params.id)
			]).then(([profileData, vaultData, keepData]) => {
				profile.value = profileData
				vaults.value = vaultData
				keeps.value = keepData
			})
		}
	})
})

</script>

<template>
	<div class="d-flex flex-column pt-3">
		<img v-if="profile.coverImg" :src="profile.coverImg" alt="CoverIMG" class="coverImg">
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
</template>

<style scoped>
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
</style>