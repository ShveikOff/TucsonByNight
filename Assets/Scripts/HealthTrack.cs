using System;
using UnityEngine;

public abstract class HealthTrack
{
    [SerializeField] protected HealthBox[] boxes;      // Массив ячеек здоровья
    [SerializeField] protected HealthTemplate template;  // Шаблон (конфиг) для определения уровней, штрафов и т.д.

    /// <summary>
    /// Инициализация системы здоровья.
    /// Обычно создаёт массив ячеек на основе данных из template.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Применяет урон определённого типа (например, Bashing, Lethal, Aggravated) в заданном количестве.
    /// </summary>
    public abstract void ApplyDamage(DamageType damageType, int amount);

    /// <summary>
    /// Лечит заданное количество ячеек (переводит их в состояние None).
    /// </summary>
    public abstract void HealDamage(int amount);

    /// <summary>
    /// Возвращает текущий уровень ранений (например, число заполненных ячеек).
    /// </summary>
    public abstract int GetWoundLevel();

    /// <summary>
    /// Возвращает текущий штраф к броскам, основанный на заполненных ячейках и шаблоне.
    /// </summary>
    public abstract int GetWoundPenalty();

    /// <summary>
    /// Отладочный метод, выводящий в консоль состояние всех ячеек.
    /// </summary>
    public abstract void DebugPrintBoxes();
}
