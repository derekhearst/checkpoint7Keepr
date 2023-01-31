import { AppState } from "../AppState.js"
import { api } from "./AxiosService.js"

class KeepsService {
	async getKeeps() {
		const res = await api.get("api/keeps")
		console.log(res.data)
		AppState.keeps = res.data
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
}

export const keepsService = new KeepsService()
