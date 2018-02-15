using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObject
{
	public Image Icon { get; set; }
    public GameObject Owner { get; set; }

    public RadarObject(GameObject obj, Image im)
    {
        Icon = im;
        Owner = obj;
    }
}

public class Radar : MonoBehaviour
{
    public Transform playerPos;
    public float mapScale;

    public List<RadarObject> radObjects = new List<RadarObject>();
    
    public void RegisterRadarObject(RadarObject radObj)
    {
        if(!radObjects.Contains(radObj))
        {
            radObj.Icon.enabled = true;
            radObjects.Add(radObj);            
        }        
    }

    public void RemoveRadarObject(RadarObject radObj)
    {
        if(radObjects.Contains(radObj))
        {
            radObj.Icon.enabled = false;
            radObjects.Remove(radObj);
        }
    }

    private void Update()
    {
        foreach (RadarObject ro in radObjects)
        {
            if(!ro.Icon.enabled) continue;

            Vector3 radarPos = (ro.Owner.transform.position - playerPos.position);
            float distToObject = Vector3.Distance(playerPos.position, ro.Owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);
            
            ro.Icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + this.transform.position;
        }
    }
}
