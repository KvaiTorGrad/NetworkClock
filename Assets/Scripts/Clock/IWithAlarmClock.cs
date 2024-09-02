using System;
using TMPro;
using UnityEngine;

namespace Clock
{
    public interface IWithAlarmClock
    {
        public TextMeshProUGUI TimeTextAlarmClock { get; }
        public bool IsSetValueAlarmClock { get; set; }
        public void OnSelect(string value);
        public void DisplayAlarmTime(DateTime time);
    }

    public interface IHandOfAlarmClock : IWithAlarmClock
    {
        public RectTransform HourHand { get; }
        public RectTransform MinuteHand { get; }
        public RectTransform SecondHand { get; }
        public Time.Time ClockTime { get; }
        public void SetTimeArrows(DateTime currentTime);
        public void OnEndEdit();
    }

    public interface IInputTimeAlarmClock : IWithAlarmClock
    {
        public void OnValueChanged(string text);
        public void OnEndEdit(string text);

    }
}