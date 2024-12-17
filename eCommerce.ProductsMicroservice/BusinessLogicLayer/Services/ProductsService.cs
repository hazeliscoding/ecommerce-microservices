using AutoMapper;
using BusinessLogicLayer.Dto;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using System.Linq.Expressions;
using FluentValidation.Results;

namespace BusinessLogicLayer.Services;

public class ProductsService(
    IValidator<ProductAddRequest> productAddRequestValidator,
    IValidator<ProductUpdateRequest> productUpdateRequestValidator,
    IMapper mapper,
    IProductsRepository productsRepository)
    : IProductsService
{
    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        // Validate the product using Fluent Validation
        var validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

        // Check the validation result
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
            throw new ArgumentException(errors);
        }


        // Attempt to add product
        var productInput = mapper.Map<Product>(productAddRequest); //Map productAddRequest into 'Product' type (it invokes ProductAddRequestToProductMappingProfile)
        var addedProduct = await productsRepository.AddProduct(productInput);

        if (addedProduct == null)
        {
            return null;
        }

        var addedProductResponse = mapper.Map<ProductResponse>(addedProduct); //Map addedProduct into 'ProductRepsonse' type (it invokes ProductToProductResponseMappingProfile)

        return addedProductResponse;
    }


    public async Task<bool> DeleteProduct(Guid productId)
    {
        var existingProduct = await productsRepository.GetProductByCondition(temp => temp.ProductID == productId);

        if (existingProduct == null)
        {
            return false;
        }

        // Attempt to delete product
        var isDeleted = await productsRepository.DeleteProduct(productId);
        return isDeleted;
    }


    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        var product = await productsRepository.GetProductByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }

        var productResponse = mapper.Map<ProductResponse>(product); //Invokes ProductToProductResponseMappingProfile
        return productResponse;
    }


    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await productsRepository.GetProducts();


        IEnumerable<ProductResponse?> productResponses = mapper.Map<IEnumerable<ProductResponse>>(products); //Invokes ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }


    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        var products = await productsRepository.GetProductsByCondition(conditionExpression);

        IEnumerable<ProductResponse?> productResponses = mapper.Map<IEnumerable<ProductResponse>>(products); //Invokes ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }


    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        var existingProduct = await productsRepository.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductId);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }


        // Validate the product using Fluent Validation
        var validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        // Check the validation result
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
            throw new ArgumentException(errors);
        }


        // Map from ProductUpdateRequest to Product type
        var product = mapper.Map<Product>(productUpdateRequest); //Invokes ProductUpdateRequestToProductMappingProfile

        var updatedProduct = await productsRepository.UpdateProduct(product);

        var updatedProductResponse = mapper.Map<ProductResponse>(updatedProduct);

        return updatedProductResponse;
    }
}