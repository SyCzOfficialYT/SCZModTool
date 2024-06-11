using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SCZModTool
{
	public class MitmInterceptor
	{
		private string baseUrl;
		private List<int> numberPositions;
		private List<string> languages;

		public MitmInterceptor(string baseUrl, List<int> numberPositions, List<string> languages)
		{
			this.baseUrl = baseUrl;
			this.numberPositions = numberPositions;
			this.languages = languages;
		}

		public async void Start()
		{
			var currentValues = new List<int>(new int[numberPositions.Count]);
			do
			{
				foreach (var lang in languages)
				{
					string url = GenerateUrl(baseUrl, currentValues, lang);
					var (success, jsonResponse) = await FetchJson(url);
					if (success)
					{
						ParseJson(jsonResponse);
					}
				}
				int i = 0;
				while (i < currentValues.Count && ++currentValues[i] == 10)
				{
					currentValues[i] = 0;
					i++;
				}
			} while (!currentValues.TrueForAll(v => v == 0));
		}

		private string GenerateUrl(string baseStr, List<int> values, string language)
		{
			string url = baseStr;
			int pos = 0;
			foreach (int value in values)
			{
				pos = url.IndexOf("{x}", pos);
				if (pos != -1)
				{
					url = url.Remove(pos, 3).Insert(pos, value.ToString());
				}
			}
			pos = url.IndexOf("{xx}");
			if (pos != -1)
			{
				url = url.Remove(pos, 4).Insert(pos, language);
			}
			return url;
		}

		private async Task<(bool, JObject)> FetchJson(string url)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					var response = await client.GetStringAsync(url);
					JObject jsonResponse = JObject.Parse(response);
					return (true, jsonResponse);
				}
				catch (Exception e)
				{
					Console.WriteLine("Error fetching JSON: " + e.Message);
					return (false, null);
				}
			}
		}

		private void ParseJson(JObject jsonResponse)
		{
			// Parse and extract necessary information from the JSON response
			// This function should be adapted based on the structure of the JSON data
			Console.WriteLine(jsonResponse.ToString());
		}
	}
}
