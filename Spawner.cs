using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Spawner : MonoBehaviour
	{

		[Serializable]
		public class SpawnType
		{
			public GameObject	spawnedPrefab;
			public float spawnChance;
			// As a portion between 0-1
		}

		public float spawnInterval;
		public int maxSpawns;
		public bool spawnsWhenVisible;
		public int spawnsBeforeFinished;
		public bool spawnOnAwake;

		[SerializeField]
		public SpawnType[] spawns;

		private int currentNumOfSpawns;

		private bool isVisible;
		private bool finishedSpawning;
		private int numberSpawned;

		private List<GameObject> spawnedSpawns;

		private void Awake ()
		{
			spawnedSpawns = new List<GameObject>();
			finishedSpawning = true;
			if (spawnOnAwake) {
				StartSpawning ();
			}
		}

		private void StartSpawning ()
		{
			finishedSpawning = false;
			StartCoroutine("Spawn");
		}

		public bool HasBeenCleared ()
		{
			return finishedSpawning && numberSpawned > 0 && currentNumOfSpawns <= 0;
		}

		private IEnumerator Spawn ()
		{
			yield return new WaitForSeconds(spawnInterval);
			while (currentNumOfSpawns >= maxSpawns)
				yield return new WaitForSeconds(spawnInterval);

			while (isVisible && !spawnsWhenVisible) {
				yield return new WaitForSeconds(spawnInterval);
			}

			if (finishedSpawning) {
				StopCoroutine("Spawn");
				yield return null;
			}

			if (numberSpawned >= spawnsBeforeFinished) {
				finishedSpawning = true;
				StopCoroutine("Spawn");
				yield return null;
			}

			if (spawns.Length <= 0) {
				Debug.LogWarning ("Spawner has no spawn types setup.");
				StopCoroutine("Spawn");
				yield return null;
			}

			var spawn = Instantiate(spawns[0].spawnedPrefab, transform.position, transform.rotation);
			spawnedSpawns.Add(spawn);
			currentNumOfSpawns++;
			numberSpawned++;
			StartCoroutine("Spawn");
		}

		private void LateUpdate()
		{
			if (currentNumOfSpawns <= 0)
				return;
			CheckSpawnStatus();
		}

		private void CheckSpawnStatus()
		{
			foreach (var spawn in spawnedSpawns)
			{
				if (!spawn) continue;
				var character = spawn.GetComponent<Character>();
				if (!character) continue;
				if (!character.isDead)
				{
					return;
				}
			}

			currentNumOfSpawns = -1;
		}

		private void OnBecameVisible ()
		{
			isVisible = true;
		}

		private void OnBecameInvisible ()
		{
			isVisible = false;
		}
	}

