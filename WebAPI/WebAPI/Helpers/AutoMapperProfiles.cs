using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AddSupplierDto, Suppliers>();
            CreateMap<EditSupplierDto, Suppliers>();
            CreateMap<Suppliers, SupplierResponseDto>();
        }
    }
}