
using Character;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class MlDemo_GameUI : MonoBehaviour
{
    [SerializeField] StaminaBar _playerStaminaBar;
    [SerializeField] ManaBar _playerManaBar;
    [SerializeField] HealthBar _playerHealthBar;

    [SerializeField] HealthBar _bossHealthBar;
    [SerializeField] AbstractCharacterManager _player;
    [SerializeField] AbstractCharacterManager _boss;


    void Start()
    {
        _playerManaBar.AssignNewPlayerManager(_player);
        _playerStaminaBar.AssignNewPlayerManager(_player);
        _playerHealthBar.AssignNewPlayerManager(_player);

        _bossHealthBar.AssignNewPlayerManager(_boss);
    }



}
