using UnityEngine;

[CreateAssetMenu(fileName = "CrashMessages", menuName = "Crash Message Database")]
public class CrashMessages : ScriptableObject
{
    public CrashMessage[] crashMessages;
}