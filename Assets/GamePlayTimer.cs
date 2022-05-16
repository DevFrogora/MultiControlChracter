using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GamePlayTimer : MonoBehaviour
{

    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;
    public AeroPlaneController aeroPlaneController;
    public PlayerAeroPlaneLink player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "Go!";
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().SetLayerWeight(player.GetComponent<Animator>().GetLayerIndex("Plane"), 1);
        yield return new WaitForSeconds(0.7f);
        PlayerStatus.Instance.status = PlayerStatus.Status.IsInPlane;
        aeroPlaneController.activePlayerAeroplaneLink(player);
        countdownDisplay.gameObject.SetActive(false);
    }
}
