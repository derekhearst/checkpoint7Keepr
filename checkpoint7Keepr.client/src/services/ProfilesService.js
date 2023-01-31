import { api } from "./AxiosService.js"

class ProfilesService {
	async getById(id) {
		const res = await api.get("api/profiles/" + id)
		console.log(res.data)
		return res.data
	}

	async getKeepsByProfileId(id) {
		const res = await api.get("api/profiles/" + id + "/keeps")
		console.log(res.data)
		return res.data
	}

	async getVaultsByProfileId(id) {
		const res = await api.get("api/profiles/" + id + "/vaults")
		console.log(res.data)
		return res.data
	}
}
export const profilesService = new ProfilesService()
