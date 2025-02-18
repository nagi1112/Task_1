using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
[ApiController]
[Route("api/[controller]")]

public class WriteCheckPrice : ControllerBase

{
    [HttpPost]
    public async Task<IActionResult> CheckInput([FromBody] RequestModel request)
    {
        Db.InsertRequest(request.Url, request.Email,0);
        return Ok(new { Message = "Запрос обработан", Data = request });
    }
}
public class RequestModel
{
    public string Url { get; set; }
    public string Email { get; set; }
}


