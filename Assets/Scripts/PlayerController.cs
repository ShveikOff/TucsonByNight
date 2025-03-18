using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [Tooltip("Шаблон персонажа, определяющий характеристики для бросков")]
    public CharacterVtMTemplate characterTemplate;

    // Текущая цель движения (если нужно для логики)
    private Vector3 currentDestination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Подписываемся на событие клика по NPC
        NPCController[] npcs = FindObjectsOfType<NPCController>();
        foreach (NPCController npc in npcs)
        {
            npc.OnNPCClicked += HandleNPCClicked;
        }
    }

    private void OnDestroy()
    {
        NPCController[] npcs = FindObjectsOfType<NPCController>();
        foreach (NPCController npc in npcs)
        {
            npc.OnNPCClicked -= HandleNPCClicked;
        }
    }

    /// <summary>
    /// Унифицированный метод для установки цели перемещения.
    /// </summary>
    /// <param name="destination">Целевая точка движения.</param>
    public void MoveTo(Vector3 destination)
    {
        currentDestination = destination;
        agent.SetDestination(destination);
        Debug.Log("Движение к точке: " + destination);
    }

    /// <summary>
    /// Обработчик события клика по NPC.
    /// Находит ближайшую точку на NavMesh вокруг NPC и устанавливает её как цель.
    /// </summary>
    private void HandleNPCClicked(NPCController npc)
    {
        Debug.Log("NPC clicked: " + npc.gameObject.name);
        Vector3 npcPosition = npc.transform.position;
        if (NavMesh.SamplePosition(npcPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            Vector3 offset = (agent.transform.position - npc.transform.position).normalized * 1.5f;
            Vector3 targetPosition = hit.position + offset;
            MoveTo(targetPosition);
        }
        else
        {
            Debug.LogWarning("Не удалось найти точку на NavMesh рядом с NPC.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Если клик попал на объект с компонентом NPCController, то ничего не делаем,
                // т.к. NPC сам вызовет своё событие OnNPCClicked
                if (hit.collider.GetComponent<NPCController>() != null)
                {
                    return;
                }
                
                // Иначе, если клик не по NPC, перемещаем игрока
                MoveTo(hit.point);
            }
        }

        float currentSpeed = agent.velocity.magnitude;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            currentSpeed = 0f;
        anim.SetFloat("Speed", currentSpeed);

        // Пример: по нажатию пробела выполняется бросок кубиков
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dicePool = CalculateDicePool();
            int difficulty = 6;
            DiceRoller.RequestStandardRoll(dicePool, difficulty);
        }
    }

    private int CalculateDicePool()
    {
        if (characterTemplate == null)
        {
            Debug.LogWarning("CharacterTemplate не назначен!");
            return 0;
        }
        
        int dexterity = characterTemplate.physical != null ? characterTemplate.physical.Dexterity : 0;
        int brawl = characterTemplate.talents != null ? characterTemplate.talents.Brawl : 0;
        return dexterity + brawl;
    }
}
