using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    [SerializeField] InputField _inputToast;
    [SerializeField] Text _dateLabel;
    [SerializeField] Text _timeLabel;

    struct DialogMessage
    {
        public string Title;
        public string Body;
    }

    public static bool IsPreloading
    {
        get { return isPreloading; }
    }

    static DialogMessage dialogMessage;
    static float hideDuration, hideStartTime;
    static bool isPreloading = false;

    void Start()
    {
        Input.backButtonLeavesApp = true;
    }

    void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            if (!string.IsNullOrEmpty(dialogMessage.Title) || !string.IsNullOrEmpty(dialogMessage.Body))
            {
                if (hideDuration > 0)
                {
                    DisplayPreloader(dialogMessage.Title, dialogMessage.Body, hideDuration);
                }
                else
                {
                    DisplayPreloader(dialogMessage.Title, dialogMessage.Body);
                }
            }
        }
        else
        {
            if (IsPreloading)
            {
                if (hideDuration > 1f)
                {
                    hideDuration = Time.unscaledTime - hideStartTime;
                }

                AndroidNativeDialog.DismissPreloader();
            }
        }
    }

    public void DisplayToast(string message)
    {
        AndroidNativeDialog.ShowToastDialog(message);
    }

    public void DisplayToast(string message, float duration)
    {
        DisplayToast(message, duration, (int)AndroidNativeDialog.ToastPosition.BOTTOM);
    }

    public void DisplayToast(string message, float duration, int position, int xOffset = 0, int yOffset = 0)
    {
        AndroidNativeDialog.ToastDuration toastDuration = duration <= 2 ? AndroidNativeDialog.ToastDuration.Short : AndroidNativeDialog.ToastDuration.Long;
        AndroidNativeDialog.ShowToastDialog(message, (int)toastDuration, position, xOffset, yOffset);
    }

    public void DisplayPreloader(string title)
    {
        DisplayPreloader(title, "");
    }

    public void DisplayPreloader(string title, string message, AndroidNativeDialog.DialogTheme theme = AndroidNativeDialog.DialogTheme.ThemeTraditional)
    {
        if (IsPreloading)
        {
            Dismiss();
        }

        AndroidNativeDialog.ShowPreloader(title, message, theme);
        isPreloading = true;
    }

    public void DisplayPreloader(string title, string message, float duration)
    {
        DisplayPreloader(title, message);
        hideStartTime = Time.unscaledTime; //Time.time
        Invoke("Dismiss", duration);
    }

    public void DisplayAlert(string title, string message, string buttonText, Action callback, Action dismissCallback = null)
    {
        if (IsPreloading)
        {
            Dismiss();
        }

        AndroidAlertDialog dialog = new AndroidAlertDialog(title, message);
        dialog.AddAction(AlertActionType.Positive, buttonText, () => { if (callback != null) callback(); });
        dialog.AddDismissListener(() => { if (dismissCallback != null) dismissCallback(); });
        dialog.Show();
    }

    public void DisplayAlert(string title, string message, string postiveButtonText, string negativeButtonText,
        Action positiveCallback, Action negativeCallback, Action dismissCallback = null)
    {
        if (IsPreloading)
        {
            Dismiss();
        }

        AndroidAlertDialog dialog = new AndroidAlertDialog(title, message);
        dialog.AddAction(AlertActionType.Positive, postiveButtonText, () => { if (positiveCallback != null) positiveCallback(); });
        dialog.AddAction(AlertActionType.Negative, negativeButtonText, () => { if (negativeCallback != null) negativeCallback(); });
        dialog.AddDismissListener(() => { if (dismissCallback != null) dismissCallback(); });
        dialog.Show();
    }

    public void DisplayAlert(string title, string message, string postiveButtonText, string neutralButtonText, string negativeButtonText,
         Action positiveCallback, Action neutralCallback, Action negativeCallback, Action dismissCallback = null)
    {
        if (IsPreloading)
        {
            Dismiss();
        }

        AndroidAlertDialog dialog = new AndroidAlertDialog(title, message);
        dialog.AddAction(AlertActionType.Positive, postiveButtonText, () => { if (positiveCallback != null) positiveCallback(); });
        dialog.AddAction(AlertActionType.Neutral, neutralButtonText, () => { if (neutralCallback != null) neutralCallback(); });
        dialog.AddAction(AlertActionType.Negative, negativeButtonText, () => { if (negativeCallback != null) negativeCallback(); });
        dialog.AddDismissListener(() => { if (dismissCallback != null) dismissCallback(); });
        dialog.Show();
    }

    public void DisplayDatePicker(DateTime date, Action<int, int, int> onSetCallback)
    {
        AndroidDatePickerDialog dialog = new AndroidDatePickerDialog(date.Day, date.Month, date.Year, onSetCallback);
        dialog.Show();
    }

    public void DisplayDatePicker(int day, int month, int year, Action<int, int, int> onSetCallback)
    {
        AndroidDatePickerDialog dialog = new AndroidDatePickerDialog(day, month, year, onSetCallback);
        dialog.Show();
    }

    public void DisplayTimePicker(int hour, int minute, bool use24hrsFormat, Action<int, int> onSetCallback)
    {
        AndroidTimePickerDialog dialog = new AndroidTimePickerDialog(hour, minute, use24hrsFormat, onSetCallback);
        dialog.Show();
    }

    public void Dismiss()
    {
        dialogMessage.Title = string.Empty;
        dialogMessage.Body = string.Empty;
        hideStartTime = 0;
        hideDuration = 0;
        isPreloading = false;

        AndroidNativeDialog.DismissPreloader();
    }

    public void InputToast()
    {
        DisplayToast(_inputToast.text);
    }

    public void DymanicToast()
    {
        AndroidNativeDialog.ToastPosition[] positionArrays = Enum.GetValues(typeof(AndroidNativeDialog.ToastPosition)).Cast<AndroidNativeDialog.ToastPosition>().ToArray();
        AndroidNativeDialog.ToastPosition position = positionArrays[UnityEngine.Random.Range(0, positionArrays.Length)]; // AndroidNativeDialog.ToastPosition.CENTER
        DisplayToast(_inputToast.text, 10f, (int)position);
    }

    public void ShowPreloaderFor3Secs()
    {
        DisplayPreloader("title", "message", 3f);
    }

    public void SingleAlert()
    {
        DisplayAlert("title", "message", "okay", null, null);
    }

    public void DoubleAlert()
    {
        DisplayAlert("title", "message", "yes", "no", null, null);
    }

    public void TripleAlert()
    {
        DisplayAlert("title", "message", "yes", "maybe", "no", null, null, null);
    }

    public void DatePicker()
    {
        DateTime date = DateTime.Now;
        DisplayDatePicker(date.Day, date.Month - 1, date.Year, OnDatePicked);
    }

    public void OnDatePicked(int day, int month, int year)
    { 
        _dateLabel.text = String.Format("{0}/{1}/{2}", day, month, year);
    }

    public void TimePicker()
    {
        DateTime time = DateTime.Now;
        DisplayTimePicker(time.Hour, time.Minute, false, OnTimePicked);
    }

    public void OnTimePicked(int hours, int minutes)
    {
        _timeLabel.text = String.Format("{0}:{1}", hours, minutes);
    }

}
