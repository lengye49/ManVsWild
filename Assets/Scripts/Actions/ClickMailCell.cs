using UnityEngine;
using System.Collections;

public class ClickMailCell : MonoBehaviour {

	private MailBoxActions _mailBoxActions;
	void Start(){
		_mailBoxActions = this.gameObject.GetComponentInParent<MailBoxActions> ();
	}

	public void OnClick(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		int i = int.Parse (this.gameObject.name);
		_mailBoxActions.OpenMail (i);
	}
}
