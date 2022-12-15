using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    private RPGClass.Stats stats;

    public struct CurrentStats
    {
        public int level;
        public int hp;

        public int strength;
        public int constitution;
        public int mana;
        public int initiative;
        public int luck;
    }

    public CurrentStats currentStats;
    private int speed;
    public EnemyCharacter linkedCharacter;

    private void Start()
    {
        currentStats = new CurrentStats()
        {
            level = 1,

            strength = (int)stats.strength.Evaluate(1),
            mana = (int)stats.mana.Evaluate(1),
            constitution = (int)stats.constitution.Evaluate(1),
            initiative = (int)stats.initiative.Evaluate(1),
            luck = (int)stats.luck.Evaluate(1),
        };

        currentStats.hp = currentStats.constitution; // À CHANGER

        EventManager.StartListening("EnemyAttack", EnemyAttack);
    }

    public void Init(EnemyCharacter enemy, int speed)
    {
        stats = enemy.rpgClass.stats;
        this.speed = speed;
    }

    private void EnemyAttack(Dictionary<string, object> args)
    {
        Skill skill = (Skill)args["skill"];
        CharacterInstance target = (CharacterInstance)args["target"];
        Transform targetTransform = (Transform)args["targetTransform"];

        int effectDamage = (int)Random.Range(skill.skillPowerRange.x, skill.skillPowerRange.y);
        if (skill.effect.type.HasFlag(Skill.Effect.EffectType.InstDamage))
        {
            target.currentStats.hp -= effectDamage;
        }
        else if (skill.effect.type.HasFlag(Skill.Effect.EffectType.InstHeal))
        {
            target.currentStats.hp += effectDamage;
        }

        SpawnFX(skill.vfx, targetTransform);
    }

    private void SpawnFX(GameObject vfx, Transform t)
    {
        GameObject fx = Instantiate(vfx);
        fx.transform.position = t.position;
    }
}
