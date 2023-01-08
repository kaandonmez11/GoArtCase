using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private int placedObjectCount = 0;

    private void Update()
    {
        transform.position = target.position + target.forward * 1.5f + target.up * -1.5f;
    }

    public void OnAssetDownloaded(GameObject gameObject)
    {
        gameObject = Instantiate(gameObject);
        gameObject.transform.position = transform.position + (transform.right * placedObjectCount);
        placedObjectCount++;
    }
}