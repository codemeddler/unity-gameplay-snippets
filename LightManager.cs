using UnityEngine;

	public class LightManager : MonoBehaviour {

		public bool startLight;

		public Light sun;

		public Color darkAmbientColor;
		public Color darkFogColor;
		public Color darkLightColor;
		public Color lightAmbientColor;
		public Color lightFogColor;
		public Color lightLightColor;

		private Color targetFogColor;
		private float targetFogDensity;
		private Color targetAmbientColor;
		private Color targetLightColor;

		private static LightManager Instance { get; set; }

		private void Start()
		{
			if (Instance != null)
			{
				Debug.LogWarning("LightManager instance already exists. Destroying " + gameObject + " because of this.");
				Destroy(gameObject);
				return;
			}

			Instance = this;

			if (startLight)
			{			
				EnableLightness();
			}
			else
			{
				EnableDarkness();
			}

			sun.color = targetLightColor;
			RenderSettings.ambientLight = targetAmbientColor;
			RenderSettings.fogDensity = targetFogDensity;
			RenderSettings.fogColor = targetFogColor;
		}

		private void EnableDarkness()
		{
			targetLightColor = darkLightColor;
			targetFogColor = darkFogColor;
			targetFogDensity = 0.04f;
			targetAmbientColor = darkAmbientColor;
		}

		private void EnableLightness()
		{
			targetLightColor = lightLightColor;
			targetFogColor = lightFogColor;
			targetFogDensity = 0.03f;
			targetAmbientColor = lightAmbientColor;
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.O))
				EnableDarkness();
			if (Input.GetKey(KeyCode.P))
				EnableLightness();

			if (sun.color != targetLightColor)
				sun.color = Color.Lerp(sun.color, targetLightColor, Time.deltaTime);
			if (RenderSettings.fogColor != targetFogColor)
				RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, Time.deltaTime);
			if (RenderSettings.fogDensity != targetFogDensity)
				RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime);
			if (RenderSettings.ambientLight != targetAmbientColor)
				RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetAmbientColor, Time.deltaTime);
		}
	}

