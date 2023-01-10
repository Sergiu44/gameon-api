using AutoMapper;
using Infrastructure;

namespace Services
{
    public class BaseService
    {

        protected readonly UnitOfWork _unitOfWork;
        protected readonly IMapper mapper;
        public BaseService(ServiceDependencies serviceDependencies)
        {
            _unitOfWork = serviceDependencies.UnitOfWork;
            mapper = serviceDependencies.Mapper;
        }
    }
}