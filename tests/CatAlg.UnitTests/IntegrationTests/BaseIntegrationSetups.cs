using System;
using System.Net;
using AutoFixture;
using CatAlg.Domain.Actions;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;
using CatAlg.Domain.Repository;
using CatAlg.Domain.Services;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
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
        private readonly IWillSmithDatabase _database;
        private readonly Fixture _fixture;
        protected Mock<IHeadActions> HeadActionsMock;
        protected Mock<IPawsActions> PawsActionsMock;
        protected Mock<ICatRepository> CatRepository;
        private IProviderActions _providerActions;

        public BaseIntegrationSetups()
        {
            HeadActionsMock = new Mock<IHeadActions>();
            PawsActionsMock = new Mock<IPawsActions>();
            CatRepository = new Mock<ICatRepository>();
            _database = WillSmithDatabase.GetInMemoryDatabase();
            
            _fixture = new Fixture();
        }

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
        
        protected CatService BuildCatService()
        {
            _providerActions = new ProviderActions(new RestClient(GetServerUrl()));
            return new CatService("xpto", HeadActionsMock.Object, PawsActionsMock.Object, _providerActions, CatRepository.Object);
        }

        protected CatService BuildCatServiceWithDatabase()
        {
            _providerActions = new ProviderActions(new RestClient(GetServerUrl()));
            var catRepository = new CatRepository(_database);
            return new CatService("xpto", HeadActionsMock.Object, PawsActionsMock.Object, _providerActions, catRepository);
        }
        
        protected void SetupUnsatisfiedCatAndGoodProvider()
        {
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, true).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
            SetupGetRequest("/api/furniture",
                _fixture.Build<Furniture>().With(x=> x.QualityStatus, Quality.New).CreateMany(5), 
                200);
        }

        protected void SetupGoodProvider()
        {
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, false).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
        }

        protected void SetupSleepyProvider()
        {
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, true).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
        }
    }
}