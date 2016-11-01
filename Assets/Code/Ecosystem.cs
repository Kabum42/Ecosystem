using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Ecosystem {

	private static bool started = false;

	public static float deathRate = 1f/100f;
	public static float cleaning = 100f/100f;
	public static List<SpeciesData> speciesDataList = new List<SpeciesData>();

	public static void Start() {
		if (!started) {
			started = true;
			TrueStart ();
		}
	}

	private static void TrueStart() {

		SpeciesData sp;

		//TREE
		sp = new SpeciesData(Species.Tree, 20f);
		sp.populationCap = 1000;
		sp.AddBooster (Species.Bee);
		speciesDataList.Add (sp);

		//BUSH
		sp = new SpeciesData(Species.Bush, 20f);
		sp.populationCap = 2000;
		sp.AddBooster (Species.Bee);
		speciesDataList.Add (sp);

		//BEE
		sp = new SpeciesData(Species.Bee, 20f);
		sp.populationCap = 3000;
		sp.AddBooster (Species.Tree);
		sp.AddBooster (Species.Bush);
		speciesDataList.Add (sp);

		//RABBIT
		sp = new SpeciesData(Species.Rabbit, 20f);
		sp.AddPrey (Species.Bush);
		speciesDataList.Add (sp);

		//DEER
		sp = new SpeciesData(Species.Deer, 5f);
		sp.AddPrey (Species.Bush);
		speciesDataList.Add (sp);

		//WOLF
		sp = new SpeciesData(Species.Wolf, 5f);
		sp.AddPrey (Species.Rabbit);
		sp.AddPrey (Species.Deer);
		speciesDataList.Add (sp);

		//BEAR
		sp = new SpeciesData(Species.Bear, 1.1f);
		sp.AddPrey (Species.Bush);
		sp.AddPrey (Species.Bee);
		sp.AddPrey (Species.Rabbit);
		sp.AddPrey (Species.Salmon);
		speciesDataList.Add (sp);

		//SALMON
		sp = new SpeciesData(Species.Salmon, 20f);
		sp.populationCap = 100;
		speciesDataList.Add (sp);


	}


	public static SpeciesData GetSpeciesData(Species species) {

		foreach (SpeciesData sp in speciesDataList) {
			if (sp.species == species) {
				return sp;
			}
		}

		return null;
	}


	public static void Simulate() {

		foreach (SpeciesData sp in speciesDataList) {
			sp.Grow ();
		}

		foreach (SpeciesData sp in speciesDataList) {
			if (sp.populationCap >= 0 && sp.population > sp.populationCap) {
				sp.population = sp.populationCap;
			}
		}

	}

}

public class SpeciesData {

	public Species species;
	public float population = 100f;
	public float reproductionRate = 0f;
	public int populationCap = -1;

	private List<Species> preys = new List<Species> ();
	private List<Species> boosters = new List<Species> ();

	public SpeciesData(Species auxSpecies, float auxReproductionRate) {

		species = auxSpecies;
		reproductionRate = auxReproductionRate;

	}

	public void AddPrey(Species prey) {

		preys.Add (prey);

	}

	public void AddBooster(Species booster) {

		boosters.Add (booster);

	}

	private float Predate() {

		float foodModifier = 0f;

		if (preys.Count == 0 && boosters.Count == 0) {
			
			foodModifier = 1f;

		} else {
			
			foreach (Species prey in preys) {

				float weight = 1f / ((float)preys.Count + (float) boosters.Count);
				SpeciesData preyData = Ecosystem.GetSpeciesData (prey);

				float wantToEat = population * weight;
				float eat = wantToEat;

				if (eat > preyData.population * preyData.reproductionRate/100f *0.1f) {
					eat = preyData.population * preyData.reproductionRate/100f *0.1f;
				}

				preyData.population -= eat;
				foodModifier += (eat/wantToEat)*weight;

			}

			foreach (Species booster in boosters) {

				float weight = 1f / ((float)preys.Count + (float) boosters.Count);
				SpeciesData boosterData = Ecosystem.GetSpeciesData (booster);

				float wantToEat = population * weight * 0.1f;
				float eat = wantToEat;

				if (eat > boosterData.population) {
					eat = boosterData.population;
				}
					
				// BOOSTERS'POPULATION ISN'T REDUCED IN THE PROCESS
				foodModifier += (eat/wantToEat)*weight;

			}

		}

		return foodModifier;

	}

	public void Grow() {

		float populationChange = 0f;

		// REPRODUCTION
		if (population >= 2) {
			float foodModifier = Predate();
			populationChange += (reproductionRate/100f) * foodModifier;
		}

		// DEATH
		populationChange -= Ecosystem.deathRate;

		population += population * populationChange;

		if (population < 0) {
			population = 0;
		}

	}

}

public enum Species {
	Tree,
	Bush,
	Bee,
	Rabbit,
	Deer,
	Wolf,
	Bear,
	Salmon
}


