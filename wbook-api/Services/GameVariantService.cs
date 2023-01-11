using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
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
    public class GameVariantService: BaseService
    {
        public GameVariantService(ServiceDependencies serviceDependencies) : base(serviceDependencies) { }
        public int createGameVariantId()
        {
            return _unitOfWork.GameVariants.Get().ToList().Count + 1;
        }

        public async Task<List<GameVariantItemModel>> GetGameVariants(int gameId)
        {
            var variantsList = new List<GameVariantItemModel>();
            var listOfGameVariants = await _unitOfWork.GameVariants.Get().Where(gv => gv.GameId == gameId).ToListAsync();

            if (listOfGameVariants == null)
            {
                return variantsList;
            }

            foreach (var variant in listOfGameVariants)
            {
                var variantModel = new GameVariantItemModel()
                {
                    GameId = gameId,
                    Id = variant.Id,
                    Title = variant.Title,
                    Description = variant.Description,
                    Price = variant.Price,
                    Rrp = variant.Rrp ?? variant.Price,
                    Image = variant.Image,
                    HoverImage = variant.HoverImage
                };
                variantsList.Add(variantModel);
            }

            return variantsList;
        }

        public void AddVariant(GameVariantPostModel variantPostModel)
        {
            // create manual mapper from variantPostModel to GameVariant
            var newVariant = new GameVariant();
            var variantMap = mapper.Map<GameVariantPostModel, GameVariant>(variantPostModel);
            variantMap.CreatedAt = DateTime.UtcNow;
            variantMap.Id = createGameVariantId();
            using (var ms = new MemoryStream())
            {
                variantPostModel.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                variantMap.Image = fileBytes.ToString();
            }
            _unitOfWork.GameVariants.Insert(variantMap);
            _unitOfWork.SaveChanges();
        }

        public void EditVariant(GameVariantPostModel variant)
        {
            /*var variantById = _unitOfWork.GameVariants.Get().FirstOrDefault(g => g.Id == variant.Id);
            if (variantById == null)
            {
                throw new NotFound("Variant not Found");
            }

            variantById.Title = variant.Title;
            variantById.Description = variant.Description;
            variantById.CreatedAt = DateTime.UtcNow;
            variantById.Price = variant.Price;
            variantById.Rrp = variant.Rrp;*/
            //variantById.Image = variant.Image ?? "";
            //variantById.HoverImage = variant.HoverImage;

          /*  _unitOfWork.GameVariants.Update(variantById);
            _unitOfWork.SaveChanges();*/
        }

        public void DeleteVariant(int variantId)
        {
            var variant = _unitOfWork.GameVariants.Get().FirstOrDefault(g => g.Id == variantId);
            if (variant == null)
            {
                throw new NotFound("Variant not found");
            }

            _unitOfWork.GameVariants.Delete(variant);
            _unitOfWork.SaveChanges();
        }
    }
}
