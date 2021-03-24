using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Pages
{
    public partial class ProductCardComponent
    {
        [Parameter] public ProductCardComponent Product { get; set; }

        [Parameter] public string prodImgSrc { get; set; }

        [Parameter] public string prodName { get; set; }

        [Parameter] public string prodPrice { get; set; }

        [Parameter] public string prodLink { get; set; }

        [Parameter] public string prodEpid { get; set; }

        [Parameter] public string prodSource { get; set; }

    }
}
