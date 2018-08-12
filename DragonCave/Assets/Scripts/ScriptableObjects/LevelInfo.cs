using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Level Info", menuName = "Level Info")]
public class LevelInfo : ScriptableObject
{
    public LevelCheckPoint[] levelCheckPoints;

    public int CurrentIndexCheckPoint;

    public void NextCheckPoint()
    {
        if(CurrentIndexCheckPoint< levelCheckPoints.Length)
        CurrentIndexCheckPoint += 1;
    }

    public bool IsCurrentCheckPointActivated(Vector3 position)
    {
        bool result = false;

        var info = levelCheckPoints[CurrentIndexCheckPoint];

        result = info.IsCheckPointActivated(position);

        return result;
    }

    public void ActivateEvent()
    {
        var info = levelCheckPoints[CurrentIndexCheckPoint];
        info.ActivateEvent();
    }
}

[System.Serializable]
public class LevelCheckPoint
{
    public Transform checkPoint;

    public LevelEvent levelEnvent;

    public string Name;

    public float MinDistance;

    public CheckPointComparision comparisionType;

    public bool IsCheckPointActivated(Vector3 position)
    {
        bool result = false;
        switch (comparisionType)
        {
            case CheckPointComparision.X:
            return     this.checkPoint.position.x - position.x >= MinDistance;
                break;
            case CheckPointComparision.X_abs:
                return Mathf.Abs(this.checkPoint.position.x - position.x) >= MinDistance;
                break;
            case CheckPointComparision.Y:
                return this.checkPoint.position.y - position.y >= MinDistance;
                break;
            case CheckPointComparision.Y_abs:
                return Mathf.Abs(this.checkPoint.position.y - position.y) >= MinDistance;
                break;
            case CheckPointComparision.Magnitude:

                return (this.checkPoint.position - position).magnitude >= MinDistance;
                break;
            case CheckPointComparision.Magnitude_abs:
                return Mathf.Abs((this.checkPoint.position - position).magnitude) >= MinDistance;

                break;
        }

        return result;
    }

    public void ActivateEvent()
    {
        if(levelEnvent!= null)
            levelEnvent.Invoke();
    }

}

[System.Serializable]
public class LevelEvent : UnityEvent{

}
     

public enum CheckPointComparision
{
    X,
    X_abs,
  
    Y,
    Y_abs,

    Magnitude,
    Magnitude_abs
}