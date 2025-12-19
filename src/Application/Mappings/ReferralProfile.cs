using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    internal class ReferralProfile : Profile
    {
        public ReferralProfile() 
        {
            CreateMap<CreateInvitationRequest, Invitation>().ReverseMap();
            CreateMap<Invitation, ReferralResponse>().ReverseMap();
            CreateMap<Invitation, StatusReferralResponse>().ReverseMap();
        }
    }
}
