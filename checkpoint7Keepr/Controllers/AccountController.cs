namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
	private readonly AccountService _accountService;
	private readonly Auth0Provider _auth0Provider;

	private readonly VaultsService _vs;
	private readonly KeepsService _ks;

	public AccountController(AccountService accountService, Auth0Provider auth0Provider, VaultsService vs, KeepsService ks)
	{
		_accountService = accountService;
		_auth0Provider = auth0Provider;
		_vs = vs;
		_ks = ks;
	}



	[HttpGet]
	[Authorize]
	public async Task<ActionResult<Account>> Get()
	{
		try
		{
			Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
			return Ok(_accountService.GetOrCreateProfile(userInfo));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPut]
	[Authorize]
	public async Task<ActionResult<Account>> Edit([FromBody] Account editData)
	{
		try
		{
			Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
			editData.Id = userInfo.Id;
			return Ok(_accountService.Edit(editData));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("vaults")]
	[Authorize]
	public async Task<ActionResult<List<Vault>>> GetVaultsByAccountId()
	{
		try
		{
			Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
			return Ok(_vs.GetVaultsByAccountId(userInfo.Id, userInfo.Id));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("keeps")]
	[Authorize]
	public async Task<ActionResult<List<Keep>>> GetKeepsByAccountId()
	{
		try
		{
			Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
			return Ok(_ks.GetKeepsByAccountId(userInfo.Id));
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}


}
