using GTERP.Interfaces.Base;
using GTERP.Models;
using System.Collections.Generic;

namespace GTERP.Interfaces.ControllerFolder
{
    public interface IAddToCartRepository : IBaseRepository<CartOrderMain>
    {
        void AddToCartList(CartOrderMain cartordermain);
        List<CartOrderDetails> MyOrder();
        void RemoveToCartList(CartOrderDetails mob);
    }
}
