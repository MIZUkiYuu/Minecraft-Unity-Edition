using Unity.Collections;
using UnityEngine;
using UnityEngine.Pool;
using World;

namespace Utilities
{
    public class BaseGameObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] public T prefab;
        public WorldData_SO worldDataSO;

        [DisplayOnly] [SerializeField] private int defaultCapacity;
        [DisplayOnly] [SerializeField] private int maxSize;

        private ObjectPool<T> pool;

        public int CountActive => pool.CountActive;
        public int CountInactive => pool.CountInactive;
        public int CountAll => pool.CountAll;

        protected void Initialize(int _defaultCapacity, int _maxSize)
        {
            defaultCapacity = _defaultCapacity;
            maxSize = _maxSize;
            pool = new ObjectPool<T>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, false, _defaultCapacity, _maxSize);
        }

        protected virtual T OnCreatePoolItem() => Instantiate(prefab, transform);
        protected virtual void OnGetPoolItem(T _obj) => _obj.gameObject.SetActive(true);
        protected virtual void OnReleasePoolItem(T _obj) => _obj.gameObject.SetActive(false);
        protected virtual void OnDestroyPoolItem(T _obj) => Destroy(_obj.gameObject);

        public T Get() => pool.Get();
        public void Release(T _obj) => pool.Release(_obj);
        public void Clear() => pool.Clear();
    }
}