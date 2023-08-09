using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();
    public abstract bool RetrieveJumpDownInput();

    public abstract bool RetrieveJumpUpInput();

    public abstract bool RetrieveJumpHoldInput();

    public abstract float RetrieveYMoveInput();

    public abstract bool RetrieveAttackInput();
}
