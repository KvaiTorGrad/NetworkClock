using System;
using TMPro;
using UnityEngine;

namespace Clock
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] protected RectTransform _secondHand;
        [SerializeField] protected RectTransform _minuteHand;
        [SerializeField] protected RectTransform _hourHand;
        protected TMP_InputField _timeTextAndInput;
        protected Time.Time _clockTime;
        protected bool _isActiveUpdateTimeClock;

        protected virtual void Awake()
        {
            _timeTextAndInput = GetComponentInChildren<TMP_InputField>();
            _isActiveUpdateTimeClock = true;
            _timeTextAndInput.interactable = false;
            _clockTime = new();
            _clockTime.SetPresentAlamInClock(false);
            _clockTime.UpdateTime += UpdateClock;
        }

        protected virtual void UpdateClock(DateTime currentTime)
        {
            if (!_isActiveUpdateTimeClock) return;
            RotateHandOfClock(currentTime);
            SetUIClockText(currentTime);
        }

        protected virtual void RotateHandOfClock(DateTime currentTime)
        {
            _hourHand.localRotation = Quaternion.Euler(0f, 0f, -(currentTime.Hour % 24) * 30f);
            _minuteHand.localRotation = Quaternion.Euler(0f, 0f, -(currentTime.Minute) * 6f);
            _secondHand.localRotation = Quaternion.Euler(0f, 0f, -currentTime.Second * 6f);
        }

        private void SetUIClockText(DateTime currentTime) => _timeTextAndInput.text = currentTime.ToString("HH:mm:ss");

        private void OnDestroy()
        {
            _clockTime.UpdateTime -= UpdateClock;
        }
    }
}