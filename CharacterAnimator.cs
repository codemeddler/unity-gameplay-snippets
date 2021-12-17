using UnityEngine;

	/// <summary>
	/// Handles the animaton part of a Character
	/// </summary>
	public class CharacterAnimator : MonoBehaviour
	{

		// Enumeration for locomotion modes
		public enum LocomotionMode
		{
			BANK,
			STRAFE
		};

		// Properties
		public LocomotionMode locomotionMode;

		// Variables
		private int deadControl;

		// Holds the hash to control the dead boolean
		private int speedControl;

		// Holds the hash to control the speed float
		private int attackControl;

		// Holds the hash to control attacks
		private int moveDirControl;

		// Holds the hash to control moveDir
		private int combatControl;

		//Hold the hash to control special abilities
		private int abilityControl;

		// Cached components
		private Animator myAnimator;

		private void Start()
		{
			myAnimator = GetComponent<Animator>();

			if (myAnimator == null)
			{
				myAnimator = GetComponentInChildren<Animator>();
			}

			if (myAnimator == null)
			{
				Debug.LogError(gameObject +
				               " has no Animator component in it or it's children. Destroying it because of this");
				Destroy(gameObject);
			}

			CacheAnimations();
		}

		/// <summary>
		/// Fetches all control hashes
		/// </summary>
		private void CacheAnimations()
		{
			deadControl = Animator.StringToHash("isDead");
			speedControl = Animator.StringToHash("moveSpeed");
			attackControl = Animator.StringToHash("doAttack");
			combatControl = Animator.StringToHash("inCombat");
			if (locomotionMode == LocomotionMode.STRAFE)
				moveDirControl = Animator.StringToHash("moveDir");
			abilityControl = Animator.StringToHash("ability");
		}

		/// <summary>
		/// Called when the character this animator belongs to dies
		/// </summary>
		public void OnDead()
		{
			myAnimator.SetBool(deadControl, true);
		}

		/// <summary>
		/// Called while the character this animator belongs to is dead
		/// </summary>
		public void DuringDead()
		{
			myAnimator.SetBool(deadControl, false);
		}

		/// <summary>
		/// Used to update the speed variable
		/// </summary>
		/// <param name="speed">The speed to move at</param>
		public void UpdateSpeed(float speed)
		{
			myAnimator.SetFloat(speedControl, speed);
		}

		public void UpdateAttack(bool state)
		{
			myAnimator.SetBool(attackControl, state);
		}

		public void UpdateMoveDir(float angle)
		{
			myAnimator.SetFloat(moveDirControl, angle);
		}

		public void UpdateInCombat(bool state)
		{
			myAnimator.SetBool(combatControl, state);
		}

		public void UpdateAbility(int ability)
		{
			myAnimator.SetInteger(abilityControl, ability);
		}
	}

