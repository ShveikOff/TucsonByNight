using UnityEngine;

public abstract class CharacterWoDTemplate: CharacterTemplate
{
    [Header("General Info")]
    [Tooltip("Вид персонажа, например 'kindred', 'mortal', 'werwolf'")]
    public string characterType;

    [Tooltip("Натура персонажа")]
    public string nature;
    
    [Tooltip("Маска персонажа")]
    public string demeanor;

    [Header("Базовые показатели WoD")]
    [Tooltip("Максимальное значение постоянной воли.")]
    public int maxPermanentWill = 10;

    [Tooltip("Значение постоянной воли")]
    public int permanentWill;

    [Tooltip("Значение временной воли")]
    public int temporalWill;

    // Заменяем поле maxStat на виртуальное свойство с базовым значением 5.
    [Tooltip("Максимальное значение атрибутов и черт.")]
    public virtual int maxStat
    {
        get { return 5; }
    }

    [Header("Физические атрибуты")]
    public int Strength;
    public int Dexterity;
    public int Stamina;

    [Header("Социальные атрибуты")]
    public int Charisma;
    public int Manipulation;
    public int Appearance;

    [Header("Ментальные атрибуты")]
    public int Perception;
    public int Intelligence;
    public int Wits;

    private void OnValidate()
    {
        // Ограничиваем значение от 0 до maxPermanentWill
        if (permanentWill > maxPermanentWill)
            permanentWill = maxPermanentWill;
        if (permanentWill < 0)
            permanentWill = 0;
        
        if (temporalWill > permanentWill)
            permanentWill = maxPermanentWill;
        if (temporalWill < 0)
            temporalWill = 0;
    }
}