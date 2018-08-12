using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public ISwitchListener listener;

    public SwitchTypes switchType;

    public SwitchStates switchState = SwitchStates.Normal;

    public Sprite normalState;

    public Sprite pressedState;

    // Use this for initialization
    void Start()
    {
        normalState = SwitchSpritesMannager.instance.SwtitchSprites[(int)switchType * 2];
        pressedState = SwitchSpritesMannager.instance.SwtitchSprites[((int)switchType * 2) + 1];

        if (normalState != null)
            this.GetComponent<SpriteRenderer>().sprite = normalState;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        ProcessCollision();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("OnCollisionStay2D");
        ProcessCollision();

    }

    private void ProcessCollision()
    {
        if (switchState != SwitchStates.Normal)
            return;


        switchState = SwitchStates.Pressed;
        this.GetComponent<SpriteRenderer>().sprite = pressedState;

        if (listener != null)
        {
            listener.SwitchOn(this.switchType);
        }
    }
}
