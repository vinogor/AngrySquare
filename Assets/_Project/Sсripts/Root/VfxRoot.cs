using Domain;
using Domain.Effects.Player;
using NaughtyAttributes;
using UnityEngine;
using View.Vfx;

namespace Root
{
    public class VfxRoot : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] [Required] private DamageVfx _playerDamageVfx;
        [SerializeField] [Required] private ManaVfx _playerManaVfx;
        [SerializeField] [Required] private TeleportVfx _teleportVfx;
        [SerializeField] [Required] private HealthReplenishVfx _playerHealthReplenishVfx;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private DamageVfx _enemyDamageVfx;
        [SerializeField] [Required] private HealthReplenishVfx _enemyHealthReplenishVfx;

        public void Initialize(Health playerHealth, Mana playerMana, PlayerPortal playerPortal, Health enemyHealth)
        {
            _playerDamageVfx.Initialize(playerHealth);
            _playerManaVfx.Initialize(playerMana);
            _teleportVfx.Initialize(playerPortal);
            _enemyDamageVfx.Initialize(enemyHealth);
            _playerHealthReplenishVfx.Initialize(playerHealth);
            _enemyHealthReplenishVfx.Initialize(enemyHealth);
        }
    }
}