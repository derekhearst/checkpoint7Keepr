namespace checkpoint7Keepr.Services;

public class VaultKeepsService
{
	private readonly VaultKeepsRepository _vkr;

	public VaultKeepsService(VaultKeepsRepository vkr)
	{
		_vkr = vkr;
	}


	public KeepVault Create(KeepVault newVaultKeep)
	{
		return _vkr.Create(newVaultKeep);
	}

	public bool Delete(int id, string userId)
	{
		return _vkr.Delete(id, userId);
	}
}
