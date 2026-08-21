// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class InvalidSwissZipCodeException : BadRequestException
{
    public InvalidSwissZipCodeException()
    {
    }

    public InvalidSwissZipCodeException(string zipCode)
        : base($"Zip code {zipCode} is an invalid Swiss zip code.")
    {
    }
}
