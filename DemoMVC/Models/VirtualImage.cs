namespace DemoMVC.Models;

public class VirtualImage
{
    public Guid Id { get; set; }

    public string? Url { get; set; }
    public Property Property { get; set; } = default!;
}