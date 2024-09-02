using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Clock
{
    public class HandSelector : Selectable, IDragHandler, IPointerDownHandler
    {
        private enum HandType
        {
            Hour,
            Minute,
            Second
        }

        private SettingClockSO _settingClock;
        private Quaternion _initialRotation;
        private Vector2 _initialMousePosition;
        private IHandOfAlarmClock _handOfClock;
        [SerializeField] private HandType _handType;
        private int _indexHand;

        protected override void Awake()
        {
            base.Awake();
            interactable = false;
            _handOfClock = GetComponentInParent<IHandOfAlarmClock>();
            if (_handOfClock == null) return;
            interactable = true;
            _indexHand = (int)_handType;
            _settingClock = (SettingClockSO)Resources.Load("SettingArrow");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (_handOfClock == null) return;
            _initialMousePosition = eventData.position;
            _initialRotation = transform.localRotation;
            _handOfClock.OnSelect(string.Empty);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (_handOfClock == null) return;
            _handOfClock.OnEndEdit();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_handOfClock == null) return;
            Vector2 mouseDelta = eventData.position - _initialMousePosition;
            float rotationAmount = 0;
            switch (_settingClock.ArrowClocks[_indexHand].mouseTranslate)
            {
                case SettingClockSO.MouseTranslate.X:
                    rotationAmount = mouseDelta.x * _settingClock.ArrowClocks[_indexHand].speedMove;
                    break;
                case SettingClockSO.MouseTranslate.Y:
                    rotationAmount = mouseDelta.y * _settingClock.ArrowClocks[_indexHand].speedMove;
                    break;
            }
            transform.localRotation = _initialRotation * Quaternion.Euler(0f, 0f, -rotationAmount);
            UpdateClockManager();
        }

        private void UpdateClockManager()
        {
            float hourAngle = _handOfClock.HourHand.eulerAngles.z;
            float minuteAngle = _handOfClock.MinuteHand.eulerAngles.z;
            float secondAngle = _handOfClock.SecondHand.eulerAngles.z;

            int hours = (int)((360 - hourAngle) / 15f) % 24;
            int minutes = (int)((360 - minuteAngle) / 6f) % 60;
            int seconds = (int)((360 - secondAngle) / 6f) % 60;
            var alarmClockTime = _handOfClock.ClockTime.alarmClockTime;
            switch (_handType)
            {
                case HandType.Hour:
                    _handOfClock.ClockTime.alarmClockTime = new DateTime(alarmClockTime.Year, alarmClockTime.Month, alarmClockTime.Day,
                        hours, alarmClockTime.Minute, alarmClockTime.Second);
                    break;
                case HandType.Minute:
                    _handOfClock.ClockTime.alarmClockTime = new DateTime(alarmClockTime.Year, alarmClockTime.Month, alarmClockTime.Day,
                        alarmClockTime.Hour, minutes, alarmClockTime.Second);
                    break;
                case HandType.Second:
                    _handOfClock.ClockTime.alarmClockTime = new DateTime(alarmClockTime.Year, alarmClockTime.Month, alarmClockTime.Day,
                        alarmClockTime.Hour, alarmClockTime.Minute, seconds);
                    break;
            }
            _handOfClock.SetTimeArrows(_handOfClock.ClockTime.alarmClockTime);
        }
    }
}