using UnityEngine;

	[RequireComponent(typeof(Collider))]
	public class EnemyDetector : MonoBehaviour {

		public delegate void detectionDelegate(GameObject gameObject);

		private detectionDelegate detection;
		private detectionDelegate _unDetection;

		public void RegisterForDetection(detectionDelegate delegateFunction)
		{
			if (detection == null)
			{
				detection = delegateFunction;
			}
			else
			{
				detection += delegateFunction;
			}
		}

		public void RegisterForUnDetection(detectionDelegate delegateFunction)
		{
			if (_unDetection == null)
			{
				_unDetection = delegateFunction;
			}
			else
			{
				_unDetection += delegateFunction;
			}
		}

		public void UnregisterFromDetection(detectionDelegate delegateFunction)
		{
			if (detection == null)
				return;
			else
			{
				detection -= delegateFunction;
			}
		}

		public void UnregisterFromUnDetection(detectionDelegate delegateFunction)
		{
			if (_unDetection == null)
				return;
			else
			{
				_unDetection -= delegateFunction;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if(detection != null)
				detection(other.gameObject);
		}

		private void OnTriggerExit(Collider other)
		{
			if(_unDetection != null)
				_unDetection(other.gameObject);
		}
	}

