<script setup>
import { ref, defineProps, watchEffect } from 'vue';
import { AppState } from '../AppState.js';
import { keepsService } from '../services/KeepsService.js';
import Pop from '../utils/Pop.js';

const props = defineProps({
	keep: {
		type: Object,
		required: true
	}
})
const emits = defineEmits(['removeKeep'])

const modelOpen = ref(false)
const keptCount = ref(0)
watchEffect(async () => {
	let mainBody = document.querySelector('#mainBody')
	if (modelOpen.value) {
		mainBody.style.overflow = 'hidden'
		let res = await keepsService.getById(props.keep.id)
		keptCount.value = res.kept
		props.keep.vies = res.views
	}
	else {
		mainBody.style.overflow = 'visible'
	}
})
const vaultId = ref(0)
async function saveKeep() {
	let res = await keepsService.saveKeep(props.keep.id, vaultId.value)
	console.log(res)
	if (res.status == 200) {
		Pop.success("Successfully Kept")
	}
	keptCount.value++
	modelOpen.value = false
}

async function removeKeep() {
	if (await Pop.confirm("Are you sure you want to un-keep this keep?")) {
		try {
			await keepsService.removeKeep(props.keep?.vaultKeepId)
			Pop.success('Keep Removed')
			emits("removeKeep", props.keep.id)
		} catch (error) {
			Pop.error(error)
		}
	}
}

async function deleteKeep() {
	if (await Pop.confirm("Are you sure you want to delete this keep?")) {
		try {
			await keepsService.deleteKeep(props.keep.id)
			Pop.success('Keep Deleted')
			emits("removeKeep", props.keep.id)
		} catch (error) {
			Pop.error(error)
		}
	}
	modelOpen.value = false
}

</script>

<template>
	<div class="keepCard" @click="modelOpen = true">
		<img class="keepBG" :src="keep.img" />
		<h1>{{ keep.name }}</h1>
		<img class="keepCreator" :src="keep.creator.picture" />
	</div>

	<div class="modalBg" v-if="modelOpen" @click="modelOpen = false">
		<div class="modalBody" @click.stop="">
			<img :src="keep.img" class="keepImage" />
			<div class="keepBody">
				<div class="keepHeader">

					<i class="mdi mdi-eye"></i>
					<p>{{ keep.views }}</p>
					<i class="mdi mdi-share"></i>
					<p>{{ keptCount }}</p>
				</div>
				<div class="keepInfo">

					<h2>{{ keep.name }}</h2>
					<p>{{ keep.description }}</p>

				</div>
				<div class="keepFooter">
					<button class="removeKeep" @click="removeKeep" v-if="keep.vaultKeepId">
						<i class="mdi fs-4 mdi-close-octagon-outline"></i>
						Un-Keep
					</button>

					<div class="d-flex align-items-center gap-3" v-else>
						<select v-model="vaultId">
							<option v-for="vault in AppState.myVaults" :value="vault.id">{{ vault.name }}</option>
						</select>
						<button class="niceButton" @click="saveKeep">Keep</button>
					</div>
					<button class="deleteKeep" @click="deleteKeep" v-if="keep.creatorId == AppState.account?.id">
						<i class="mdi fs-4 mdi-trash-can"></i>
						Delete Keep
					</button>

					<RouterLink :to="{
						name: 'Profile', params: { id: keep.creator.id }
					}">
						<div class="d-flex align-items-center gap-2">
							<img :src="keep.creator.picture" class="keepCreatorSmall" />
							<p class="m-0">{{ keep.creator.name }}</p>
						</div>
					</RouterLink>
				</div>
			</div>
		</div>
	</div>
</template>

<style scoped lang="scss">
.keepCard {
	position: relative;
	margin-bottom: 1rem;
	cursor: pointer;


}

.keepBG {
	position: static;
	height: 100%;
	width: 100%;
	border-radius: 10px;
	filter: brightness(80%);
}

h1 {
	position: absolute;
	bottom: 0;
	left: .25rem;
	margin: 0;
	color: white;
	// background-color: rgba(0, 0, 0, 0.212);

}

.keepCreator {
	position: absolute;
	bottom: .25rem;
	right: .25rem;
	height: 50px;
	width: 50px;
	border-radius: 50%;

}

.deleteKeep {
	background-color: transparent;
	color: lightcoral;
	border: none;
	font-weight: bold;
	border-bottom: 2px solid lightcoral;
	padding: -.5rem .5rem;

	display: flex;
	align-items: center;
}

.removeKeep {
	background-color: transparent;
	color: plum;
	border: none;
	font-weight: bold;
	border-bottom: 2px solid plum;
	padding: -.5rem .5rem;

	display: flex;
	align-items: center;
}

.modalBg {
	position: fixed;
	width: 100%;
	height: 100%;
	top: 0;
	left: 0;
	background: rgba(0, 0, 0, 0.5);
	display: flex;
	justify-content: center;
	align-items: center;
	z-index: 2;
}

.modalBody {
	background: #FEF6F0;
	display: flex;
	max-width: 80%;
	max-height: 80%;
}

.keepImage {
	width: 40%;
}

.keepBody {
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: space-between;
	padding: 1rem;
	width: 60%;
}

.keepHeader {
	display: flex;
	gap: 1rem;
	justify-content: center;
	color: rgba(0, 0, 0, 0.599);
}

.keepInfo {
	display: flex;
	flex-direction: column;
	align-items: center;


}

.keepFooter {
	display: flex;
	gap: 1rem;
	justify-content: space-between;
	flex-wrap: wrap;

	align-items: center;
	width: 100%;

}

.keepCreatorSmall {
	height: 30px;
	width: 30px;
	border-radius: 50%;
}

.niceButton {
	background-color: rgb(212, 59, 212);
	color: white;
	border: none;
	border-radius: 5px;
	padding: 5px 10px;
}
</style>