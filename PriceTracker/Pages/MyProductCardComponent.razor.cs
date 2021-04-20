using Microsoft.AspNetCore.Components;
using PriceTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Pages
{
    public partial class MyProductCardComponent
    {
        [Parameter] public MyProductCardComponent Product { get; set; }

        [Parameter] public DisplayItem prodItem { get; set; }

    }
}
