namespace ElectroAutoApi.Data.SeedingData;

public class CarSeedData
{
    public List<string> Brands { get; set; } = [];
    public Dictionary<string, List<string>> Models { get; set; } = [];
    public List<string> Colors { get; set; } = [];
    public Range<int> PriceRange { get; set; } = new();
    public Range<int> MileageRange { get; set; } = new();
    public Range<int> SeatsRange { get; set; } = new();
    public Range<int> DoorsRange { get; set; } = new();
    public Range<int> ProductionYearRange { get; set; } = new();
    public Range<int> WeightRange { get; set; } = new();
    public Range<int> ViewsRange { get; set; } = new();
}

public class Range<T>
{
    public T Min { get; set; } = default!;
    public T Max { get; set; } = default!;
}