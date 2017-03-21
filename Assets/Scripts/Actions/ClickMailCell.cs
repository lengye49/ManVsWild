using UnityEngine;
using System.Collections;

public class ClickMailCell : MonoBehaviour {

	private MailBoxActions _mailBoxActions;
	void Start(){
		_mailBoxActions = this.gameObject.GetComponentInParent<MailBoxActions> ();
	}

	public void OnClick(){
		int i = int.Parse (this.gameObject.name);
		_mailBoxActions.OpenMail (i);
	}
}
