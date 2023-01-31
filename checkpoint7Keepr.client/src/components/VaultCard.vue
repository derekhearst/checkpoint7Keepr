<script setup>
import { ref, watchEffect, onMounted } from 'vue';

const props = defineProps({
	vault: { type: Object, required: true }
})

</script>

<template>
	<div class="vaultCard" :style="{ backgroundImage: `url(${vault.img})` }">
		<h2>{{ vault.name }}</h2>
		<p v-if="vault.isPrivate">{{ vault.isPrivate }}</p>
	</div>
</template>

<style scoped>
.vaultCard {
	width: 15rem;
	height: 10rem;
	display: flex;
	align-items: flex-end;
	justify-content: space-between;
	padding: .25rem;
	background-size: cover;
	background-blend-mode: color;

}

h2 {
	color: white;
	background-color: rgba(0, 0, 0, 0.5);
	margin: 0;
}
</style>