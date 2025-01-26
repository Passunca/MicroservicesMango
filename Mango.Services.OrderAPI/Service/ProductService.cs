using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.OrderAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var request = await client.GetAsync($"/api/product");
            var apiContent = await request.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
            }

            return new List<ProductDto>();
        }
    }
}
