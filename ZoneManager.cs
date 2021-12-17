using System;
using System.Collections.Generic;
using UnityEngine;

	public class ZoneManager : MonoBehaviour
	{

		[Serializable]
		public class Zone
		{
			public bool startLit;
			public Light areaLight;
			public float lightIntensityWhenLit;
			public Spawner[] spawners;

			private float targetIntensity;

			public void Lighten ()
			{
				targetIntensity = lightIntensityWhenLit;
			}

			public void Darken ()
			{
				targetIntensity = 0.0f;
			}

			public bool IsCleared ()
			{
				var allClear = true;
				foreach (var spawner in spawners) {
					if (!spawner.HasBeenCleared ())
						allClear = false;
					break;
				}
				return allClear;
			}

			public void OnUpdate (float deltaTime)
			{
				if (areaLight.intensity != targetIntensity) {
					areaLight.intensity = Mathf.Lerp (areaLight.intensity, targetIntensity, deltaTime);
				}
			}
		}
		private static ZoneManager Instance { get; set; }

		[SerializeField]
		public List<Zone> zones;

		private void Start ()
		{
			if (Instance != null) {
				Debug.LogWarning ("ZoneManager instance already exists. Destroying " + gameObject + " because of this.");
				Destroy (gameObject);
				return;
			}

			Instance = this;

			foreach (var zone in zones) {
				if (zone.startLit) {
					zone.areaLight.intensity = zone.lightIntensityWhenLit;
					zone.Lighten ();
				} else {
					zone.areaLight.intensity = 0.0f;
					zone.Darken ();
				}
			}
		}

		public void DarkenZone (int index)
		{
			zones [index].Darken ();
		}

		public void LightenZone (int index)
		{
			zones [index].Lighten ();
		}

		private void LateUpdate ()
		{
			foreach (var zone in zones) {
				if (zone.IsCleared ())
					zone.Lighten ();
				zone.OnUpdate (Time.deltaTime);
			}
		}
	}

