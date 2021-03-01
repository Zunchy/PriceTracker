using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
   
    public class EbayProductService
    {
        static HttpClient client;
        public IServiceProvider serviceProvider;

        public EbayProductService(IServiceProvider services)
        {
            client = new HttpClient();
            serviceProvider = services;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "v^1.1#i^1#p^1#f^0#I^3#r^0#t^H4sIAAAAAAAAAOVYf2wTVRxvt64ENmAaA2aAlJsoP7z23bv26A5a0q1bqGFbR0vFRcHr3Ss7dr273L3S1RhTpxlK3KIhkkg0sEQxKkIggMaIGTEaiSIRjRgEjEgCiRLxB9FI/HF3K6ObZCBrcIn9p7nv+77v+3w/n+/3vXcH8s6JC3qW9fw62T6hbFse5MvsdqoSTHRWLJxSXlZTYQNFDvZt+Tvzju7yc0t0Li2p7Aqkq4qsI1dXWpJ11jIGiIwmswqnizorc2mks5hnY6Hm5Sx0A1bVFKzwikS4IuEA4ffSKQiQX6B8PMMhwyhfDhlXAkQK8NALqEUCRfkRT9PGuK5nUETWMSfjAAEBpEhAk4CKQ8hSgIXQzUBfO+FKIE0XFdlwcQMiaKFlrblaEdTRkXK6jjRsBCGCkVBTrDUUCTe2xJd4imIFCzTEMIcz+vCnBkVArgQnZdDoy+iWNxvL8DzSdcITHFxheFA2dBnMDcC3mPYByNBen5eqE3iIuJIw2aRoaQ6PDsO0iAKZslxZJGMR565FqEFGch3iceGpxQgRCbvMv7YMJ4kpEWkBorE+dH8oGiWC7RzfwWm5+8gGTtWxIiMyuiJMCgLP8DxdlyQ5gecAFPyFhQajFVgesVKDIguiyZnualFwPTJQo5HcgCJuDKdWuVULpbCJaMgPxg2nyxyCdlPTQREzuEM2ZUVpgwiX9XhtBYZmY6yJyQxGQxFGDlgUBQhOVUWBGDlolWKherr0ANGBscp6PNls1p2l3Yq21gMBoDyrmpfH+A6UNirE9DV73fQXrz2BFK1UeKOLDX8W51QDS5dRqgYAeS0R9NJeo5kLvA+HFRxp/YehKGfP8IYoVYPwDEwKPhqlvJARIPCWokOChSL1mDhQksuRaU7rRFiVOB6RvFFnmTTSRIGlfSlI+1OIFJi6FOmtS6XIpE9gSCqFEEAomeTr/P+nRrneUo8hXkO4RLVeojpvU3LNOrUOg2iTj8FRCa7D99I5j4I8cFkSxhYtj1Bt4S5GYrJ84Hq74erJ84qKoook8rmSMGD2eslYoDUhymk4F0OSZBjGlKhuJjq+RDbn60YAThXdZmO7eSXtUThjRzdNayzEY8o5pKqRdDqDuaSEIqXazf+Tnfyq6YnGVWdc5WToNyikKAxeUtyWmm59Pe/WkK5kNON65m41z+y40olkYwfEmiJJSEtQYxb65utrneuj8fEvD4sby72UN5XxU9u8JBoltGa8ZXZTFBW5cXYaUwzlZWiGgXBMeTVYmsZz4+0cWqboGAmjp+ZovKFrtWf4K37QZv2obvs+0G3fXWa3Aw+YS9WCOc7ylY7yqhpdxMgtcim3Lq6VjVdXDbk7UU7lRK3Mac9Of/vlA0UfFbY9CG4f+qwwsZyqLPrGAGZeGamgpk6fDClAAwoafxC2g9orow5qmuM2pv+skug9cHLhbyLXJe/e2nfPV1PA5CEnu73C5ui22za++eWCJ5Z6X9t1ZFJr/6G+E65TUnV8ZXPiqYOnzr3bOOGIL5F4jj4m+C/MuvVU7aaBHY99PuPHzesPPf8Tk++r+qWROdqRfuRJaff3/cw3i73z39mw6eET28+fWbxfrkRpuPrxvuqa09nshjUzBpr7X7p4R/jT/T0Ne7Ve9SJcdPzMAy21A5mnbY7fZ959OvTDH/Vf94SDPpt015TDRJUyf+lS2/tbJg2crD40e9/W7bW3zDpZc+w7NPVPZ9mqg23TXi8/Ouev2Xvz9Tuj54Wmbz9yHq9+dPPqZ3Y89Oqeqg9e6fzixd4PnYpt16XDc8/urLy0cY/tk4/nydt/xku25NGFF57tnHcYnHnrjfc+G5Tvbx61tjLuEQAA");

        }

        public  async Task<string> SearchProductAsync(string query, int numResults)
        {
            string content = null;
            HttpResponseMessage response = await client.GetAsync($"https://api.ebay.com/buy/browse/v1/item_summary/search?q={query}&limit={numResults}");
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
