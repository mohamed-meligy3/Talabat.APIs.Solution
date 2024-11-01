using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductsController(IUnitOfWork unitOfWork
            ,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]//api/Products
        public async Task<ActionResult<Pagenation<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductSpecifications(specParams);
            var Products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductsWithFiltrationForCountSpecification(specParams);
            var count = await unitOfWork.Repository<Product>().GetCountBySpecAsync(CountSpec);
            return Ok(new Pagenation<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count,data));

        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        [HttpGet("{id}")]
        public async  Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductSpecifications(id);
            var Product = await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (Product is null) return NotFound(new ApiErrorResponse(404));
            var MappedProduct = mapper.Map<Product, ProductToReturnDto>(Product);

            return Ok(MappedProduct);//200
        }

        [HttpGet("brands")] //api/Products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("types")] //api/Products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var types = await unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(types);
        }
      

    }
}
