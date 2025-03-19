using UnityEngine;
using UnityEngine.AI;
using System;
using System.Data.Common;

public class NPCController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    // Ссылка на шаблон персонажа, задается через инспектор
    [Tooltip("Шаблон персонажа, определяющий характеристики для бросков")]
    public CharacterVtMTemplate characterTemplate;
    public event Action<NPCController> OnNPCClicked;

    private bool died = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Подписываемся на событие игрока
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnPlayerAnimationTriggered += HandlePlayerAnimationTriggered;
            player.OnDamageHurt += TakingDamage;
        }
    }

    private void OnDestroy()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnPlayerAnimationTriggered -= HandlePlayerAnimationTriggered;
            player.OnDamageHurt += TakingDamage;
        }
    }

    private void OnMouseDown()
    {
        OnNPCClicked?.Invoke(this);
    }

    private void HandlePlayerAnimationTriggered(string playerTrigger)
    {
        // Например, если игрок запустил анимацию "Brawl", NPC реагирует анимацией "React"
        if (playerTrigger == "Brawl" && !died)
        {
            anim.SetTrigger("React");
        }
        if (playerTrigger == "Brawl" && died)
        {
            anim.SetTrigger("Die");
        }        
    }

    private void DieAnimationTrigger() {

    }

    private void TakingDamage(int damage) {
        if (characterTemplate == null) {
            Debug.LogWarning("CharacterTemplate не назначен!");
        }
        int stamina = characterTemplate.physical != null ? characterTemplate.physical.Stamina :0;
        int dicePool = stamina;
        int difficulty = 6;
        int absorbe = DiceRoller.RequestStandardRoll(dicePool, difficulty);
        Debug.Log($"NPC смог поглотить {absorbe} кол-во урона");
        if (absorbe < damage) {
            characterTemplate.healthTrack.ApplyDamage(DamageType.Bashing, damage - absorbe);
            characterTemplate.healthTrack.DebugPrintBoxes();
            Debug.Log("Current Penalty: " + characterTemplate.healthTrack.GetWoundPenalty());
            Debug.Log("Current Wound Level: " + characterTemplate.healthTrack.GetWoundName());
            if (characterTemplate.healthTrack.GetWoundPenalty() == -999) {
                died = true;
            }
        }
    }
}
