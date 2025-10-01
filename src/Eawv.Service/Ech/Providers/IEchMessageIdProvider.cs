// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Ech.Providers;

/// <summary>
/// Provides eCH message IDs.
/// VOTING-3320: after dotnet 6 update, reference from Voting.Lib.Ech.
/// </summary>
public interface IEchMessageIdProvider
{
    /// <summary>
    /// Creates a new unique message ID.
    /// </summary>
    /// <returns>The created message ID.</returns>
    string NewId();
}
