using System;
using UnityEngine;

namespace Clock.Time
{
    [CreateAssetMenu(fileName = "ApiTime")]
    public class ApiTimeSO : ScriptableObject
    {
        [Serializable]
        public struct TimeApis
        {
            public string[] timeAPI;
        }

        [SerializeField]
        private TimeApis _timeAPIs;
        public TimeApis TimeApi => _timeAPIs;
    }
}