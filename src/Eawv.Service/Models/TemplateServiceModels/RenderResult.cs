// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Threading.Tasks;

namespace Eawv.Service.Models.TemplateServiceModels;

public class RenderResult
{
    private static readonly char[] InvalidFilenameChars =
    [
        '\\',
        '/',
        ':',
        '*',
        '?',
        '"',
        '<',
        '>',
        '|',
        '\n',
        '\r',
        '\t',
    ];

    public RenderResult(string filename, string mimeType, Func<Stream, Task> writerFunction)
    {
        Filename = ReplaceInvalidFilenameChars(filename);
        MimeType = mimeType;
        WriterFunction = writerFunction;
    }

    public string Filename { get; }

    public string MimeType { get; }

    public Func<Stream, Task> WriterFunction { get; }

    public async Task<string> ReadAsString()
    {
        await using var ms = new MemoryStream();
        await WriterFunction(ms);
        ms.Position = 0;

        using var sr = new StreamReader(ms);
        return await sr.ReadToEndAsync();
    }

    // Since all input strings are already validated and restricted to certain characters,
    // we should be fine to simply replace invalid windows filename characters here.
    private static string ReplaceInvalidFilenameChars(string filename)
        => string.Concat(filename.Split(InvalidFilenameChars, StringSplitOptions.RemoveEmptyEntries));
}
