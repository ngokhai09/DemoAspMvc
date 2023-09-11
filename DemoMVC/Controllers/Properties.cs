using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.Data.SqlClient;
using DemoMVC.Models.ViewModel;
using System.Runtime.CompilerServices;
using System.Drawing.Printing;

namespace DemoMVC.Controllers
{
    public class Properties : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public Properties(ApplicationContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Properties
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "time_asc" : "";
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            return View();
        }
        public async Task<IActionResult> GetHouseItems(string sortOrder, int size = 8)
        {
            var @property = (from n in _context.Property orderby n.CreateOnDate descending select n).Take(size);
            ViewData["size"] = size;

            switch (sortOrder)
            {
                case "time_asc":
                    @property = property.OrderBy(x => x.CreateOnDate);
                    break;
                case "price_asc":
                    property = property.OrderBy(x => x.TotalPrice);
                    break;
                case "price_desc":
                    property = property.OrderByDescending(x => x.TotalPrice);
                    break;
                default:
                    break;
            }
            return PartialView("HouseItems",property.ToList());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            // var @property = await _context.Property
            //     .FirstOrDefaultAsync(m => m.Id == id);
            var @property = (from p in _context.Property
                             join c in _context.CodeTypes on p.PropertyCodeType.Id equals c.Id
                             join u in _context.User on p.CreateByUser.Id equals u.Id
                             where p.Id.Equals(id)
                             select new Property(p, c, u)).FirstOrDefault();
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Properties/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ProvinceName,DistrictName,CommuneName,ThumbnailUrl,PropertyCodeType,TotalArea,TotalPrice,TransactionTypeCode")] PropertyViewModel @property, IFormFile file)
        {


            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var filePath = Path.Combine(wwwRootPath + "/img/", file.FileName);
                property.ThumbnailUrl = file.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                @property.Id = Guid.NewGuid();
                Property newProperty = new Property(@property);
                newProperty.PropertyCodeType =
                    _context.CodeTypes.Find(Guid.Parse(@property.PropertyCodeType));
                newProperty.CreateOnDate = DateTime.Now;
                newProperty.LastModifiedOnDate = DateTime.Now;
                newProperty.AllowTransaction = true;
                newProperty.CreateByUser = (from user in _context.User where user.UserName == User.Identity.Name select user).ToList()[0];
                _context.Add(newProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@property);
        }

        [Authorize]
        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = (from p in _context.Property
                             join c in _context.User on p.CreateByUser.Id equals c.Id
                             where p.Id.Equals(id)
                             select new Property(p, null, c)).FirstOrDefault();
            if (property.CreateByUser.UserName != User.Identity.Name)
            {
                return RedirectToAction("Index", "Properties");
            }
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,ProvinceName,DistrictName,CommuneName,ThumbnailUrl,PropertyCodeType,TotalArea,TotalPrice,TransactionTypeCode")] PropertyViewModel @property, IFormFile? file)
        {
            Property old = _context.Property.Include(p => p.PropertyCodeType).Include(p => p.CreateByUser).Where(p => p.Id == id).FirstOrDefault();
            if (old == null && id != @property.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var filePath = Path.Combine(wwwRootPath + "/img/", file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        old.ThumbnailUrl = file.FileName;
                    }

                    old.Title = property.Title;
                    old.Description = property.Description;
                    old.ProvinceName = property.ProvinceName;
                    old.DistrictName = property.DistrictName;
                    old.CommuneName = property.CommuneName;
                    old.TransactionTypeCode = property.TransactionTypeCode;
                    old.TotalArea = property.TotalArea;
                    old.TotalPrice = decimal.Parse(property.TotalPrice.Replace(".",""));
                    old.PropertyCodeType = _context.CodeTypes.Find(Guid.Parse(@property.PropertyCodeType));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
                return View(@property);
        }

        [Authorize]
        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'DemoMVCContext.Property'  is null.");
            }
            var @property = await _context.Property.FindAsync(id);
            if (@property != null)
            {
                _context.Property.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(Guid id)
        {
            return (_context.Property?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> SalePropertyNews(SearchParams searchParams, string currentFilter, decimal addressFilter, string propertyFilter, int? pageNumber)
        {

            var @property = from n in _context.Property orderby n.CreateOnDate descending where n.TransactionTypeCode == "SALE" select n;
            if (!String.IsNullOrEmpty(searchParams.Reset))
            {
                searchParams = new SearchParams();
            }
            if (searchParams.SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.SearchString = currentFilter;
            }
            if (searchParams.PropertyAddress != 0)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.PropertyAddress = addressFilter;
            }
            if (searchParams.PropertyType != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.PropertyType = propertyFilter;
            }
            ViewData["CurrentFilter"] = searchParams.SearchString;
            ViewData["PropertyFilter"] = searchParams.PropertyType;
            ViewBag.AddressFilter = searchParams.PropertyAddress;
            ViewData["PriceFilter"] = String.IsNullOrEmpty(searchParams.PriceFilter) ? "0-30" : searchParams.PriceFilter;
            ViewData["AreaFilter"] = String.IsNullOrEmpty(searchParams.AreaFilter) ? "0-1000" : searchParams.AreaFilter;

            if (!String.IsNullOrEmpty(searchParams.SearchString))
            {
                property = property.Where(p => p.Title.Contains(searchParams.SearchString) || p.Description.Contains(searchParams.SearchString));
            }
            if (!String.IsNullOrEmpty(searchParams.PropertyType))
            {
                property = property.Where(p => p.PropertyCodeTypeId == new Guid(searchParams.PropertyType));
            }
            if (searchParams.PropertyAddress != 0)
            {
                property = property.Where(p => p.ProvinceCode == (decimal)searchParams.PropertyAddress);
            }
            if (!String.IsNullOrEmpty(searchParams.PriceFilter))
            {
                double[] price = Array.ConvertAll(searchParams.PriceFilter.Split("-"), double.Parse);
                property = property.Where(p => p.TotalPrice >= (decimal)(price[0] * 1e9) && p.TotalPrice <= (decimal)(price[1] * 1e9));
            }
            if (!String.IsNullOrEmpty(searchParams.AreaFilter))
            {
                double[] price = Array.ConvertAll(searchParams.AreaFilter.Split("-"), double.Parse);
                property = property.Where(p => p.TotalArea >= (decimal)price[0] && p.TotalArea <= (decimal)price[1]);
            }
            int pageSize = 2;
            return View(await PaginatedList<Property>.CreateAync(property.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> RentPropertyNews(SearchParams searchParams, string currentFilter, decimal addressFilter, string propertyFilter, int? pageNumber)
        {
            var @property = from n in _context.Property orderby n.CreateOnDate descending where n.TransactionTypeCode == "RENT" select n;

            if (searchParams.SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.SearchString = currentFilter;
            }
            if (searchParams.PropertyAddress != 0)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.PropertyAddress = addressFilter;
            }
            if (searchParams.PropertyType != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchParams.PropertyType = propertyFilter;
            }
            ViewData["CurrentFilter"] = searchParams.SearchString;
            ViewData["PropertyFilter"] = searchParams.PropertyType;
            ViewBag.AddressFilter = searchParams.PropertyAddress;
            ViewData["PriceFilter"] = String.IsNullOrEmpty(searchParams.PriceFilter) ? "0-999" : searchParams.PriceFilter;
            ViewData["AreaFilter"] = String.IsNullOrEmpty(searchParams.AreaFilter) ? "0-1000" : searchParams.AreaFilter;

            if (!String.IsNullOrEmpty(searchParams.SearchString))
            {
                property = property.Where(p => p.Title.Contains(searchParams.SearchString) || p.Description.Contains(searchParams.SearchString));
            }
            if (!String.IsNullOrEmpty(searchParams.PropertyType))
            {
                property = property.Where(p => p.PropertyCodeTypeId == new Guid(searchParams.PropertyType));
            }
            if (searchParams.PropertyAddress != 0)
            {
                property = property.Where(p => p.ProvinceCode == (decimal)searchParams.PropertyAddress);
            }
            if (!String.IsNullOrEmpty(searchParams.PriceFilter))
            {
                double[] price = Array.ConvertAll(searchParams.PriceFilter.Split("-"), double.Parse);
                property = property.Where(p => p.TotalPrice >= (decimal)(price[0] * 1e6) && p.TotalPrice <= (decimal)(price[1] * 1e6));
            }
            if (!String.IsNullOrEmpty(searchParams.AreaFilter))
            {
                double[] price = Array.ConvertAll(searchParams.AreaFilter.Split("-"), double.Parse);
                property = property.Where(p => p.TotalArea >= (decimal)price[0] && p.TotalArea <= (decimal)price[1]);
            }
            int pageSize = 2;
            return View(await PaginatedList<Property>.CreateAync(property.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize]
        public async Task<IActionResult> PropertyManage()
        {
            var property = _context.Property.Include(p => p.CreateByUser).Include(p => p.PropertyCodeType).Where(u => u.CreateByUser.UserName == User.Identity.Name);
            int pageSize = 3;
            return View(await PaginatedList<Property>.CreateAync(property.AsNoTracking(), 1, pageSize));
        }
        [Authorize]
        public async Task<ActionResult> SearchPartial(string searchString, int? pageNumber, string currentFilter)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var property = from p in _context.Property select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                property = property.Where(p => p.Title.Contains(searchString) || p.ProvinceName.Contains(searchString) || p.DistrictName.Contains(searchString) || p.CommuneName.Contains(searchString));
            }
            int pageSize = 3;

            return PartialView(await PaginatedList<Property>.CreateAync(property.Include(p=> p.PropertyCodeType).Include(p=>p.CreateByUser).Where(p=>p.CreateByUser.UserName == User.Identity.Name).AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize]
        public async Task<IActionResult> ChangeTransactionStatus(Guid id)
        {
            var property = _context.Property.Where(p=>p.Id == id).FirstOrDefault();
            if (property != null)
            {
                property.AllowTransaction = !property.AllowTransaction;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PropertyManage));
        }
    }
}
