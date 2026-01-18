using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Models;


namespace Chinese_Auction.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Category
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();

            // Gift
            CreateMap<Gift, GetGiftDto>();
            CreateMap<CreateGiftDto, Gift>();
            CreateMap<UserUpdateGiftDto, Gift>();

            // Package
            CreateMap<Package, GetPackageDto>();
            CreateMap<CreatePackageDto, Package>();

            // Purchase
            CreateMap<CreatePurchaseDto, Purchase>();
            CreateMap<Purchase, GetPurchaseDto>();
            CreateMap<UpdatePurchaseDto, Purchase>();

            // Donor
            CreateMap<CreateDonorDto, Donor>();
            CreateMap<Donor, ManagerGetDonorDto>();
            CreateMap<Donor, UserGetDonorDto>();

            // User
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetUserDto>();

        }
    }
}