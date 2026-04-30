using MediatR;

namespace E_Commerce.Features.Products.GetAllProducts;

public readonly record struct GetAllProductsQuery(string? Category = null)
    : IRequest<List<GetAllProductsResults>>;
