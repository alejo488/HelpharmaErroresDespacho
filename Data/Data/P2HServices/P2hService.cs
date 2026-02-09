using Constant.P2hConstant;
using Data.Interfaces;
using Models.Models.P2h;
using Models.ModelsDto.Payload;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Data.P2HServices
{
    public class P2hService : IP2hService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public P2hService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Configurar BaseAddress y headers en un solo lugar
            ConfigureHttpClient();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        public async Task<P2hGetResponse> GetDespachoAsync(string autorizacion)
        {
            try
            {
                // Usar UriBuilder para construir URL correctamente
                var url = $"/api/dispatch/get?autorizacion={Uri.EscapeDataString(autorizacion)}";


                using var response = await _httpClient.GetAsync(url);

                // Verificar si la respuesta es exitosa
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(
                        $"Error en la API: {response.StatusCode}. Detalles: {errorContent}");
                }

                // Leer contenido como string primero para debugging
                var json = await response.Content.ReadAsStringAsync();

                // Verificar si la respuesta contiene datos
                if (string.IsNullOrWhiteSpace(json) || json == "[]" || json == "{}")
                {
                    return default;
                }


                // Deserializar
                var obj = JsonSerializer.Deserialize<P2hGetResponse>(json, _jsonOptions);
                return obj;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<P2hGetResponse?> SendDespachoAsync(PayloadRoot payload)
        {

            try
            {
                var json = JsonSerializer.Serialize(payload, _jsonOptions);

                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await _httpClient.PostAsync("/api/dispatch/send", content);

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<P2hGetResponse>(responseJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public async Task<P2hGetResponse?> GetDispatchAsync(string autorizacion)
        {
            try
            {
                var requestUrl =
                    $"api/dispatch/get?autorizacion={Uri.EscapeDataString(autorizacion)}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                    return default;

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<P2hGetResponse>(json, _jsonOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<P2hGetResponse?> GetOrdersAsync(
        string identification,
        string identificationType,
        string status,
        string? authorization = null)
        {
            var query = new List<string>
            {
                $"identification={Uri.EscapeDataString(identification)}",
                $"identificationType={Uri.EscapeDataString(identificationType)}",
                $"status={Uri.EscapeDataString(status)}"
            };

            if (!string.IsNullOrWhiteSpace(authorization))
                query.Add($"authorization={Uri.EscapeDataString(authorization)}");

            var url = $"/api/scgo/v1/orders?{string.Join("&", query)}";

            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<P2hGetResponse>(json, _jsonOptions);
        }


        private void ConfigureHttpClient()
        {
            // Configurar BaseAddress
            _httpClient.BaseAddress = new Uri(P2hConstants.BaseUrl);

            // Limpiar headers existentes
            _httpClient.DefaultRequestHeaders.Clear();

            // Agregar headers de autenticación
            _httpClient.DefaultRequestHeaders.Add("nit", P2hConstants.Nit);
            _httpClient.DefaultRequestHeaders.Add("password", P2hConstants.Password);

            // Agregar Accept header para JSON
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Opcional: agregar User-Agent
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ErrorDespacho/1.0");
        }
    }

}
