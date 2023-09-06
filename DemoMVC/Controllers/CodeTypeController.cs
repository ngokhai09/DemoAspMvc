
using DemoMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DemoMVC.Controllers
{
    public class CodeType : Controller
    {
        // GET: CodeType
        private readonly ApplicationContext _context;

        public CodeType(ApplicationContext context)
        {
            _context = context;
        }

        public void Index()
        {
           
        }

        public IActionResult GetCodeTypes()
        {
            var codeType = from m in _context.CodeTypes where "PropertyTypeCode" == m.Type select m;
            Console.WriteLine(codeType.ToList());
            if (!codeType.IsNullOrEmpty())
            {
                
                return PartialView("_Header", codeType.Distinct().ToList());

            }

            return PartialView("_Header",  null);
        }

        public IActionResult GetCodeTypeOptions()
        {
            var codeType = from m in _context.CodeTypes where "PropertyTypeCode" == m.Type select m;
            if (!codeType.IsNullOrEmpty())
            {
                
                return PartialView("_CodeTypeOptions", codeType.Distinct().ToList());

            }

            return PartialView("_CodeTypeOptions",  null);
        }
    }
}