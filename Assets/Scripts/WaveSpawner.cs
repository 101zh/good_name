using UnityEngine;
using System.Collections;

public class WaveSpawnner : MonoBehaviour {

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave[] waves;
}
