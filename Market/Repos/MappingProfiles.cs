using AutoMapper;
using Market.Models;
using Market.Models.Dtos;
using System.Text.RegularExpressions;

namespace Market.Repos
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination).ReverseMap();
            CreateMap<Group, CategoryDto>(MemberList.Destination).ReverseMap();
            CreateMap<Store, StoreDto>(MemberList.Destination).ReverseMap();
        }
    }
}
