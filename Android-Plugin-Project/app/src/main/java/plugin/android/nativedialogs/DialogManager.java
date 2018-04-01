package plugin.android.nativedialogs;

/**
 * Created by oluwaseyeayinla on 09/03/2018.
 */

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.app.TimePickerDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnKeyListener;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.util.Log;
import android.view.ContextThemeWrapper;
import android.view.KeyEvent;
import android.widget.DatePicker;
import android.widget.TimePicker;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

import java.util.Calendar;


@SuppressLint("InlinedApi")
public class DialogManager
{
    private static String DebugTag = "Native Dialogs";
    private static AlertDialog AlertDialog = null;
    private static ProgressDialog ProgressDialog = null;
    private static DatePickerDialog DatePickerDialog = null;
    private static TimePickerDialog TimePickerDialog = null;


    public static void ShowToastDialog(String message)
    {
        Log.d(DebugTag, "ShowToastDialog: " + message);

        Toast.makeText(ActivityUtils.GetApplicationContext(), message, Toast.LENGTH_SHORT).show();
    }

    public static void ShowToastDialog(String message, int duration, int gravity, int xOffset, int yOffset)
    {
        Log.d(DebugTag, "ShowToastDialog: " + " | " + message + " | " + duration + " | " + gravity);

        Toast toast = Toast.makeText(ActivityUtils.GetApplicationContext(), message, Toast.LENGTH_SHORT);
        toast.setDuration(duration <= 0 ? Toast.LENGTH_SHORT : Toast.LENGTH_LONG);
        toast.setGravity(gravity, xOffset, yOffset);
        toast.show();
    }

    public static void ShowAlertDialog(String title, String message, String[] actions)
    {
        ShowAlertDialog(title, message, actions, GetDefaultTheme());
    }

    public static void ShowAlertDialog(String title, String message, String[] actions, int theme)
    {
        if(actions.length == 1)
        {
            ShowAlertDialog(title, message, actions[0], theme);
        }
        else if(actions.length == 2)
        {
            ShowAlertDialog(title, message, actions[0], actions[1], theme);
        }
        else if(actions.length == 3)
        {
            ShowAlertDialog(title, message, actions[0], actions[1], actions[2], theme);
        }
        else
        {
            Log.d(DebugTag, "No Signature exists for " + actions.length +" parameters.");
        }
    }

    public static void ShowAlertDialog(String title, String message, String positive)
    {
        ShowAlertDialog(title, message, positive, GetDefaultTheme());
    }

