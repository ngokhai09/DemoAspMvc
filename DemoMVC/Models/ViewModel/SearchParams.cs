namespace DemoMVC.Models.ViewModel
{
    public class SearchParams
    {
        public string SearchString { get; set; }
        public string PropertyType { get; set; }
        public decimal PropertyAddress { get; set; }
        public string Reset { get; set; }
        public string PriceFilter { get; set; }
        public string AreaFilter { get; set; }
    }
}
