using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public MissionObjectHUD missionObjectHUD;
    public List<GameObject> gameObjects;
    public List<GameObject> decals;

    void Start()
    {
        foreach (var obj in gameObjects)
        {
            ObjectHandler handler = obj.AddComponent<ObjectHandler>();
            handler.SetManager(this);
            handler.SetObjectType(ObjectType.GameObject);
        }

        foreach (var decal in decals)
        {
            ObjectHandler handler = decal.AddComponent<ObjectHandler>();
            handler.SetManager(this);
            handler.SetObjectType(ObjectType.Decal);
        }

        missionObjectHUD.totalObjects = gameObjects.Count;
        missionObjectHUD.totalDecals = decals.Count;
        missionObjectHUD.UpdateHUD();
    }

    public void ObjectDestroyed(ObjectType type)
    {
        if (type == ObjectType.GameObject)
        {
            missionObjectHUD.DecreaseObjectCount();
        }
        else if (type == ObjectType.Decal)
        {
            missionObjectHUD.DecreaseDecalCount();
        }
    }
}

public enum ObjectType
{
    GameObject,
    Decal
}

public class ObjectHandler : MonoBehaviour
{
    private ObjectManager manager;
    private ObjectType objectType;

    public void SetManager(ObjectManager manager)
    {
        this.manager = manager;
    }

    public void SetObjectType(ObjectType objectType)
    {
        this.objectType = objectType;
    }

    void OnDestroy()
    {
        if (manager != null)
        {
            manager.ObjectDestroyed(objectType);
        }
    }
}
