import { AppState } from "../AppState.js"
import { api } from "./AxiosService.js"

class KeepsService {
	async getKeeps() {
		const res = await api.get("api/keeps")
		console.log(res.data)
		AppState.keeps = res.data
		return res.data
	}

	async createKeep(keepData) {
		const res = await api.post("api/keeps", keepData)
		console.log(res.data)
		AppState.keeps = [...AppState.keeps, res.data]
		AppState.myKeeps = [...AppState.myKeeps, res.data]
		return res.data
	}

	async saveKeep(keepId, vaultId) {
		const res = await api.post("api/vaultkeeps", { keepId, vaultId })
		console.log(res.data)
		return res
	}

	async getById(id) {
		const res = await api.get("api/keeps/" + id)
		console.log(res.data)
		return res.data
	}

	async getByVaultId(vaultId) {
		const res = await api.get("api/vaults/" + vaultId + "/keeps")
		console.log(res.data)
		return res.data
	}
	async removeKeep(vaultKeepId) {
		const res = await api.delete("api/vaultkeeps/" + vaultKeepId)
		console.log(res.data)
		return res
	}

	async deleteKeep(id) {
		const res = await api.delete("api/keeps/" + id)
		console.log(res.data)
		AppState.keeps = AppState.keeps.filter(k => k.id != id)
		AppState.myKeeps = AppState.myKeeps.filter(k => k.id != id)
		return res.data
	}
}

export const keepsService = new KeepsService()
