
using Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public abstract class AbstractCharacter : MonoBehaviour
    {

        [SerializeField] AbstractCharacterManager _characterManager;

        [Header("Stats")]
        [SerializeField] int _vitality = 10;
        [SerializeField] int _endurance = 10;
        [SerializeField] int _willPower = 10;

        [Header("Stamina")]
        [SerializeField] float _staminaRegenerationRatePerSecond = 45;
        [SerializeField] float _timeBeforeStaminaRegeneration = 1.5f;

        [Header("Poise")]
        [SerializeField] int _poise = 50;
        [SerializeField] float _timeBeforePoiseRegeration = 4f;

        public int CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }
        public float CurrentStamina { get; private set; }
        public int CurrentPoise { get; private set; }
        public int CurrentMana { get; private set; }
        public int MaxMana { get => _maxMana; }
        public int MaxHealth { get => _maxHealth; }
        public float MaxStamina { get => _maxStamina; }
        public float CurrentHealthPercent { get => CurrentHealth / MaxHealth; }
        public float CurrenStaminaPercent { get => CurrentStamina / MaxStamina; }
        public float CurrentManaPercent { get => CurrentMana / MaxMana; }

        public EventHandler<HealthUpdatedEventArgs> HealthUpdatedEvent;
        public EventHandler<StaminaUpdatedEventArgs> StaminaUpdatedEvent;
        public EventHandler<ManaUpdatedEventArgs> ManaUpdatedEvent;
        public EventHandler<DamageTakenEventArgs> DamageTakenEvent;
        public EventHandler<EventArgs> CharacterDied;
        public EventHandler<PoiseLostEventArgs> PoiseLostEvent;


        private float _maxStamina;
        private int _maxHealth;
        private float _timeSinceLastStaminaUse;
        private float _timeSinceLastPoiseLoss;
        private int _maxMana;

        private void Awake()
        {
            SetStartingStats();
        }


        public void SetStartingStats()
        {
            IsDead = false;
            SetMaxHealthFromFormula();
            SetMaxStamina();
            SetMaxMana();

            SetCurrentHealthToValue(_maxHealth);
            SetCurrentStamina(_maxStamina);
            SetCurrentMana(_maxMana);
            CurrentPoise = _poise;
        }

        private void FixedUpdate()
        {
            if (CurrentStamina < _maxStamina && !_characterManager.IsInteracting)
            {
                _timeSinceLastStaminaUse = _timeSinceLastStaminaUse += Time.deltaTime;
                if (_timeSinceLastStaminaUse >= _timeBeforeStaminaRegeneration)
                {
                    float staminaGain = _staminaRegenerationRatePerSecond * Time.deltaTime;
                    GainStamina(staminaGain);
                }
            }
            if (CurrentPoise < _poise)
            {
                _timeSinceLastPoiseLoss = _timeSinceLastPoiseLoss += Time.deltaTime;
                if (_timeSinceLastPoiseLoss >= _timeBeforePoiseRegeration)
                {
                    CurrentPoise = _poise;
                    _timeSinceLastPoiseLoss = 0;
                }
            }
        }

        private void GainStamina(float staminaGain)
        {
            CurrentStamina = Mathf.Min(staminaGain + CurrentStamina, _maxStamina);
            StaminaUpdatedEvent?.Invoke(this, new StaminaUpdatedEventArgs() { MaxStamina = _maxStamina, CurrentStamina = CurrentStamina });
        }

        private void SetMaxHealthFromFormula()
        {
            _maxHealth = _vitality * 10;
        }

        private void SetMaxStamina()
        {
            _maxStamina = _endurance * 10;
        }

        private void SetMaxMana()
        {
            _maxMana = _willPower;
        }

        public void SetCurrentHealthToValue(int health)
        {
            CurrentHealth = health;
            if (CurrentHealth <= 0 && !IsDead)
            {
                CurrentHealth = 0;
                CharacterDied?.Invoke(this, EventArgs.Empty);
                IsDead = true;
            }

            HealthUpdatedEvent?.Invoke(this, new HealthUpdatedEventArgs() { MaxHealth = _maxHealth, CurrentHealth = CurrentHealth });
        }

        private void SetCurrentStamina(float newStamina)
        {
            CurrentStamina = newStamina;
            StaminaUpdatedEvent?.Invoke(this, new StaminaUpdatedEventArgs() { MaxStamina = _maxStamina, CurrentStamina = CurrentStamina });
        }

        private void SetCurrentMana(int newMana)
        {
            CurrentMana = newMana;
            ManaUpdatedEvent?.Invoke(this, new ManaUpdatedEventArgs() { MaxMana = _maxMana, CurrentMana = CurrentMana });
        }

        public void TakeDamage(int damage, AbstractDamageSource source = null)
        {
            int newHealth = CurrentHealth - damage;
            DamageTakenEvent?.Invoke(source == null ? this : source, new DamageTakenEventArgs() { DamageTaken = damage });
            SetCurrentHealthToValue(newHealth);

        }

        public void TakePercentDamage(float percent)
        {
            int damage = (int)(MaxHealth * percent);
            int newHealth = CurrentHealth - damage;


            DamageTakenEvent?.Invoke(this, new DamageTakenEventArgs() { DamageTaken = damage });
            SetCurrentHealthToValue(newHealth);
        }


        public void LosePoise(int poiseLoss)
        {
            int newPoise = CurrentPoise - poiseLoss;
            PoiseLostEvent?.Invoke(this, new PoiseLostEventArgs() { PoiseLost = poiseLoss });

            CurrentPoise = newPoise;
            _timeSinceLastPoiseLoss = 0;
        }

        public void UseStamina(float staminaUsed)
        {
            float newStamina = CurrentStamina - staminaUsed;
            SetCurrentStamina(newStamina);
            _timeSinceLastStaminaUse = 0f;
        }

        public void UseMana(int manaUsed)
        {
            int newMana = CurrentMana - manaUsed;
            if (newMana < 0)
            {
                newMana = 0;
            }
            SetCurrentMana(newMana);
        }

        public void ResetStats()
        {
            SetCurrentHealthToValue(_maxHealth);
            IsDead = false;
        }
    }


    public class HealthUpdatedEventArgs : EventArgs
    {
        public int MaxHealth;
        public int CurrentHealth;
    }

    public class StaminaUpdatedEventArgs : EventArgs
    {
        public float MaxStamina;
        public float CurrentStamina;
    }

    public class ManaUpdatedEventArgs : EventArgs
    {
        public float MaxMana;
        public float CurrentMana;
    }

    public class DamageTakenEventArgs : EventArgs
    {
        public int DamageTaken;
    }

    public class PoiseLostEventArgs : EventArgs
    {
        public int PoiseLost;
    }
}