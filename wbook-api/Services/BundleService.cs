using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Bundle;
using Infrastructure.Models.GameVariant;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Services
{
    public class BundleService: BaseService
    {
        public BundleService(ServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public int createBundleId()
        {
            return _unitOfWork.Bundles.Get().ToList().Count + 1;
        }

        public async Task<List<BundleItemModel>> GetBundles()
        {
            var bundlesList = new List<BundleItemModel>();
            var listOfBundles = await _unitOfWork.Bundles.Get().OrderBy(b => b.CreatedAt).ToListAsync();

            if(listOfBundles == null)
            {
                return bundlesList;
            }

            foreach(var bundle in listOfBundles)
            {
                var bundleModel = new BundleItemModel()
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

        public BundleGamesModel GetBundleById(int bundleId)
        {
            var bundleGames = new BundleGamesModel();
            var bundleMapping = _unitOfWork.BundleGameMapping.Get()
                .Include(g => g.IdVariantNavigation)
                .Where(g => g.IdBundle == bundleId);
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == bundleId);

            if (bundleMapping == null)
            {
                throw new NotFound("No game found for this bundle");
            }

            if(bundle == null)
            {
                throw new NotFound("No bundle found!");
            }

            var bundleItems = new List<GameVariantBundleModel>();

            foreach (var prod in bundleMapping)
            {
                var item = new GameVariantBundleModel();
                item.Id = prod.IdVariantNavigation.Id;
                item.Title = prod.IdVariantNavigation.Title;
                item.Description = prod.IdVariantNavigation.Description;
                bundleItems.Add(item);
            }

            bundleGames.Id = bundleId;
            bundleGames.Title = bundle.Title;
            bundleGames.Description = bundle.Description;
            bundleGames.Price = bundle.Price;
            bundleGames.Rrp = bundle.Rrp;
            bundleGames.GameVariants = bundleItems;

            return bundleGames;
        }

        public async Task AddBundle(BundlePostModel bundle)
        {
            await ExecuteInTransaction(async uow =>
            {
                var newBundle = new Bundle();
                var bundleId = createBundleId();
                newBundle.Id = bundleId;
                newBundle.Title = bundle.Title;
                newBundle.Description = bundle.Description;
                newBundle.Price = bundle.Price;
                newBundle.Rrp = bundle.Rrp;
                using (var ms = new MemoryStream())
                {
                    bundle.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newBundle.Image = fileBytes;
                }
                newBundle.CreatedAt = DateTime.Now;
                newBundle.Category = bundle.Category;
                uow.Bundles.Insert(newBundle);

                foreach (var variantId in bundle.GameVariantsId)
                {
                    var variant = uow.GameVariants.Get().FirstOrDefault(v => v.Id == variantId);

                    if(variant != null)
                    {
                        var bundleVariantMap = new BundleGameMapping();
                        bundleVariantMap.IdBundle = bundleId;
                        bundleVariantMap.IdVariant = (int)variantId;


                        bundleVariantMap.IdVariantNavigation = variant;
                        uow.BundleGameMapping.Insert(bundleVariantMap);
                    }
                    
                }

                uow.SaveChanges();
            });
        }

        public void AddToBundle(int bundleId, int variantId)
        {
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == bundleId);
            if (bundle != null)
            {
                throw new NotFound("Bundle not found!");
            }

            var variant = _unitOfWork.GameVariants.Get().FirstOrDefault(v => v.Id == variantId);
            if (variant != null)
            {
                throw new NotFound("Variant not found!");
            }

            var bundleMapping = new BundleGameMapping();
            bundleMapping.IdBundle = bundleId;
            bundleMapping.IdVariant = variantId;
            bundleMapping.IdVariantNavigation = variant;
            _unitOfWork.BundleGameMapping.Insert(bundleMapping);
            _unitOfWork.SaveChanges();
        }

        public void RemoveFromBundle(int bundleId, int variantId)
        {
            var bundleMapping = _unitOfWork.BundleGameMapping.Get().First(bm => bm.IdVariant == variantId && bm.IdBundle == bundleId);
            if(bundleMapping == null)
            {
                throw new NotFound("Variant in bundle not found");
            }

            _unitOfWork.BundleGameMapping.Delete(bundleMapping);
            _unitOfWork.SaveChanges();
        }

        public void DeleteBundle(int bundleId)
        {
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(b => b.Id == bundleId);
            if(bundle == null)
            {
                throw new NotFound("Bundle not found!");
            }
            _unitOfWork.Bundles.Delete(bundle);
            _unitOfWork.SaveChanges();
        }

        public byte[] GetImg(int id)
        {
            var bundle = _unitOfWork.Bundles.Get().FirstOrDefault(i => i.Id == id);
            if (bundle == null)
            {
                throw new NotFound("Image not found");
            }

            return bundle.Image;
        }
    }
}
