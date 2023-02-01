<script setup>
import { ref, watchEffect, onMounted } from 'vue';
import { vaultsService } from '../services/VaultsService.js';
import { keepsService } from '../services/KeepsService.js';
import { useRoute, useRouter } from 'vue-router';
import Pop from '../utils/Pop.js';


const route = useRoute();
const router = useRouter();
const vault = ref({});
const keeps = ref([]);
onMounted(async () => {
	try {
		vault.value = await vaultsService.getVaultById(route.params.id)

		keeps.value = await keepsService.getByVaultId(route.params.id)
	} catch (error) {
		if (vault.value.id == null || vault.value.id == undefined) {
			Pop.error('Vault private or not found')
			router.push({ name: 'Home' })
		} else {
			Pop.error(error)
		}
	}
})



async function deleteVault() {
	if (await Pop.confirm("Are you sure you want to delete this vault?")) {
		try {
			await vaultsService.deleteVault(vault.value.id)
			Pop.success('Vault Deleted')
			router.push({ name: 'Home' })
		} catch (error) {
			Pop.error(error)
		}
	}
}
const editModalOpen = ref(false)
async function editVault(e) {
	let form = e.target
	let formData = {
		name: form.name?.value,
		description: form.description?.value,
		img: form.img?.value,
		isPrivate: form.isPrivate?.checked
	}
	try {
		const res = await vaultsService.editVault(vault.value.id, formData)
		vault.value.name = res.name
		vault.value.description = res.description
		vault.value.img = res.img
		vault.value.isPrivate = res.isPrivate
		editModalOpen.value = false
		Pop.success('Vault Updated')
	} catch (error) {
		Pop.error(error)
	}
}
watchEffect(() => {
	let mainBody = document.querySelector('#mainBody')
	if (editModalOpen.value) {
		mainBody.style.overflow = 'hidden'
	} else {
		mainBody.style.overflow = 'visible'
	}
})

</script>

<template >
	<div class="groupPage">
		<div class="vaultHeader">
			<div class="vaultCover">
				<img :src="vault.img" alt="Vault Cover Img">
				<h1>{{ vault.name }}</h1>
				<h5>by {{ vault.creator?.name }}</h5>
				<i v-if="vault.isPrivate" class="mdi mdi-lock"></i>
			</div>
			<div class="d-flex align-items-center justify-content-end">
				<div class="dropdown">
					<button class="dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown"
						aria-expanded="false">
						<i class="mdi mdi-dots-horizontal fs-4"></i>
					</button>
					<ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
						<li><button class="dropdown-item" @click="deleteVault">Delete</button></li>
						<li><button class="dropdown-item" @click="editModalOpen = true">Edit</button></li>
					</ul>
				</div>
			</div>
			<div class="keepCount">{{ keeps.length }} Keeps</div>
		</div>

		<div class="vaultKeeps">
			<KeepCard v-for="keep in keeps" :key="keep.id" :keep="keep"
				@removeKeep="keeps = keeps.filter(k => k.id != keep.id)" />
		</div>
	</div>

	<div class="modal" v-if="editModalOpen" @click="editModalOpen = false">
		<div class="body" @click.stop="">
			<h2>Edit Vault</h2>
			<form @submit.prevent="editVault">
				<label for="name">
					Name
					<input type="text" id="name" name="name" :value="vault.name" placeholder="Vault Name">
				</label>
				<label for="description">
					Description
					<input type="text" id="description" name="description" :value="vault.description"
						placeholder="Vault Description">
				</label>

				<label for="img">
					Image
					<input type="text" id="img" name="img" :value="vault.img" placeholder="Vault Image">
				</label>

				<label for="isPrivate">
					Private
					<input type="checkbox" id="isPrivate" name="isPrivate" :checked="vault.isPrivate">
				</label>

				<div class="d-flex align-items-center justify-content-between">
					<button type="button" class="px-3" @click="editModalOpen = false">Cancel</button>
					<button type="submit" class="px-3" @click="editVault">Save</button>
				</div>

			</form>

		</div>
	</div>

</template>
<style scoped lang="scss">
.groupPage {
	display: flex;
	flex-direction: column;
	align-items: center;
	padding: 1rem;
}

.keepCount {
	background-color: rgb(171, 95, 171);
	color: white;
	padding: .35rem;
	border-radius: 10px;
	width: max-content;
	margin: auto;
	margin-top: -1.5rem;
}

.dropdown-toggle {
	border: none !important;
	padding: .25rem;
	background-color: transparent;
}

.vaultCover {
	position: relative;
	width: 25rem;
	height: 13rem;

	img {
		width: 100%;
		height: 100%;
		object-fit: cover;
		border-radius: 10px;
		// background: linear-gradient(rgba(0, 0, 0, 0.0), rgba(0, 0, 0, 0.9));
		filter: brightness(70%);
	}

	h1 {
		position: absolute;
		bottom: 2rem;
		left: 30%;
		color: white;
	}

	h5 {
		position: absolute;
		bottom: 0;
		right: 30%;
		color: white;
	}

	i {
		position: absolute;
		bottom: 0;
		right: 0;
		font-size: 1.5rem;
		color: white;
	}


}

.vaultKeeps {
	padding-top: 2rem;
	columns: 3;
}

.modal {
	position: fixed;
	top: 0;
	left: 0;
	background-color: rgba(0, 0, 0, 0.5);
	width: 100%;
	height: 100%;
	z-index: 1000;
	display: flex;
	align-items: center;
	justify-content: center;

	.body {
		background-color: #FEF6F0;
		max-width: 80%;
		max-height: 80%;
		padding: .5rem;
		display: flex;
		flex-direction: column;
	}

	button {
		background-color: #FEF6F0;
		border: 1px solid black;
		border-radius: 10px;
	}

	form {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	label {
		display: flex;
		gap: 0rem;
		align-items: center;
		justify-content: space-between;

		input {
			width: 20rem;
		}
	}

}

@media (max-width: 768px) {
	.vaultCover {
		width: 100%;
		height: 20rem;
	}

	.vaultKeeps {
		columns: 1;
	}

	label {
		flex-direction: column;
		gap: 1rem;
	}

	input {
		width: 100% !important;
	}

}
</style>