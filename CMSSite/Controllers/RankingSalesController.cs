using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using Microsoft.Extensions.Configuration;

namespace CRMSite.Controllers
{
    public class RankingSalesController : Controller
    {
        private readonly IRatingSale _ratingSale;
        private readonly IConfiguration _configuration;
        public RankingSalesController(IRatingSale ratingSale, IConfiguration configuration)
        {
            _ratingSale = ratingSale;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            DateTime today = DateTime.Now;
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var top10ByWeek = _ratingSale.GetTop10Week(today, out week);
            var top10ByMonth = _ratingSale.GetTop10Month(today, out month);
            string dateToarst = _configuration["DateToarst"].ToString();

            RankingSaleViewModel data = new RankingSaleViewModel()
            {
                Week = week,
                Month = month,
                Timmer = dateToarst ?? "0:0:0",
                LstSaleTop10ByDay = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByWeek = top10ByWeek.Result != null ? top10ByWeek.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByMonth = top10ByMonth.Result != null ? top10ByMonth.Result.ToList().Take(3).ToList() : new List<SaleTop10>()
            };
            return View(data);
        }
        public IActionResult History()
        {
            DateTime today = DateTime.Now;
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var top10ByWeek = _ratingSale.GetTop10Week(today, out week);
            var top10ByMonth = _ratingSale.GetTop10Month(today, out month);
            RankingSaleViewModel data = new RankingSaleViewModel()
            {
                Week = week,
                Month = month,
                LstSaleTop10ByDay = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByWeek = top10ByWeek.Result != null ? top10ByWeek.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByMonth = top10ByMonth.Result != null ? top10ByMonth.Result.ToList().Take(3).ToList() : new List<SaleTop10>()
            };
            return View(data);
        }
     
        public IActionResult SearchDay(string dateToday)
        {
            DateTime today;
            today = DateTime.ParseExact(dateToday, "dd-MM-yyyy", null);
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var top10ByWeek = _ratingSale.GetTop10Week(today, out week);
            var top10ByMonth = _ratingSale.GetTop10Month(today, out month);
            RankingSaleViewModel data = new RankingSaleViewModel()
            {
                Week = week,
                Month = month,
                LstSaleTop10ByDay = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByWeek = top10ByWeek.Result != null ? top10ByWeek.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByMonth = top10ByMonth.Result != null ? top10ByMonth.Result.ToList().Take(3).ToList() : new List<SaleTop10>()
            };
            return PartialView("~/Views/RankingSales/_data.cshtml", data);
        }
        public IActionResult SearchWeek(string dateToday)
        {
            DateTime today;
            today = DateTime.ParseExact(dateToday, "dd-MM-yyyy", null);
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var top10ByWeek = _ratingSale.GetTop10Week(today, out week);
            var top10ByMonth = _ratingSale.GetTop10Month(today, out month);
            RankingSaleViewModel data = new RankingSaleViewModel()
            {
                Week = week,
                Month = month,
                LstSaleTop10ByDay = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByWeek = top10ByWeek.Result != null ? top10ByWeek.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByMonth = top10ByMonth.Result != null ? top10ByMonth.Result.ToList().Take(3).ToList() : new List<SaleTop10>()
            };
            return PartialView("~/Views/RankingSales/_data_week.cshtml", data);
        }
        public IActionResult SearchMonth(string dateToday)
        {
            string newDateToday = string.Concat("01-", dateToday);
            DateTime today;
            today = DateTime.ParseExact(newDateToday, "dd-MM-yyyy", null);
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var top10ByWeek = _ratingSale.GetTop10Week(today, out week);
            var top10ByMonth = _ratingSale.GetTop10Month(today, out month);
            RankingSaleViewModel data = new RankingSaleViewModel()
            {
                Week = week,
                Month = month,
                LstSaleTop10ByDay = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByWeek = top10ByWeek.Result != null ? top10ByWeek.Result.ToList().Take(3).ToList() : new List<SaleTop10>(),
                LstSaleTop10ByMonth = top10ByMonth.Result != null ? top10ByMonth.Result.ToList().Take(3).ToList() : new List<SaleTop10>()
            };
            return PartialView("~/Views/RankingSales/_data_month.cshtml", data);

        }
        public IActionResult TopOfDay(string dateToday)
        {
            DateTime today;
            today = DateTime.ParseExact(dateToday, "dd-MM-yyyy", null);
            int week = 0;
            int month = 0;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            var data = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(1).SingleOrDefault() : new SaleTop10();
            if(data != null)
            {
                return Ok(new { status = true, data = data });
            }
            else
            {
                return Ok(new { status = false });
            }
        }
        public IActionResult TopOfYesterday(string dateToday)
        {
            DateTime today;
            today = DateTime.ParseExact(dateToday, "dd-MM-yyyy", null);
            var yesterDay = today.AddDays(-1);
            var top10ByDay = _ratingSale.GetTop10Day(yesterDay);
            var data = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(1).SingleOrDefault() : new SaleTop10();
            if (data != null)
            {
                return Ok(new { status = true, data = data });
            }
            else
            {
                return Ok(new { status = false });
            }
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult TestHistory()
        {
            return View();
        }
    }
}
