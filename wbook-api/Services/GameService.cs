using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Game;
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
                    Image = game.Image,
                    HoverImage = game.HoverImage,
                    Price = game.Price,
                    Rrp = game.Rrp ?? game.Price,

                };

                gamesList.Add(gameModel);
            }

            return gamesList;
        }


        public GameVariantsModel GetGameById(int gameId)
        {
            var game = _unitOfWork.Games.Get().Include(game => game.GameVariants).FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                throw new NotFound("Game Not Found");
            }

            var gameModel = AutoMapper.Mapper.Map<Game, GameVariantsModel>(game);
            return gameModel;
        }

        public void AddGame(GamePostModel game)
        {
            /*var image = new Image();*/

            var gameMap = AutoMapper.Mapper.Map<GamePostModel, Game>(game);
            gameMap.Title = game.Title;
            gameMap.Description = game.Description;
            gameMap.GameVariants = new List<GameVariant>();
            _unitOfWork.Games.Insert(gameMap);
            _unitOfWork.SaveChanges();
        }

        public void EditGame(GamePostModel game)
        {
            var gameById = _unitOfWork.Games.Get().FirstOrDefault(g => g.Id == game.Id);
            if (gameById == null)
            {
                throw new NotFound("Game not Found");
            }

            gameById.Title = game.Title;
            gameById.Description = game.Description;

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

            game.GameVariants.Clear();
            _unitOfWork.Games.Delete(game);
            _unitOfWork.SaveChanges();
        }
    }
}
