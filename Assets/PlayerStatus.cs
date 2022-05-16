using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Singletone<PlayerStatus>
{
    public enum Status
    {
        none,
        IsInCar,
        IsInPlane,
        IsJumpedFromPlane,
        IsInParachute,
        IsOnLand,
        IsGliding,
        
    }

    [SerializeField]
    public  Status status = Status.none;

}
