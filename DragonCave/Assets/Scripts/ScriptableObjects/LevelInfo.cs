using UnityEngine;
using System.Collections;
using UnityEngine.Events;



//[CreateAssetMenu(fileName = "New Level Info", menuName = "Level Info")]
[System.Serializable]
public class LevelInfo 
{
    public Vector3 initialCameraPosition;

    public Vector3 initialPlayerPosition;

    public string levelName;

    public LevelCheckPoint[] levelCheckPoints;

    public int currentIndexCheckPoint = 0;

    public void NextCheckPoint()
    {
        if(currentIndexCheckPoint < levelCheckPoints.Length)
        currentIndexCheckPoint ++;
    }

    public bool IsCurrentCheckPointActivated(Vector3 position)
    {
        bool result = false;
        if (currentIndexCheckPoint >= levelCheckPoints.Length)
            return result;

        var info = levelCheckPoints[currentIndexCheckPoint];

        result = info.IsCheckPointActivated(position);

        return result;
    }

    public void ActivateEvent()
    {
        var info = levelCheckPoints[currentIndexCheckPoint];
        info.ActivateEvent();
    }
}

[System.Serializable]
public class LevelCheckPoint
{
    public Transform checkPoint;

    public LevelEvent levelEnvent;

    public float minDistance;

    public CheckPointComparision comparisionType;

    public bool IsCheckPointActivated(Vector3 position)
    {
        bool result = false;
        switch (comparisionType)
        {
            case CheckPointComparision.X:
            return      position.x - this.checkPoint.position.x >= minDistance;
                break;
            case CheckPointComparision.X_abs:
                return Mathf.Abs(this.checkPoint.position.x - position.x) >= minDistance;
                break;
            case CheckPointComparision.Y:
                return position.y - this.checkPoint.position.y >= minDistance;
                break;
            case CheckPointComparision.Y_abs:
                return Mathf.Abs(this.checkPoint.position.y - position.y) >= minDistance;
                break;
            case CheckPointComparision.Magnitude:

                return (position- this.checkPoint.position).magnitude >= minDistance;
                break;
            case CheckPointComparision.Magnitude_abs:
                return Mathf.Abs((this.checkPoint.position - position).magnitude) >= minDistance;

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