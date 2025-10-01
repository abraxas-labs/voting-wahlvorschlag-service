// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Ech.Providers;

/// <inheritdoc />
public class DefaultEchMessageIdProvider : IEchMessageIdProvider
{
    /// <inheritdoc />
    public string NewId()
        => Guid.NewGuid().ToString("N");
}
