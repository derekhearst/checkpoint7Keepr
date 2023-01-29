namespace checkpoint7Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeepsController : ControllerBase
{
	private readonly KeepsService _ks;
	private readonly Auth0Provider _ap;

	public KeepsController(KeepsService ks, Auth0Provider ap)
	{
		_ks = ks;
		_ap = ap;
	}

	[HttpGet]
	public ActionResult<List<Keep>> GetAll()
	{
		try
		{
			List<Keep> keeps = _ks.GetAll();
			return Ok(keeps);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("{id}")]

	public async Task<ActionResult<Keep>> GetById(int id)
	{
		try
		{
			Account userInfo = await _ap.GetUserInfoAsync<Account>(HttpContext);
			Keep keep = _ks.GetById(id, userInfo?.Id);
			if (keep.Id == 0)
			{
				return BadRequest("Invalid Id");
			}
			return Ok(keep);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<Keep>> CreateAsync([FromBody] Keep newKeep)
	{
		try
		{
			Account userInfo = await _ap.GetUserInfoAsync<Account>(HttpContext);
			newKeep.CreatorId = userInfo.Id;
			Keep keep = _ks.Create(newKeep);
			return Ok(keep);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPut("{id}")]
	[Authorize]
	public async Task<ActionResult<Keep>> EditAsync([FromBody] Keep keepInfo, int id)
	{
		try
		{
			Account userInfo = await _ap.GetUserInfoAsync<Account>(HttpContext);
			keepInfo.Id = id;
			keepInfo.CreatorId = userInfo.Id;
			if (!_ks.Edit(keepInfo)) // if no rows were affected 
			{
				return BadRequest("You are not the creator of this keep, or it does not exist.");
			}
			return Ok(keepInfo);
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
			Account userInfo = await _ap.GetUserInfoAsync<Account>(HttpContext);
			if (!_ks.Delete(id, userInfo.Id)) // if no rows were affected
			{
				return BadRequest("You are not the creator of this keep, or it does not exist.");
			}
			return Ok("Successfully Deleted");
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}
}
