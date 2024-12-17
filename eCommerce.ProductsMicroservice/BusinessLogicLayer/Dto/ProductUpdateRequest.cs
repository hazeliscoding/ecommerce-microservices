namespace BusinessLogicLayer.Dto;

public record ProductUpdateRequest(Guid ProductId, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductUpdateRequest() : this(default, default, default, default, default)
    {
    }
}