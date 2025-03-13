using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Шаблон для шкалы здоровья (например, VampireHealthTemplate).")]
    [SerializeField] private HealthTemplate template; // Назначьте через Inspector

    [Tooltip("Система шкалы здоровья, созданная в Awake.")]
    [SerializeField] private HealthTrack healthTrack;

    private void Awake()
    {
        if (template != null)
        {
            healthTrack = new VtMHealthTrack(template);
            healthTrack.Initialize();
        }
        else
        {
            Debug.LogWarning("Health Template не назначен!");
        }
    }

    private void Update()
    {
        // Нанести 1 единицу урона Bashing по нажатию клавиши B
        if (Input.GetKeyDown(KeyCode.B))
        {
            healthTrack.ApplyDamage(DamageType.Bashing, 1);
            Debug.Log("Applied 1 point of Bashing damage.");
        }

        // Нанести 1 единицу урона Lethal по нажатию клавиши L
        if (Input.GetKeyDown(KeyCode.L))
        {
            healthTrack.ApplyDamage(DamageType.Lethal, 1);
            Debug.Log("Applied 1 point of Lethal damage.");
        }

        // Нанести 1 единицу урона Aggravated по нажатию клавиши A
        if (Input.GetKeyDown(KeyCode.A))
        {
            healthTrack.ApplyDamage(DamageType.Aggravated, 1);
            Debug.Log("Applied 1 point of Aggravated damage.");
        }

        // Для отладки лечения урона (например, кнопка C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            healthTrack.HealDamage(7);
        }

        // Для отладки вывода состояния шкалы (например, кнопка H)
        if (Input.GetKeyDown(KeyCode.H))
        {
            healthTrack.DebugPrintBoxes();
            Debug.Log("Current Penalty: " + healthTrack.GetWoundPenalty());
            Debug.Log("Current Wound Level: " + template.BoxesStatus[healthTrack.GetWoundLevel()].woundName);
        }
    }
}
