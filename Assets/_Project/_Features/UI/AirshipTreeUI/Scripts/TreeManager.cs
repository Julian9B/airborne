using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private Origins originDatabase;
    [SerializeField] private Transform originsParent;
    [SerializeField] private GameObject originsPrefab;

    [SerializeField] private ScrollRect background;
    [SerializeField] private Image bgLogo;
    [SerializeField] private Airships airshipDatabase;
    [SerializeField] private Transform airshipsParent;
    [SerializeField] private GameObject airshipsPrefab;

    [SerializeField] private UIManager ui;
    [SerializeField] private GameObject airshipDetails;

    void Start()
    {
        LoadOrigins();

        LoadTree(originDatabase.origins[0]);
    }

    public void LoadOrigins()
    {
        foreach (Origin origin in originDatabase.origins)
        {
            GenerateOriginButton(origin);
        }
    }

    private void GenerateOriginButton(Origin origin)
    {
        GameObject newButton = Instantiate(originsPrefab, originsParent);

        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => LoadTree(origin));

        Image flag = button.transform.Find("Flag").GetComponent<Image>();
        flag.sprite = origin.flag;
    }

    private void LoadTree(Origin origin)
    {
        background.normalizedPosition = new Vector2(0, 1);

        foreach (Transform child in airshipsParent)
        {
            Destroy(child.gameObject);
        }

        if(origin.bgLogo != null)
        {
            bgLogo.sprite = origin.bgLogo;
        }

        if(origin.originName != null)
        {
            gameObject.transform.Find("OriginDisplay").Find("OriginLabel").GetComponent<TextMeshProUGUI>().text = $"Airships of {origin.originName}";
        }

        foreach (Airship airship in airshipDatabase.airships.Where(a => a.origin == origin))
        {
            GenerateAirshipButton(airship);
        }
    }

    private void GenerateAirshipButton(Airship airship)
    {
        Vector2 startPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (airship.previousShip)
        {
            startPosition += airship.previousShip.buttonPosition;
        }

        GameObject newButton = Instantiate(airshipsPrefab, airshipsParent);
        newButton.transform.position = startPosition + airship.buttonPosition;

        if (airship.previousShip != null)
        {
            Vector2 intermediatePosition = new Vector2(startPosition.x + airship.buttonPosition.x, startPosition.y);
            DrawLine(startPosition, intermediatePosition);
            DrawLine(intermediatePosition, startPosition + airship.buttonPosition);
        }

        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => GameManager.instance.LoadAirship(airship));
        button.onClick.AddListener(() => ui.ToggleVisibility(gameObject));
        button.onClick.AddListener(() => ui.ToggleVisibility(airshipDetails));

        TMP_Text label = button.transform.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        label.text = airship.shipName;
    }

    public void DrawLine(Vector2 start, Vector2 end, float thickness = 5f)
    {
        GameObject lineObj = new GameObject("UILine", typeof(Image));
        lineObj.transform.SetParent(airshipsParent.transform, false);

        Image lineImage = lineObj.GetComponent<Image>();
        lineImage.color = Color.white;

        RectTransform rect = lineImage.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.SetAsFirstSibling();

        Vector2 diff = end - start;
        rect.sizeDelta = new Vector2(diff.magnitude, thickness);
        rect.position = start + diff / 2;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }
}
