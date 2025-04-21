using Unity.Netcode.Components;
using UnityEngine;

public class NetworkTransformClientAuth : NetworkTransform
{
	protected override bool OnIsServerAuthoritative()
	{
		return false;
	}
}
