namespace checkpoint7Keepr.Services;

public class KeepsService
{
	private readonly KeepsRepository _repo;

	public KeepsService(KeepsRepository repo)
	{
		_repo = repo;
	}

	public List<Keep> GetAll()
	{
		return _repo.GetAll();
	}

	public Keep GetById(int id, string userId)
	{
		Keep keep = _repo.GetById(id, userId);
		if (keep == null)
		{
			throw new Exception("Invalid Id");
		}
		return keep;
	}

	public Keep Create(Keep newKeep)
	{
		return _repo.Create(newKeep);
	}

	public bool Edit(Keep keepInfo)
	{
		return _repo.Edit(keepInfo);
	}

	public bool Delete(int id, string userId)
	{
		return _repo.Delete(id, userId);
	}

	internal List<KeepInVault> GetKeepsByVaultId(int vaultId, string userId)
	{
		return _repo.GetKeepsByVaultId(vaultId, userId);
	}

	internal List<Keep> GetKeepsByAccountId(string id)
	{
		return _repo.GetKeepsByAccountId(id);
	}
}
