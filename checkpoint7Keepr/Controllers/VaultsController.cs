namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultsController : ControllerBase
{
	private readonly VaultsService _vs;
	private readonly KeepsService _ks;
	private readonly Auth0Provider _authS;

	public VaultsController(VaultsService vs, Auth0Provider authS, KeepsService ks)
	{
		_vs = vs;
		_authS = authS;
		_ks = ks;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Vault>> GetById(int id)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			Vault vault = _vs.GetById(id, user?.Id);
			if (vault == null)
			{
				throw new Exception("Invalid Id");
			}
			return Ok(vault);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("{id}/keeps")]
	public async Task<ActionResult<List<KeepInVault>>> GetKeepsByVaultId(int id)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			List<KeepInVault> keeps = _ks.GetKeepsByVaultId(id, user?.Id);

			return Ok(keeps);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}



	[HttpPost]
	[Authorize]
	public async Task<ActionResult<Vault>> CreateAsync([FromBody] Vault newVault)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			newVault.CreatorId = user.Id;
			return Ok(_vs.Create(newVault));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPut("{id}")]
	[Authorize]
	public async Task<ActionResult<Vault>> EditAsync([FromBody] Vault updatedVault, int id)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			updatedVault.Id = id;
			updatedVault.CreatorId = user.Id;
			if (!_vs.Edit(updatedVault))
			{
				throw new Exception("Invalid Id");
			}
			return Ok(updatedVault);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpDelete("{id}")]
	[Authorize]
	public async Task<ActionResult<string>> DeleteAsync(int id)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			if (!_vs.Delete(id, user.Id))
			{
				throw new Exception("Invalid Id");
			}
			return Ok("Successfully Deleted");

		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}
}
