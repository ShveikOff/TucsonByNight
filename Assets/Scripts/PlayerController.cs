using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    // Ссылка на шаблон персонажа, задается через инспектор
    [Tooltip("Шаблон персонажа, определяющий характеристики для бросков")]
    public CharacterVtMTemplate characterTemplate;

    // Ссылка на DiceRoller для выполнения бросков
    private DiceRoller diceRoller;

    private void Awake()
    {
        diceRoller = new DiceRoller();
        diceRoller.OnDiceRolled += HandleDiceRollResult;
    }

    private void OnDestroy()
    {
        diceRoller.OnDiceRolled -= HandleDiceRollResult;
    }

    private void HandleDiceRollResult(DiceRollResult result)
    {
        Debug.Log("=== Dice Roll Completed ===");
        Debug.Log("Dice Rolls: " + string.Join(", ", result.rolls));
        Debug.Log($"rolledSuccesses = {result.rolledSuccesses}, onesCount = {result.onesCount}, netSuccesses = {result.netSuccesses}");
        Debug.Log($"Final result = {result.finalResult}");

    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Реализуем point & click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                agent.SetDestination(hit.point);
            }
        }

        // Узнаём "сырую" скорость
        float currentSpeed = agent.velocity.magnitude;

        // Проверяем, насколько близко к цели
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Считаем, что персонаж "дошёл" - обнуляем скорость
            currentSpeed = 0f;
            // при желании можно вообще "остановить" агента:
            // agent.isStopped = true;
        }

        // Передаём значение в аниматор
        anim.SetFloat("Speed", currentSpeed);

        // Пример: по нажатию клавиши бросаем кубики
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dicePool = CalculateDicePool();
            int difficulty = 6; // задайте нужную сложность броска
            diceRoller.RequestStandardRoll(dicePool, difficulty);
        }
    }

    private int CalculateDicePool()
    {
        if (characterTemplate == null)
        {
            Debug.LogWarning("CharacterTemplate не назначен!");
            return 0;
        }
        
        // Предполагается, что Dexterity задан в базовом классе, а Brawl — в talents.
        int dexterity = characterTemplate.Dexterity;
        int brawl = characterTemplate.talents != null ? characterTemplate.talents.Brawl : 0;
        return dexterity + brawl;
    }
}
