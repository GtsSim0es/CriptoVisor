﻿using CryptoVisor.Application.Extensions;
using CryptoVisor.Application.Interfaces;
using CryptoVisor.Core.Entities;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json;

namespace CryptoVisor.Infrastructure.Adapters
{
	internal class CoinGeckoApi : ICryptoGetterApi
	{
		private readonly ConnectionStrings _connectionStrings;

		public CoinGeckoApi(ConnectionStrings connectionStrings)
		{
			_connectionStrings = connectionStrings;
		}

		public async Task<IEnumerable<OhlcCoinHistory>> GetOhclValuesList(int period, ECoinType eCoinType)
		{
			string coin = eCoinType.GetDisplayName();

            var jsonElement = await MakeRequest($"/coins/{coin}/ohlc?days={period}&vs_currency=usd");

			var ohclCoinHistoryList = ConvertJsonElementToOhclList(jsonElement, eCoinType);

			return ohclCoinHistoryList;
		}

		private async Task<JsonElement> MakeRequest(string endPoint)
		{
			try
			{
				var options = new RestClientOptions(_connectionStrings.CoinGeckoUrlApi + endPoint);
				var client = new RestClient(options);
				var request = new RestRequest("");

				request.AddHeader("accept", "application/json");
				request.AddHeader("x-cg-pro-api-key", _connectionStrings.CoinGeckoCredentials);

				return await client.GetAsync<JsonElement>(request);
			}
			catch
			{
				return new JsonElement();
			}
		}

		private IEnumerable<OhlcCoinHistory> ConvertJsonElementToOhclList(JsonElement jsonElement, ECoinType coinType)
		{
			List<OhlcCoinHistory> ohclCoinHistoryList = [];

			foreach (var ohclValue in jsonElement.EnumerateArray())
			{
				var ohclCoinHistory = new OhlcCoinHistory
				{
					CoinType = coinType,
					Date = DateTimeOffset.FromUnixTimeMilliseconds(ohclValue[0].GetInt64()).DateTime,
					Open = ohclValue[1].GetDouble(),
					High = ohclValue[2].GetDouble(),
					Low = ohclValue[3].GetDouble(),
					Close = ohclValue[4].GetDouble()
				};

				ohclCoinHistoryList.Add(ohclCoinHistory);
			}

			return ohclCoinHistoryList;
		}
	}
}
