using GymProjectApp.Models;
using GymProjectApp.DAL;
using System;
using System.Collections.Generic;

namespace GymProjectApp.BLL
{
    /// <summary>
    /// Business Logic Layer class responsible for managing Product operations.
    /// It delegates persistence tasks to ProductRepository and applies business rules if needed.
    /// </summary>
    public class ProductManager
    {
        // Repository instance for database operations
        private readonly ProductRepository _repo = new ProductRepository();

        /// <summary>
        /// Adds a new product to the database after basic validation.
        /// </summary>
        public void Add(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Name) || p.Price <= 0)
            {
                Console.WriteLine("Invalid product data.");
                return;
            }
            _repo.Insert(p);
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        public void Update(Product p)
        {
            if (p.ProductID <= 0)
            {
                Console.WriteLine("Invalid product ID.");
                return;
            }
            _repo.Update(p);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        public void Delete(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Invalid product ID.");
                return;
            }
            _repo.Delete(id);
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public List<Product> GetAll() => _repo.GetAll();

        /// <summary>
        /// Searches for a product by name (case-insensitive).
        /// </summary>
        public Product SearchByName(string name)
        {
            var products = _repo.GetAll();
            return products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves products cheaper than a given price.
        /// </summary>
        public List<Product> GetProductsCheaperThan(decimal maxPrice)
        {
            var products = _repo.GetAll();
            return products.FindAll(p => p.Price < maxPrice);
        }
    }
}