    public static void ShowAlertDialog(String title, String message, String positive, int theme)
    {
        Log.d(DebugTag, "ShowAlertDialog: " + title + " | " + message + " | " + positive);

        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme));
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setPositiveButton(positive, PopupClickListener);
        builder.setOnKeyListener(HardwareKeyListener);
        builder.setCancelable(false);

        AlertDialog = builder.show();
    }

    public static void ShowAlertDialog(String title, String message, String positive, String negative)
    {
        ShowAlertDialog(title, message, positive, negative, GetDefaultTheme());
    }

    public static void ShowAlertDialog(String title, String message, String positive, String negative, int theme)
    {
        Log.d(DebugTag, "ShowAlertDialog: " + title + " | " + message + " | " + positive + " | " + negative);

        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme));
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setPositiveButton(positive, PopupClickListener);
        builder.setNegativeButton(negative, PopupClickListener);
        builder.setOnKeyListener(HardwareKeyListener);
        builder.setCancelable(false);

        AlertDialog = builder.show();
    }

    public static void ShowAlertDialog(String title, String message, String positive, String neutral, String negative)
    {
        ShowAlertDialog(title, message, positive, neutral, negative, GetDefaultTheme());
    }

    public static void ShowAlertDialog(String title, String message, String positive, String neutral, String negative, int theme)
    {
        Log.d(DebugTag, "ShowAlertDialog: " + title + " | " + message + " | " + positive + " | " + neutral + " | " + negative);

        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme));
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setPositiveButton(positive, PopupClickListener);
        builder.setNegativeButton(negative, PopupClickListener);
        builder.setNeutralButton(neutral, PopupClickListener);
        builder.setOnKeyListener(HardwareKeyListener);
        builder.setCancelable(false);

        AlertDialog = builder.show();
    }

    public static void DismissAlertDialog()
    {
        Log.d(DebugTag, "DismissAlertDialog: ");

        if (AlertDialog != null)
        {
            AlertDialog.dismiss();
            AlertDialog = null;

            UnityPlayer.UnitySendMessage("AndroidDialog", "OnAlertActionCallBack", "-2");
        }
    }

    public static void ShowPreloaderDialog(String title, String message)
    {
        ShowPreloaderDialog(title, message, GetDefaultTheme());
    }

    public static void ShowPreloaderDialog(String title, String message, int theme)
    {
        Log.d(DebugTag, "ShowPreloaderDialog: " + title + " | " + message);

        ProgressDialog = new ProgressDialog(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme));
        ProgressDialog.setTitle(title);
        ProgressDialog.setMessage(message);
        ProgressDialog.show();
        ProgressDialog.setCancelable(false);
    }

    public static void DismissPreloaderDialog()
    {
        Log.d(DebugTag, "DismissPreloaderDialog: ");

        if (ProgressDialog != null)
        {
            ProgressDialog.hide();
        }
    }

    private static int savedYear;
    private static int savedMonth;
    private static int savedDay;

    public static void ShowDatePickerDialog()
    {
        // Get Current Date
        final Calendar c = Calendar.getInstance();
        int year = c.get(Calendar.YEAR);
        int month = c.get(Calendar.MONTH);
        int day = c.get(Calendar.DAY_OF_MONTH);

        ShowDatePickerDialog(day, month, year);
    }

    public static void ShowDatePickerDialog(int day, int month, int year)
    {
        ShowDatePickerDialog(day, month, year, GetDefaultTheme());
    }

    public static void ShowDatePickerDialog(int day, int month, int year, int theme)
    {
        Log.d(DebugTag, "ShowDatePickerDialog: " + day + " | " + month + " | " + year);

        DatePickerDialog = new DatePickerDialog(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme),
                DateSetListener,
                year,
                month,
                day);
        savedYear = year;
        savedMonth = month;
        savedDay = day;
        DatePickerDialog.show();
    }

    public static void DismissDatePickerDialog()
    {
        Log.d(DebugTag, "DismissDatePickerDialog: ");

        if (DatePickerDialog != null)
        {
            DatePickerDialog.dismiss();
            DatePickerDialog = null;

            UnityPlayer.UnitySendMessage("AndroidDialog", "OnDateSetCallBack", ToDateString(savedDay, savedMonth, savedYear));
        }
    }

    private static String ToDateString(int day, int month, int year)
    {
        Log.d(DebugTag, "ToDateString: " + day + "/" + month + "/" + year);

        return day+ "/" + (month + 1) +"/"+ year;
    }

    private static int savedHour;
    private static int savedMinute;
    private static boolean saved24HoursFormat;

    public static void ShowTimePickerDialog()
    {
        // Get Current Date
        final Calendar c = Calendar.getInstance();
        int hour = c.get(Calendar.HOUR_OF_DAY);
        int minute = c.get(Calendar.MINUTE);

        ShowTimePickerDialog(hour, minute, false);
    }

    public static void ShowTimePickerDialog(int hourOfDay, int minute, boolean use24hrsFormat)
    {
        ShowTimePickerDialog(hourOfDay, minute, use24hrsFormat, GetDefaultTheme());
    }

    public static void ShowTimePickerDialog(int hourOfDay, int minute, boolean use24hrsFormat, int theme)
    {
        Log.d(DebugTag, "ShowTimePickerDialog: " + hourOfDay + " | " + minute + " | " + use24hrsFormat);

        TimePickerDialog = new TimePickerDialog(new ContextThemeWrapper(ActivityUtils.GetLauncherActivity(), theme),
                TimeSetListener,
                hourOfDay,
                minute,
                use24hrsFormat);

        savedHour = hourOfDay;
        savedMinute = minute;
        saved24HoursFormat = use24hrsFormat;
        TimePickerDialog.show();
    }

    public static void DismissTimePickerDialog()
    {
        Log.d(DebugTag, "DismissTimePickerDialog: ");

        if (TimePickerDialog != null)
        {
            TimePickerDialog.dismiss();
            TimePickerDialog = null;

            UnityPlayer.UnitySendMessage("AndroidDialog", "OnTimeSetCallBack", ToTimeString(savedHour, savedMinute, saved24HoursFormat));
        }
    }

    private static String ToTimeString(int hour, int minute, boolean use24HoursFormat)
    {
        Log.d(DebugTag, "ToTimeString: " + hour + ":" + minute);

        /*
        if(!use24HoursFormat)
        {
            bool pm = hour >= 12;
            hour = hour == 12 ? hour : hour % 12;
            return hour + ":" + minute + (pm ? " pm" : " am");
        }
        */

        return hour + ":" + minute;
    }

    private static int GetDefaultTheme()
    {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP)
        {
            return android.R.style.Theme_Material_Light_Dialog;
        }
        else
        {
            return android.R.style.Theme_Holo_Dialog;
        }
    }

    private static DialogInterface.OnClickListener PopupClickListener = new DialogInterface.OnClickListener()
    {
        @Override
        public void onClick(DialogInterface dialog, int which)
        {
            switch (which)
            {
                case DialogInterface.BUTTON_POSITIVE:
                    UnityPlayer.UnitySendMessage("AndroidDialog", "OnAlertActionCallBack", "1");
                    //Yes button clicked
                    break;

                case DialogInterface.BUTTON_NEGATIVE:
                    UnityPlayer.UnitySendMessage("AndroidDialog", "OnAlertActionCallBack", "-1");
                    //No button clicked
                    break;

                case DialogInterface.BUTTON_NEUTRAL:
                    UnityPlayer.UnitySendMessage("AndroidDialog", "OnAlertActionCallBack", "0");
                    //neutral button clicked
                    break;
            }

            dialog = null;
            AlertDialog = null;
        }
    };

    private static OnKeyListener HardwareKeyListener = new OnKeyListener()
    {
        @Override
        public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event)
        {
            if (keyCode == KeyEvent.KEYCODE_BACK)
            {
                DismissAlertDialog();
                DismissDatePickerDialog();
                DismissTimePickerDialog();
            }

            return false;
        }
    };

    private static DatePickerDialog.OnDateSetListener DateSetListener = new DatePickerDialog.OnDateSetListener() {
        @Override
        public void onDateSet(DatePicker arg0, int year, int month, int day)
        {
            UnityPlayer.UnitySendMessage("AndroidDialog", "OnDateSetCallBack", ToDateString(day, month, year));
            // arg1 = year
            // arg2 = month (is 0 based)
            // arg3 = day
            DatePickerDialog = null;
        }
    };

    private static TimePickerDialog.OnTimeSetListener TimeSetListener = new TimePickerDialog.OnTimeSetListener() {
        @Override
        public void onTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            UnityPlayer.UnitySendMessage("AndroidDialog", "OnTimeSetCallBack", ToTimeString(hourOfDay, minute, false));

            TimePickerDialog = null;
        }
    };
}


