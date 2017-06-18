using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTimeout : MonoBehaviour {


	public GameObject Minion;
	public GameObject Thunder;
	public float killsphere;
    public int type;
	private Animation anim;
	private Vector3 Ray_origin;
	private Vector3 Ray_direction;
	private ClickHandler clickHandler;
    private int CountToDeath = 0;

	private Thor_GameControl gameController;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		GameObject clickHandlerObject = GameObject.FindGameObjectWithTag ("Click");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Thor_GameControl>();
			clickHandler = clickHandlerObject.GetComponent <ClickHandler> ();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
		StartCoroutine(CountDownExplode());
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform);
		if (gameController.ThorUltimate ()) {
			Quaternion spawnRotation = Quaternion.identity;
			GameObject spawnedHazard = Instantiate (Thunder, transform.position, spawnRotation);
			StartCoroutine(KilledByThunder());
            
        }
		if (clickHandler.fire) {
			Ray_origin = clickHandler.rayOrigin;
			Ray_direction = clickHandler.shootDirection;
			Vector3 va = transform.position - Ray_origin;
			Ray_direction = Camera.main.transform.forward;
			Vector3 vb = Ray_direction / Ray_direction.magnitude;
			float dist = Vector3.Cross (va, vb).magnitude;
            if (dist < killsphere){
                CountToDeath++;
                StartCoroutine(KilledByLaser()); 
            }
            
        }
	}

	IEnumerator CountDownExplode (){
		yield return new WaitForSeconds(22);
		Destroy (Minion);
	}

    IEnumerator KilledByThunder()
    {
        anim.Play();
        yield return new WaitForSeconds(anim.clip.length);
        gameController.AddScore(1);
        Destroy(Minion);
    }

    IEnumerator KilledByLaser()
    {
        anim.Play();
        yield return new WaitForSeconds(anim.clip.length);
        if(type == 1)//Goblin
        {
            gameController.AddScore(3);
        }
        else if (type == 2 )
        {
            if (CountToDeath >= 3)
                gameController.AddScore(10);
        }
        Destroy(Minion);
    }
}
