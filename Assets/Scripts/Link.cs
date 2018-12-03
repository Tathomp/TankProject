using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour 
{
    // Link accessor
    public static Link GetLinkState()
    {
        return GameObject.Find("Manager").GetComponent<Link>();
    }

    public void OpenLinkJSPlugin(string url)
	{
    	openWindow(url);
    }

    [DllImport("__Internal")]
	private static extern void openWindow(string url);

}