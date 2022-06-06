using System;
using System.Collections.Generic;
using System.Net;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;
using Newtonsoft.Json;
using RestSharp;

namespace CatAlg.Domain.Actions
{
    public class ProviderActions : IProviderActions
    {
        private readonly IRestClient _restClient;

        public ProviderActions(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public virtual Food ProvideFood() => Get<Food>("/api/food");

        public virtual SleepStatus GetSleepStatus() => Get<SleepStatus>("/api/sleep-status");

        public virtual IEnumerable<Furniture> ProvideFurniture() => Get<IEnumerable<Furniture>>("/api/furniture");

        private T Get<T>(string resource)
        {
            var request = new RestRequest(resource, Method.GET);
            var response = _restClient.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(response.Content);

            throw new ProviderNotRespondingException();
        }

        public class ProviderNotRespondingException : Exception
        {
        }
    }
}