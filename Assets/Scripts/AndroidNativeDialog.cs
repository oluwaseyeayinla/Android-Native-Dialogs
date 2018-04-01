using System;

public static class AndroidNativeDialog
{
    const string CLASS_NAME = "plugin.android.nativedialogs.DialogManager";

    static void RunOnUIThread(string methodName, params object[] args)
    {
        AndroidNativeUtils.CallStaticUsingUnityActivityOnUIThread(CLASS_NAME, methodName, args);
    }

    public static void ShowAlertDialog(string title, string message, string[] actions, DialogTheme theme)
    {
        RunOnUIThread("ShowAlertDialog", title, message, actions, (int)theme);
    }

    public static void ShowAlertDialog(string title, string message, string positive, DialogTheme theme)
    {
        RunOnUIThread("ShowAlertDialog", title, message, positive, (int)theme);
    }

    public static void ShowAlertDialog(string title, string message, string positive, string negative, DialogTheme theme)
    {
        RunOnUIThread("ShowAlertDialog", title, message, positive, negative, (int)theme);
    }

    public static void ShowAlertDialog(string title, string message, string positive, string neutral, string negative, DialogTheme theme)
    {
        RunOnUIThread("ShowAlertDialog", title, message, positive, neutral, negative, (int)theme);
    }

    public static void ShowDatePicker()
    {
        RunOnUIThread("ShowDatePickerDialog");
    }

    public static void ShowDatePicker(int day, int month, int year, DialogTheme theme)
    {
        RunOnUIThread("ShowDatePickerDialog", day, month, year, (int)theme);
    }

    public static void ShowPreloader(string title, string message, DialogTheme theme)
    {
        RunOnUIThread("ShowPreloaderDialog", title, message, (int)theme);
    }

    public static void DismissPreloader()
    {
        RunOnUIThread("DismissPreloaderDialog");
    }

    public static void ShowTimePicker()
    {
        RunOnUIThread("ShowTimePickerDialog");
    }

    public static void ShowTimePicker(int hour, int minute, bool use24HrsFormat, DialogTheme theme)
    {
        RunOnUIThread("ShowTimePickerDialog", hour, minute, use24HrsFormat, (int)theme);
    }

    public static void ShowToastDialog(string message)
    {
        RunOnUIThread("ShowToastDialog", message);
    }

    public static void ShowToastDialog(string message, int duration, int position, int xoffset, int yoffset)
    {
        RunOnUIThread("ShowToastDialog", message, duration, position, xoffset, yoffset);
    }

    public static void OpenUrl(string url)
    {
        RunOnUIThread("OpenUrl", url);
    }

    public enum ToastDuration
    { 
        Short = 0,
        Long = 1
    }

    [Flags]
    public enum ToastPosition
    {
        TOP = 48,
        BOTTOM = 80,
        LEFT = 3,
        RIGHT = 5,
        CENTER = 17,
        CENTER_HORIZONTAL = 1,
        CENTER_VERTICAL = 16,
    }

    public enum DialogTheme
    {
        ThemeDeviceDefaultDark,
        ThemeDeviceDefaultLight,
        ThemeHoloDark,
        ThemeHoloLight,
        ThemeTraditional
    }
}
