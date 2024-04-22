using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamreaCotroller : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    Vector3 rot;
    private RaycastHit hit;
    private float distance = 5.5f;
    private float zoomSpeed = 5f;
    void Start()
    {
        GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FollowCam();
        Zoom();
    }
    private void FollowCam()
    {
        if (Input.GetMouseButton(1))
        {
            rot = transform.rotation.eulerAngles;
            rot.y += Input.GetAxis("Mouse X");
            rot.x += -1 * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);
        }
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�.
        transform.position = Player.transform.position - transform.rotation * reverseDistance;
    }
    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            Camera.main.fieldOfView += distance;
        }
    }
    public void GetPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
