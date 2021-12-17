using System.Collections;
using UnityEngine;

	public class TimedDestroy : MonoBehaviour {

		public float timeUntilDestroy;

		private void Start()
		{
			StartCoroutine(Destroy());
		}

		private IEnumerator Destroy()
		{
			yield return new WaitForSeconds(timeUntilDestroy);
			Destroy(gameObject);
		}
	}

