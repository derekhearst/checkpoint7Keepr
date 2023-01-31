import { AppState } from "../AppState.js"
import { api } from "./AxiosService.js"

class VaultsService {
	async getVaultById(id) {
		const res = await api.get("api/vaults/" + id)
		console.log(res.data)
		return res.data
	}
	async editVault(id, vaultData) {
		const res = await api.put("api/vaults/" + id, vaultData)
		console.log(res.data)
		return res.data
	}
	async deleteVault(id) {
		const res = await api.delete("api/vaults/" + id)
		console.log(res.data)
		return res.data
	}

	async createVault(vaultData) {
		const res = await api.post("api/vaults", vaultData)
		console.log(res.data)
		AppState.myVaults = [...AppState.myVaults, res.data]
		return res.data
	}
}
export const vaultsService = new VaultsService()
