using System.Collections.Generic;
using UnityEngine;

namespace QuadTree.Scripts
{
    public class GameObjectId : MonoBehaviour
    {
        public static List<GameObject> GameObjectsList = new List<GameObject>();
        [SerializeField]private int ID;
        public int GameObjectID => ID;
        private void Awake()
        {
            GameObjectsList.Add(gameObject);
            QuadTreeChecker.Renderers.Add(ID,GetComponent<Renderer>());
        }

        [ContextMenu("Register ID")]
        void RegisterID()
        {
            ID = gameObject.GetHashCode();
        }
    }
}
