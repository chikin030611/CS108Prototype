using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    // All child objects
    private GameObject _levelText;
    private GameObject _expBar;
    private GameObject _hpBarParent;
    private GameObject _kiBarParent;
    
    // Components of the child objects
    private TextMeshProUGUI _levelTextMeshPro;
    private SpriteRenderer _expBarSpriteRenderer;
    
    // Sprites
    public List<Sprite> expBarSprites;
    private List<GameObject> _healthBarSprites = new List<GameObject>();
    private List<GameObject> _kiBarSprites = new List<GameObject>();
    public Sprite hpSprite;
    public Sprite halfHpSprite;
    public Sprite kiSprite;

    // Player
    private GameObject _player;
    private PlayerControls _playerControls;
    
    // Start is called before the first frame update
    void Start()
    {
        // Getting all child objects
        _levelText = transform.Find("Level Text").gameObject;
        _expBar = transform.Find("Exp Bar").gameObject;
        _hpBarParent = transform.Find("HP").gameObject;
        _kiBarParent = transform.Find("Ki").gameObject;
        
        // Getting all components of the child objects
        _levelTextMeshPro = _levelText.GetComponent<TextMeshProUGUI>();
        _expBarSpriteRenderer = _expBar.GetComponent<SpriteRenderer>();
        _expBarSpriteRenderer.sprite = expBarSprites[0];
        
        // Getting the player
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerControls = _player.GetComponent<PlayerControls>();
        }
        
        // Create the health bar GameObjects
        for (int i = 0; i < _playerControls.GetMaxHealth(); i++)
        {
            double x = i/5.5f;
            GameObject hp = new GameObject("HP");
            hp.transform.SetParent(_hpBarParent.transform);
            hp.transform.localPosition = new Vector3((float)x, 0, 0);
            hp.transform.localScale = new Vector3(1, 1, 1);
            hp.AddComponent<SpriteRenderer>().sprite = hpSprite;
            hp.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _healthBarSprites.Add(hp);
        }
        
        // Create the health bar GameObjects
        for (int i = 0; i < _playerControls.GetMaxKi(); i++)
        {
            double x = i/4.5f;
            GameObject ki = new GameObject("Ki");
            ki.transform.SetParent(_kiBarParent.transform);
            ki.transform.localPosition = new Vector3((float)x, 0, 0);
            ki.transform.localScale = new Vector3(1, 1, 1);
            ki.AddComponent<SpriteRenderer>().sprite = kiSprite;
            ki.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _kiBarSprites.Add(ki);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExpBar(_playerControls.GetExp());
        UpdateLevelText(_playerControls.GetLevel());
        UpdateHealthBar(_playerControls.GetHealth());
        UpdateKiBar(_playerControls.GetKi());
    }
    
    public void UpdateLevelText(int level)
    {
        _levelTextMeshPro.text = "Lv. <size=128>" + level + "</size>";
    }
    
    public void UpdateExpBar(int exp)
    {
        int spriteIndex = 0;
        switch (exp)
        {
            case 0:
                spriteIndex = 0;
                break;
            case 1:
                spriteIndex = 1;
                break;
            case 2:
                spriteIndex = 2;
                break;
            case 3:
                spriteIndex = 3;
                break;
            case 4:
                spriteIndex = 4;
                break;
            case 5:
                spriteIndex = 5;
                break;
        }
        _expBarSpriteRenderer.sprite = expBarSprites[spriteIndex];
    }
    
    public void UpdateHealthBar(int hp)
    {
        for (int i = 0; i < _healthBarSprites.Count; i++)
        {
            _healthBarSprites[i].SetActive(i < hp);
        }
    }
    
    public void UpdateKiBar(int ki)
    {
        for (int i = 0; i < _healthBarSprites.Count; i++)
        {
            _kiBarSprites[i].SetActive(i < ki);
        }
    }
    
    
}
