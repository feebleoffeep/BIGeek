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

        public IActionResult CreateDeliveryMethod()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                _context.DeliveryMethods.Add(deliveryMethod);
                _context.SaveChanges();

                return RedirectToAction("ManageDeliveryMethod");
            }

            return View(deliveryMethod); 
        }

        public IActionResult ManageDeliveryMethod()
        {
            var deliveryMethods = _context.DeliveryMethods.ToList();
            return View(deliveryMethods);
        }

        public IActionResult EditDeliveryMethod(int id)
        {
            var deliveryMethod = _context.DeliveryMethods.FirstOrDefault(d => d.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound(); 
            }
            return View(deliveryMethod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDeliveryMethod(int id, DeliveryMethod deliveryMethod)
        {
            if (id != deliveryMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethod); 
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    if (!_context.DeliveryMethods.Any(d => d.Id == deliveryMethod.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; 
                    }
                }

                return RedirectToAction(nameof(ManageDeliveryMethod)); 
            }
            return View(deliveryMethod); 
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
