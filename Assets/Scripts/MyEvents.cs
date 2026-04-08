using UnityEngine.Events;

static public class MyEvents
{
    static public UnityEvent StartDialog = new();
    static public UnityEvent EndDialog = new();

    static public UnityEvent<string> UpdateDialog = new();
}