using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers;
#if DEBUG

[Route("[controller]/[action]")]
public class TestController : Controller
{
    private readonly IMapper mapper;

    // GET
    public TestController(IMapper mapper)
    {
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult Map()
    {
        return Ok();
    }
}

#endif
