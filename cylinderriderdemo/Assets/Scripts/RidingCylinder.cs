using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingCylinder : MonoBehaviour
{
	//Silindir en büyük haline geldi mi gelmedi mi
	private bool _filled;
	
	//Silindirin ne kadar dolduğunu göstercek
	private float _value;
	
	
    public void CylinderController(float value)
	{
		_value += value;
		
		//Silindir en büyük haline geldiyse
		if(_value > 1)
		{
			//Silindirin boyutunu 1 yap
			//1'den ne kadar büyükse o büyüklükte silindir yarat
			
			float leftValue = _value - 1;
			int cylinderCount = PlayerController.Current.cylinders.Count;
			transform.localPosition = new Vector3(transform.localPosition.x, ((cylinderCount-1) * -0.5f) -0.25f , transform.localPosition.z);
			transform.localScale = new Vector3(0.5f , transform.localScale.y , 0.5f);
			
			PlayerController.Current.CreateCylinder(leftValue);
		}
		//Silindir yok olduysa
		else if(_value < 0)
		{
			//Silindiri yok et
			PlayerController.Current.DestroyCylinder(this);
		}
		else
		{
			//Silindirin boyutunu güncelle
			
			int cylinderCount = PlayerController.Current.cylinders.Count;
			transform.localPosition = new Vector3(transform.localPosition.x, ((cylinderCount-1) * -0.5f) -0.25f * _value , transform.localPosition.z);
			transform.localScale = new Vector3(0.5f * _value , transform.localScale.y , 0.5f * _value);
		}
		
		
		
		
	}
	
	
}
