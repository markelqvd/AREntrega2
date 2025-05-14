using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAlMenu : MonoBehaviour
{
    public void Volver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
