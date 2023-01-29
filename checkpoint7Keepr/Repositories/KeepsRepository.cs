namespace checkpoint7Keepr.Repositories;

public class KeepsRepository
{
	public readonly IDbConnection _db;

	public KeepsRepository(IDbConnection db)
	{
		_db = db;
	}

	public List<Keep> GetAll()
	{
		return _db.Query<Keep, Account, Keep>("SELECT * FROM keeps k JOIN accounts a ON k.creatorId = a.id", (k, a) =>
		{
			k.Creator = a;
			return k;
		}).ToList();
	}



	public Keep GetById(int id, string userId = null)
	{
		Keep k = _db.Query<Keep, Account, Keep>(@"SELECT
			k.*, COUNT(vk.id) AS kept, a.*
		  FROM keeps k 
			JOIN accounts a ON k.creatorId = a.id 
			LEFT JOIN vaultkeeps vk ON k.id = vk.keepId
			WHERE k.id = @id", (k, a) =>
		{
			k.Creator = a;
			return k;
		}, new { id }).First();

		if (k == null)
		{
			throw new Exception("Invalid Id");
		}

		if (k.CreatorId != userId)
		{
			_db.ExecuteScalar<int>("UPDATE keeps SET views = views + 1 WHERE id = @id", new { id });
		}
		return k;
	}

	public List<KeepInVault> GetKeepsByVaultId(int vaultId, string userId)
	{
		if (userId == null)
		{
			userId = "";
		}

		Vault vault = _db.Query<Vault, Account, Vault>(@"
		SELECT * FROM vaults v
		JOIN accounts a ON v.creatorId = a.id
		WHERE v.id = @vaultId
		", (v, a) =>
		{
			v.Creator = a;
			return v;
		}, new { vaultId }).FirstOrDefault();

		if (vault == null)
		{
			throw new Exception("Invalid Id");
		}

		if (vault.CreatorId != userId && vault.IsPrivate)
		{
			throw new Exception("Invalid Id");
		}

		return _db.Query<KeepInVault, Account, KeepInVault>(@"
		SELECT 
		k.*, vk.id AS vaultKeepId, v.id AS vaultId, a.*
		FROM keeps k
		JOIN vaultkeeps vk ON k.id = vk.keepId
		JOIN vaults v ON vk.vaultId = v.id
		JOIN accounts a ON v.creatorId = a.id
		WHERE vk.vaultId = @vaultId AND (v.creatorId = @userId OR v.isPrivate = false)
		", (vk, a) =>
		{
			vk.Creator = a;
			return vk;
		}, new { vaultId, userId }).ToList();
	}

	public Keep Create(Keep keep)
	{
		int id = _db.ExecuteScalar<int>(@"
		INSERT INTO keeps (name, description, img, creatorId)
		VALUES (@Name, @Description, @Img, @CreatorId);
		SELECT LAST_INSERT_ID();
		", keep);
		return this.GetById(id);
	}

	public bool Edit(Keep keep)
	{
		Keep original = this.GetById(keep.Id);
		if (original == null)
		{
			throw new Exception("Invalid Id");
		}
		if (original.CreatorId != keep.CreatorId)
		{
			throw new Exception("You cannot edit a keep you did not create");
		}
		keep.Name ??= original.Name;
		keep.Description ??= original.Description;
		keep.Img ??= original.Img;
		keep.CreatorId = original.CreatorId;

		int rows = _db.Execute(@"
		UPDATE keeps
		SET
			name = @Name,
			description = @Description,
			img = @Img,
			creatorId = @CreatorId
		WHERE id = @Id AND creatorId = @CreatorId;
		", keep);

		return rows > 0;
	}

	public bool Delete(int id, string creatorId)
	{
		int rows = _db.Execute("DELETE FROM keeps WHERE id = @id AND creatorId = @creatorId", new { id, creatorId });
		return rows > 0;
	}

	internal List<Keep> GetKeepsByAccountId(string accountId)
	{
		return _db.Query<Keep, Account, Keep>(@"
		SELECT 
		k.*, COUNT(vk.id) AS kept, a.*
		FROM keeps k
		JOIN accounts a ON k.creatorId = a.id
		LEFT JOIN vaultkeeps vk ON k.id = vk.keepId
		WHERE k.creatorId = @accountId
		GROUP BY k.id, a.id
		", (k, a) =>
		{
			k.Creator = a;
			return k;
		}, new { accountId }).ToList();
	}
}
