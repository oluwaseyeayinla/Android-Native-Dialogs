using System;
using System.Collections;
using UnityEngine;

public class AndroidTimePickerDialog
{
    public Action<int, int> onTimeSet = null;
    protected int hour = 1;
    protected int minute = 1;
    protected bool use24hrsFormat = false;

    public AndroidTimePickerDialog(int hour, int minute, bool use24hrsFormat, Action<int, int> callback)
    {
        this.hour = hour;
        this.minute = minute;
        this.use24hrsFormat = use24hrsFormat;
        onTimeSet = callback;
    }

    public void Show()
    {
        AndroidDialog dialog = AndroidDialog.CreateTimePickerDialog(hour, minute, use24hrsFormat);
        dialog.OnTimePickerSetCompleted += OnPopupCompleted;
        dialog.ShowTimePickerDialog();
    }

    public int HourOfDay
    {
        get
        {
            return hour;
        }
    }

    public int Minute
    {
        get
        {
            return minute;
        }
    }

    public bool Is24HourFormat
    {
        get
        {
            return use24hrsFormat;
        }
    }

    //--------------------------------------
    //  EVENTS
    //--------------------------------------

    void OnPopupCompleted(string result)
    {
        int selectedHour = HourOfDay;
        int selectedMinute = Minute;

        string[] values = result.Split(':');

        selectedHour = int.Parse(values[0]);
        selectedMinute = int.Parse(values[1]);

        if (onTimeSet != null)
        {
            onTimeSet.Invoke(selectedHour, selectedMinute);
        }
    }
}
