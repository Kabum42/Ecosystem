using UnityEngine;
using System.Collections;

public abstract class Message {

	public string sender;
	public string information;

}

public class Letter : Message {

	public Letter(string auxSender, string auxInformation) {

		sender = auxSender;
		information = auxInformation;

	}

}

public class Speech : Message {

	public Speech(string auxSender, string auxInformation) {

		sender = auxSender;
		information = auxInformation;

	}

}
