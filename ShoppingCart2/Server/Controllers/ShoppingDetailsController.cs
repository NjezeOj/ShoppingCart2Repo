﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart2.Shared.Models;

namespace ShoppingCart2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingDetailsController : ControllerBase
    {
        private readonly ShoppingCart2DBContext _context;

        public ShoppingDetailsController(ShoppingCart2DBContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingDetails
        [HttpGet]
        public IEnumerable<myShopping> GetShoppingDetails()
        {
            var results = (from items in _context.ItemDetails
                           join shop in _context.ShoppingDetails
                                on items.ItemId equals shop.ItemId
                           select new myShopping
                           {

                               ShopId = shop.ShopId,
                               ItemName = items.ItemName,
                               ImageName = items.ImageName,
                               UserName = shop.UserName,
                               Qty = shop.Qty,
                               TotalAmount = shop.TotalAmount,
                               Description = shop.Description,
                               ShoppingDate = shop.ShoppingDate
                           }).ToList();


            return results;
        }

        // GET: api/ShoppingDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingDetails>> GetShoppingDetails(int id)
        {
            var shoppingDetails = await _context.ShoppingDetails.FindAsync(id);

            if (shoppingDetails == null)
            {
                return NotFound();
            }

            return shoppingDetails;
        }

        // PUT: api/ShoppingDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingDetails(int id, ShoppingDetails shoppingDetails)
        {
            if (id != shoppingDetails.ShopId)
            {
                return BadRequest();
            }

            _context.Entry(shoppingDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShoppingDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ShoppingDetails>> PostShoppingDetails(ShoppingDetails shoppingDetails)
        {
            _context.ShoppingDetails.Add(shoppingDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingDetails", new { id = shoppingDetails.ShopId }, shoppingDetails);
        }

        // DELETE: api/ShoppingDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShoppingDetails>> DeleteShoppingDetails(int id)
        {
            var shoppingDetails = await _context.ShoppingDetails.FindAsync(id);
            if (shoppingDetails == null)
            {
                return NotFound();
            }

            _context.ShoppingDetails.Remove(shoppingDetails);
            await _context.SaveChangesAsync();

            return shoppingDetails;
        }

        private bool ShoppingDetailsExists(int id)
        {
            return _context.ShoppingDetails.Any(e => e.ShopId == id);
        }
    }
}
