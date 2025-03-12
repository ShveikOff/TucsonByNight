using UnityEngine;

[CreateAssetMenu(fileName = "CharacterVtMTemplate", menuName = "VtM/Character Template")]
public class CharacterVtMTemplate : ScriptableObject
{
    [Header("General Info")]
    [Tooltip("Отображаемое имя персонажа, например 'Brujah Neonate' или 'Alice Doe'.")]
    public string displayName;

    [Tooltip("Краткое описание или биография персонажа/шаблона.")]
    [TextArea(2, 5)]
    public string description;

    [Tooltip("Пол персонажа")]
    public string gender;

    [Tooltip("Возраст персонажа")]
    public int age;

    [Tooltip("Натура персонажа")]
    public string nature;
    
    [Tooltip("Маска персонажа")]
    public string demeanor;

    [Tooltip("Клан вампира (Brujah, Toreador, Malkavian, etc.)")]
    public string clan;

    [Tooltip("Вампир обративший персонажа")]
    public string sire;

    [Tooltip("Поколение вампира (чем меньше, тем сильнее).")]
    public int generation = 13;

    [Header("Base Stats")]
    [Tooltip("Максимальное значение постоянной воли")]
    public int maxPermanentWill = 10;

    [Tooltip("Максимальное значение черт")]
    public int maxStat = 5;

    [Tooltip("Максимальное значение атрибутов при бладбаффе")]
    public int maxBloodBuffStat = 6;

    [Tooltip("Максимальный бладпул")]
    public int maxBloodPool = 10;

    [Tooltip("Количество крови которое можно тратить в ход")]
    public int bloodLimit = 1;

    [Tooltip("Здоровье персонажа")]
    public int health;

    [Header("Optional Fields")]
    [Tooltip("Спрайт/иконка персонажа, если нужно показывать портрет.")]
    public Sprite portrait;

    [Tooltip("Дополнительные дисциплины, навыки, способности и т.д.")]
    public string[] disciplines;

    // Здесь можно добавить навыки, способности, броски, показатели морали,
    // маскарад, убежденность, уровень человечности и т.д.
}
