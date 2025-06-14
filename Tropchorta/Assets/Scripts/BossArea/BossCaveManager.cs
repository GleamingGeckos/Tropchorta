using UnityEngine;

public class BossCaveManager : MonoBehaviour
{
    [SerializeField] private GameObject _bossCave;
    [SerializeField] private string _levelName;
    private PossibleBossPlace[] _possibleBossPlaces;

    void Start()
    {
        _possibleBossPlaces = FindObjectsByType<PossibleBossPlace>(FindObjectsSortMode.None);
        if (_possibleBossPlaces.Length > 0)
        {
            int index = Random.Range(0, _possibleBossPlaces.Length);
            GameObject newBossCave = Instantiate(_bossCave, _possibleBossPlaces[index].transform.position, _possibleBossPlaces[index].transform.rotation, transform);
            newBossCave.transform.localScale = _possibleBossPlaces[index].transform.localScale;
            GoToBossFight bossFight = newBossCave.GetComponent<GoToBossFight>();
            if (bossFight != null)
            {
                bossFight._levelName = _levelName;
            }

            foreach (var place in _possibleBossPlaces)
            {
                Destroy(place.gameObject);
            }
        }
    }
}
