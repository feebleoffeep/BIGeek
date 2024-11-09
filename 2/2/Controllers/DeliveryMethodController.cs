﻿using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace _2.Controllers
{
    public class DeliveryMethodController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryMethodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryMethod/CreateDeliveryMethod
        public IActionResult CreateDeliveryMethod()
        {
            return View();
        }

        // POST: DeliveryMethod/CreateDeliveryMethod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                // Додаємо новий спосіб доставки
                _context.DeliveryMethods.Add(deliveryMethod);
                _context.SaveChanges();

                return RedirectToAction("ManageDeliveryMethod");
            }

            return View(deliveryMethod); // Якщо не валідно, повертаємо форму
        }

        // GET: DeliveryMethod/ManageDeliveryMethod
        public IActionResult ManageDeliveryMethod()
        {
            var deliveryMethods = _context.DeliveryMethods.ToList();
            return View(deliveryMethods);
        }
        // GET: DeliveryMethod/Edit/5
        public IActionResult EditDeliveryMethod(int id)
        {
            var deliveryMethod = _context.DeliveryMethods.FirstOrDefault(d => d.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound(); // Якщо не знайдено доставку з таким ID
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDeliveryMethod(int id, DeliveryMethod deliveryMethod)
        {
            if (id != deliveryMethod.Id)
            {
                return NotFound(); // Якщо ID не збігається
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethod); // Оновлюємо доставку
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    // Якщо сталася помилка збереження
                    if (!_context.DeliveryMethods.Any(d => d.Id == deliveryMethod.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Якщо інша помилка, то викидаємо її
                    }
                }

                return RedirectToAction(nameof(ManageDeliveryMethod)); // Після збереження перенаправляємо до списку
            }
            return View(deliveryMethod); // Якщо модель не валідна, повертаємо на форму редагування
        }

        public async Task<IActionResult> DeleteDeliveryMethod(int id)
        {
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            if (deliveryMethod != null)
            {
                _context.DeliveryMethods.Remove(deliveryMethod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageDeliveryMethod");
        }        
    }
}
