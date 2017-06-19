using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thor_DestroyByPlayerBoundary : MonoBehaviour
{
	private Thor_GameControl gameController;
	private int scoreValue;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Thor_GameControl>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerExit(Collider other)
	{
        //Debug.Log("touch collider");
        if (other.tag == "Goblin")
        {
            gameController.MinusHealth(1);
            //Debug.Log("goblin");
        }
        else
        {
            gameController.MinusHealth(2);
            //Debug.Log("Golem");
        }
        Destroy(other.gameObject);
        //Destroy(other);
        Debug.Log(other.tag);

    }
}
