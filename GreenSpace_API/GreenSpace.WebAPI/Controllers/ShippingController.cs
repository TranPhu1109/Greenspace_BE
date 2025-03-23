using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GreenSpace.Application.ViewModels._3PartyShip;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class ShippingController : ControllerBase
{
    private readonly ShippingService _shippingService;

    public ShippingController(ShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    /// <summary>
    /// Tạo đơn hàng
    /// </summary>
    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] ShippingOrderRequest request)
    {
        var result = await _shippingService.CreateOrderAsync(request);

        var formattedResponse = new
        {
            message = "Đơn hàng đã được tạo thành công",
            data = JsonSerializer.Deserialize<object>(result.ToString()) 
        };

        return Content(JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true }), "application/json");
    }



    /// <summary>
    /// API lấy mã tỉnh/thành phố
    /// </summary>
    [HttpGet("province-id")]
    public async Task<IActionResult> GetProvinceId([FromQuery] string provinceName)
    {
        var provinceId = await _shippingService.GetProvinceIdAsync(provinceName);
        if (provinceId == null)
            return NotFound("Không tìm thấy mã tỉnh.");

        return Ok(new { provinceId });
    }

    /// <summary>
    /// API lấy mã quận/huyện theo tỉnh
    /// </summary>
    [HttpGet("district-id")]
    public async Task<IActionResult> GetDistrictId([FromQuery] int provinceId, [FromQuery] string districtName)
    {
        var districtId = await _shippingService.GetDistrictIdAsync(provinceId, districtName);
        if (districtId == null)
            return NotFound("Không tìm thấy mã quận/huyện.");

        return Ok(new { districtId });
    }

    /// <summary>
    /// API lấy mã phường/xã theo quận/huyện
    /// </summary>
    [HttpGet("ward-code")]
    public async Task<IActionResult> GetWardCode([FromQuery] int districtId, [FromQuery] string wardName)
    {
        var wardCode = await _shippingService.GetWardCodeAsync(districtId, wardName);
        if (wardCode == null)
            return NotFound("Không tìm thấy mã phường/xã.");

        return Ok(new { wardCode });
    }

    /// <summary>
    /// Tính phí vận chuyển
    /// </summary>
    [HttpPost("calculate-fee")]
    public async Task<IActionResult> CalculateFee([FromBody] ShippingFeeRequest request)
    {
        var result = await _shippingService.CalculateFeeAsync(request.ToProvinceName, request.ToDistrictName, request.ToWardName);
        var formattedResponse = new
        {
            message = "Đơn hàng đã được hủy",

            data = JsonSerializer.Deserialize<object>(result.ToString())
        };
        return Content(JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true }), "application/json");
    }

    /// <summary>
    /// Hủy đơn hàng
    /// </summary>
    [HttpPost("cancel-order")]
    public async Task<IActionResult> CancelOrder([FromBody] string[] orderCodes)
    {
        var result = await _shippingService.CancelOrderAsync(orderCodes);

        var formattedResponse = new
        {
            message = "Đơn hàng đã được hủy",
          
            data = JsonSerializer.Deserialize<object>(result.ToString())
        };
        return Content(JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true }), "application/json");
    }




    /// <summary>
    /// Trả hàng
    /// </summary>
    [HttpPost("return-order")]
    public async Task<IActionResult> ReturnOrder([FromBody] ReturnOrderRequest request)
    {

        var result = await _shippingService.ReturnOrderAsync(request.OrderCodes, request.Reason);

 
        var formattedResponse = new
        {
            message = "Đơn hàng đã được trả lại",
            data = JsonSerializer.Deserialize<object>(result.ToString())
        };

        return Content(JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true }), "application/json");
    }

    /// <summary>
    /// Theo dõi đơn hàng
    /// </summary>
    [HttpGet("track-order/{orderCode}")]
    public async Task<IActionResult> TrackOrder(string orderCode)
    {
        var result = await _shippingService.TrackOrderAsync(orderCode);

        var formattedResponse = new
        {
            message = "Thông tin đơn hàng",
            data = JsonSerializer.Deserialize<object>(result.ToString())
        };

        return Content(JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true }), "application/json");
    }

}
