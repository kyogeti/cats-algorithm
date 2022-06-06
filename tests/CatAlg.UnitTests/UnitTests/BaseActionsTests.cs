using System;
using System.Net;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;

namespace CatAlg.UnitTests.UnitTests
{
    public class BaseActionsTests<T>
    {
        protected readonly Mock<ILogger<T>> LoggerMock;
        protected readonly Mock<IRestClient> RestClientMock;

        public BaseActionsTests()
        {
            LoggerMock = new Mock<ILogger<T>>();
            RestClientMock = new Mock<IRestClient>()
            {
                CallBase = true
            };
        }

        public void SetupOkRequest()
        {
            RestClientMock.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed
                });
        }
        
        public void AssertInformationLogCalls(int expectedCalls)
        {
            LoggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Exactly(expectedCalls));
        }
        
        public void AssertCriticalLogCalls(int expectedCalls)
        {
            LoggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Exactly(expectedCalls));
        }
        
        public void AssertWarningLogCalls(int expectedCalls)
        {
            LoggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Exactly(expectedCalls));
        }
    }
}