
using Character;
using System;
using UnityEngine;

namespace Combat
{
	public abstract class AbstractDamageSource : MonoBehaviour
	{
		public AbstractCharacterManager Owner { get; protected set; }

		public int CharacterID { get => Owner.CharacterID;  }

		public TeamID CharacterTeamID { get => Owner.TeamID; }
    }

	public enum AffixType
	{
		Physical, Fire
	}

    public class DamageSourceCollisionEventArgs: EventArgs
	{
		public Collider HittingCollider;
		public AbstractDamageable Damageable;
		public Collider ColliderHit;
	}
}
