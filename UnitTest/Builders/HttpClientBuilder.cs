using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders.Base;

namespace UnitTest.Builders
{
    public class HttpClientBuilder: BuilderBaseClass<HttpClient>
    {
        public HttpClientBuilder()
        {
            _class = new HttpClient
            {
                BaseAddress = new Uri("https://api.octopus.energy/v1/"),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }
        public HttpClientBuilder WithBaseAddress(string baseAddress)
        {
            _class.BaseAddress = new Uri(baseAddress);
            return this;
        }
        public HttpClientBuilder WithDefaultHeader(string name, string value)
        {
            _class.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        public HttpClientBuilder WithHttpMessageHandler(HttpMessageHandler handler)
        {
            _class = new HttpClient(handler)
            {
                BaseAddress = _class.BaseAddress
            };
            return this;
        }
    }
}
