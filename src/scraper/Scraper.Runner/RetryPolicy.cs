using Polly.Extensions.Http;
using Polly;

namespace Scraper.Runner;

public static class RetryPolicy
{
    private const int RetryCount = 6;

    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        // In case of high concurrency in the future we can consider to add jitter to the policy to make request more random in time
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}