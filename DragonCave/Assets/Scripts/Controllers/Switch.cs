using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IRestartable
{
    private Vector3 initialpos = Vector3.zero;

    public Transform listenerObject;

    private ISwitchListener listener;

    public SwitchTypes switchType;

    public SwitchStates switchState = SwitchStates.Normal;

    private Sprite normalState;

    private Sprite pressedState;

    // Use this for initialization
    void Start()
    {
        initialpos = this.transform.position;

        normalState = SwitchSpritesMannager.instance.SwtitchSprites[(int)switchType * 2];
        pressedState = SwitchSpritesMannager.instance.SwtitchSprites[((int)switchType * 2) + 1];

        if (normalState != null)
            this.GetComponent<SpriteRenderer>().sprite = normalState;

        if(listenerObject != null)
        listener = listenerObject.gameObject.GetComponent<ISwitchListener>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("OnTriggerEnter2D");
        ProcessCollision(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("OnTriggerStay2D");
        ProcessCollision(col);

    }

    private void ProcessCollision(Collider2D col)
    {


        if (LevelController.instance.gameState != GameStates.Playing)
            return;


        if (switchState != SwitchStates.Normal)
            return;

        if (col.transform.gameObject.layer != 9)
            return;

        switchState = SwitchStates.Pressed;
        this.GetComponent<SpriteRenderer>().sprite = pressedState;
        AudioController.instance.PlaySound(Sounds.Switch);
        this.enabled = false;
        if (listener != null)
        {
            listener.SwitchOn(this.switchType);
        }


    }

    public void Restart()
    {
        this.enabled = true;
        this.transform.position = initialpos;
        switchState = SwitchStates.Normal;
        this.GetComponent<SpriteRenderer>().sprite = normalState;
    }
}
