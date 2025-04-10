namespace Scraper.ApiClient.Models;

public class CharacterDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Species { get; set; }
    
    public string Gender { get; set; }
    
    public CharacterLocationDto Location { get; set; }
}

public class CharacterLocationDto
{
    public string Name { get; set; }
    
    public string Url { get; set; }
}