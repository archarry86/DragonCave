using UnityEngine;
using System.Collections;

public abstract class GameContoller : MonoBehaviour
{
    public LevelInfo levelInformation;

    public Transform playerTransform;

    protected IPlayerStatus playerInfo;

    // Use this for initialization
    void Start()
    {
        playerInfo = playerTransform.GetComponent<IPlayerStatus>();
        ProcessStart();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessUpdate();
        GameControllerUpdate();
    }

    public  void ProcessUpdate()
    {
        if (levelInformation.IsCurrentCheckPointActivated(playerTransform.position))
        {
            levelInformation.ActivateEvent();
            levelInformation.NextCheckPoint();
        }
    }

    public  void OnGUI()
    {
        ProcessOnGUI();
    }

    public virtual void ProcessStart()
    {

    }

    public virtual void ProcessOnGUI()
    {

    }
    public  virtual void GameControllerUpdate()
    {
       
    }

   

}
