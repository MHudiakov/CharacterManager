namespace Application.Characters.Queries.GetCharactersWithPagination;

public class LocationDto
{
    public int Id { get; set; }
        
    public string Name { get; set; }
        
    public string Type { get; set; }
        
    public string? Dimension { get; set; }
}