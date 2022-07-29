using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.Common.Behaviours;

public class HealthCheckAsync : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("RewardsApi is working fine."));
        }
        catch (Exception)
        {
            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "Something went wrong."));
        }
    }
}