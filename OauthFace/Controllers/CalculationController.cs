using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhom.Data;
using WhoOwesWhom.Models;

namespace WhoOwesWhom.Controllers
{
    public class CalculationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userContext;

        public CalculationController(ApplicationDbContext context, UserManager<IdentityUser> userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IActionResult Calculation()
        {
            var curentUserId = _userContext.Users.FirstOrDefault(u => u.UserName == u.NormalizedUserName);
            var products = _context.Product.ToList();

            if (products.FirstOrDefault() == null)
            {
                return View("NotFound");
            }
            var average = products
                .Sum(m => m.Price)
                / products
                .GroupBy(m => m.UserName)
                .Count();

            List<ResultCalculation> Users = new List<ResultCalculation>();

            Product[] People = products
                .GroupBy(m => m.UserName)
                .Select(g => new Product { UserName = g.Key, Price = g.Sum(p => p.Price) }).ToArray();

            Product[] PaidMoreMoney = People.Where(m => m.Price > average).ToArray();
            Product[] PaidLessMoney = People.Where(m => m.Price < average).ToArray();

            for (int i = 0; i < PaidMoreMoney.Length; i++)
            {
                for (int j = 0; j < PaidLessMoney.Length; j++)
                {
                    int count = 0;
                    while (PaidLessMoney[j].Price != average)
                    {
                        if (PaidMoreMoney[i].Price != average)
                        {
                            PaidMoreMoney[i].Price -= 1;
                            PaidLessMoney[j].Price += 1;
                            count++;
                        }
                        if (PaidLessMoney[j].Price == average)
                        {
                            Users.Add(new ResultCalculation
                            {
                                UserLess = PaidLessMoney[j].UserName,
                                UserMore = PaidMoreMoney[i].UserName,
                                Money = count,
                                Average = average,
                            });
                        }
                        else if (PaidMoreMoney[i].Price == average)
                        {
                            Users.Add(new ResultCalculation
                            {
                                UserLess = PaidLessMoney[j].UserName,
                                UserMore = PaidMoreMoney[i].UserName,
                                Money = count
                            });
                            i++;
                            count = 0;
                        }
                    }
                }
            }
            return View(Users);
        }
    }
}
