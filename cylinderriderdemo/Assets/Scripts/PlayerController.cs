using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Karakterin koşma hızı
	public float runningSpeed;
	private float _currentRunningSpeed;
	
	//Karakterin sağ ve sol limiti
	private float limitX = 3.78f;	
	//Karakterin sağ sola ne kadar hızla gideceğini tutacak
	public float xSpeed;
	
    // Start is called before the first frame update
    void Start()
    {
        _currentRunningSpeed = runningSpeed;
    }

    // Update is called once per frame
    void Update()
    {	

		//Karakterin sağ sol pozisyonun tutacak
		float newX = 0;
		
		//Parmağıyla ne kadar sağ sola götürdüğünü tutacak
		float touchXdelta = 0;
		
		//Eğer dokunma varsa telefon kontrolüne giriyoruz.
		//Sağda ki kod dokunulan parmak hareket halinde mi diye sorguluyor
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			touchXdelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
		}
		else if(Input.GetMouseButton(0) )
		{
			touchXdelta = Input.GetAxis("Mouse X");
		}
		
		newX = transform.position.x + xSpeed * touchXdelta * Time.deltaTime;
		
		//Sağ ve solu sınırlıyoruz
		newX = Mathf.Clamp(newX, -limitX, limitX);
		
		//Karakterin koşma kodu
		Vector3 newPosition = new Vector3(newX , transform.position.y , transform.position.z + _currentRunningSpeed * Time.deltaTime);
		transform.position = newPosition;
		
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "AddCylinder")
		{
			
			IncrementCylinderVolume(0.2f);
			Destroy(other.gameObject);
		}
	}
	
	//Silindir hacmi büyütme
	public void IncrementCylinderVolume(float value)
	{
		
	}
}
