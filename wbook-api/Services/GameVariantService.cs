using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GameVariantService: BaseService
    {
        private GameVariantService(UnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
