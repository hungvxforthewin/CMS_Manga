using CRMSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using System;

namespace CRMSite.Hubs
{
    public class ChatHub : Hub
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRatingSale _ratingSale;
        protected Token tokenModel = new Token();    
        public ChatHub(IHttpContextAccessor httpContextAccessor, IRatingSale ratingSale)
        {
            _httpContextAccessor = httpContextAccessor;
            _ratingSale = ratingSale;
            if (_httpContextAccessor.HttpContext.User.Claims.Any())
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                if (claims != null)
                {
                    tokenModel.Phone = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                }
            }
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task TopOfDay()
        {
            DateTime today = DateTime.Now; ;
            var top10ByDay = _ratingSale.GetTop10Day(today);
            bool flag = false;
            var data = new SaleTop10();
            if (!flag)
            {
                data = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(1).SingleOrDefault() : new SaleTop10();
                flag = true;
            }
            if (data != null)
            {
                await Clients.All.SendAsync("ReceiveWinner", data.FullName, data.Avatar, flag);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveWinner", "", "");
            }
        }
        public async Task TopOfYesterday()
        {
            DateTime today = DateTime.Now;
            var yesterDay = today.AddDays(-1);
            var top10ByDay = _ratingSale.GetTop10Day(yesterDay);
            bool flag = false;
            var data = new SaleTop10();
            if (!flag)
            {
                data = top10ByDay.Result != null ? top10ByDay.Result.ToList().Take(1).SingleOrDefault() : new SaleTop10();
                flag = true;
            }
            if (data != null)
            {
                await Clients.All.SendAsync("ReceiveWinner", data.FullName, data.Avatar, flag, data.DepartmentName);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveWinner", "", "");
            }
        }
    }
}
