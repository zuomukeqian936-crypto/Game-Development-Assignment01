using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PanelGameOverController _panelGameOverController;
    private GameSceneDirector _gameSceneDirector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_gameSceneDirector == null)
        {
            _gameSceneDirector = FindAnyObjectByType<GameSceneDirector>();
        }
        _panelGameOverController.Init(this);
    }


    //タイトルへ
    public void LoadSceneTitle()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("TitleScene");
    }

    //メイン画面へ
    public void LoadMainScenen()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainScene");
    }

    //ゲームオーバーパネルを表示
    public void DispPanelGameOver()
    {
        //パネル表示
        _panelGameOverController.DispPanel(_gameSceneDirector._playerController._weaponSpawner);
        
    }
}
