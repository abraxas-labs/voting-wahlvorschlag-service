// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Eawv.Service.Exceptions;
using HeyRed.Mime;
using Voting.Lib.MalwareScanner.Services;

namespace Eawv.Service.Services;

public class FileValidationService
{
    private static readonly HashSet<string> JpegFileExtensions = ["jpg", "jpeg"];
    private readonly IMalwareScannerService _malwareScanner;

    public FileValidationService(IMalwareScannerService malwareScanner)
    {
        _malwareScanner = malwareScanner;
    }

    public Task ValidateFile(byte[] content, HashSet<string> allowedMimeTypes, CancellationToken cancellationToken)
        => ValidateFileInternal(null, content, allowedMimeTypes, cancellationToken);

    public Task ValidateFile(string name, byte[] content, HashSet<string> allowedMimeTypes, CancellationToken cancellationToken)
        => ValidateFileInternal(name, content, allowedMimeTypes, cancellationToken);

    private async Task ValidateFileInternal(string name, byte[] content, HashSet<string> allowedMimeTypes, CancellationToken cancellationToken)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }

        EnsureCorrectMimeType(name, content, allowedMimeTypes);
        await _malwareScanner.EnsureFileIsClean(content, cancellationToken);
    }

    private void EnsureCorrectMimeType(string name, byte[] content, HashSet<string> allowedMimeTypes)
    {
        var guessedFileType = MimeGuesser.GuessFileType(content);
        if (!allowedMimeTypes.Contains(guessedFileType.MimeType))
        {
            var allowed = string.Join(", ", allowedMimeTypes);
            throw new InvalidMimeTypeException($"Mime type {guessedFileType.MimeType} is not allowed, should be one of {allowed}");
        }

        if (name == null)
        {
            return;
        }

        var allowedExtensions = guessedFileType.MimeType == MediaTypeNames.Image.Jpeg
            ? JpegFileExtensions
            : [guessedFileType.Extension];
        var extension = Path.GetExtension(name).TrimStart('.').ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidMimeTypeException($"File extension {extension} is not allowed, expected {allowedExtensions.First()}");
        }
    }
}
