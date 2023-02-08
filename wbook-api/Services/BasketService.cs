using Infrastructure.Entities;
using Infrastructure.Models.Bundle;
using Infrastructure.Models.GameVariant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class BasketService: BaseService
    {

        public BasketService(ServiceDependencies dependencies) : base(dependencies) { }

        private int getBasketCount()
        {
            return _unitOfWork.BasketItems.Get().Count() + 1; 
        }

        public async Task<List<Tuple<GameVariantItemModel, BundleItemModel>>> GetVariantsForBasket(int userId)
        {
            var variantsList = new List<Tuple<GameVariantItemModel, BundleItemModel>>();
            
            var listOfVariants =
                await _unitOfWork.BasketItems.Get()
                                        .Include(v => v.IdVariantNavigation)
                                        .Include(v => v.IdBundleNavigation)
                                        .Where(v => v.IdUser == userId).ToListAsync();

            if (listOfVariants == null)
            {
                return variantsList;
            }

            foreach (var variant in listOfVariants)
            {
                if (variant.IdVariant != null && variant.IdVariantNavigation != null)
                {
                    var variantModel = new GameVariantItemModel()
                    {
                        Id = (int)variant.IdVariant,
                        Title = variant.IdVariantNavigation.Title,
                        Description = variant.IdVariantNavigation.Description,
                        Price = variant.IdVariantNavigation.Price,
                        Rrp = variant.IdVariantNavigation.Rrp ?? variant.IdVariantNavigation.Price,
                    };
                    variantsList.Add(Tuple.Create<GameVariantItemModel, BundleItemModel>(variantModel, null));
                }
                else if (variant.IdBundle != null && variant.IdBundleNavigation != null)
                {
                    var bundleModel = new BundleItemModel()
                    {
                        Id = (int)variant.IdBundle,
                        Title = variant.IdBundleNavigation.Title,
                        Description = variant.IdBundleNavigation.Description,
                        Price = variant.IdBundleNavigation.Price,
                        Rrp = variant.IdBundleNavigation.Rrp ?? variant.IdBundleNavigation.Price,
                    };
                    variantsList.Add(Tuple.Create<GameVariantItemModel, BundleItemModel>(null, bundleModel));
                }
            }

            return variantsList;
        }

        public void AddProductToBasket(int userId, int idProduct, bool isVariant)
        {
            var basketItem = new BasketItem();
            basketItem.Id = getBasketCount();
            if(isVariant == true)
            {
                var product = _unitOfWork.GameVariants.Get().FirstOrDefault(v => v.Id == idProduct);
                if (product != null)
                {
                    basketItem.IdUser = userId;
                    basketItem.IdUserNavigation = _unitOfWork.Users.Get().First(u => u.Id == userId);
                    basketItem.IdVariant = product.Id;
                    basketItem.IdVariantNavigation = product;
                    basketItem.IdBundle = null;
                    basketItem.IdBundleNavigation = null;
                    _unitOfWork.BasketItems.Insert(basketItem);
                }
            } else
            {
                var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == idProduct);
                if (bundle != null)
                {
                    basketItem.IdUser = userId;
                    basketItem.IdUserNavigation = _unitOfWork.Users.Get().First(u => u.Id == userId);
                    basketItem.IdVariant = null;
                    basketItem.IdVariantNavigation = null;
                    basketItem.IdBundle = bundle.Id;
                    basketItem.IdBundleNavigation = bundle;
                    _unitOfWork.BasketItems.Insert(basketItem);
                }
            }
            _unitOfWork.SaveChanges();

        }

        public void DeleteProductFromBasket(int userId, int idProduct, bool isVariant)
        {
            if(isVariant == true)
            {
                var product = _unitOfWork.BasketItems.Get().FirstOrDefault(v => v.IdVariant == idProduct && v.IdUser == userId);
                if (product != null)
                {
                    _unitOfWork.BasketItems.Delete(product);
                }
            } else
            {
                var bundle = _unitOfWork.BasketItems.Get().FirstOrDefault(b => b.IdBundle == idProduct && b.IdUser == userId);
                if (bundle != null)
                {
                    _unitOfWork.BasketItems.Delete(bundle);
                }
            }
            _unitOfWork.SaveChanges();

        }
    }
}
