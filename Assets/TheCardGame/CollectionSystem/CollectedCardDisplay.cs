using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedCardDisplay : CardRenderBase
{
    // protected CollectionSet data ^

    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    protected override void Visualize()
    {

        text.text = data.data.cardName;
        if(!data.Has) image.color = Color.gray;



    }
    
    
    
    
}

public abstract class CardRenderBase : MonoBehaviour
{
    protected CollectionSet data;
    public void Setup(CollectionSet cardSet)
    {
        data = cardSet;
        Render();
    }
    public void Render()
    {
        if (data == null) return;
        Visualize();
    }
    protected abstract void Visualize();
}