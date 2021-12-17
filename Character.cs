using System;
using System.Collections;
using UnityEngine;

	/// <summary>
	/// Base class for characters
	/// </summary>
	[RequireComponent (typeof(CharacterAnimator))]
	public class Character : MonoBehaviour
	{
		[Serializable]
		public struct Ability
		{
			public float castTime;
			public float primeTime;
		}
		
		// Variables
		public float turnRate;
		// How fast the character turns
		public float accelerationRate;
		// How fast the character accelerates
		public float walkSpeed;
		// How fast the character walks
		public float runSpeed;
		// How fast the character runs
		public float health;
		// How much health the character has TODO: Use some sort of complexer stats
		public float attackRange;
		// How far the character can attack TODO: Use some sort of complexer stats
		public float attackDamage;
		// How much damage the character does TODO: Use some sort of complexer stats
		public float attackSpeed;

		public GameObject textPrefab;
		public AnimationCurve deathRamp;
		public Material deathMaterial;
		public Renderer myModel;

		public Ability[] abilities;
		
		// Properties
		public bool						isDead {				// Is the character dead
			get;
			private set;
		}

		// Cached components
		private CharacterAnimator myAnimator;

		private void Start ()
		{
			InitVariables ();		
		}

		/// <summary>
		/// Caches components and initializes variables
		/// </summary>
		private void InitVariables ()
		{
			isDead = false;
			myAnimator = GetComponent<CharacterAnimator> ();
		}

		/// <summary>
		/// Sets move speed to given value
		/// </summary>
		/// <param name="value"></param>
		public void SetMoveSpeed (float value)
		{
			myAnimator.UpdateSpeed (value);
		}

		public void AbilityPriming(int ability)
		{
			if (ability > abilities.Length) return;

			if (abilities[ability - 1].primeTime <= 0.0f)
			{
				DoAbility(ability);
			}
		}

		void DoAbility(int ability)
		{
			myAnimator.UpdateAbility(ability);
		}
		
		public void DoBasicAttack ()
		{
			myAnimator.UpdateAttack (true);
		}

		public void StopBasicAttack ()
		{
			myAnimator.UpdateAttack (false);
		}

		public void SetInCombat (bool status)
		{
			myAnimator.UpdateInCombat (status);
		}

		public void SetMoveDirection (float angle)
		{
			myAnimator.UpdateMoveDir (angle);
		}

		public void TakeDamage (float amount)
		{
			health -= amount;
			var spawnPos = transform.position;
			spawnPos.y += 1.2f;
			var TextObject = Instantiate(textPrefab, spawnPos, Quaternion.identity);
			TextObject.GetComponent<CombatText>().SetInitialValues(spawnPos, amount.ToString());
		}

		private void Update ()
		{
			if (isDead) {
				myAnimator.DuringDead ();
				return;
			}

			if (!(health <= 0.0f)) return;
			isDead = true;
			myAnimator.OnDead ();
			Death();
		}

		private void Death()
		{
			myModel.material = deathMaterial;
			StartCoroutine("DeathEffectAnim");
		}

		private IEnumerator DeathEffectAnim()
		{
			var anim = 1.0f;
			var time = 0.0f;
			myModel.material.SetFloat("_Progress", anim);
			do
			{
				time += Time.deltaTime;
				anim = deathRamp.Evaluate(time);;
				myModel.material.SetFloat("_Progress", anim);
				yield return null;
			} while (anim > 0.0f);
			StopAllCoroutines();
			Destroy(gameObject);
		}
	}

