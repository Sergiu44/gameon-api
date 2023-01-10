using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Bundle;
using Infrastructure.Models.Game;
using Infrastructure.Models.GameVariant;
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
        public BundleService(ServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public async Task<List<BundleModel>> GetBundles()
        {
            var bundlesList = new List<BundleModel>();
            var listOfBundles = await _unitOfWork.Bundles.Get().OrderBy(b => b.CreatedAt).ToListAsync();

            if(listOfBundles == null)
            {
                return bundlesList;
            }

            foreach(var bundle in listOfBundles)
            {
                var bundleModel = new BundleModel()
                {
                    Id = bundle.Id,
                    Title = bundle.Title,
                    Description = bundle.Description,
                    Price = bundle.Price,
                    Rrp = bundle.Rrp ?? bundle.Price,
                    Image = bundle.Image
                };
                bundlesList.Add(bundleModel);
            }
            return bundlesList;
        } 

        public BundleItemModel GetBundleById(int bundleId)
        {
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == bundleId);

            var bundleProducts = _unitOfWork.BundleGameMapping.Get().Include(g => g.IdVariantNavigation).Include(g => g.IdGameNavigation).Where(g => g.IdBundle == bundleId);


            if(bundle == null)
            {
                throw new NotFound("Bundle not found");
            }

            if(bundleProducts == null)
            {
                throw new NotFound("There are no products");
            }

            var bundleItems = new List<Tuple<GameVariantItemModel, GameModel>>();

            foreach(var prod in bundleProducts)
            {
                if (prod.IdGame != null)
                {
                    var item = AutoMapper.Mapper.Map<Game, GameModel>(prod.IdGameNavigation);
                    bundleItems.Add(Tuple.Create<GameVariantItemModel, GameModel>(null, item));
                }
                else if (prod.IdVariant != null)
                {
                    var item = AutoMapper.Mapper.Map<GameVariant, GameVariantItemModel>(prod.IdVariantNavigation);
                    bundleItems.Add(Tuple.Create<GameVariantItemModel, GameModel>(item, null));
                }
            }

            var bundleItemModel = new BundleItemModel()
            {
                Id = bundle.Id,
                Title = bundle.Title,
                Description = bundle.Description,
                Price = bundle.Price,
                Rrp = bundle.Rrp ?? bundle.Price,
                GameVariants = bundleItems
            };

            return bundleItemModel;
        }

        public void CreateBundle(BundlePostModel bundle, List<Tuple<GameVariantItemModel, GameModel>> products)
        {
            var bundleMap = AutoMapper.Mapper.Map<BundlePostModel, Bundle>(bundle);
        }
    }
}
