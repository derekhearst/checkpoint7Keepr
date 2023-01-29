namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
	private readonly AccountService _acs;
	private readonly VaultsService _vs;
	private readonly KeepsService _ks;

	public ProfilesController(AccountService acs, VaultsService vs, KeepsService ks)
	{
		_acs = acs;
		_vs = vs;
		_ks = ks;
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
	public ActionResult<List<Vault>> GetVaultsByAccountId(string id)
	{
		try
		{
			return Ok(_vs.GetVaultsByAccountId(id));
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
