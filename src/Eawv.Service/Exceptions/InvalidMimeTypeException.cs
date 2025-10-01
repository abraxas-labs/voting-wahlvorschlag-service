// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Exceptions;

public class InvalidMimeTypeException : BadRequestException
{
    public InvalidMimeTypeException(string msg)
        : base(msg)
    {
    }
}
