using System;

public class AndroidDatePickerDialog
{
    public Action<int, int, int> onDateSet = null;
    protected int day = 1;
    protected int month = 1;
    protected int year = 1000;

    public AndroidDatePickerDialog(int day, int month, int year, Action<int, int, int> callback)
    {
        this.day = day;
        this.month = month;
        this.year = year;
        onDateSet = callback;
    }

    public void Show()
    {
        AndroidDialog dialog = AndroidDialog.CreateDatePickerDialog(day, month, year);
        dialog.OnDatePickerSetCompleted += OnPopupCompleted;
        dialog.ShowDatePickerDialog();
    }

    public int Day
    {
        get
        {
            return day;
        }
    }

    public int Month
    {
        get
        {
            return month;
        }
    }

    public int Year
    {
        get
        {
            return year;
        }
    }

    //--------------------------------------
    //  EVENTS
    //--------------------------------------

    void OnPopupCompleted(string result)
    {
        int selectedDay = Day;
        int selectedMonth = Month;
        int selectedYear = Year;

        string[] values = result.Split('/');

        selectedDay = int.Parse(values[0]);
        selectedMonth = int.Parse(values[1]);
        selectedYear = int.Parse(values[2]);

        if (onDateSet != null)
        {
            onDateSet.Invoke(selectedDay, selectedMonth, selectedYear);
        }
    }
}
