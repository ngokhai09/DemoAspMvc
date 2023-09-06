using DemoMVC.Models;
using System.Diagnostics.CodeAnalysis;

namespace DemoMVC.ViewModel;

public class PropertyViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public string ProvinceName { get; set; }
    public string DistrictName { get; set; }
    public string CommuneName { get; set; }
    public string ThumbnailUrl { get; set; }
    public string PropertyCodeType { get; set; }
    public decimal TotalArea { get; set; }
    public decimal TotalPrice { get; set; }

}