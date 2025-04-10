namespace Scraper.ApiClient.Models;

public class CharactersResponseDto
{
    public InfoDto Info { get; set; }
    
    public List<CharacterDto> Results { get; set; }
}

public class InfoDto
{
    public int Count { get; set; }
    
    public int Pages { get; set; }
}