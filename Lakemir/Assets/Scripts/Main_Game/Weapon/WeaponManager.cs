using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon; // ���� ������ ����
    public Weapon newWeapon; // ��ü�� ����

    // UI ��ҵ�
    public Text currentWeaponInfoText; // ���� ������ ���� �ؽ�Ʈ
    public Text newWeaponInfoText; // ��ü�� ������ ���� �ؽ�Ʈ
    public Image currentWeaponImage; // ���� ������ �̹���
    public Image newWeaponImage; // ��ü�� ������ �̹���
    public Button changeWeaponButton; // ��ü ��ư

    void Start()
    {
        UpdateUI(); // �ʱ� UI ����
        changeWeaponButton.onClick.AddListener(ChangeWeapon); // ��ư Ŭ�� �� ChangeWeapon �Լ� ȣ��
        changeWeaponButton.gameObject.SetActive(false); // ó������ ��ư ��Ȱ��ȭ (��ü�� ���Ⱑ ������)
    }

    // UI ������Ʈ �Լ�
    void UpdateUI()
    {
        if (currentWeapon != null)
        {
            currentWeaponInfoText.text = currentWeapon.GetWeaponInfo();
            currentWeaponImage.sprite = currentWeapon.weaponImage;
        }

        if (newWeapon != null)
        {
            newWeaponInfoText.text = newWeapon.GetWeaponInfo();
            newWeaponImage.sprite = newWeapon.weaponImage;
            changeWeaponButton.gameObject.SetActive(true); // ��ü�� ���Ⱑ ������ ��ư Ȱ��ȭ
        }
        else
        {
            newWeaponInfoText.text = "��ü�� ���Ⱑ �����ϴ�.";
            newWeaponImage.sprite = null;
            changeWeaponButton.gameObject.SetActive(false); // ��ü�� ���Ⱑ ������ ��ư ��Ȱ��ȭ
        }
    }

    // ���� ��ü �Լ�
    void ChangeWeapon()
    {
        if (currentWeapon != null)
        {
            Debug.Log($"{currentWeapon.weaponName}���� {newWeapon.weaponName}���� ����Ǿ����ϴ�.");
        }

        currentWeapon = newWeapon; // ���ο� ���⸦ ���� ������ ����� ����
        newWeapon = null; // ���ο� ���⸦ ����� (��ü �Ϸ� ��)

        UpdateUI(); // UI ����
    }
}