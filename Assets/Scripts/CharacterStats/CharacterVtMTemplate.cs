using UnityEngine;

[CreateAssetMenu(fileName = "CharacterVtMTemplate", menuName = "VtM/Vtm Character Template")]
public class CharacterVtMTemplate : CharacterWoDTemplate
{
    [Header("Abilities")]
    public Talents talents;
    public Skills skills;
    public Knowledges knowledges;

    [Header("Особенности вампира")]
    [Tooltip("Клан вампира (Brujah, Toreador, Malkavian, etc.)")]
    public string clan;

    [Tooltip("Вампир обративший персонажа")]
    public string sire;

    [Tooltip("Поколение вампира (чем меньше, тем сильнее).")]
    public int generation;

    [Tooltip("Ссылка на настройки поколений для вампиров")]
    public VampireGenerationStats vampireGenerationStatsSO;

    public override int maxStat =>
        vampireGenerationStatsSO != null
            ? vampireGenerationStatsSO.getMaxStat(generation)
            : base.maxStat;
    
    [Tooltip("Максимальное значение атрибутов при бладбаффе")]
    public int maxBloodBuffStat => vampireGenerationStatsSO != null
        ? (vampireGenerationStatsSO.getMaxStat(generation) + 1)
        : 0;

    [Tooltip("Максимальный бладпул")]
    public int maxBloodPool => vampireGenerationStatsSO != null
        ? vampireGenerationStatsSO.getMaxBloodPool(generation)
        : 0;

    [Tooltip("Количество крови которое можно тратить в ход")]
    public int bloodLimit => vampireGenerationStatsSO != null
        ? vampireGenerationStatsSO.getBloodLimit(generation)
        : 0;

    [Tooltip("Бладпул")]
    public int bloodPool;

    [Header("Health Settings")]
    [Tooltip("Система шкалы здоровья")]
    [SerializeField] public VtMHealthTrack healthTrack;

    // Need add class disciplines later
}
