using TMPro;
using UnityEngine;

// source : https://github.com/Maraakis/ChristinaCreatesGames/tree/main/Interactive%20Book%20with%20TextMeshPro
// tutorial: https://www.youtube.com/watch?v=Hu7nZL8Wqc8
public class BookContents : MonoBehaviour
{
    [TextArea(10, 20)] [SerializeField] private string content;
    [Space] [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space] [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;
    
    private void OnValidate()
    {
        UpdatePagination();
        if (leftSide.text == content)
            return;
        SetupContent();
    }

    private void Awake()
    {
        SetupContent();
        UpdatePagination();
    }

    // places content in text box (change overflow attribute to "page" to have page stystem)
    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }

    // changes text box value to correspond to page number in content distribution
    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
    }

    // changes page number backward for content display
    public void PreviousPage()
    {
        if (leftSide.pageToDisplay < 1)
        {
            leftSide.pageToDisplay = 1;
            return;
        }
        if (leftSide.pageToDisplay - 2 > 1)
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1;
        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        UpdatePagination();
    }


    // changes page number forward for content display
    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;
        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1)
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        UpdatePagination();
    }
}