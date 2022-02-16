namespace myria_core_sdk.Utilities.ObjectPool
{
    using UnityEngine;

    public static class GameObjectExtension
    {
        public static string Path(this GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj  = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    }
}