using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Ecosystem {

	private static bool started = false;
	private static List<SpeciesData> speciesDataList = new List<SpeciesData>();

	public static void Start() {
		if (!started) {
			started = true;
			TrueStart ();
		}
	}

	private static void TrueStart() {

		SpeciesData sp;

		sp = new SpeciesData (Species.Bear);


	}


	public static SpeciesData GetSpeciesData(Species species) {
		return null;
	}
		

}

public class SpeciesData {

	public Species species;

	public SpeciesData(Species auxSpecies) {

		species = auxSpecies;

	}

}

public enum Species {
	Bear,
	Flower
}


