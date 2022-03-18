using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChestState : MonoBehaviour
{
    protected ChestSlot ChestSlot;

    void Awake()
    {
        ChestSlot = GetComponent<ChestSlot>();
    }

    public abstract void OnEnter();

    public abstract void OnExit();

}
