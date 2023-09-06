using DemoMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                       serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                
                if (context.CodeTypes.Any())
                {
                    return;
                }

                context.CodeTypes.AddRange(new CodeType()
                    {
                        Title = "Dẫn khách",
                        Code = "CUSTOMER",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Thỏa thuận",
                        Code = "AGREE",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Đông",
                        Code = "HD4",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Tổng giá(VND)",
                        Code = "VND",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Nguồn chính chủ",
                        Code = "S1",
                        Type = "PropertySourceCode"
                    }, new CodeType()
                    {
                        Title = "Đơn giá(Giá/m2)",
                        Code = "UNIT",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Đất",
                        Code = "TP1",
                        Type = "PropertyTypeCode"
                    }, new CodeType()
                    {
                        Title = "Báo cáo bài đăng",
                        Code = "Post",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Nguồn liên kết",
                        Code = "S2",
                        Type = "PropertySourceCode"
                    }, new CodeType()
                    {
                        Title = "Đất",
                        Code = "TP1",
                        Type = "PropertyTypeCode"
                    }, new CodeType()
                    {
                        Title = "Nhà ở",
                        Code = "TP2",
                        Type = "PropertyTypeCode"
                    }, new CodeType()
                    {
                        Title = "Tây",
                        Code = "HD1",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Nam",
                        Code = "HD2",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Căn hộ, chung cư",
                        Code = "TP3",
                        Type = "PropertyTypeCode"
                    }, new CodeType()
                    {
                        Title = "Kho, xưởng",
                        Code = "TP4",
                        Type = "PropertyTypeCode"
                    }, new CodeType()
                    {
                        Title = "Nguồn sơ cấp",
                        Code = "S4",
                        Type = "PropertySourceCode"
                    }, new CodeType()
                    {
                        Title = "Bắc",
                        Code = "HD3",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Nguồn thứ cấp",
                        Code = "S5",
                        Type = "PropertySourceCode"
                    }, new CodeType()
                    {
                        Title = "Đông Bắc",
                        Code = "HD5",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Tây Bắc",
                        Code = "HD6",
                        Type = ""
                    }, new CodeType()
                    {
                        Title = "Tây Nam",
                        Code = "HD7",
                        Type = ""
                    }
                );
                if (context.User.Any())
                {
                    return;
                }
                
                context.User.AddRange(new ApplicationUser()
                {
                    Name = "Khai",
                    Phone = "0394229171"
                });
                context.SaveChanges();
                
            }
        }
    }
}