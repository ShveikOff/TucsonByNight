using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [Tooltip("Шаблон персонажа, определяющий характеристики для бросков")]
    public CharacterVtMTemplate characterTemplate;

    // Событие, которое уведомляет о том, что игрок запустил анимацию по триггеру.
    public event Action<string> OnPlayerAnimationTriggered;
    public event Action<int> OnDamageHurt;

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
    }

    /// <summary>
    /// Обработчик события клика по NPC.
    /// Находит ближайшую точку на NavMesh вокруг NPC и устанавливает её как цель.
    /// </summary>
    private void HandleNPCClicked(NPCController npc)
    {
        Vector3 npcPosition = npc.transform.position;
        if (NavMesh.SamplePosition(npcPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            Vector3 offset = (agent.transform.position - npc.transform.position).normalized * 1.5f;
            Vector3 targetPosition = hit.position + offset;
            MoveTo(targetPosition);
            StartCoroutine(WaitForArrivalAndPlayAnimation("Brawl"));
        }
        else
        {
            Debug.LogWarning("Не удалось найти точку на NavMesh рядом с NPC.");
        }
    }

    public void OnAnimationComplete()
    {
        OnPlayerAnimationTriggered?.Invoke("Brawl");
    }


    private System.Collections.IEnumerator WaitForArrivalAndPlayAnimation(string triggerName)
    {
        // Ждем, пока агент не перестанет рассчитывать путь или не приблизится достаточно к цели
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        
        // Запускаем анимацию, например, по триггеру
        BrawlRoll();
        anim.SetTrigger(triggerName);
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

    private void BrawlRoll() {
        if (characterTemplate == null) {
            Debug.LogWarning("CharacterTemplate не назначен!");
        }
        int dexterity = characterTemplate.physical != null ? characterTemplate.physical.Dexterity : 0;
        int brawl = characterTemplate.talents != null ? characterTemplate.talents.Brawl : 0;
        int strenght = characterTemplate.physical != null ? characterTemplate.physical.Strength : 0;
        int dicePool = dexterity + brawl;
        int difficulty = 6;
        int result = DiceRoller.RequestStandardRoll(dicePool, difficulty);
        if (result > 0) {
            int damagePool = strenght + result - 1;
            int damage = DiceRoller.RequestStandardRoll(damagePool, difficulty);
            Debug.Log($"Player нанес NPC {damage} Bashing урона");
            OnDamageHurt?.Invoke(damage);
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
