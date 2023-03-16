using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct playerInputData : INetworkInput
{
    //public Vector3 movementInput;
    //public playerInputData playerInformationInput;
    public int playerSort;

    public int moodSelectionSort;
    public int characterSelectionSort;
}

public struct AtkInputData : INetworkInput
{
    public int Hps;
    public int Str;
    public int Rac;
    public int Cur;
    public int Ablock;

    public float AtkDiraction;

    public float MaxmentEnergy;
    public float Energy;
}