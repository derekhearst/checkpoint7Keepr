namespace checkpoint7Keepr.Repositories;

public class VaultsRepository
{
	private readonly IDbConnection _db;

	public VaultsRepository(IDbConnection db)
	{
		_db = db;
	}

	public Vault GetById(int id, string userId)
	{
		return _db.Query<Vault, Account, Vault>(@"
		SELECT * FROM vaults v 
		JOIN accounts a ON v.creatorId = a.id  
		WHERE v.id = @id AND (v.creatorId = @userId OR v.isPrivate = false);",
		(v, a) =>
		{
			v.Creator = a;
			return v;
		}, new { id, userId }).FirstOrDefault();
	}

	public Vault Create(Vault newVault)
	{
		int id = _db.ExecuteScalar<int>(@"
		INSERT INTO vaults (name, description, img, isPrivate, creatorId)
		VALUES (@Name, @Description, @Img, @IsPrivate, @CreatorId);
		SELECT LAST_INSERT_ID();
		", newVault);
		return GetById(id, newVault.CreatorId);
	}

	public bool Edit(Vault updatedVault)
	{
		Vault original = GetById(updatedVault.Id, updatedVault.CreatorId);

		if (original == null)
		{
			throw new Exception("Invalid Id");
		}

		updatedVault.Name ??= original.Name;
		updatedVault.Description ??= original.Description;
		updatedVault.Img ??= original.Img;

		int rows = _db.Execute(@"
		UPDATE vaults
		SET
			name = @Name,
			description = @Description,
			img = @Img,
			isPrivate = @IsPrivate
		WHERE id = @Id AND creatorId = @CreatorId;
		", updatedVault);
		return rows > 0;
	}

	public bool Delete(int id, string userId)
	{
		int rows = _db.Execute(@"
		DELETE FROM vaults
		WHERE id = @id AND creatorId = @userId;
		", new { id, userId });
		return rows > 0;
	}

	internal List<Vault> GetVaultsByAccountId(string accountId, string userId)
	{
		return _db.Query<Vault, Account, Vault>(@"
		SELECT * FROM vaults v 
		JOIN accounts a ON v.creatorId = a.id  
		WHERE v.creatorId = @accountId AND (v.creatorId = @userId OR v.isPrivate = false);",
		(v, a) =>
		{
			v.Creator = a;
			return v;
		}, new { accountId, userId }).ToList();
	}

}
