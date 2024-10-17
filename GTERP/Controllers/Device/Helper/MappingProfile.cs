using AutoMapper;
using GTERP.Models;

namespace GTERP.Controllers.Device.Helper
{
   

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HR_ProcessedDataSal, HR_ProcessedDataSal>();
        }
    }
}
