using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Maps[] maps;

	private Maps currentMap = new Maps();
	private int mapIndex;

	void Start ()
	{
		mapIndex = 0;
		currentMap.map = Instantiate (maps [mapIndex].map, maps[mapIndex].spawnPosition, Quaternion.identity) as GameObject;
		currentMap.mapName = maps [mapIndex].mapName;
	}

	void changeCurrentMap()
	{
		if(mapIndex == maps.Length - 1)
		{
			mapIndex = 0;
		}
		else
		{
			mapIndex++;
		}

		if(currentMap.map != null)
		{
			Destroy (currentMap.map);
			currentMap.map = Instantiate (maps [mapIndex].map, maps[mapIndex].spawnPosition, Quaternion.identity) as GameObject;
			currentMap.mapName = maps [mapIndex].mapName;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Projectile")
		{
			Destroy (col.gameObject);
			changeCurrentMap ();
		}
	}

	public string getCurrentMapName{get{return currentMap.mapName;}}
}
