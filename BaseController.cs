using System.Collections.Generic;
using UnityEngine;

	public class BaseController : MonoBehaviour
	{
		private EnemyDetector _enemyDetector;

		protected List<Character> targetList;
		private EnemyDetector.detectionDelegate _detectEnemy;
		private EnemyDetector.detectionDelegate _unDetectEnemy;

		protected bool blockMove;

		protected Transform myTransform;

		private void Awake ()
		{
			myTransform = transform;
			_enemyDetector = GetComponentInChildren<EnemyDetector> ();
			if (_enemyDetector == null) {
				Debug.LogError ("BaseController: Didn't find an enemy detector component. This is required for gameplay to work.");
			} else {
				targetList = new List<Character> ();
				_detectEnemy = DetectEnemy;
				_unDetectEnemy = UnDetectEnemy;
				_enemyDetector.RegisterForDetection (_detectEnemy);
				_enemyDetector.RegisterForUnDetection (_unDetectEnemy);
			}
		}

		protected void OnDestroy ()
		{
			_enemyDetector.UnregisterFromDetection (_detectEnemy);
			_enemyDetector.UnregisterFromUnDetection (_unDetectEnemy);
		}

		private void DetectEnemy (GameObject enemy)
		{
			var enemyCharacter = enemy.GetComponent<Character> ();

			if (enemyCharacter == null)
				return;

			if (enemyCharacter.health <= 0.0f) {
				return;
			}
			targetList.Add (enemyCharacter);
		}

		private void UnDetectEnemy (GameObject enemy)
		{
			var enemyCharacter = enemy.GetComponent<Character> ();

			if (enemyCharacter == null)
				return;

			targetList.Remove (enemyCharacter);
		}
	}

