using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class PlayerController : BaseController
	{
		[Flags]
		private enum ControlOutput	// What the output after input processing is
		{
			NONE = 0,
			LEFT = 1 << 0,
			RIGHT = 1 << 1,
			UP = 1 << 2,
			DOWN = 1 << 3,
			BASIC_ATTACK = 1 << 4,
			SPECIAL_ATTACK1 = 1 << 5,
			SPECIAL_ATTACK2 = 1 << 6,
			SPECIAL_ATTACK3 = 1 << 7,
			SPECIAL_ATTACK4 = 1 << 8
		}
		
		private ControlOutput output { get; set; }	// Current processed input actions

		public static Character ControlledCharacter { get; private set; }

		private bool attacking;
		private bool queueAttack;
		
		private void Start ()
		{
			InitVariables ();
		}

		private IEnumerator Hit ()
		{
			yield return new WaitForSeconds(ControlledCharacter.attackSpeed);
			if (targetList.Count > 0)
			{
				if (Math.IsInRange(targetList[0].transform.position, transform.position,
					ControlledCharacter.attackRange))
					targetList[0].TakeDamage(ControlledCharacter.attackDamage);
			}

			blockMove = false;
			attacking = false;
			if (!queueAttack) yield break;
			ControlledCharacter.DoBasicAttack ();
			blockMove = true;
			attacking = true;
			StartCoroutine("Hit");
			queueAttack = false;
		}

		private void InitVariables ()
		{
			ControlledCharacter = GetComponent<Character> ();
			blockMove = false;
			queueAttack = false;
			attacking = false;
		}

		private new void OnDestroy ()
		{
			base.OnDestroy ();
		}

		private void Update()
		{
			output = ControlOutput.NONE;

			// Move up
			if (Input.GetKey(KeyCode.W))
			{
				output |= ControlOutput.UP;
			}

			// Move down
			if (Input.GetKey(KeyCode.S))
			{
				output |= ControlOutput.DOWN;
			}

			// Move left
			if (Input.GetKey(KeyCode.A))
			{
				output |= ControlOutput.LEFT;
			}

			// Move right
			if (Input.GetKey(KeyCode.D))
			{
				output |= ControlOutput.RIGHT;
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				output |= ControlOutput.SPECIAL_ATTACK1;
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha2))
			{
				output |= ControlOutput.SPECIAL_ATTACK2;
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha3))
			{
				output |= ControlOutput.SPECIAL_ATTACK3;
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha4))
			{
				output |= ControlOutput.SPECIAL_ATTACK4;
			}

			// Basic attack
			if (Input.GetMouseButtonUp(0))
			{
				output |= ControlOutput.BASIC_ATTACK;
			}
		}
		private void LateUpdate ()
		{
			var moveRotation = new Vector3 (0.0f, 0.0f, 0.0f);
			var up = new Vector3 (0.0f, 0.0f, 1.0f);
			var right = new Vector3 (1.0f, 0.0f, 0.0f);

			var inCombat = targetList.Count > 0;

			var moveSpeed = inCombat ? ControlledCharacter.walkSpeed : ControlledCharacter.runSpeed;
			moveSpeed = blockMove ? 0.0f : moveSpeed;
			if ((output & ControlOutput.BASIC_ATTACK) != ControlOutput.NONE) {
				ControlledCharacter.DoBasicAttack ();
				if (!attacking)
				{
					blockMove = true;
					attacking = true;
					StartCoroutine("Hit");
				}
				else
				{
					queueAttack = true;
				}
			}

			if ((output & ControlOutput.SPECIAL_ATTACK1) != ControlOutput.NONE)
			{
				ControlledCharacter.AbilityPriming(1);
			}
			
			if ((output & ControlOutput.SPECIAL_ATTACK2) != ControlOutput.NONE)
			{
				ControlledCharacter.AbilityPriming(2);
			}
			
			if ((output & ControlOutput.SPECIAL_ATTACK3) != ControlOutput.NONE)
			{
				ControlledCharacter.AbilityPriming(3);
			}
			
			if ((output & ControlOutput.SPECIAL_ATTACK4) != ControlOutput.NONE)
			{
				ControlledCharacter.AbilityPriming(4);
			}

			var movementHappening = false;
			var movement = new Vector3 (0.0f, 0.0f, 0.0f);
			if ((output & ControlOutput.UP) != ControlOutput.NONE) {
				movement += up;
				moveRotation += up;
				movementHappening = true;
			}

			if ((output & ControlOutput.DOWN) != ControlOutput.NONE) {
				movement -= up;
				moveRotation -= up;
				movementHappening = true;
			}

			if ((output & ControlOutput.LEFT) != ControlOutput.NONE) {
				movement -= right;
				moveRotation -= right;
				movementHappening = true;
			}

			if ((output & ControlOutput.RIGHT) != ControlOutput.NONE) {
				movement += right;
				moveRotation += right;
				movementHappening = true;
			}

			movement.Normalize ();
			movement *= moveSpeed * Time.deltaTime;
			if(!blockMove)
				myTransform.position += movement;

			if (queueAttack && movementHappening)
				queueAttack = false;

			if (moveRotation == Vector3.zero) {
				moveRotation = myTransform.forward;
				ControlledCharacter.SetMoveSpeed (0.0f);
			} else {
				ControlledCharacter.SetMoveSpeed (ControlledCharacter.walkSpeed);
			}
		
			var targetPosition = new Vector3 (myTransform.position.x + moveRotation.x,
				myTransform.position.y,
				myTransform.position.z + moveRotation.z);

			if (!inCombat) {
				ControlledCharacter.SetMoveDirection (0.0f);
				ControlledCharacter.SetInCombat (false);
				if (!blockMove)
				{
					var lookDir = targetPosition - myTransform.position;
					var lookRotation = Quaternion.LookRotation (lookDir);
					myTransform.rotation = Quaternion.RotateTowards (myTransform.rotation, lookRotation, Time.deltaTime * ControlledCharacter.turnRate);
				}
			} else {
				var lookTarget = new Vector3 (targetList [0].transform.position.x,
					myTransform.position.y,
					targetList [0].transform.position.z);
				//myTransform.LookAt(lookTarget);

				var lookDir = lookTarget - myTransform.position;
				var lookRotation = Quaternion.LookRotation (lookDir);
				myTransform.rotation = Quaternion.RotateTowards (myTransform.rotation, lookRotation, Time.deltaTime * ControlledCharacter.turnRate);

				var lookVector = lookTarget - myTransform.position;
				lookVector.Normalize ();
				var moveVector = targetPosition - myTransform.position;
				moveVector.Normalize ();
			
				var dirAngle = Vector3.Angle (moveVector, lookVector);

				var rightVector = Vector3.Cross (Vector3.up, lookVector);			
				var sign = (Vector3.Dot (moveVector, rightVector) > 0.0f) ? 1.0f : -1.0f;
 
				var moveDir = sign * dirAngle;

				ControlledCharacter.SetMoveDirection (moveDir);
				ControlledCharacter.SetInCombat (true);
			}

			SortTargetList ();
		}

		private void SortTargetList ()
		{
			targetList.RemoveAll (IsDead);
			targetList.Sort ((unit1, unit2) => (myTransform.position - unit1.transform.position).sqrMagnitude.CompareTo ((myTransform.position - unit2.transform.position).sqrMagnitude));
		}

		private static bool IsDead (Character c)
		{
			return c.health <= 0.0f;
		}
	}

