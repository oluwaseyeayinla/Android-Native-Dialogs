using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AndroidDialog : MonoBehaviour
{
    public AndroidNativeDialog.DialogTheme AndroidDialogTheme;


    string title = string.Empty;
    string message = string.Empty;
    IEnumerable<string> actions;

    public Action<string> OnAlertActionCompleted = delegate { };

    public static AndroidDialog CreateAlertDialog(string title, string message, IEnumerable<string> actions, AndroidNativeDialog.DialogTheme theme = AndroidNativeDialog.DialogTheme.ThemeTraditional)
    {
        AndroidDialog dialog = new GameObject("AndroidDialog").AddComponent<AndroidDialog>();
        dialog.title = title;
        dialog.message = message;
        dialog.actions = actions;
        dialog.AndroidDialogTheme = theme;

        return dialog;
    }

    //--------------------------------------
    //  PUBLIC METHODS
    //--------------------------------------

    public void ShowAlertDialog()
    {
        StringBuilder builder = new StringBuilder();
        IEnumerator<string> enumerator = actions.GetEnumerator();
        if (enumerator.MoveNext())
        {
            builder.Append(enumerator.Current);
        }

        while (enumerator.MoveNext())
        {
            builder.Append("|");
            builder.Append(enumerator.Current);
        }

        //AndroidNativePopup.ShowAlertPopup(title, message, builder.ToString(), MNP_PlatformSettings.Instance.AndroidDialogTheme);
        AndroidNativeDialog.ShowAlertDialog(title, message, builder.ToString().Split('|').ToArray(), AndroidDialogTheme);
    }

    public void OnAlertActionCallBack(string result)
    {
        Debug.Log("OnAlertActionCallBack Result: " + result);
        OnAlertActionCompleted(result);
        Destroy(gameObject);
    }



    int day = 1;
    int month = 0;
    int year = 1000;

    public Action<string> OnDatePickerSetCompleted = delegate { };

    public static AndroidDialog CreateDatePickerDialog(int day, int month, int year, AndroidNativeDialog.DialogTheme theme = AndroidNativeDialog.DialogTheme.ThemeTraditional)
    {
        AndroidDialog dialog = new GameObject("AndroidDialog").AddComponent<AndroidDialog>();
        dialog.day = Mathf.Clamp(day, 1, 31);
        dialog.month = Mathf.Clamp(month, 1, 12);
        dialog.year = Mathf.Clamp(year, 1000, 9000);
        dialog.AndroidDialogTheme = theme;

        return dialog;
    }

    public void ShowDatePickerDialog()
    {
        AndroidNativeDialog.ShowDatePicker(day, month, year, AndroidDialogTheme);
    }

    public void OnDateSetCallBack(string result)
    {
        Debug.Log("OnDateSetCallBack Result: " + result);
        OnDatePickerSetCompleted(result);
        Destroy(gameObject);
    }


    int hour = 1;
    int minute = 0;
    bool use24hrsFormat = false;

    public Action<string> OnTimePickerSetCompleted = delegate { };

    public static AndroidDialog CreateTimePickerDialog(int hour, int minute, bool use24hrsFormat, AndroidNativeDialog.DialogTheme theme = AndroidNativeDialog.DialogTheme.ThemeTraditional)
    {
        AndroidDialog dialog = new GameObject("AndroidDialog").AddComponent<AndroidDialog>();
        dialog.hour = Mathf.Clamp(hour, 0, 23);
        dialog.minute = Mathf.Clamp(minute, 0, 59);
        dialog.use24hrsFormat = use24hrsFormat;
        dialog.AndroidDialogTheme = theme;

        return dialog;
    }

    public void ShowTimePickerDialog()
    {
        AndroidNativeDialog.ShowTimePicker(hour, minute, use24hrsFormat, AndroidDialogTheme);
    }

    public void OnTimeSetCallBack(string result)
    {
        Debug.Log("OnTimeSetCallBack Result: " + result);
        OnTimePickerSetCompleted(result);
        Destroy(gameObject);
    }
}
