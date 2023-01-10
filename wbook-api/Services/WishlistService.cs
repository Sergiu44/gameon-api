using Infrastructure.Entities;
using Infrastructure.Models.Bundle;
using Infrastructure.Models.GameVariant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class WishlistService: BaseService
    {
        public WishlistService(ServiceDependencies dependencies) : base(dependencies) { }

        public async Task<List<Tuple<GameVariantItemModel, BundleItemModel>>> GetVariantsForWishlist(int userId)
        {
            var variantsList = new List<Tuple<GameVariantItemModel, BundleItemModel>>();

            var listOfVariants = 
                await _unitOfWork.WishlistItems.Get()
                                        .Include(v => v.IdVariantNavigation)
                                        .Include(v => v.IdBundleNavigation)
                                        .Where(v => v.IdUser == userId).ToListAsync();
        
            if(listOfVariants == null)
            {
                return variantsList;
            }

            foreach(var variant in listOfVariants )
            {
                if (variant.IdVariant != null)
                {
                    var variantModel = new GameVariantItemModel()
                    {
                        Id = (int)variant.IdVariant,
                        Title = variant.IdVariantNavigation.Title,
                        Description = variant.IdVariantNavigation.Description,
                        Image = variant.IdVariantNavigation.Image,
                        HoverImage = variant.IdVariantNavigation.HoverImage,
                        Price = variant.IdVariantNavigation.Price,
                        Rrp = variant.IdVariantNavigation.Rrp ?? variant.IdVariantNavigation.Price,
                    };
                    variantsList.Add(Tuple.Create<GameVariantItemModel, BundleItemModel>(variantModel, null));
                } else if(variant.IdBundle!= null)
                {
                    var bundleModel = new BundleItemModel()
                    {
                        Id = (int)variant.IdBundle,
                        Title = variant.IdBundleNavigation.Title,
                        Description = variant.IdBundleNavigation.Description,
                        Image = variant.IdBundleNavigation.Image,
                        Price = variant.IdBundleNavigation.Price,
                        Rrp = variant.IdBundleNavigation.Rrp ?? variant.IdBundleNavigation.Price,
                    };
                    variantsList.Add(Tuple.Create<GameVariantItemModel, BundleItemModel>(null, bundleModel));
                }
            }

            return variantsList;
        }

        public void AddProductToWishlist(int idProduct)
        {
            var wishlistItem = new WishlistItem();
            var product = _unitOfWork.GameVariants.Get().FirstOrDefault(v => v.Id == idProduct);
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == idProduct);

            if(product != null) 
            {
                wishlistItem.IdVariant = product.Id;
                wishlistItem.IdVariantNavigation = product;
                wishlistItem.IdBundle = null;
                wishlistItem.IdBundleNavigation = null;
                _unitOfWork.WishlistItems.Insert(wishlistItem);
            } else if(bundle != null)
            {
                wishlistItem.IdVariant = null;
                wishlistItem.IdVariantNavigation = null;
                wishlistItem.IdBundle = bundle.Id;
                wishlistItem.IdBundleNavigation = bundle;
                _unitOfWork.WishlistItems.Insert(wishlistItem);
            }

            _unitOfWork.SaveChanges();

        }

        public void DeleteProductFromWishlist(int idProduct)
        {
            var product = _unitOfWork.WishlistItems.Get().FirstOrDefault(v => v.IdVariant== idProduct);
            var bundle = _unitOfWork.WishlistItems.Get().FirstOrDefault(b => b.IdBundle == idProduct);

            if (product != null)
            {
                _unitOfWork.WishlistItems.Delete(product);
            }
            else if (bundle != null)
            {
                _unitOfWork.WishlistItems.Delete(bundle);
            }

            _unitOfWork.SaveChanges();

        }
    }
}
