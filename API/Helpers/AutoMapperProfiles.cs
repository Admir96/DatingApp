using API.Entities;
using AutoMapper;
using API.DTOs;
using System.Linq;
using API.Extensions;
using System;

namespace API.Helpers
{
    /*library that transforms one object type to another object type, which means, it's a mapper between two objects*/
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUsers,MemberDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>           
             src.Photos.FirstOrDefault(x => x.IsMain).Url))

             .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUsers>();
            CreateMap<RegisterDto,AppUsers>();
            CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
            src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
            src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}