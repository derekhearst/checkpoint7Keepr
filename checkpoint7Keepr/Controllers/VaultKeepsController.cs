namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultKeepsController : ControllerBase
{
	private readonly VaultKeepsService _vks;
	private readonly Auth0Provider _authS;

	public VaultKeepsController(VaultKeepsService vks, Auth0Provider authS)
	{
		_vks = vks;
		_authS = authS;
	}


	[HttpPost]
	[Authorize]
	public async Task<ActionResult<KeepVault>> CreateAsync([FromBody] KeepVault newVaultKeep)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			newVaultKeep.CreatorId = user.Id;
			return Ok(_vks.Create(newVaultKeep));
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
			if (!_vks.Delete(id, user.Id))
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
