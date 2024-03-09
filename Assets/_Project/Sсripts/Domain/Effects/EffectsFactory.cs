using System.Collections.Generic;
using Config;
using Controllers;
using Controllers.PopupChoice;
using Domain.Effects.Enemy;
using Domain.Effects.Player;
using Domain.Movement;
using Domain.Spells;

namespace Domain.Effects
{
    public static class EffectsFactory
    {
        public static Dictionary<EffectName, Effect> CreatePlayerEffectsDictionary(
            Health playerHealth, Damage playerDamage, Mana playerMana, PlayerJumper playerJumper,
            Health enemyHealth, Coefficients coefficients, PlayerPortal playerPortal,
            PopUpChoiceEffectPresenter choiceEffectPresenter,
            PopUpChoiceSpellPresenter choiceSpellPresenter)
        {
            Dictionary<EffectName, Effect> playerEffects = new()
            {
                { EffectName.Swords, new PlayerSwords(playerJumper, enemyHealth, playerDamage) },
                { EffectName.Health, new PlayerHealth(playerHealth, playerJumper, coefficients) },
                { EffectName.Mana, new PlayerMana(playerMana, playerJumper, coefficients) },
                { EffectName.Portal, playerPortal },
                { EffectName.Question, new PlayerQuestion(playerJumper, choiceEffectPresenter) },
                { EffectName.SpellBook, new PlayerSpellBook(playerJumper, choiceSpellPresenter) }
            };
            return playerEffects;
        }

        public static Dictionary<SpellName, Effect> CreatePlayerSpellsDictionary(
            Health playerHealth, Damage playerDamage, Coefficients coefficients, Defence playerDefence,
            Mana playerMana)
        {
            Dictionary<SpellName, Effect> playerSpells = new Dictionary<SpellName, Effect>
            {
                { SpellName.FullHealth, new FullHealthSpell(playerHealth) },
                { SpellName.UpDamage, new UpDamageSpell(playerDamage, coefficients) },
                { SpellName.UpMaxHealth, new UpMaxHealthSpell(playerHealth, coefficients) },
                { SpellName.UpDefence, new UpDefenceSpell(playerDefence, coefficients) },
                { SpellName.UpMaxMana, new UpMaxManaSpell(playerMana, coefficients) }
            };

            return playerSpells;
        }

        public static Dictionary<EffectName, Effect> CreateEnemyEffectsDictionary(
            EnemyJumper enemyJumper, Health playerHealth, Damage enemyDamage, Health enemyHealth,
            Coefficients coefficients, EnemyTargetController enemyTargetController
        )
        {
            Dictionary<EffectName, Effect> enemyEffects = new()
            {
                { EffectName.Swords, new EnemySwords(enemyJumper, playerHealth, enemyDamage) },
                { EffectName.Health, new EnemyHealth(enemyJumper, enemyHealth, coefficients, enemyTargetController) },
                { EffectName.Mana, new EnemyMana(enemyJumper) },
                { EffectName.Portal, new EnemyPortal(enemyJumper) },
                { EffectName.Question, new EnemyQuestion(enemyJumper) },
                { EffectName.SpellBook, new EnemySpellBook(enemyJumper) }
            };

            return enemyEffects;
        }
    }
}