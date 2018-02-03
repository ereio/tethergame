using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class RandomTween : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (doThis ());
	}
	
	// Update is called once per frame
	IEnumerator doThis(){
		while (true) {
			yield return StartCoroutine(HOTween.To(this.transform, Random.Range(5f,20f), "position", 
			                                       new Vector3(Random.Range(-150,100),Random.Range(-50,50),Random.Range(-50,50)), true).WaitForCompletion());
			if(transform.position.y < 0f)
				makeSquare();
		}
	}

	IEnumerator makeSquare(){
		yield return StartCoroutine(HOTween.To(this.transform, 5f, "position", new Vector3(0,20,0), true).WaitForCompletion());
	}
}
