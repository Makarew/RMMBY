using System.Collections.Generic;
using UnityEngine;

namespace RMMBY.Helpers
{
    public class ObjectFinders
    {
        public static GameObject[] FindAllObjectsWithName(string name)
        {
            List<GameObject> list = new List<GameObject>();

            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in objects)
            {
                if(obj.name == name)
                {
                    list.Add(obj);
                }
            }

            return list.ToArray();
        }

        public static GameObject[] FindAllObjectsOnLayer(int layer)
        {
            List<GameObject> list = new List<GameObject>();

            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in objects)
            {
                if (obj.layer == layer)
                {
                    list.Add(obj);
                }
            }

            return list.ToArray();
        }
    }
}
