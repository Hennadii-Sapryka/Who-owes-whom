﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WhoWhom.Data;
using WhoWhom.Models;

namespace WhoWhom.Controllers
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
            var U = _userContext.Users.ToList();

            var products = _context.Product.ToList();

            if (products.FirstOrDefault() == null)
            {
                return View("NotFound");
            }
            // знаходимо середнє арифметичне значення ціни
            var average = products
                .Sum(m => m.Price)
                / products
                .GroupBy(m => m.User)
                .Count();

            List<ResultCalculation> Users = new List<ResultCalculation>();

            // визначаємо кількість осіб та їхню загальну суму витрат
            Product[] People = products
                .GroupBy(m => m.User)
                .Select(g => new Product { User = g.Key, Price = g.Sum(p => p.Price) }).ToArray();
           
            // сортуємо осіб по тому хто більше а хто менше витратив відносно середньої ціни
            Product[] GiveMoreMoney = People.Where(m => m.Price > average).ToArray();
            Product[] GiveLessMoney = People.Where(m => m.Price < average).ToArray();

            for (int i = 0; i < GiveMoreMoney.Length; i++)
            {
                for (int j = 0; j < GiveLessMoney.Length; j++)
                {
                    int count = 0;
                    while (GiveLessMoney[j].Price != average)
                    {
                        if (GiveMoreMoney[i].Price != average)
                        {
                            GiveMoreMoney[i].Price -= 1;
                            GiveLessMoney[j].Price += 1;
                            count++;
                        }
                        if (GiveLessMoney[j].Price == average)
                        {
                            // добавляємо в список осіб для відображення у View
                            Users.Add(new ResultCalculation
                            {
                                UserLess = GiveLessMoney[j].User,
                                UserMore = GiveMoreMoney[i].User,
                                Money = count,
                                Average = average,

                            });

                        }
                        else if (GiveMoreMoney[i].Price == average)
                        {
                            // добавляємо в список осіб для відображення у View
                            Users.Add(new ResultCalculation
                            {
                                UserLess = GiveLessMoney[j].User,
                                UserMore = GiveMoreMoney[i].User,
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