using UnityEngine;


	public class Cine : MonoBehaviour
	{

		public Camera cam;

		public static Cine Instance { get; private set; }

		private const float height = 0.8f;
		private const float yoffset = 0.1f;

		private bool fadeIn;
		private bool fadeOut;
		private float timer;

		private void Start ()
		{
			Instance = this;
			fadeIn = false;
			fadeOut = false;
			timer = 0.0f;
		}

		public void PlayCine ()
		{
			fadeIn = true;
			fadeOut = false;
			timer = 1.0f;
		}

		public void EndCine ()
		{
			fadeOut = true;
			fadeIn = false;
			timer = 1.0f;
		}

		private void LateUpdate ()
		{
			var lerptime = Time.deltaTime * 7.5f;
			if (fadeIn) {
				var newRect = new Rect (0, Mathf.Lerp (cam.rect.y, yoffset, lerptime), 1.0f, Mathf.Lerp (cam.rect.height, height, lerptime));
				cam.rect = newRect;
			} else if (fadeOut) {
				var newRect = new Rect (0, Mathf.Lerp (cam.rect.y, 0.0f, lerptime), 1.0f, Mathf.Lerp (cam.rect.height, 1.0f, lerptime));
				cam.rect = newRect;
			}
			timer -= Time.deltaTime;
			if (timer <= 0.0f) {
				if (fadeIn)
					fadeIn = false;
				else if (fadeOut)
					fadeOut = false;
			}
		}
	}

