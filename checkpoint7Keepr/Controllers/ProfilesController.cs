namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
	private readonly AccountService _acs;
	private readonly VaultsService _vs;
	private readonly KeepsService _ks;
	private readonly Auth0Provider _authS;


	public ProfilesController(AccountService acs, VaultsService vs, KeepsService ks, Auth0Provider authS)
	{
		_acs = acs;
		_vs = vs;
		_ks = ks;
		_authS = authS;
	}

	[HttpGet("{id}")]
	public ActionResult<Account> GetAccountById(string id)
	{
		try
		{
			return Ok(_acs.GetAccountById(id));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("{id}/vaults")]
	public async Task<ActionResult<List<Vault>>> GetVaultsByAccountIdAsync(string id)
	{
		try
		{
			Account user = await _authS.GetUserInfoAsync<Account>(HttpContext);
			return Ok(_vs.GetVaultsByAccountId(id, user?.Id));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("{id}/keeps")]
	public ActionResult<List<Keep>> GetKeepsByAccountId(string id)
	{
		try
		{
			return Ok(_ks.GetKeepsByAccountId(id));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}
}
