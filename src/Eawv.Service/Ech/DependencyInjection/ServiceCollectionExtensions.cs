// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Ech.Configuration;
using Eawv.Service.Ech.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Eawv.Service.Ech.DependencyInjection;

/// <summary>
/// Service collection extensions for eCH.
/// VOTING-3320: after dotnet 6 update, reference from Voting.Lib.Ech.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds voting lib eCH helpers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The eCH config.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddVotingLibEch(this IServiceCollection services, EchConfig config)
        => services
            .AddSingleton(config)
            .AddSingleton<IEchMessageIdProvider, DefaultEchMessageIdProvider>()
            .AddSingleton<DeliveryHeaderProvider>();
}
