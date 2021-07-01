using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public enum PowerUpType
    {
        FireRateIncrease,
        UltraShot,
        RecoverHealth,
        Invulnerability
    }

    public PowerUpType powerUpType;

}
