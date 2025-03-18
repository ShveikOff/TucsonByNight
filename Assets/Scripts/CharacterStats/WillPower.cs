using UnityEngine;

[System.Serializable]
public class WillPower
{   
    [Tooltip("Максимальное значение постоянной воли.")]
    public int maxPermanentWill = 10;

    [Tooltip("Значение постоянной воли")]
    public int permanentWill;

    [Tooltip("Значение временной воли")]
    public int temporalWill;
    
}