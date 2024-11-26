using Core.Dtos;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseController
{
    private readonly IGenericService<Product, ProductDto> _productService;

    public ProductController(IGenericService<Product, ProductDto> productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return ActionResultInstance(await _productService.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> SaveProduct(ProductDto productDto)
    {
        return ActionResultInstance(await _productService.AddAsync(productDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto)
    {
        return ActionResultInstance(await _productService.Update(productDto, productDto.Id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        return ActionResultInstance(await _productService.Remove(id));
    }
}