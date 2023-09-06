namespace DemoMVC.Models;

public class CodeType
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }

    public ICollection<Property> Properties { get; } = default!;
}