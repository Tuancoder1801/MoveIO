using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public Enemy enemy;  // Đối tượng kẻ địch cần theo dõi
    public Image icon;  // UI của chỉ báo
    public TextMeshProUGUI textScore;

    private RectTransform indicatorRect;  // RectTransform của chỉ báo
    private RectTransform canvasRectTransform;
    private Camera mainCamera;

    private void Awake()
    {
        indicatorRect = GetComponent<RectTransform>();
        canvasRectTransform = UIGameManager.Instance.GetComponent<RectTransform>();
        mainCamera = FindObjectOfType<Camera>();

    }

    void Start()
    {
        icon.color = enemy.nameTagEnemy.bg_score.color;
    }

    void Update()
    {
        Tracking();
    }

    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    private void Tracking()
    {
        Player player = GameController.Instance.playerInstance;
        if (enemy != null && player != null)
        {
            if (enemy.isDead)
            {
                gameObject.SetActive(false);
                return;
            }

            Vector3 directionToEnemy = new Vector3(enemy.transform.position.x - player.transform.position.x, 0,
                enemy.transform.position.z - player.transform.position.z);
            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                // Nếu enemy trong tầm nhìn
                icon.enabled = false;
                textScore.enabled = false;
            }
            else
            {
                icon.enabled = true;
                textScore.enabled = true;

                float angle = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;
                
                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                Vector3 fromCenterToEnemy = screenPos - screenCenter;

                Vector2 canvasSize = canvasRectTransform.sizeDelta;
                Vector2 screenDir = new Vector2(fromCenterToEnemy.x, fromCenterToEnemy.y).normalized;

                Vector2 cappedPosition = screenDir * (Mathf.Min(canvasSize.x / 2, canvasSize.y / 2) - 50);  

                if (Mathf.Abs(screenDir.x) > Mathf.Abs(screenDir.y))
                {
                    // Đặt indicator ở rìa trái hoặc phải
                    cappedPosition.x = Mathf.Sign(screenDir.x) * (canvasSize.x / 2 - 50); // 50 là margin
                }
                else
                {
                    // Đặt indicator ở rìa trên hoặc dưới
                    cappedPosition.y = Mathf.Sign(screenDir.y) * (canvasSize.y / 2 - 50); // 50 là margin
                }

                // Cập nhật vị trí anchoredPosition cho indicator
                indicatorRect.anchoredPosition = cappedPosition;
            }
        }
    }
}
