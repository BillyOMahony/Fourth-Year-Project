using UnityEngine;
using System.Collections;

public class AsteroidSpawner : Photon.PunBehaviour {

    public int[,,] positions = new int[50,50,50];
    public GameObject asteroid01;
    public GameObject asteroid02;
    public GameObject asteroid03;

    public GameObject loadingScreen;

	// Use this for initialization
	void Start () {

        loadingScreen.SetActive(true);
        //Possibly disable clients ships here??

        if (PhotonNetwork.isMasterClient)
        {
            SpawnAsteroids();
        }
	}

    void SpawnAsteroids()
    {
        for(int x = 0; x < positions.GetLength(0); x++)
        {
            for(int y = 0; y < positions.GetLength(1); y++)
            {
                for(int z = 0; z < positions.GetLength(2); z++) {

                    if (RandomSpawn())
                    {
                        GameObject asteroid = AsteroidToSpawn();
                        asteroid = PhotonNetwork.Instantiate(asteroid.name, new Vector3(x * 20 - 100, y * 20 - 250, z * 20 - 250), AsteroidRotation(), 0);
                        float scale = AsteroidScale();
                        asteroid.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
        }

        GetComponent<PhotonView>().RPC("RemoveLoadingScreen", PhotonTargets.All);
    }

    bool RandomSpawn()
    {
        bool value = false;
        int num = Random.Range(1, 200);
        if (num == 1)
        {
            value = true;
        }
        return value;
    }

    GameObject AsteroidToSpawn()
    {
        int num = Random.Range(1, 3);
        if (num == 1)
        {
            return asteroid01;
        } else if (num == 2)
        {
            return asteroid02;
        }
        else
        {
            return asteroid03;
        }
    }

    Quaternion AsteroidRotation()
    {
        float x = Random.Range(0f, 359.9f);
        float y = Random.Range(0f, 359.9f);
        float z = Random.Range(0f, 359.9f);

        return new Quaternion(x, y, z, 0);
    }

    float AsteroidScale()
    {
        int size = Random.Range(1, 10);
        if(size == 1)
        {
            return Random.Range(7f, 15f);
        }else if(size > 1 && size <= 4)
        {
            return Random.Range(2f, 7f);
        }else
        {
            return Random.Range(0.5f, 2f);
        }
    }

    [PunRPC]
    public void RemoveLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.isWriting == true && PhotonNetwork.isMasterClient)
        {
        }
        else
        {
        }
        */
    }

}
