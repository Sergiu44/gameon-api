using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Bundle;
using Infrastructure.Models.Game;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BundleService: BaseService
    {
        public BundleService(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<BundleItemModel>> GetBundles()
        {
            var bundleList = new List<BundleItemModel>();
            var listOfBundles = await _unitOfWork.Bundles.Get().OrderBy(e => e.CreatedAt).ToListAsync();

            if (listOfBundles == null)
            {
                return bundleList;
            }

            foreach (var bundle in bundleList)
            {
                var gameModel = new BundleItemModel()
                {
                    Id = bundle.Id,
                    Title = bundle.Title,
                    Description = bundle.Description,
                    Image = bundle.Image,
                    Price = bundle.Price,
                    Rrp = bundle.Rrp
                };

                bundleList.Add(gameModel);
            }

            return bundleList;
        }


        public BundleModel GetBundleById(Guid bundleId)
        {
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(g => g.Id == bundleId);
            if (bundle == null)
            {
                throw new NotFound("Bundle Not Found");
            }

            var products = _unitOfWork.GameVariants.Get().FirstOrDefault(v => v.GameId == bundle.Id);
            var bundleModel = AutoMapper.Mapper.Map<Bundle, BundleModel>(bundle);
            return bundleModel;
        }

        public void AddBundle(BundlePostModel bundle)
        {
            var bundleMap = AutoMapper.Mapper.Map<BundlePostModel, Bundle>(bundle);
            bundleMap.Title = bundle.Title;
            bundleMap.Description = bundle.Description;
            bundleMap.Products = new List<GameVariant>();
            _unitOfWork.Bundles.Insert(bundleMap);
            _unitOfWork.SaveChanges();
        }

        public void EditBundle(BundlePostModel bundle)
        {
            var bundleById = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == b.Id);
            if (bundleById == null)
            {
                throw new NotFound("Bundle not Found");
            }

            bundleById.Title = bundle.Title;
            bundleById.Description = bundle.Description;

            _unitOfWork.Bundles.Update(bundleById);
            _unitOfWork.SaveChanges();
        }

        public void DeleteBundle(Guid bundleId)
        {
            var bundle = _unitOfWork.Bundles.Get().Include(g => g.Products).FirstOrDefault(g => g.Id == bundleId);
            if (bundle == null)
            {
                throw new NotFound("Bundle not found");
            }

            bundle.Products.Clear();
            _unitOfWork.Bundles.Delete(bundle);
            _unitOfWork.SaveChanges();
        }
    }
}
