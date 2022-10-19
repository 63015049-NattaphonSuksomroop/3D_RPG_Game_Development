using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public GameObject ArrowPrefab;
    RaycastHit hit;
    float range = 1000f;
    public Transform ArrowSpawPosition;

    public GameObject HandArrow;

    void shoot()
    {
        HandArrow.gameObject.SetActive(true);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, range))
        {
            GameObject ArrowInstantiate = GameObject.Instantiate(ArrowPrefab, ArrowSpawPosition.transform.position, ArrowSpawPosition.transform.rotation) as GameObject;
            ArrowInstantiate.GetComponent<Arrow>().setTarget(hit.point);
        }
    }
}
