using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObject
{
	public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour
{
    public Transform playerPos;
    public float mapScale;

    public static List<RadarObject> radObjects = new List<RadarObject>();
    
    public static void RegisterRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarObject(){ owner = o, icon = image });
    }

    public static void RemoveRadarObject(GameObject o)
    {
        for (int i = 0; i < radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                radObjects[i].icon.enabled = false;
                break;
            }
        }
    }

    private void Update()
    {
        foreach(RadarObject ro in radObjects)
        {
            Vector3 radarPos = (ro.owner.transform.position - playerPos.position);
            float distToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            ro.icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + this.transform.position;
        }
    }
}
