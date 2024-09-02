using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Clock.Time
{
    public struct TimeSynchronizer
    {
        private ApiTimeSO _apiTimeSO;

        public void StartSyncTime()
        {
            _apiTimeSO = (ApiTimeSO)Resources.Load("ApiTime");
        }

        public readonly async Task<DateTime> SynchronizeTime()
        {
            await Task.Yield();
            var timeAPI = _apiTimeSO.TimeApi.timeAPI;
            using HttpClient httpClient = new();
            Task<DateTime>[] tasks = new Task<DateTime>[timeAPI.Length];

            for (int i = 0; i < timeAPI.Length; i++)
            {
                int index = i;
                tasks[i] = FetchTimeFromApiAsync(httpClient, timeAPI[index]);
            }

            DateTime[] times = await Task.WhenAll(tasks);

            return new DateTime(Convert.ToInt64(times.Average(time => time.Ticks)));
        }

        private readonly async Task<DateTime> FetchTimeFromApiAsync(HttpClient httpClient, string url)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResult = await response.Content.ReadAsStringAsync();
                DeserializationTimeJson timeData = JsonUtility.FromJson<DeserializationTimeJson>(jsonResult);
                if (timeData != null)
                    return DateTime.Parse(timeData.datetime);

                throw new Exception("Неизвестный формат ответа API");
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка при получении времени с сервера: " + ex.Message);
                return DateTime.MinValue;
            }
        }
    }

    [Serializable]
    public class DeserializationTimeJson
    {
        public string datetime;
    }
}