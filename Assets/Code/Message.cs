using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Message {

	public Faction sender;
	public string information;
	public List<Option> options = new List<Option>();
	private Header lastHeader = Header.Null;
	private Option lastOption = null;
	private Consequence lastConsequence = null;

	// LETTER ONLY
	public Stamp stamp;

	// SPEECH ONLY

	public Message(TextAsset tAsset) {

		string info = tAsset.text;
		string[] subStrings = info.Split (MessageCrafter.infoSeparator.ToCharArray());

		foreach (string s in subStrings) {
			if (s [0].ToString() == MessageCrafter.infoHeader) {
				// IT'S A HEADER
				string s2 = s.Substring (1, s.Length - 1);

				foreach (Header header in Enum.GetValues(typeof(Header))) {
					if (s2 == header.ToString ()) {
						lastHeader = header;
						if (lastHeader == Header.Option) {
							lastOption = new Option ();
							options.Add (lastOption);
						} else if (lastHeader == Header.Consequence) {
							lastConsequence = new Consequence ();
							lastOption.consequences.Add (lastConsequence);
						}
						break;
					}
				}

			} else {
				// NOT A HEADER
				// type = (Type)Enum.Parse (typeof(Type), s);
				if (lastHeader == Header.Sender) {
					sender = (Faction)Enum.Parse (typeof(Faction), s);
				} else if (lastHeader == Header.Information) {
					information = s;
				} else if (lastHeader == Header.Option) {
					// OPTION
					lastOption.text = s;
				} else if (lastHeader == Header.Consequence) {
					// CONSEQUENCE

					float change = 0f;

					if (float.TryParse (s, out change)) {
						lastConsequence.change = change;
					} else {
						
						string[] subS = s.Split ('.');

						if (subS.Length == 1) {
							lastConsequence.action = s;
						} else if (subS.Length == 2) {
							lastConsequence.species = (Species)Enum.Parse (typeof(Species), subS[0]);
							lastConsequence.action = subS [1];
						}

					}

				}
			}
		}

	}

	public enum Faction {
		Gobierno,
		Cooperativa,
		Ecologistas,
		Otro
	}

	public enum Stamp {
		TopSecret,
		Forbidden
	}

}

public class Option {

	public string text;
	public List<Consequence> consequences = new List<Consequence>();

	public Option() {

	}

}

public class Consequence {

	public Species species;
	public float change = 0;
	public string action;

	public Consequence() {

	}

}
