using System;
using TMPro;
using UnityEngine;

namespace Clock
{
    public class AlarmClock : Clock, IHandOfAlarmClock, IInputTimeAlarmClock
    {
        [SerializeField] private TextMeshProUGUI _timeTextAlarmClock;
        private bool _isSetValueAlarmClock;

        public TextMeshProUGUI TimeTextAlarmClock => _timeTextAlarmClock;
        public bool IsSetValueAlarmClock { get => _isSetValueAlarmClock; set => _isSetValueAlarmClock = value; }
        public RectTransform HourHand => _hourHand;
        public RectTransform MinuteHand => _minuteHand;
        public RectTransform SecondHand => _secondHand;
        public Time.Time ClockTime => _clockTime;

        protected override void Awake()
        {
            base.Awake();
            _clockTime.SetPresentAlamInClock(true);
            _timeTextAndInput.interactable = true;
            _timeTextAndInput.onSelect.AddListener(OnSelect);
            _timeTextAndInput.onValueChanged.AddListener(OnValueChanged);
            _timeTextAndInput.onEndEdit.AddListener(OnEndEdit);
        }

        public void OnSelect(string value)
        {
            IsSetValueAlarmClock = true;
            _isActiveUpdateTimeClock = false;
        }

        public void DisplayAlarmTime(DateTime time) => TimeTextAlarmClock.text = time.ToString("HH:mm:ss");

        #region InputTimeAlarmClock

        public void OnValueChanged(string text)
        {
            string formattedText = FormatInput(text);
            _timeTextAndInput.text = formattedText;
            _timeTextAndInput.caretPosition = formattedText.Length;
            if (DateTime.TryParseExact(text, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                RotateHandOfClock(parsedTime);
        }

        private string FormatInput(string input)
        {
            input = input.Replace(":", "");
            if (input.Length > 6) input = input[..6];

            string formattedInput = "";
            if (input.Length > 0) formattedInput += input[..Mathf.Min(2, input.Length)];
            if (input.Length > 2) formattedInput += ":" + input.Substring(2, Mathf.Min(2, input.Length - 2));
            if (input.Length > 4) formattedInput += ":" + input.Substring(4, Mathf.Min(2, input.Length - 4));

            return formattedInput;
        }

        public void OnEndEdit(string text)
        {
            if (DateTime.TryParseExact(text, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            {
                ClockTime.alarmClockTime = parsedTime;
                DisplayAlarmTime(ClockTime.alarmClockTime);
                OnEndEdit();
            }
        }

        #endregion InputTimeAlarmClock

        #region HandOfAlarmClock

        public void OnEndEdit()
        {
            IsSetValueAlarmClock = false;
            _isActiveUpdateTimeClock = true;
        }

        public void SetTimeArrows(DateTime currentTime)
        {
            RotateHandOfClock(currentTime);
        }

        protected override void RotateHandOfClock(DateTime currentTime)
        {
            if (IsSetValueAlarmClock)
            {
                base.RotateHandOfClock(currentTime);
                DisplayAlarmTime(ClockTime.alarmClockTime);
            }
            else
                base.RotateHandOfClock(currentTime);
        }

        #endregion HandOfAlarmClock

    }
}