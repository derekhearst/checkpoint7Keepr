namespace checkpoint7Keepr.Services;

public class AccountService
{
	private readonly AccountsRepository _repo;


	public AccountService(AccountsRepository repo)
	{
		_repo = repo;
	}

	internal Account GetProfileByEmail(string email)
	{
		return _repo.GetByEmail(email);
	}

	internal Account GetOrCreateProfile(Account userInfo)
	{
		Account profile = _repo.GetById(userInfo.Id);
		if (profile == null)
		{
			return _repo.Create(userInfo);
		}
		return profile;
	}

	internal Account Edit(Account editData)
	{
		return _repo.Edit(editData);
	}

	internal Account GetAccountById(string id)
	{
		Account profile = _repo.GetById(id);
		if (profile == null)
		{
			throw new Exception("Invalid Id");
		}
		return profile;
	}


}
