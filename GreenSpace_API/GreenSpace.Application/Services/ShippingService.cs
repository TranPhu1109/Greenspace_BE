using Azure.Core;
using GreenSpace.Application;
using GreenSpace.Application.ViewModels._3PartyShip;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class ShippingService
{
    private readonly HttpClient _httpClient;
    private readonly GhnSettings _ghnSettings;

    public ShippingService(HttpClient httpClient, IOptions<GhnSettings> ghnSettings)
    {
        _httpClient = httpClient;
        _ghnSettings = ghnSettings.Value;
    }

    /// <summary>
    /// Tạo đơn hàng
    /// </summary>
    public async Task<string> CreateOrderAsync(ShippingOrderRequest request)
    {
        ////  Lấy mã tỉnh/quận/phường của NGƯỜI NHẬN
        //var toProvinceId = await GetProvinceIdAsync(request.ToProvince);
        //if (toProvinceId == null) return "Không tìm thấy mã tỉnh người nhận.";

        //var toDistrictId = await GetDistrictIdAsync(toProvinceId.Value, request.ToDistrict);
        //if (toDistrictId == null) return "Không tìm thấy mã quận/huyện người nhận.";

        //var toWardCode = await GetWardCodeAsync(toDistrictId.Value, request.ToWard);
        //if (toWardCode == null) return "Không tìm thấy mã phường/xã người nhận.";

        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create";

        var data = new
        {
            payment_type_id = 1,
            note = "Giao hàng nhanh",
            required_note = "CHOXEMHANGKHONGTHU",

            //  Thông tin NGƯỜI TRẢ HÀNG (bắt buộc)
            return_phone = "0987654321",
            return_address = "7 D1, Long Thạnh Mỹ, Thủ Đức, Hồ Chí Minh",
            return_district_id = "1451",
            return_ward_code = "20904",

            //  Mã đơn hàng của khách
            client_order_code = Guid.NewGuid().ToString(),

            //  Thông tin NGƯỜI GỬI (SET CỨNG)
            from_address = "7 D1, Long Thạnh Mỹ, Thủ Đức, Hồ Chí Minh",
            from_ward_code = "20904",
            from_district_id = "1451",
            from_province_id = "202",

            //  Thông tin NGƯỜI NHẬN
            to_name = request.ToName,
            to_phone = request.ToPhone,
            to_address = request.ToAddress,
            to_ward_code = request.ToWard,
            to_district_id = request.ToDistrict,
            to_province_id = request.ToProvince,

            //  Dịch vụ GHN
            service_id = 53320,
            service_type_id = 2,
            weight = 500,
            //  Tiền thu hộ (COD) và bảo hiểm
            cod_amount = 0,
            insurance_value = 0,

            //  Danh sách sản phẩm
            items = request.Items.Select(item => new
            {
                name = item.Name,
                code = item.Code,
                quantity = item.Quantity,
                width = 10,
                height = 10,
             
            }).ToList()
              
        };
        Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        Console.WriteLine($"ShopId: {_ghnSettings.ShopId}, Token: {_ghnSettings.Token}");


        return await SendPostRequest(apiUrl, data);
    }




    /// <summary>
    /// Gửi request GET
    /// </summary>
    private async Task<string> SendGetRequest(string url)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ShopId", _ghnSettings.ShopId);
            _httpClient.DefaultRequestHeaders.Add("Token", _ghnSettings.Token);

            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }
        catch (Exception ex)
        {
            return $"Lỗi: {ex.Message}";
        }
    }
    ///// <summary>
    ///// Lấy danh sách tỉnh/thành phố
    ///// </summary>
    //public async Task<int?> GetProvinceIdAsync(string provinceName)
    //{
    //    var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province";
    //    var response = await SendGetRequest(apiUrl);
    //    if (response == null) return null;

    //    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
    //    foreach (var province in jsonResponse.data)
    //    {
    //        if (province.ProvinceName.ToString().ToLower() == provinceName.ToLower())
    //        {
    //            return (int)province.ProvinceID;
    //        }
    //    }
    //    return null;
    //}

    ///// <summary>
    ///// Lấy danh sách quận/huyện theo mã tỉnh
    ///// </summary>
    //public async Task<int?> GetDistrictIdAsync(int provinceId, string districtName)
    //{
    //    var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district";
    //    var data = new { province_id = provinceId };
    //    var response = await SendPostRequest(apiUrl, data);
    //    if (response == null) return null;

    //    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
    //    foreach (var district in jsonResponse.data)
    //    {
    //        if (district.DistrictName.ToString().ToLower() == districtName.ToLower())
    //        {
    //            return (int)district.DistrictID;
    //        }
    //    }
    //    return null;
    //}

    ///// <summary>
    ///// Lấy danh sách phường/xã theo mã quận/huyện
    ///// </summary>
    //public async Task<string> GetWardCodeAsync(int districtId, string wardName)
    //{
    //    var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward";
    //    var data = new { district_id = districtId };
    //    var response = await SendPostRequest(apiUrl, data);
    //    if (response == null) return null;

    //    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
    //    foreach (var ward in jsonResponse.data)
    //    {
    //        if (ward.WardName.ToString().ToLower() == wardName.ToLower())
    //        {
    //            return ward.WardCode.ToString();
    //        }
    //    }
    //    return null;
    //}

    /// <summary>
    /// Tính phí vận chuyển
    /// </summary>
    public async Task<string> CalculateFeeAsync(int toProvinceName, int toDistrictName, string toWardName)
    {  //  Lấy mã tỉnh/quận/phường của NGƯỜI NHẬN
        //var toProvinceId = await GetProvinceIdAsync(toProvinceName);
        //if (toProvinceId == null) return "Không tìm thấy mã tỉnh người nhận.";

        //var toDistrictId = await GetDistrictIdAsync(toProvinceId.Value, toDistrictName);
        //if (toDistrictId == null) return "Không tìm thấy mã quận/huyện người nhận.";

        //var toWardCode = await GetWardCodeAsync(toDistrictId.Value, toWardName);
        //if (toWardCode == null) return "Không tìm thấy mã phường/xã người nhận.";

        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";

        var data = new
        {
            from_district_id = 1451,
            from_ward_code = "20901",
            from_province_id = 202,
            service_id = 53321,
            service_type_id = 2,
            to_ward_code = toWardName,
            to_district_id = toDistrictName,
            to_province_id = toProvinceName,
            weight = 500,
            height = 10,
            length = 20,
            width = 10,
            items = new[] {
            new {
                name = "TEST1",
                quantity = 1,
                height = 200,
                weight = 1000,
                length = 200,
                width = 200
            }
        },
            insurance_value = 0,
            cod_failed_amount = 0,
            coupon = (string)null
        };

        return await SendPostRequest(apiUrl, data);
    }




    /// <summary>
    /// Hủy đơn hàng
    /// </summary>
    public async Task<string> CancelOrderAsync(string[] orderCodes)
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/switch-status/cancel";

        var data = new { order_codes = orderCodes };  

        return await SendPostRequest(apiUrl, data);
    }


    /// <summary>
    /// Trả hàng
    /// </summary>
    public async Task<string> ReturnOrderAsync(string[] orderCodes, string reason)
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/switch-status/return";

        var data = new { order_codes = orderCodes, reason };

        return await SendPostRequest(apiUrl, data);
    }

    /// <summary>
    /// Theo dõi đơn hàng
    /// </summary>
    public async Task<string> TrackOrderAsync(string orderCode)
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail";

        var data = new { order_code = orderCode };

        return await SendPostRequest(apiUrl, data);
    }

    /// <summary>
    /// Hàm dùng chung để gửi POST request
    /// </summary>
    private async Task<string> SendPostRequest(string url, object data)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Token", _ghnSettings.Token);
            client.DefaultRequestHeaders.Add("ShopId", _ghnSettings.ShopId);

            var json = JsonConvert.SerializeObject(data);
            var utf8WithBom = new UTF8Encoding(true); 
            var content = new StringContent(json, utf8WithBom, "application/json");

            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }

    public async Task<List<(int ProvinceID, string ProvinceName)>> GetProvincesAsync()
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province";
        var response = await SendGetRequest(apiUrl);
        if (response == null) return new List<(int, string)>();

        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
        var provinces = new List<(int, string)>();

        foreach (var province in jsonResponse.data)
        {
            provinces.Add(((int)province.ProvinceID, province.ProvinceName.ToString()));
        }

        return provinces;
    }


    /// <summary>
    /// Lấy toàn bộ danh sách quận/huyện trên cả nước
    /// </summary>
    /// <summary>
    /// Lấy danh sách quận/huyện theo mã tỉnh
    /// </summary>
    public async Task<List<(int DistrictID, string DistrictName)>> GetDistrictsAsync(int provinceId)
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district";
        var data = new { province_id = provinceId };
        var response = await SendPostRequest(apiUrl, data);
        if (response == null) return new List<(int, string)>();

        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
        var districts = new List<(int, string)>();

        foreach (var district in jsonResponse.data)
        {
            districts.Add(((int)district.DistrictID, district.DistrictName.ToString()));
        }

        return districts;
    }


    /// <summary>
    /// Lấy danh sách phường/xã theo mã quận/huyện
    /// </summary>
    public async Task<List<(string WardCode, string WardName)>> GetWardsAsync(int districtId)
    {
        var apiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id";
        var data = new { district_id = districtId };
        var response = await SendPostRequest(apiUrl, data);
        if (response == null) return new List<(string, string)>();

        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
        var wards = new List<(string, string)>();

        foreach (var ward in jsonResponse.data)
        {
            wards.Add((ward.WardCode.ToString(), ward.WardName.ToString()));
        }

        return wards;
    }
  

}
