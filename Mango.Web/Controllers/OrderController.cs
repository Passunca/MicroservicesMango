using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> OrderIndex(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto response = await _orderService.GetAllOrders(userId);

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));

                switch (status)
                {
                    case "approved":
                        list = list.Where(o => o.Status == StaticDetails.Status_Approved);
                            break;
                    case "readyforpickup":
                        list = list.Where(o => o.Status == StaticDetails.Status_ReadyForPickup);
                        break;
                    case "canceled":
                        list = list.Where(o => o.Status == StaticDetails.Status_Cancelled);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }

            return View(list);
        }

        public async Task<IActionResult> OrderDetailIndex(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = await _orderService.GetOrder(orderId);

            if (response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            }

            if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }

            return View(orderHeaderDto);
        }


        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_ReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfuly";
                return RedirectToAction(nameof(OrderDetailIndex), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> OrderComplete(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.StatusCompleted);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfuly";
                return RedirectToAction(nameof(OrderDetailIndex), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> OrderCancel(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_Cancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfuly";
                return RedirectToAction(nameof(OrderDetailIndex), new { orderId = orderId });
            }

            return View();
        }
    }
}
