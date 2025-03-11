using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

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
    }
}
