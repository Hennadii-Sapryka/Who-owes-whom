﻿using Microsoft.AspNetCore.Identity;
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
            //var curentUserId = _userContext.Users.FirstOrDefault(u => u.UserName == u.NormalizedUserName);
            var products = _context.Product.ToList();

            if (products.FirstOrDefault() == null)
            {
                return View("NotFound");
            }

            int average = products
                .Sum(m => m.Price)
                / products
                .GroupBy(m => m.UserName)
                .Count();

            List<ResultCalculation> results = new List<ResultCalculation>();

            Product[] people = products
                .GroupBy(m => m.UserName)
                .Select(g => new Product
                {
                    UserName = g.Key,
                    Price = g.Sum(p => p.Price)
                }).ToArray();

            Product[] paidMoreMoney = people.Where(m => m.Price > average).ToArray();
            Product[] paidLessMoney = people.Where(m => m.Price < average).ToArray();

            for (int i = 0; i < paidMoreMoney.Length; i++)
            {
                for (int j = 0; j < paidLessMoney.Length; j++)
                {
                    int count = 0;
                    while (paidLessMoney[j].Price != average)
                    {
                        if (paidMoreMoney[i].Price != average)
                        {
                            paidMoreMoney[i].Price -= 1;
                            paidLessMoney[j].Price += 1;
                            count++;
                        }
                        if (paidLessMoney[j].Price == average)
                        {
                            results.Add(new ResultCalculation
                            {
                                UserLess = paidLessMoney[j].UserName,
                                UserMore = paidMoreMoney[i].UserName,
                                Money = count,
                                Average = average,
                            });
                        }
                        else if (paidMoreMoney[i].Price == average)
                        {
                            results.Add(new ResultCalculation
                            {
                                UserLess = paidLessMoney[j].UserName,
                                UserMore = paidMoreMoney[i].UserName,
                                Money = count
                            });
                            i++;
                            count = 0;
                        }
                    }
                }
            }

            return View(results);
        }
    }
}
