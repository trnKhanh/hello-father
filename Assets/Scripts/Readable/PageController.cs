using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [Header("Page References")]
    public GameObject[] pages;
    public int startPage;
    int currentPage = 0;

    void OnEnable()
    {
        OpenPage(startPage);
    }

    public void OpenPage(int pageId)
    {
        currentPage = pageId;
        if (currentPage < 0)
            currentPage = 0;
        if (currentPage >= pages.Length)
            currentPage = pages.Length - 1;

        foreach (GameObject page in pages)
            page.SetActive(false);

        pages[currentPage].SetActive(true);
    }

    public bool CanGoToNextPage()
    {
        return currentPage + 1 < pages.Length;
    }

    public void NextPage()
    {
        OpenPage(currentPage + 1);
    }

    public bool CanGoToPreviousPage()
    {
        return currentPage - 1 >= 0;
    }

    public void PreviousPage()
    {
        OpenPage(currentPage - 1);
    }
}
