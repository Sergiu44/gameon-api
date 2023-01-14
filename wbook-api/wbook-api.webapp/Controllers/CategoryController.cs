using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {

        private readonly UnitOfWork _unitOfWork;
        public CategoryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get")]
        public IActionResult GetCategories() 
        {
            var categories = _unitOfWork.Categories.Get();
            return Ok(categories);
        }

        [HttpPost("post")]
        public IActionResult PostCategory([FromForm] string categoryName)
        {
            var existingCategory = _unitOfWork.Categories.Get().FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower());
            if (existingCategory == null)
            {
                var count = _unitOfWork.Categories.Get().Count() + 1;
                var newCategory = new Category()
                {
                    Id = count,
                    CategoryName = categoryName
                };
                _unitOfWork.Categories.Insert(newCategory);
                _unitOfWork.SaveChanges();
                return Ok();
            } else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("delete/{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var categoryById = _unitOfWork.Categories.Get().FirstOrDefault(c => c.Id == categoryId);
            if(categoryById != null) {
                var games = _unitOfWork.Games.Get().Where(g => g.Category == categoryById.CategoryName);
                foreach(var game in games)
                {
                    _unitOfWork.Games.Delete(game);
                }
                _unitOfWork.Categories.Delete(categoryById);
                _unitOfWork.SaveChanges();
            }
            return Ok();
        }
    }
}
