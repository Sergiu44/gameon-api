using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.GameVariant;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<GameVariantItemModel>> GetVariants()
        {
            var variantsList = new List<GameVariantItemModel>();
            var listOfVariants = await _unitOfWork.GameVariants.Get().OrderBy(x => x.Title).ToListAsync();

            if(listOfVariants == null)
            {
                return variantsList;
            }

            foreach(var variant in listOfVariants)
            {
                var variantModel = new GameVariantItemModel()
                {
                    Title = variant.Title,
                    Description = variant.Description,
                    Price = variant.Price,
                    Rrp = variant.Rrp,
                    GameId = variant.GameId,
                };
                variantsList.Add(variantModel);
            }

            return variantsList;
        }

        public void AddVariant(GameVariantPostModel variantPostModel)
        {
            var newVariant = new GameVariant();
            var findGameById = _unitOfWork.Games.Get().FirstOrDefault(g => g.Id == variantPostModel.GameId);
            if(findGameById == null)
            {
                throw new NotFound("The game with the given id doesn't exist");
            }
            newVariant.GameId = variantPostModel.GameId;
            newVariant.Title = variantPostModel.Title;
            newVariant.Description = variantPostModel.Description;
            newVariant.Price = variantPostModel.Price;
            newVariant.Rrp = variantPostModel.Rrp;
            newVariant.CreatedAt = DateTime.UtcNow;
            newVariant.Id = createGameVariantId();
            if (variantPostModel.HoverImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    variantPostModel.HoverImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newVariant.HoverImage = fileBytes;
                }
            }
            using (var ms = new MemoryStream())
            {
                variantPostModel.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                newVariant.Image = fileBytes;
            }
            _unitOfWork.GameVariants.Insert(newVariant);
            _unitOfWork.SaveChanges();
        }

        public void EditVariant(GameVariantEditModel variant, int gameVariantId)
        {
            var variantById = _unitOfWork.GameVariants.Get().FirstOrDefault(g => g.Id == gameVariantId);
            if (variantById == null)
            {
                throw new NotFound("Variant not Found");
            }

            variantById.Title = variant.Title;
            variantById.Description = variant.Description;
            variantById.CreatedAt = DateTime.UtcNow;
            if(variant.Price != null)
            {
                variantById.Price = (double)variant.Price;
            }
            if(variant.Rrp != null)
            {
                variantById.Rrp = variant.Rrp;
            }
            if(variant.HoverImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    variant.HoverImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    variantById.HoverImage = fileBytes;
                }
            }
            if(variant.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    variant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    variantById.Image = fileBytes;
                }
            }
            _unitOfWork.GameVariants.Update(variantById);
            _unitOfWork.SaveChanges();
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

        public byte[] GetImg(int id)
        {
            var game = _unitOfWork.GameVariants.Get().FirstOrDefault(i => i.Id == id);
            if (game == null)
            {
                throw new NotFound("Image not found");
            }

            return game.Image;
        }

        public byte[] GetHoverImg(int id)
        {
            var game = _unitOfWork.GameVariants.Get().FirstOrDefault(i => i.Id == id);
            if (game == null)
            {
                throw new NotFound("Image not found");
            }

            return game.HoverImage ?? game.Image;
        }
    }
}
