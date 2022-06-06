using System;
using System.Net;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace CatAlg.Domain.Actions
{
    public class HeadActions : IHeadActions
    {
        private readonly ILogger<IHeadActions> _logger;
        private readonly IRestClient _restClient;

        public HeadActions(ILogger<IHeadActions> logger, IRestClient restClient)
        {
            _restClient = restClient;
            _logger = logger;
        }
        public virtual void DoEat(Food food)
        {
            if (food.IsYummy)
            {
                _logger.LogInformation("mmm...");
                return;
            }
            
            _logger.LogCritical("MEOWWW");
        }

        public virtual void DoMeow(DateTime inconvenientHour)
        {
            if (inconvenientHour.Hour >= 3 && inconvenientHour.Hour <= 6)
            {
                _logger.LogCritical("MEOWWW");
            }
            
            _logger.LogInformation("meow :3");
        }

        public virtual void DoKillerLookWhileYouSleep(Guid providerId)
        {
            var request = new RestRequest($"/provider/{providerId}/notify-killing-intentions", Method.POST);
            var result = _restClient.Execute(request);
            
            if(result.StatusCode == HttpStatusCode.OK)
                _logger.LogWarning("You better run, human.");
        }
    }
}