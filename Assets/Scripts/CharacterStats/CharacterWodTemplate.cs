using UnityEngine;

public abstract class CharacterWoDTemplate: CharacterTemplate
{
    [Tooltip("Вид персонажа, например 'kindred', 'mortal', 'werwolf'")]
    public string characterType;

    [Tooltip("Натура персонажа")]
    public string nature;
    
    [Tooltip("Маска персонажа")]
    public string demeanor;

    // Заменяем поле maxStat на виртуальное свойство с базовым значением 5.
    [Tooltip("Максимальное значение атрибутов и черт.")]
    public virtual int maxStat
    {
        get { return 5; }
    }

    [Header("Атрибуты")]
    public Physical physical;
    public Social social;
    public Mental mental;
    public Virtues virtues;
    public WillPower willPower;

    private void OnValidate()
    {
        // Ограничиваем значение от 0 до maxPermanentWill
        if (willPower.permanentWill > willPower.maxPermanentWill)
            willPower.permanentWill = willPower.maxPermanentWill;
        if (willPower.permanentWill < 0)
            willPower.permanentWill = 0;
        
        if (willPower.temporalWill > willPower.permanentWill)
            willPower.permanentWill = willPower.maxPermanentWill;
        if (willPower.temporalWill < 0)
            willPower.temporalWill = 0;
    }
}