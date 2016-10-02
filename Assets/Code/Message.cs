using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Message {

	public string sender;
	public string information;
	public List<Option> options;

}

public class Letter : Message {

	public Stamp stamp;

	public Letter(string auxSender, string auxInformation, List<Option> auxOptions, Stamp auxStamp) {

		sender = auxSender;
		information = auxInformation;
		options = auxOptions;
		stamp = auxStamp;

	}

	public enum Stamp {
		TopSecret,
		Forbidden
	}

}

public class Speech : Message {

	public Speech(string auxSender, string auxInformation, List<Option> auxOptions) {

		sender = auxSender;
		information = auxInformation;
		options = auxOptions;

	}

}

public class Option {

	public string text;
	public List<Consequence> consequences;

	public Option(string auxText, List<Consequence> auxConsequences) {

		text = auxText;
		consequences = auxConsequences;

	}

}

public class Consequence {

	public Type type;
	public float change = 0;

	public Consequence(Type auxType, float auxChange) {
		type = auxType;
		change = auxChange;
	}

	public enum Type {
		bee,
		flower
	}

}
