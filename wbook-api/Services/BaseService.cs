using Infrastructure;

namespace Services
{
    public class BaseService
    {

        protected readonly UnitOfWork _unitOfWork;
        public BaseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}