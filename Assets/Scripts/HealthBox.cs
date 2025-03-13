[System.Serializable]
public class HealthBox
{
    public DamageType damageType = DamageType.None;

    public HealthBox() { }

    public HealthBox(DamageType damageType)
    {
        this.damageType = damageType;
    }
}