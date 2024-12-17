namespace BusinessLogicLayer.Dto;

public record ProductResponse(Guid ProductId, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductResponse() : this(default, default, default, default, default)
    {
    }
}