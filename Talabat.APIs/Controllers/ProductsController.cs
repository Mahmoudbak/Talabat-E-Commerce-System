using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.APIs.Helper;
using Talabat.Core.Entity.Product;
using Talabat.Core.Repository.content;
using Talabat.Core.Services.Content;
using Talabat.Core.specifications.Product_Spec;
using Talabat.Core.specifications.ProductSpec;


namespace Talabat.APIs.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;

    ///كنت بستخدمهم قبل ال unit of work 
    ///private readonly IGenaricrepository<Product> _ProductRepo;
    ///private readonly IGenaricrepository<ProductBrand> _brandRepo;
    ///private readonly IGenaricrepository<ProductCategory> _categoriesRepo;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductService productService
    ///IGenaricrepository<Product> productRepo,
    ///    IGenaricrepository<ProductBrand>BrandRepo,
    ///    IGenaricrepository<ProductCategory>CategorysRepo
        ,IMapper mapper)
    {
        _productService = productService;

        ///_ProductRepo = productRepo;
        ///_brandRepo = BrandRepo;
        ///_categoriesRepo = CategorysRepo;
        _mapper = mapper;
    }
    #region berfore Mapping
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Product>>> Getallproduct()
    //{
    //    var spec = new ProductWithBranAndCategory_Spec();
    //    var product = await _ProductRepo.GetAllwithSpecAsync(spec);//_dbcontex.set<product>().include(p=>p.productbrand).include(p=>p.productcategory).tolistasync();

    //    return Ok(product);




    //[HttpGet("{Id}")]
    //public async Task<ActionResult<Product>> GetProductID(int id)
    //{
    //    var spec = new ProductWithBranAndCategory_Spec(id);
    //    var product = await _ProductRepo.GetwithSpecAsync(spec);
    //    if (product is null)
    //        return NotFound(new { massege = "NOT FOUNT THE PRODUCT", statusCode = 404 });
    //    return Ok(product);

    //}
    //}
    #endregion
    // [Authorize ]

    [CachedAttribute(600)] //Action Filter
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> Getallproduct([FromQuery] ProductSprcParams SpecParams)
        {

        //var spec = new ProductWithBranAndCategory_Spec(SpecParams);

        var product = await _productService.GetProductAsync(SpecParams);//_dbcontex.set<product>().include(p=>p.productbrand).include(p=>p.productcategory).tolistasync();

        var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(product);
            
        //var countSpec = new productWithFilterationForCountSpecifications(SpecParams);

        var count = await _productService.GetCountAsync(SpecParams);


        return Ok(new pagination<ProductToReturnDto>(SpecParams.PageIndex, SpecParams.PageSize, count, data));
    }




    #region get product before specification
    //public async Task<ActionResult<IEnumerable<Product>>> Getallproduct()
    //{

    //    var product = await _ProductRepo.GetAllAsync();
    //    return Ok(product);
    //}




    //public async Task<ActionResult<Product>> GetProductID(int id)
    //{
    //    var product=await _ProductRepo.GetAsync(id);
    //    if (product is null)
    //        return NotFound(new{ massege="NOT FOUNT THE PRODUCT",statusCode=404});
    //    return Ok(product);
    //}

    #endregion


    //more readaple in Swagger
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProductID(int id)
    {
        //var spec = new ProductWithBranAndCategory_Spec(id);
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound(new ApiResponse(404));
        return Ok(_mapper.Map<Product,ProductToReturnDto>(product));

    }


    [HttpGet("Brands")]//Get: /api/Product/Brands
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrand()
    {
        var Brand=await _productService.productBrandsAsync();
        return Ok(Brand);
    }
    [HttpGet("categories")] //Get: /api/Product/Categories
    public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategorys()
    {
        var Category = await _productService.productCategoriesAsync();
        return Ok(Category);
    }

    //[HttpGet("types")]
    //public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
    //{
    //    var types = await _typesRepo.GetAllAsync();
    //    return Ok(types);
    //}



}