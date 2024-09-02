using System;
using UnityEngine;

namespace Clock
{
    [CreateAssetMenu(fileName = "SettingArrow", menuName = "SettingClockSO/Arrow")]
    public class SettingClockSO : ScriptableObject
    {
        [Serializable]
        public struct ArrowClock
        {
            public string nameArrow;
            public float speedMove;
            public MouseTranslate mouseTranslate;
        }
        public enum MouseTranslate
        {
            X,
            Y
        }

        [SerializeField] private ArrowClock[] _arrowClocks;
        public ArrowClock[] ArrowClocks => _arrowClocks;
    }
}