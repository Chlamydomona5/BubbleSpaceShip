using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cover : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private List<Image> images;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.1f);
        images[0].gameObject.SetActive(false);
        images[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        images[1].gameObject.SetActive(false);
        images[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        images[2].gameObject.SetActive(false);
        images[3].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        images[3].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}