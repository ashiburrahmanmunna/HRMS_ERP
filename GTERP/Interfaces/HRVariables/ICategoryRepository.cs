using GTERP.Interfaces.Base;
using GTERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GTERP.Interfaces.HRVariables
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryList();
        public void CreateCategory(Category catagory, IFormFile file, string imageDatatest);
    }
}
