using GTERP.Interfaces.ControllerFolder;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
//using System.Web.Mvc;

namespace GTERP.Repository.ControllerFolder
{
    public class AddToCartRepository : BaseRepository<CartOrderMain>, IAddToCartRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<AddToCartRepository> _logger;

        public AddToCartRepository(
            GTRDBContext context,
            IHttpContextAccessor httpcontext,
            ILogger<AddToCartRepository> logger
            ) : base(context)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }

        public void AddToCartList(CartOrderMain cartordermain)
        {

            GTERP.Models.Product cartproduct = _context.Products.Where(x => x.ProductId == cartordermain.CartorderDetails.FirstOrDefault().ProductId).FirstOrDefault();

            var cartlist = _httpcontext.HttpContext.Session.GetObject<List<CartOrderDetails>>("cartlist");

            int i = cartlist.Count() + 1;

            if (cartlist.Count == 0)
            {
                List<CartOrderDetails> li = new List<CartOrderDetails>();

                foreach (CartOrderDetails mo in cartordermain.CartorderDetails)
                {
                    mo.vProductName = cartproduct;
                    mo.RowNo = i;
                    mo.UnitPrice = float.Parse(mo.vProductName.SalePrice.ToString());
                    mo.Amount = float.Parse(mo.vProductName.SalePrice.ToString()) * mo.Qty;
                    li.Add(mo);
                    i++;
                }

                _httpcontext.HttpContext.Session.SetObject("cartlist", li);
                _httpcontext.HttpContext.Session.SetObject("cartlistcount", li.Count());
            }
            else
            {
                foreach (CartOrderDetails mo in cartordermain.CartorderDetails)
                {
                    var prevCartproduct = cartlist.Where(x => x.ProductId == cartordermain.CartorderDetails.FirstOrDefault().ProductId).FirstOrDefault();

                    if (prevCartproduct != null)
                    {
                        prevCartproduct.Qty = (mo.Qty + prevCartproduct.Qty);
                        prevCartproduct.Amount = float.Parse(prevCartproduct.UnitPrice.ToString()) * (float.Parse(prevCartproduct.Qty.ToString()));

                        i++;
                    }
                    else
                    {
                        mo.vProductName = cartproduct;
                        mo.RowNo = i;
                        mo.UnitPrice = float.Parse(mo.vProductName.SalePrice.ToString());
                        mo.Amount = float.Parse(mo.vProductName.SalePrice.ToString()) * mo.Qty;
                        cartlist.Add(mo);
                        i++;
                    }
                }
                _httpcontext.HttpContext.Session.SetObject("cartlist", cartlist);
                _httpcontext.HttpContext.Session.SetObject("cartlistcount", cartlist.Count());


            }

        }

        public List<CartOrderDetails> MyOrder()
        {
            List<CartOrderDetails> myorderabc = _httpcontext.HttpContext.Session.GetObject<List<CartOrderDetails>>("cartlist").ToList();
            return myorderabc;
        }

        public void RemoveToCartList(CartOrderDetails mob)
        {
            List<CartOrderDetails> li = _httpcontext.HttpContext.Session.GetObject<List<CartOrderDetails>>("cartlist");
            li.RemoveAll(x => x.ProductId == mob.ProductId);
            _httpcontext.HttpContext.Session.SetObject("cartlist", li);
            _httpcontext.HttpContext.Session.SetObject("cartlistcount", li.Count()); ;
        }
    }
}
