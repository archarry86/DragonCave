using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "LevelStatus", menuName = "New LevelStatus")]
public class LevelStatus : ScriptableObject
{
    public int lastCheckPoint;

    public string inidate;

    public string lastdate;
}