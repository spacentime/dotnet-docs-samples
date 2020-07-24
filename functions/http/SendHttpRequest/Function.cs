﻿// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START functions_concepts_requests]
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Invoker.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

// Register the Startup class to configure dependency injection
[assembly: FunctionsStartup(typeof(SendHttpRequest.Startup))]

namespace SendHttpRequest
{
    // Dependency injection configuration, executed during server startup.
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Make IHttpClientFactory available for dependency injection.
            // There are many options here; see
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
            // for more details.
            builder.Services.AddHttpClient();
        }
    }

    // Function
    public class Function : IHttpFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Function(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        public async Task HandleAsync(HttpContext context)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string url = "http://example.com";
            using (HttpResponseMessage clientResponse = await client.GetAsync(url))
            {
                await context.Response.WriteAsync($"Received code '{(int) clientResponse.StatusCode}' from URL '{url}'.");
            }
        }
    }
}
// [END functions_concepts_requests]
