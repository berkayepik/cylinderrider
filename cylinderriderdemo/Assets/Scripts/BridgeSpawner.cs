using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
	public GameObject startRef, endRef;
	public BoxCollider hiddenPlatform;
    // Start is called before the first frame update
    void Start()
    {
        //İki referans arasında ki yönü tutacak
		Vector3 direction = endRef.transform.position - startRef.transform.position;
		
		//İki nokta arasında ki mesafe
		float distance = direction.magnitude;
		
		direction = direction.normalized;
		
		hiddenPlatform.transform.forward = direction;
		
		hiddenPlatform.size = new Vector3(hiddenPlatform.size.x , hiddenPlatform.size.y , distance);
		
		hiddenPlatform.transform.position = startRef.transform.position + ( direction * distance/2) + (new Vector3(0, -direction.z , direction.y) * hiddenPlatform.size.y / 2);
    }

    
}
