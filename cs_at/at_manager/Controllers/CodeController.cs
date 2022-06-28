using DataLib.Entity;
using DataLib.Repostiory;
using DataLib.Service;
using DownByWind.DbSet;
using DownByWind.Entity;
using Microsoft.AspNetCore.Mvc;

namespace at_manager.Controllers
{
    public class CodeController : Controller
    {
        private readonly ILogger<CodeController> _logger;

        public CodeController(ILogger<CodeController> logger)
        {
            _logger = logger;
            var codes = codeInfoRepository.GetRunCodeInfos();
            Console.WriteLine("GetRunCodeInfos: {0}", codes.Count);
        }

        CodeInfoRepository codeInfoRepository = new CodeInfoRepository();

        BarRepository barRepository = new BarRepository();

        [HttpGet]
        public IActionResult Index()
        {
            // 这里获得当前交易的代码
            var codes = codeInfoRepository.GetRunCodeInfos();   

            return View(new { Codes = codes });
        }

        BarService service = new BarService();

        [HttpGet]
        [Route("Code/{codeId}")]
        public IActionResult ViewCode(string codeId)
        {
            Console.WriteLine(codeId);

            CodeInfo code = codeInfoRepository.GetByCodeId(codeId);

            List<Bar2> dayBars = new List<Bar2>();
            if (code != null)
            {
                dayBars = service.GetBars(codeId, BarType.M1);
                Console.WriteLine($"get KBars {code.Name} {dayBars.Count}");
            }

            return View(new { Code = code, dayBars });
        }
    }
}
