using System;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace CatAlg.UnitTests.IntegrationTests
{
    public class BaseIntegrationSetups
    {
        private WireMockServer _server;

        protected string GetServerUrl() => _server.Url;
        
        protected void SetupGetRequest<T>(string resourcePath, T responseBody, int statusCode)
        {
            _server
                .Given(Request.Create().WithPath(resourcePath).UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(statusCode)
                    .WithBody(JsonConvert.SerializeObject(responseBody)));
        }

        protected void StartServer()
        {
            _server = WireMockServer.Start();
        }

        protected void StopServer()
        {
            _server.Stop();
        }
    }
}