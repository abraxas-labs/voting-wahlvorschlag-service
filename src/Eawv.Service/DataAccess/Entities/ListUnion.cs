// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class ListUnion : BaseEntity
{
    public ListUnion()
    {
    }

    public ListUnion(Guid? rootListId, ICollection<List> lists)
    {
        // set rootlistid first to correctly set the lists
        RootListId = rootListId;
        Lists = lists;
    }

    // non null if it's a subunion
    public List RootList { get; set; }

    public Guid? RootListId { get; set; }

    public ICollection<List> UnionLists { get; set; }

    public ICollection<List> SubUnionLists { get; set; }

    public bool IsSubUnion => RootListId.HasValue;

    public ICollection<List> Lists
    {
        get => IsSubUnion ? SubUnionLists : UnionLists;
        set
        {
            if (IsSubUnion)
            {
                SubUnionLists = value;
                foreach (var list in SubUnionLists)
                {
                    list.ListSubUnion = this;
                }
            }
            else
            {
                UnionLists = value;
                foreach (var list in UnionLists)
                {
                    list.ListUnion = this;
                }
            }
        }
    }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<ListUnion>(modelBuilder);
        modelBuilder.Entity<ListUnion>(entity =>
        {
            entity.HasOne(x => x.RootList)
                .WithOne(x => x.RootListUnion)
                .HasForeignKey<ListUnion>(x => x.RootListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(x => x.Lists);
        });
    }
}
