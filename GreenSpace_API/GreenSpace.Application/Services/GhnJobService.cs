using GreenSpace.Domain.Enum;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GreenSpace.Application.Services
{
    public class GhnJobService
    {
        private readonly ILogger<GhnJobService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;

        public GhnJobService(ILogger<GhnJobService> logger, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task FetchGhnOrder()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://dev-online-gateway.ghn.vn/");
            client.DefaultRequestHeaders.Add("Token", "3dd45171-07cc-11f0-9f28-eacfdef119b3"); 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var order = await _unitOfWork.OrderRepository.WhereAsync(x => x.DeliveryCode != null && x.Status != 2 && x.Status != 3 && x.Status >= 9);
            foreach (var item in order)
            {
                try
                {
                    var requestBody = new { order_code = item.DeliveryCode }; 
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/shiip/public-api/v2/shipping-order/detail", content);
                    var responseData = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseData);
                    var status = doc.RootElement.GetProperty("data").GetProperty("status").GetString();
                   
                    OrderProductStatus mappedStatus = status switch
                    {
                        "ready_to_pick" => OrderProductStatus.Processing,
                        "delivering" => OrderProductStatus.PickedPackageAndDelivery,
                        "delivery_fail" => OrderProductStatus.DeliveryFail,
                        "return" => OrderProductStatus.ReDelivery,
                        "delivered" => OrderProductStatus.DeliveredSuccessfully,
                        "cancel" => OrderProductStatus.Cancelled,
                        _ => throw new NotImplementedException("Cannot get status"),
                    };
                    if(item.Status != (int)mappedStatus)
                    {
                        item.Status = (int)mappedStatus;
                        _unitOfWork.OrderRepository.Update(item);
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching GHN order for order code: {OrderCode}", item.DeliveryCode);
                    continue;
                }
            }

            var orderService = await _unitOfWork.ServiceOrderRepository.WhereAsync(x => x.DeliveryCode != null && x.Status < 14 && x.Status > 8);
            foreach (var item in orderService)
            {
                try
                {
                    var requestBody = new { order_code = item.DeliveryCode };
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/shiip/public-api/v2/shipping-order/detail", content);
                    var responseData = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseData);
                    var status = doc.RootElement.GetProperty("data").GetProperty("status").GetString();

                    OrderProductStatus mappedStatus = status switch
                    {
                        "ready_to_pick" => OrderProductStatus.Processing,
                        "delivering" => OrderProductStatus.PickedPackageAndDelivery,
                        "delivery_fail" => OrderProductStatus.DeliveryFail,
                        "return" => OrderProductStatus.ReDelivery,
                        "delivered" => OrderProductStatus.DeliveredSuccessfully,
                        "cancel" => OrderProductStatus.Cancelled,
                        _ => throw new NotImplementedException("Cannot get status"),
                    };
                    if (item.Status != (int)mappedStatus)
                    {
                        item.Status = (int)mappedStatus;
                        _unitOfWork.ServiceOrderRepository.Update(item);
                    }

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching GHN order for order code: {OrderCode}", item.DeliveryCode);
                    continue;
                }
            }
            var complaint = await _unitOfWork.ComplaintRepository.WhereAsync(x => x.DeliveryCode != null && x.Status >6);
            foreach (var item in complaint)
            {
                try
                {
                    var requestBody = new { order_code = item.DeliveryCode };
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/shiip/public-api/v2/shipping-order/detail", content);
                    var responseData = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseData);
                    var status = doc.RootElement.GetProperty("data").GetProperty("status").GetString();

                    ComplaintStatusEnum mappedStatus = status switch
                    {
                        "delivering" => ComplaintStatusEnum.Delivery,
                        "delivered" => ComplaintStatusEnum.delivered,
                        _ => throw new NotImplementedException("Cannot get status"),
                    };
                    if (item.Status != (int)mappedStatus)
                    {
                        item.Status = (int)mappedStatus;
                        _unitOfWork.ComplaintRepository.Update(item);
                    }

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching GHN order for order code: {OrderCode}", item.DeliveryCode);
                    continue;
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
        
    }

}
