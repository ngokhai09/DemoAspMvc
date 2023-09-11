using System.ComponentModel.DataAnnotations;
using DemoMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoMVC.Models;

public class Property
{
    public Property()
    {
    }
    public Property(PropertyViewModel newProperty)
    {
        Id = newProperty.Id;
        Title = newProperty.Title;
        Description = newProperty.Description;
        ProvinceName = newProperty.ProvinceName;
        DistrictName = newProperty.DistrictName;
        CommuneName = newProperty.CommuneName;
        ThumbnailUrl = newProperty.ThumbnailUrl;
        TotalArea = newProperty.TotalArea;
        TotalPrice = decimal.Parse(newProperty.TotalPrice.Replace(".", ""));
        TransactionTypeCode = newProperty.TransactionTypeCode;
    }
    

    public Property(Property newProperty, CodeType propertyCodeType = null, ApplicationUser user = null)
    {
        Id = newProperty.Id;
        Title = newProperty.Title;
        Description = newProperty.Description;
        ProvinceCode = newProperty.ProvinceCode;
        ProvinceName = newProperty.ProvinceName;
        DistrictCode = newProperty.DistrictCode;
        DistrictName = newProperty.DistrictName;
        CommuneCode = newProperty.CommuneCode;
        CommuneName = newProperty.CommuneName;
        ThumbnailUrl = newProperty.ThumbnailUrl;
        TransactionTypeCode = newProperty.TransactionTypeCode;
        PropertyCodeType = propertyCodeType != null ? propertyCodeType : newProperty.PropertyCodeType;
        TotalArea = newProperty.TotalArea;
        TotalPrice = newProperty.TotalPrice;
        StatusCode = newProperty.StatusCode;
        CreateByUser = user != null ? user : newProperty.CreateByUser;
        CreateOnDate = newProperty.CreateOnDate;
        LastModifiedOnDate = newProperty.LastModifiedOnDate;
        VirtualImageListUrl = newProperty.VirtualImageListUrl;
    }
    public Guid Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [BindNever]
    public decimal ProvinceCode { get; set; }
    public string? ProvinceName { get; set; }
    [BindNever]
    public decimal DistrictCode { get; set; }
    public string? DistrictName { get; set; }
    [BindNever]
    public decimal CommuneCode { get; set; }
    public string? CommuneName { get; set; }
    public string? ThumbnailUrl { get; set; }
    [BindNever]
    public string? TransactionTypeCode { get; set; }
    public Guid PropertyCodeTypeId { get; set; }
    public CodeType PropertyCodeType { get; set; } = default!;
    [Required]
    public decimal TotalArea { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }

    [BindNever]
    public bool AllowTransaction { get; set; }
    [BindNever]
    public string? StatusCode { get; set; }
    [BindNever]
    public ApplicationUser? CreateByUser { get; set; } = default!;
    [BindNever]
    public DateTime CreateOnDate { get; set; }
    [BindNever]
    public DateTime LastModifiedOnDate { get; set; }
    public ICollection<VirtualImage> VirtualImageListUrl { get; set; } = default!;


}