using System.Collections.Generic;
using UnityEngine;

public enum AlertActionType
{
    Dismiss,
    Neutral,
    Positive,
    Negative
}

public class AndroidAlertDialog
{
    public delegate void AlertPopupAction();

    protected Dictionary<string, AlertPopupAction> actions = new Dictionary<string, AlertPopupAction>();
    protected Dictionary<AlertActionType, string> actionIdentifiers = new Dictionary<AlertActionType, string>();
    protected AlertPopupAction dismissCallback = null;
    protected System.Action<string> debugOnCompleted = null;
    protected string title = string.Empty;
    protected string message = string.Empty;
    protected const int MAX_ACTIONS = 3;
    protected const string DISMISS_ACTION = "-2";

    //--------------------------------------
    // INITIALIZE
    //--------------------------------------

    public AndroidAlertDialog(string title, string message)
    {
        actions = new Dictionary<string, AlertPopupAction>();

        this.title = title;
        this.message = message;
    }

    //--------------------------------------
    //  PUBLIC METHODS
    //--------------------------------------

    public void AddAction(AlertActionType type, string title, AlertPopupAction callback)
    {
        if (actions.Count >= MAX_ACTIONS)
        {
            Debug.LogWarning("Action NOT added! Actions limit exceeded");
        }
        else if (actions.ContainsKey(title) || actionIdentifiers.ContainsKey(type))
        {
            Debug.LogWarning("Action NOT added! Action with this Title or Type already exists");
        }
        else
        {
            actionIdentifiers.Add(type, title);
            actions.Add(title, callback);
        }
    }

    public void AddDismissListener(AlertPopupAction callback)
    {
        dismissCallback = callback;
    }

    public void AddOnCompletedListener(System.Action<string> callback)
    {
        debugOnCompleted = callback;
    }

    public void Show()
    {
        AndroidDialog dialog = AndroidDialog.CreateAlertDialog(this.title, this.message, this.actions.Keys);
        dialog.OnAlertActionCompleted += OnPopupCompleted;
        dialog.ShowAlertDialog();
    }

    //--------------------------------------
    //  GET/SET
    //--------------------------------------

    public string Title
    {
        get
        {
            return title;
        }
    }

    public string Message
    {
        get
        {
            return message;
        }
    }

    public Dictionary<string, AlertPopupAction> Actions
    {
        get
        {
            return actions;
        }
    }

    //--------------------------------------
    //  EVENTS
    //--------------------------------------

    void OnPopupCompleted(string result)
    {
        if (debugOnCompleted != null)
        {
            debugOnCompleted.Invoke(result);
        }

        AlertActionType actionType = GetAlertActionType(result);

        if (actionType == AlertActionType.Dismiss)
        {
            if (result.Equals(DISMISS_ACTION) && dismissCallback != null)
            {
                dismissCallback.Invoke();
            }
        }
        else
        { 
            actions[actionIdentifiers[actionType]].Invoke();
        }
    }

    AlertActionType GetAlertActionType(string result)
    {
        int which = int.Parse(result);

        switch (which)
        { 
            case -1: return AlertActionType.Negative;
            case 0: return AlertActionType.Neutral;
            case 1: return AlertActionType.Positive;
            default : return AlertActionType.Dismiss;
        }
    }

    //--------------------------------------
    //  PRIVATE METHODS
    //--------------------------------------

    //--------------------------------------
    //  DESTROY
    //--------------------------------------
}
