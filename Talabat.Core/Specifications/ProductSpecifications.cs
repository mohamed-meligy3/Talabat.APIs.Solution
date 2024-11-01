using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecifications:BaseSpecification<Product>
    {//where(P=> true && true) 
     //where(P=> P.ProductBrandId == brandId && true) 
     //where(true && P=> P.ProductTypeId == typeId)
     //where( p=>P.ProductBrandId== brandId &&P=> P.ProductTypeId == typeId)

        public ProductSpecifications(ProductSpecParams specParams)//Get
            :base(P=>
            (!specParams.BrandId.HasValue||P.ProductBrandId == specParams.BrandId) &&
            (!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId)
            )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            //Total 100
            // Size 10
            // Index 3 
            //10 10 10 10 10 10 10 10 
            ApplyPagenation(specParams.PageSize, specParams.PageSize * (specParams.PageIndex - 1));

        }

        public ProductSpecifications(int id):base(P=>P.Id == id)//GetById
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
