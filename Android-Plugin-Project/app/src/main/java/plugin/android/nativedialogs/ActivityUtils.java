package plugin.android.nativedialogs;

/**
 * Created by oluwaseyeayinla on 14/03/2018.
 */

import android.app.Activity;
import android.content.Context;

import com.unity3d.player.UnityPlayer;

public class ActivityUtils
{
    public static Activity GetLauncherActivity()
    {
        return UnityPlayer.currentActivity;
    }

    public static Context GetApplicationContext()
    {
        return GetLauncherActivity().getApplicationContext();
    }
}


