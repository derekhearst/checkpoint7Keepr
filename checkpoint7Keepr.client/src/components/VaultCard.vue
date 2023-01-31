<script setup>
import { ref, watchEffect, onMounted } from 'vue';

const props = defineProps({
	vault: { type: Object, required: true }
})

</script>

<template>
	<RouterLink :to="{ name: 'Vault', params: { id: vault.id } }">
		<div class="vaultCard">
			<img :src="vault.img" alt="Vault Cover Img">
			<h2>{{ vault.name }}</h2>
			<i v-if="vault.isPrivate" class="mdi mdi-lock text-white fs-4 "></i>
		</div>
	</RouterLink>
</template>

<style scoped>
.vaultCard {
	width: 15rem;
	height: 10rem;
	position: relative;
	background-size: cover;
	background-position: center;
	background-repeat: no-repeat;
}

img {
	width: 100%;
	height: 100%;
	object-fit: cover;
	position: static;
	filter: brightness(80%);
	border-radius: 10px;
}

i {
	font-size: 1.5rem;
	position: absolute;
	right: 0;
	bottom: 0;
}

h2 {
	color: white;
	margin: 0;
	position: absolute;
	padding: .25rem;
	left: 0;
	bottom: 0;

}
</style>