using UnityEngine;
using UnityEngine.AI;
using System;

public class NPCController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    // Ссылка на шаблон персонажа, задается через инспектор
    [Tooltip("Шаблон персонажа, определяющий характеристики для бросков")]
    public CharacterVtMTemplate characterTemplate;
    public event Action<NPCController> OnNPCClicked;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("NPCController: NPC clicked: " + gameObject.name);
        OnNPCClicked?.Invoke(this);
    }
}
