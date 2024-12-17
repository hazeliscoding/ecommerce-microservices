using BusinessLogicLayer.Dto;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;

namespace ProductsMicroservice.API.Endpoints;

public static class ProductApiEndpoints
{
    public static IEndpointRouteBuilder MapProductApiEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async (IProductsService productsService) =>
        {
            var products = await productsService.GetProducts();
            return Results.Ok(products);
        });


        //GET /api/products/search/product-id/00000000-0000-0000-0000-000000000000
        app.MapGet("/api/products/search/product-id/{productId:guid}", async (IProductsService productsService, Guid productId) =>
        {
            var product = await productsService.GetProductByCondition(temp => temp.ProductID == productId);
            return Results.Ok(product);
        });


        //GET /api/products/search/xxxxxxxxxxxxxxxxxx
        app.MapGet("/api/products/search/{searchString}", async (IProductsService productsService, string searchString) =>
        {
            var productsByProductName = await productsService.GetProductsByCondition(temp => temp.ProductName != null && temp.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var productsByCategory = await productsService.GetProductsByCondition(temp => temp.Category != null && temp.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });


        //POST /api/products
        app.MapPost("/api/products", async (IProductsService productsService, IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest productAddRequest) =>
        {
            // Validate the ProductAddRequest object using Fluent Validation
            var validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            // Check the validation result
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }


            var addedProductResponse = await productsService.AddProduct(productAddRequest);
            if (addedProductResponse != null)
                return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductId}", addedProductResponse);
            return Results.Problem("Error in adding product");
        });


        //PUT /api/products
        app.MapPut("/api/products", async (IProductsService productsService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
        {
            // Validate the ProductUpdateRequest object using Fluent Validation
            var validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            // Check the validation result
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }


            var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
            if (updatedProductResponse != null)
                return Results.Ok(updatedProductResponse);
            return Results.Problem("Error in updating product");
        });


        //DELETE /api/products/xxxxxxxxxxxxxxxxxxx
        app.MapDelete("/api/products/{productId:guid}", async (IProductsService productsService, Guid productId) =>
        {
            var isDeleted = await productsService.DeleteProduct(productId);
            if (isDeleted)
                return Results.Ok(true);
            return Results.Problem("Error in deleting product");
        });
        return app;
    }
}