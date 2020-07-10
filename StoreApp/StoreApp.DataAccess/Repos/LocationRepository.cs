using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreApp.DataAccess.Model;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;

namespace StoreApp.DataAccess.Repos
{
    public class LocationRepository : ILocationRepository
    {
        private readonly _2006StoreApplicationContext _context;

        public LocationRepository(_2006StoreApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new store in the database
        /// </summary>
        /// <param name="location">The store location</param>
        public void Create(Store location)
        {
            var entity = new Stores
            {
                StoreName = location.Name,
            };

        }

        /// <summary>
        /// Gets all the locations without the inventories
        /// </summary>
        /// <returns>The locations</returns>
        public IEnumerable<Store> GetAll()
        {
            var entities = _context.Stores.ToList();

            return entities.Select(e => new Store
            {
                StoreId = e.StoreId,
                Name = e.StoreName
            });
        }

        /// <summary>
        /// Gets all the products from the Stores Inventory
        /// </summary>
        /// <param name="id">The store id</param>
        /// <returns>A dictionary of products and their amounts</returns>
        public Dictionary<Product, int> GetAllProducts(int id)
        {
            var entity = _context.Stores
                    .Include(s => s.Inventory)
                    .First(s => s.StoreId == id);
            Dictionary<Product, int> inventory = new Dictionary<Product, int>();
            foreach (var item in entity.Inventory)
            {
                var product = _context.Products.Find(item.ProductId);
                Product p = new Product
                {
                    ProductId = product.ProductId,
                    Name = product.ProductName,
                    Price = product.Price
                };
                inventory.Add(p, item.Amount);
            }
            return inventory;
        }

        /// <summary>
        /// Gets a store without the inventory based on the id given
        /// </summary>
        /// <param name="id">The store id</param>
        /// <returns>The Store</returns>
        public Store GetById(int id)
        {
            var entity = _context.Stores.Find(id);
                
            return new Store { Name = entity.StoreName, StoreId = entity.StoreId };
        }

        /// <summary>
        /// Updates store inventory
        /// </summary>
        /// <param name="location">The store location</param>
        public void Update(Store location)
        {
            var entity = _context.Stores
                .Include(s => s.Inventory)
                .First(s => s.StoreId == location.StoreId);
            foreach (var item in location.Inventory.Keys)
            {
                entity.Inventory.First(i => i.ProductId == item.ProductId).Amount = location.Inventory[item];
            }
            _context.SaveChanges();
        }
    }
}
