using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Message {

	public Type type;
	public string sender;
	public string information;
	public List<Option> options = new List<Option>();
	private Header lastHeader = Header.Null;
	private Option lastOption = null;
	private Consequence lastConsequence = null;

	// LETTER ONLY
	public Stamp stamp;

	// SPEECH ONLY

	public Message(string path) {

		path = "Messages/" + path;
		string info = (Resources.Load (path) as TextAsset).text;
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
				if (lastHeader == Header.Type) {
					type = (Type)Enum.Parse (typeof(Type), s);
				} else if (lastHeader == Header.Sender) {
					sender = s;
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
						lastConsequence.species = (Species)Enum.Parse (typeof(Species), s);
					}

				}
			}
		}

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
	public List<Consequence> consequences = new List<Consequence>();

	public Option() {

	}

}

public class Consequence {

	public Species species;
	public float change = 0;

	public Consequence() {

	}

}
