using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCutView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler
{
    public Image img;
    public CutFruitController cutFruitController;
    public ItemCutModel itemCutModel;


    public void Setup(Sprite image, ItemCutModel itemCutModel, CutFruitController cutFruitController)
    {
        img.sprite = image;
        this.itemCutModel = itemCutModel;
        this.cutFruitController = cutFruitController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Đã nhấn vào: " + itemCutModel.name);
        //cutFruitController.CreateExplode(itemCutModel, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Kéo qua: " + name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cutFruitController.CreateExplode(itemCutModel, this);

    }

}
