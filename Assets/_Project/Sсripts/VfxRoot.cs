using _Project.Sсripts.Animation;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Model.Effects;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts
{
    public class VfxRoot : MonoBehaviour
    {
        // player
        [field: SerializeField] public DamageVfx PlayerDamageVfx { get; private set; }
        [field: SerializeField] public ManaVfx PlayerManaVfx { get; private set; }
        [field: SerializeField] public TeleportVfx TeleportVfx { get; private set; }
        [field: SerializeField] public HealthReplenishVfx PlayerHealthReplenishVfx { get; private set; }

        // enemy
        [field: SerializeField] public DamageVfx EnemyDamageVfx { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(PlayerDamageVfx);
            Assert.IsNotNull(PlayerManaVfx);
            Assert.IsNotNull(TeleportVfx);
            Assert.IsNotNull(PlayerHealthReplenishVfx);

            Assert.IsNotNull(EnemyDamageVfx);
        }

        public void Initialize(Health playerHealth, Mana playerMana, PlayerPortal playerPortal, Health enemyHealth)
        {
            PlayerDamageVfx.Initialize(playerHealth);
            PlayerManaVfx.Initialize(playerMana);
            TeleportVfx.Initialize(playerPortal);
            EnemyDamageVfx.Initialize(enemyHealth);
            PlayerHealthReplenishVfx.Initialize(playerHealth);
        }
    }
}