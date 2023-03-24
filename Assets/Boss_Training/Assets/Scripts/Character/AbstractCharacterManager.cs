
using Ability;
using Animation;
using Player;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public abstract class AbstractCharacterManager : MonoBehaviour
    {
        [Header("Character ID. Use Context Menu to assign")]
        [SerializeField] int _characterID;

        [Header("Base Character Parameters")]
        [SerializeField] protected Transform _lockOnTransform;

        [SerializeField]
        private AbstractDamageable _damageHandler;

        [SerializeField] protected WeaponSlotManager _weaponSlotManager;
        [SerializeField] protected AbstractCharacter _stats;
        [SerializeField] protected CharacterVfxController _characterVfxController;
        [SerializeField] protected AbstractAnimatorController _animatorController;
        [SerializeField] protected AbilityHandler _abilityHandler;
        [SerializeField] protected CharacterAttacker _characterAttacker;
        [SerializeField] protected PlayerInventory _playerInventory;
        [SerializeField] private TeamID teamID;



		public bool IsInAir { get; set; }
        public bool RollFlag { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsRunning { get; set; }

        public Transform RangedAttackTarget { get; set; }

        public AbstractCharacter Stats { get => _stats; }
        public CharacterVfxController CharacterVfxController { get => _characterVfxController; }
        public AbstractAnimatorController AnimatorController { get => _animatorController; }
        public Transform LockOnTransform { get => _lockOnTransform; }
        public WeaponSlotManager WeaponSlotManager { get => _weaponSlotManager; }
        public AbilityHandler AbilityHandler { get => _abilityHandler; }
        public CharacterAttacker CharacterAttacker { get => _characterAttacker; }
        public PlayerInventory Inventory { get => _playerInventory; }

        public bool IsInteracting { get => _animatorController.IsInteracting; }

        public bool IsDead { get; protected set; }
        public int CharacterID { get => _characterID; set => _characterID = value; }
        public TeamID TeamID { get => teamID; protected set => teamID = value; }
        public bool IsTargetLocked { get => _isTargetLocked; }
        public AbstractDamageable DamageHandler { get => _damageHandler; }

        private bool _isTargetLocked;

        public EventHandler<GenericAbstractCharacterManagerEvent> CharacterDied;
        public EventHandler<GenericAbstractCharacterManagerEvent> TargetLockedUpdated;
        public EventHandler<LoadedAbilityEventArgs> LoadedAbilityEvent;
        public EventHandler<FiredAbilityEventArgs> FiredAbilityEvent;

        virtual protected void Awake()
        {
            if (CharacterID == 0)
            {
                Debug.Log("INVALID CHARACTER ID" + gameObject.name);
                CharacterID = GetHashCode();
            }

            _damageHandler.CharacterID = CharacterID;
            _damageHandler.TeamID = TeamID;
        }

        private void Reset()
        {
            if (CharacterID == 0)
            {
                CharacterID = GetHashCode();
            }

            _damageHandler = gameObject.GetComponentInChildren<AbstractDamageable>();
            _weaponSlotManager = gameObject.GetComponentInChildren<WeaponSlotManager>();
            _stats = gameObject.GetComponentInChildren<AbstractCharacter>();
            _characterVfxController = gameObject.GetComponentInChildren<CharacterVfxController>();
            _animatorController = gameObject.GetComponentInChildren<AbstractAnimatorController>();
            _abilityHandler = gameObject.GetComponentInChildren<AbilityHandler>();
            _characterAttacker = gameObject.GetComponentInChildren<CharacterAttacker>();
            _playerInventory = gameObject.GetComponentInChildren<PlayerInventory>();
        }

        [ContextMenu("SetID")]
        public void SetID()
        {
            CharacterID = GetHashCode();
        }

        public void SetTargetLock(bool locked)
        {
            _isTargetLocked = locked;
            TargetLockedUpdated?.Invoke(this, new GenericAbstractCharacterManagerEvent() { CharacterManager = this });
        }

        internal void SendFiredAbilityEvent(GameObject abilityGameObject)
		{
            FiredAbilityEvent?.Invoke(this, new FiredAbilityEventArgs() { AbilityGameObject = abilityGameObject, CharacterId = this.CharacterID });
        }

        internal void SendLoadedAbilityEvent(int abilityNumber)
        {
            LoadedAbilityEvent?.Invoke(this, new LoadedAbilityEventArgs() { AbilityNumber = abilityNumber });
        }
    }

    public enum TeamID
    {
        Player, NPC, Envrioment
    }

    public class GenericAbstractCharacterManagerEvent : EventArgs
    {
        public AbstractCharacterManager CharacterManager;
    }
    public class LoadedAbilityEventArgs : EventArgs
    {
        public int AbilityNumber;
    }

    public class FiredAbilityEventArgs : EventArgs
    {
        public GameObject AbilityGameObject;
        public int CharacterId; 
    }
}
