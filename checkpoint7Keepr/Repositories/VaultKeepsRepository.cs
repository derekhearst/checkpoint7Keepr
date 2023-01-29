namespace checkpoint7Keepr.Repositories;

public class VaultKeepsRepository
{
	private readonly IDbConnection _db;

	public VaultKeepsRepository(IDbConnection db)
	{
		_db = db;
	}


	public KeepVault Create(KeepVault newVaultKeep)
	{
		Vault vault = _db.QueryFirstOrDefault<Vault>(@"
				SELECT * FROM vaults WHERE id = @VaultId;
				", newVaultKeep);
		if (vault == null)
		{
			throw new Exception("Invalid Id");
		}

		if (vault.CreatorId != newVaultKeep.CreatorId)
		{
			throw new Exception("You do not have access to this vault");
		}



		int id = _db.ExecuteScalar<int>(@"
				INSERT INTO vaultkeeps (vaultId, keepId, creatorId)
				VALUES (@VaultId, @KeepId, @CreatorId);
				SELECT LAST_INSERT_ID();
				", newVaultKeep);
		newVaultKeep.Id = id;
		return newVaultKeep;
	}

	public bool Delete(int id, string userId)
	{
		Vault vault = _db.QueryFirstOrDefault<Vault>(@"
				SELECT * FROM vaultkeeps vk
				JOIN vaults v ON vk.vaultId = v.id
				WHERE vk.id = @id;
				", new { id });

		if (vault == null)
		{
			throw new Exception("Invalid Id");
		}

		if (vault.CreatorId != userId)
		{
			throw new Exception("You do not have access to this vault");
		}

		int rows = _db.Execute(@"
				DELETE FROM vaultkeeps WHERE id = @id AND creatorId = @userId;
				", new { id, userId });
		return rows > 0;
	}
}
