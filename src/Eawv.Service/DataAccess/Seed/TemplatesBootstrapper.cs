// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Linq;
using System.Text;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Extensions;
using Eawv.Service.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Eawv.Service.DataAccess.Seed;

public static class TemplatesBootstrapper
{
    private const string BaseDir = "./Data/Templates/";

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TemplateEntity>(entity =>
        {
            var templates = JsonConvert
                .DeserializeObject<TemplateEntity[]>(
                    File.ReadAllText(Path.Join(BaseDir, "templates.json"), Encoding.UTF8))
                .Peek(t => t.Id = GuidUtils.GuidFromString(t.TemplateKey))
                .ToArray();

            entity.HasData(templates);
        });
    }
}
