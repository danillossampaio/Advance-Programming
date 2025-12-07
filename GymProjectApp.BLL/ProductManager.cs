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
        private readonly ProductRepository _repo;

        /// <summary>
        /// Constructor that receives a ProductRepository instance.
        /// </summary>
        public ProductManager(ProductRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Adds a new product to the database after basic validation.
        /// Returns true if successful, false otherwise.
        /// </summary>
        public bool Add(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Name) || p.Price <= 0)
                return false;

            _repo.Insert(p);
            return true;
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// Returns true if successful, false otherwise.
        /// </summary>
        public bool Update(Product p)
        {
            if (p.ProductID <= 0)
                return false;

            _repo.Update(p);
            return true;
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// Returns true if successful, false otherwise.
        /// </summary>
        public bool Delete(int id)
        {
            if (id <= 0)
                return false;

            _repo.Delete(id);
            return true;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public List<Product> GetAll() => _repo.GetAll();

        /// <summary>
        /// Searches for a product by name (case-insensitive).
        /// Returns null if not found.
        /// </summary>
        public Product? SearchByName(string name)
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