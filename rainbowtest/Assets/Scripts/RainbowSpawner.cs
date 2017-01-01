using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RainbowSpawner : NetworkBehaviour {

    public GameObject rainbowPrefab;
    public int numberOfRainbows;
    private int rainbowVelspeedX;
    private int rainbowVelspeedZ;
    private float startTime;
    private float minSpawnTime = 10f;
    public bool rainbowSpawned = false;


    void OnServerStart() {
    	startTime = Time.fixedTime;
    }

    void FixedUpdate() {
    	if (isServer && !rainbowSpawned && Input.GetKey(KeyCode.F)){
	    	if (minSpawnTime < Time.fixedTime - startTime) {
	    		Debug.Log ("spawntime boven 10 sec, ready to spawn");
	    		SpawnRainbow();
	    		rainbowSpawned = true;
	    	}
    	}
    }

    void RandomVelo() {
	rainbowVelspeedX = Random.Range(-15, 15);
    rainbowVelspeedZ = Random.Range(-15, 15);
    }

    void SpawnRainbow() {
		for (int i=0; i < numberOfRainbows; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(60.0f, 200.0f),
                30.0f,
                Random.Range(60.0f, 200.0f));

            var spawnRotation = Quaternion.Euler( 
                Random.Range(0, 180), 
                Random.Range(0,180), 
                0.0f);

			RandomVelo();
            var rainbow = (GameObject)Instantiate(rainbowPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(rainbow);
            RandomVelo();
			rainbow.GetComponent<Rigidbody>().velocity = new Vector3 (rainbowVelspeedX,15,rainbowVelspeedZ);
			rainbow.GetComponent<Rigidbody>().useGravity = true;
        }
     }
}