using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
	public static PlayerController Current;
	
	//Karakterin koşma hızı
	public float runningSpeed;
	private float _currentRunningSpeed;
	
	//Karakterin sağ ve sol limiti
	private float limitX = 3.78f;	
	//Karakterin sağ sola ne kadar hızla gideceğini tutacak
	public float xSpeed;
	
	public GameObject ridingCylinderPrefab;
	
	public List<RidingCylinder> cylinders;
	
	
	//Köprü oluşturma sorgusu
	private bool _spawningBridge;
	
	//Köprü parçaları
	public GameObject bridgePiece;
	
	private BridgeSpawner _bridgeSpawner;
	
	private float _creatingBridgeTimer;
	
    // Start is called before the first frame update
    void Start()
    {
        _currentRunningSpeed = runningSpeed;
		
		Current = this;
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
		
		if(_spawningBridge)
		{
			_creatingBridgeTimer -= Time.deltaTime;
			if(_creatingBridgeTimer < 0)
			{
				_creatingBridgeTimer = Time.fixedDeltaTime;
				IncrementCylinderVolume(-Time.fixedDeltaTime);
				GameObject createdBridgePiece = Instantiate(bridgePiece);
				
				Vector3 direction = _bridgeSpawner.endRef.transform.position - _bridgeSpawner.startRef.transform.position;
				float distance = direction.magnitude;
				direction = direction.normalized;
				
				createdBridgePiece.transform.forward = direction;
				float characterDistance = transform.position.z - _bridgeSpawner.startRef.transform.position.z;
				characterDistance = Mathf.Clamp(characterDistance, 0, distance);
				Vector3 newPiecePosition = _bridgeSpawner.startRef.transform.position + direction * characterDistance;
				newPiecePosition.x = transform.position.x;
				createdBridgePiece.transform.position = newPiecePosition;
			}
		}
		
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "AddCylinder")
		{
			
			IncrementCylinderVolume(0.1f);
			Destroy(other.gameObject);
		}
		
		else if(other.tag == "SpawnBridge")
		{
			StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());
		}
		
		else if(other.tag == "StopSpawnBridge")
		{
			StopSpawningBridge();
		}
	}
	
	
	private void OnTriggerStay(Collider other)
	{
		if(other.tag == "Trap")
		{
			IncrementCylinderVolume(-Time.fixedDeltaTime);
		}
	}
	
	//Silindir hacmi büyütme
	public void IncrementCylinderVolume(float value)
	{
		//Hiç silindir yoksa silindir yarat
		if(cylinders.Count == 0)
		{
			//Yük alıyorsak
			if(value > 0)
			{
				CreateCylinder(value);
			}
			else
			{
				//gameover
			}
		}
		//Altımızda silindir varsa en alttakinin boyutunu güncelleyecez
		else
		{
			cylinders[cylinders.Count - 1].CylinderController(value);
		}
	}
	
	
	public void CreateCylinder(float value)
	{
		RidingCylinder createdCylinder = Instantiate(ridingCylinderPrefab, transform).GetComponent<RidingCylinder>();
		cylinders.Add(createdCylinder);
		createdCylinder.CylinderController(value);
	}
	
	public void DestroyCylinder(RidingCylinder cylinder)
	{
		cylinders.Remove(cylinder);
		Destroy(cylinder.gameObject);
	}
	
	
	//Köprü yaratma fonksiyonu
	public void StartSpawningBridge(BridgeSpawner spawner)
	{
		_bridgeSpawner = spawner;
		_spawningBridge = true;
	}
	
	//Köprü yaratmayı durdurma fonksiyonu
	public void StopSpawningBridge()
	{
		_spawningBridge = false;
	}
}
