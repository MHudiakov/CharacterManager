namespace Scraper.Runner.Scraper;

public interface IScraperService
{
    Task RunAsync(CancellationToken cancellationToken = default);
}