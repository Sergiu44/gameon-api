using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Game;
using Infrastructure.Models.GameVariant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GameService: BaseService
    {
        public GameService(ServiceDependencies serviceDependencies): base(serviceDependencies) { }

        public int createGameId()
        {
            return _unitOfWork.Games.Get().ToList().Count + 1;
        }

        public async Task<List<GameModel>> GetGames()
        {
            var gamesList = new List<GameModel>();
            var listOfGames = await _unitOfWork.Games.Get().OrderBy(e => e.CreatedAt).ToListAsync();

            if (listOfGames == null)
            {
                return gamesList;
            }

            foreach (var game in listOfGames)
            {
                var gameModel = new GameModel()
                {
                    Id = game.Id,
                    Title = game.Title,
                    Description = game.Description,
                    Price = game.Price,
                    Rrp = game.Rrp ?? game.Price,

                };

                gamesList.Add(gameModel);
            }

            return gamesList;
        }


        public GameVariantsModel GetGameById(int gameId)
        {
            var gameToSent = new GameVariantsModel();
            var game = _unitOfWork.Games.Get().Include(game => game.GameVariants).FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                throw new NotFound("Game Not Found");
            }

            gameToSent.Id = game.Id;
            gameToSent.Title = game.Title;
            gameToSent.Description = game.Description;
            gameToSent.Price = game.Price;
            gameToSent.Rrp = game.Rrp ?? game.Price;
            var images = GetImg(game.Id);
            gameToSent.Image = images[0].ToString() ?? "";
            gameToSent.HoverImage = images[1].ToString();
            gameToSent.GameVariants = new List<GameVariantItemModel>();

            if (game.GameVariants != null)
            {
                foreach (var gameVariant in game.GameVariants)
                {
                    var newGameVar = new GameVariantItemModel();
                    newGameVar.Id = gameVariant.Id;
                    newGameVar.GameId = gameVariant.GameId;
                    newGameVar.Title = gameVariant.Title;
                    newGameVar.Description = gameVariant.Description;
                    newGameVar.Price = gameVariant.Price;
                    newGameVar.Rrp = gameVariant.Rrp;

                    gameToSent.GameVariants.Add(newGameVar);
                }
            }
            return gameToSent;
        }

        public async Task AddGame(GamePostModel game)
        {
            await ExecuteInTransaction(async uow =>
            {
                var newGame = new Game();
                newGame.Id = createGameId();
                newGame.Title = game.Title;
                newGame.Description = game.Description;
                if (game.HoverImage != null) {
                    using (var ms = new MemoryStream())
                    {
                        game.HoverImage.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        newGame.HoverImage = fileBytes;
                    }
                }
                using (var ms = new MemoryStream())
                {
                    game.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newGame.Image = fileBytes;
                }
                newGame.Price = game.Price;
                newGame.Rrp = game.Rrp;
                newGame.CreatedAt = DateTime.Now;
                newGame.Category = game.Type;
                uow.Games.Insert(newGame);
                uow.SaveChanges();
            });
            
        }

        public void EditGame(GamePutModel game, int gameId)
        {
            var gameById = _unitOfWork.Games.Get().FirstOrDefault(g => g.Id == gameId);
            if (gameById == null)
            {
                throw new NotFound("Game not Found");
            }

            gameById.Title = game.Title;
            gameById.Description = game.Description;
            gameById.Price = game.Price;
            gameById.Rrp = game.Rrp;
            if(game.HoverImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    game.HoverImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    gameById.HoverImage = fileBytes;
                    ;
                }
            }
            
            using (var ms = new MemoryStream())
            {
                game.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                gameById.Image = fileBytes;
            }
            _unitOfWork.Games.Update(gameById);
            _unitOfWork.SaveChanges();
        }

        public void DeleteGame(int gameId)
        {
            var game = _unitOfWork.Games.Get().Include(g => g.GameVariants).FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                throw new NotFound("Game not found");
            }
            foreach(var variant in game.GameVariants) {
                var basketItem = _unitOfWork.BasketItems.Get().FirstOrDefault(bi => bi.IdVariant== variant.Id);
                if (basketItem != null)
                {
                    _unitOfWork.BasketItems.Delete(basketItem);
                }

                var wishlistItem = _unitOfWork.WishlistItems.Get().FirstOrDefault(bi => bi.IdVariant == variant.Id);
                if (wishlistItem != null)
                {
                    _unitOfWork.WishlistItems.Delete(wishlistItem);
                }
            }
            game.GameVariants.Clear();

            _unitOfWork.Games.Delete(game);
            _unitOfWork.SaveChanges();
        }

        public byte[] GetImg(int id)
        {
            var game = _unitOfWork.Games.Get().FirstOrDefault(i => i.Id == id);
            if (game == null)
            {
                throw new NotFound("Image not found");
            }

            return game.Image;
        }

        public byte[] GetHoverImg(int id)
        {
            var game = _unitOfWork.Games.Get().FirstOrDefault(i => i.Id == id);
            if (game == null)
            {
                throw new NotFound("Image not found");
            }

            return game.HoverImage ?? game.Image;
        }
    }
}
