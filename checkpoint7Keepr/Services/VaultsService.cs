namespace checkpoint7Keepr.Services;

public class VaultsService
{
	private readonly VaultsRepository _vr;

	public VaultsService(VaultsRepository vr)
	{
		_vr = vr;
	}

	public Vault GetById(int id, string userId)
	{
		return _vr.GetById(id, userId);
	}

	public Vault Create(Vault newVault)
	{
		return _vr.Create(newVault);
	}

	public bool Edit(Vault updatedVault)
	{
		return _vr.Edit(updatedVault);
	}

	public bool Delete(int id, string userId)
	{
		return _vr.Delete(id, userId);
	}

	internal object GetVaultsByAccountId(string id, string userId)
	{
		return _vr.GetVaultsByAccountId(id, userId);
	}

}