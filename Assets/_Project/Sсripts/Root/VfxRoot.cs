using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Effects.Player;
using _Project.Sсripts.View.Animation;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Root
{
    public class VfxRoot : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private DamageVfx _playerDamageVfx;
        [SerializeField] private ManaVfx _playerManaVfx;
        [SerializeField] private TeleportVfx _teleportVfx;
        [SerializeField] private HealthReplenishVfx _playerHealthReplenishVfx;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] private DamageVfx _enemyDamageVfx;

        private void Awake()
        {
            Assert.IsNotNull(_playerDamageVfx);
            Assert.IsNotNull(_playerManaVfx);
            Assert.IsNotNull(_teleportVfx);
            Assert.IsNotNull(_playerHealthReplenishVfx);

            Assert.IsNotNull(_enemyDamageVfx);
        }

        public void Initialize(Health playerHealth, Mana playerMana, PlayerPortal playerPortal, Health enemyHealth)
        {
            _playerDamageVfx.Initialize(playerHealth);
            _playerManaVfx.Initialize(playerMana);
            _teleportVfx.Initialize(playerPortal);
            _enemyDamageVfx.Initialize(enemyHealth);
            _playerHealthReplenishVfx.Initialize(playerHealth);
        }
    }
}