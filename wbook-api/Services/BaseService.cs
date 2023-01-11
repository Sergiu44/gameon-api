using AutoMapper;
using Infrastructure;
using System.Transactions;

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

        protected TResult ExecuteInTransaction<TResult>(Func<UnitOfWork, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (var transactionScope = new TransactionScope())
            {
                var result = func(_unitOfWork);

                transactionScope.Complete();

                return result;
            }
        }

        protected void ExecuteInTransaction(Action<UnitOfWork> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transactionScope = new TransactionScope())
            {
                action(_unitOfWork);

                transactionScope.Complete();
            }
        }
    }
}