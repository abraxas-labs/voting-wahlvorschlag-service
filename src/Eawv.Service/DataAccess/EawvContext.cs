// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.DataAccess.Entities;
using Eawv.Service.DataAccess.Seed;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess;

public class EawvContext : DbContext
{
    public EawvContext(DbContextOptions<EawvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BallotDocument> BallotDocuments { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<DomainOfInfluence> DomainsOfInfluence { get; set; }

    public virtual DbSet<DomainOfInfluenceElection> DomainOfInfluenceElections { get; set; }

    public virtual DbSet<Election> Elections { get; set; }

    public virtual DbSet<InfoText> InfoTexts { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<ListUnion> ListUnions { get; set; }

    public virtual DbSet<MarkedElement> MarkedElements { get; set; }

    public virtual DbSet<TemplateEntity> Templates { get; set; }

    public virtual DbSet<ListComment> ListComments { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BallotDocument.Map(modelBuilder);
        Candidate.Map(modelBuilder);
        DomainOfInfluence.Map(modelBuilder);
        DomainOfInfluenceElection.Map(modelBuilder);
        Election.Map(modelBuilder);
        InfoText.Map(modelBuilder);
        List.Map(modelBuilder);
        ListUnion.Map(modelBuilder);
        MarkedElement.Map(modelBuilder);
        TemplateEntity.Map(modelBuilder);
        ListComment.Map(modelBuilder);
        Setting.Map(modelBuilder);
        TemplatesBootstrapper.Seed(modelBuilder);
    }
}
