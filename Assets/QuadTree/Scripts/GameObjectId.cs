using System.Collections.Generic;
using UnityEngine;

namespace QuadTree.Scripts
{
    public class GameObjectId : MonoBehaviour
    {
        private GameObject _gameObject;
        public static List<GameObject> GameObjectsList = new List<GameObject>();
        private int _id;
        private void Awake()
        {
            _gameObject = gameObject;
            GameObjectsList.Add(_gameObject);
            _id = _gameObject.GetHashCode();
        
            QuadTreeChecker.Renderers.Add(_id,GetComponent<Renderer>());
        }
    }
}
