using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductsRepository(ApplicationDbContext dbContext) : IProductsRepository
{
    public async Task<Product?> AddProduct(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var existingProduct = await dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productId);
        if (existingProduct == null)
        {
            return false;
        }

        dbContext.Products.Remove(existingProduct);
        var affectedRowsCount = await dbContext.SaveChangesAsync();
        return affectedRowsCount > 0;
    }


    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }


    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await dbContext.Products.ToListAsync();
    }


    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.Where(conditionExpression).ToListAsync();
    }


    public async Task<Product?> UpdateProduct(Product product)
    {
        var existingProduct = await dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.Category = product.Category;

        await dbContext.SaveChangesAsync();

        return existingProduct;
    }
}