using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Message {

	public Type type;
	public string sender;
	public string information;
	public List<Option> options;

	// LETTER ONLY
	public Stamp stamp;

	// SPEECH ONLY

	public Message(Type auxType, string auxSender, string auxInformation, List<Option> auxOptions, Stamp auxStamp) {

		type = auxType;
		sender = auxSender;
		information = auxInformation;
		options = auxOptions;
		stamp = auxStamp;

	}

	public enum Type {
		Letter,
		Speech
	}

	public enum Stamp {
		TopSecret,
		Forbidden
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

	public Statistic statistic;
	public float change = 0;

	public Consequence(Statistic auxStatistic, float auxChange) {
		statistic = auxStatistic;
		change = auxChange;
	}

}

public enum Statistic {
	Deer,
	Bear,
	Wolf
}
