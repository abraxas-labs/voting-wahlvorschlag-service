// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using AutoMapper;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service;

public class MappingProfile : Profile
{
    // Gets called automatically by AutoMapper
    public MappingProfile()
    {
        CreateMap<ModifyDomainOfInfluenceModel, DomainOfInfluence>(MemberList.Source);
        CreateMap<DomainOfInfluenceModel, DomainOfInfluence>(MemberList.Source);
        CreateMap<DomainOfInfluence, DomainOfInfluenceModel>(MemberList.Destination);

        CreateDomainOfInfluenceElectionMaps();

        CreateMap<CreateElectionModel, Election>(MemberList.Source);
        CreateMap<UpdateElectionModel, Election>(MemberList.Source);
        CreateMap<Election, ElectionModel>(MemberList.Destination)
            .ForMember(dst => dst.IsArchived, opts => opts.Ignore()); // mapped manually
        CreateMap<Election, ElectionOverviewModel>(MemberList.Destination)
            .ForMember(dst => dst.IsArchived, opts => opts.Ignore()); // mapped manually

        CreateMap<ModifyBallotDocumentModel, BallotDocument>(MemberList.Source);
        CreateMap<BallotDocument, EmptyBallotDocumentModel>(MemberList.Destination);
        CreateMap<BallotDocument, BallotDocumentModel>(MemberList.Destination);

        CreateMap<ModifyListModel, List>(MemberList.Source);
        CreateMap<PatchListModel, List>(MemberList.Source);
        CreateMap<List, ListModel>(MemberList.Destination);
        CreateMap<List, IdModel>(MemberList.Destination);
        CreateMap<ListUnion, ListUnionModel>();

        CreateMap<ModifyInfoTextModel, InfoText>(MemberList.Source);
        CreateMap<InfoText, InfoTextModel>(MemberList.Destination);

        CreateMap<PermissionClient.V1Tenant, PartyModel>()
            .ForMember(dst => dst.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dst => dst.Name, opts => opts.MapFrom(src => src.Name))
            .ForMember(dst => dst.TenantId, opts => opts.MapFrom(src => src.Id));

        CreateMap<ModifyCandidateModel, Candidate>(MemberList.Source)
            .ForMember(dst => dst.MarkedElements, opts => opts.MapFrom(src => src.Markings));
        CreateMap<Candidate, CandidateModel>(MemberList.Destination)
            .ForMember(dst => dst.Markings, opts => opts.MapFrom(src => src.MarkedElements));
        CreateMap<ModifyMarkedElementModel, MarkedElement>(MemberList.Source)
            .ForMember(dst => dst.Id, opts => opts.Ignore());
        CreateMap<MarkedElement, MarkedElementModel>(MemberList.Destination);

        CreateMap<ModifySettingModel, Setting>()
            .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
        CreateMap<Setting, SettingModel>(MemberList.Destination);

        CreateMap<ModifyListCommentModel, ListComment>(MemberList.Source);
        CreateMap<ListComment, ListCommentModel>(MemberList.Destination)
            .ForMember(dst => dst.CreatorFirstName, opts => opts.Ignore())
            .ForMember(dst => dst.CreatorLastName, opts => opts.Ignore());

        CreateMap<CreateUserModel, IdentityClient.V1User>(MemberList.Source);
        CreateMap<IdentityClient.V1User, UserModel>();
        CreateMap<TenantUser, UserModel>().IncludeMembers(tu => tu.User);

        CreateMap<PermissionClient.V1User, IdentityClient.V1User>().ReverseMap();
        CreateMap<PermissionClient.Apiv1Email, IdentityClient.Apiv1Email>().ReverseMap();
        CreateMap<PermissionClient.Apiv1PhoneNumber, IdentityClient.Apiv1PhoneNumber>().ReverseMap();
        CreateMap<PermissionClient.CommonapiLabel, IdentityClient.CommonapiLabel>().ReverseMap();
    }

    private void CreateDomainOfInfluenceElectionMaps()
    {
        CreateMap<UpdateDomainOfInfluenceElectionModel, DomainOfInfluenceElection>(MemberList.Source);

        CreateMap<CreateDomainOfInfluenceElectionModel, DomainOfInfluenceElection>(MemberList.Source)
            .ForMember(dst => dst.DomainOfInfluenceId, opts => opts.MapFrom(src => src.Id))
            .ForMember(dst => dst.Id, opts => opts.Ignore());

        CreateMap<DomainOfInfluenceElection, DomainOfInfluenceElectionModel>(MemberList.Destination)
            .ForMember(
                dst => dst.Id,
                opts => opts.MapFrom(src => src.DomainOfInfluenceId))
            .ForMember(
                dst => dst.DomainOfInfluenceType,
                opts => opts.MapFrom(src => src.DomainOfInfluence.DomainOfInfluenceType))
            .ForMember(
                dst => dst.Name,
                opts => opts.MapFrom(src => src.DomainOfInfluence.Name))
            .ForMember(
                dst => dst.ShortName,
                opts => opts.MapFrom(src => src.DomainOfInfluence.ShortName))
            .ForMember(
                dst => dst.OfficialId,
                opts => opts.MapFrom(src => src.DomainOfInfluence.OfficialId))
            .ForMember(
                dst => dst.TenantId,
                opts => opts.MapFrom(src => src.DomainOfInfluence.TenantId));
    }
}
