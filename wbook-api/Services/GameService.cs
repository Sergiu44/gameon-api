using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Entities;
using Infrastructure.Models.Game;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GameService: BaseService
    {
        private GameService(UnitOfWork unitOfWork): base(unitOfWork) { }

        public async Task<List<GameItemModel>> GetGames()
        {
            var gamesList = new List<GameItemModel>();
            var listOfGames = await _unitOfWork.Games.Get().OrderBy(e => e.CreatedAt).ToListAsync();

            if(listOfGames == null)
            {
                return gamesList;
            }

            foreach(var game in listOfGames)
            {
                var gameModel = new GameItemModel()
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


        public GameModel GetGameById(Guid gameId)
        {
            var game = _unitOfWork.Games.Get().FirstOrDefault(g => g.Id == gameId);
            if(game == null)
            {
                throw new NotFound("Game Not Found");
            }

            var variants = _unitOfWork.GameVariants.Get().FirstOrDefault(v => v.GameId == game.Id);
            var gameModel = AutoMapper.Mapper.Map<Game, GameModel>(game);
            return gameModel;
        }

        public void AddGame(GamePostModel game)
        {
            var gameMap= AutoMapper.Mapper.Map<GamePostModel, Game>(game);
            gameMap.Title= game.Title;
            gameMap.Description= game.Description;
            gameMap.GameVariants = new List<GameVariant>();
            _unitOfWork.Games.Insert(gameMap);
            _unitOfWork.SaveChanges();
        }

        public void EditGame(GamePostModel game)
        {
            var gameById = _unitOfWork.Games.Get().FirstOrDefault(g => g.Id == game.Id);
            if(gameById == null)
            {
                throw new NotFound("Game not Found");
            }

            gameById.Title = game.Title;
            gameById.Description = game.Description;

            _unitOfWork.Games.Update(gameById);
            _unitOfWork.SaveChanges();
        }

        public void DeleteGame(Guid gameId)
        {
            var game = _unitOfWork.Games.Get().Include(g => g.GameVariants).FirstOrDefault(g => g.Id == gameId);
            if(game == null)
            {
                throw new NotFound("Game not found");
            }

            game.GameVariants.Clear();
            _unitOfWork.Games.Delete(game);
            _unitOfWork.SaveChanges();
        }
    }
}
