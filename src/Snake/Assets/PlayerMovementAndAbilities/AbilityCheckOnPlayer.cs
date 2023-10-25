using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCheckOnPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<AbilityCheck>(out AbilityCheck abilityCheck))
        {
            if (abilityCheck._thisItem == Ability.speedBoost)
            {
                _player.GetComponent<PlayerMovement>().SetAbility(Ability.speedBoost);
            }
            if (abilityCheck._thisItem == Ability.stone)
            {
                _player.GetComponent<PlayerMovement>().SetAbility(Ability.stone);
            }
            if (abilityCheck._thisItem == Ability.shield)
            {
                _player.GetComponent<PlayerMovement>().SetAbility(Ability.shield);
            }
            if (abilityCheck._thisItem == Ability.plusPoints)
            {
                // ƒобавить к текущему количеству очков столько, сколько надо за предмет с доп очками (ещЄ, вроде, зме€ вырастает)
            }
            Destroy(collision.gameObject);
        }
    }
}
