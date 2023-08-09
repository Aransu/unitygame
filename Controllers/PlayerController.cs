using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    public override bool RetrieveJumpDownInput()
    {
        return Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.X);
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveJumpUpInput()
    {
        return Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.X);
    }

    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump") || Input.GetKey(KeyCode.X);
    }

    public override float RetrieveYMoveInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetKeyDown(KeyCode.C);
    }
}
