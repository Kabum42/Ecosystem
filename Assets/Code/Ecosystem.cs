using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Ecosystem {

	private static bool started = false;

	public static float deathRate = 1f/100f;
	public static List<SpeciesData> speciesDataList = new List<SpeciesData>();

	private static int vegetationCap = 1000;
	private static float vegetationReproduction = 0.3f;

	public static float friendshipGobierno = 0f;
	public static float friendshipCooperativa = 0f;
	public static float friendshipEcologistas = 0f;

	public static void Start() {
		if (!started) {
			started = true;
			TrueStart ();
		}
	}

	private static void TrueStart() {

		SpeciesData sp;

		//VEGETATION
		sp = new SpeciesData(Species.Vegetacion, deathRate + vegetationReproduction);
		sp.population = 1000;
		sp.initialPopulation = sp.population;
		sp.populationCap = vegetationCap;
		sp.AddBooster (Species.NidoAbejas);
		speciesDataList.Add (sp);

		//BEE
		sp = new SpeciesData(Species.NidoAbejas, deathRate + vegetationReproduction);
		sp.population = 50;
		sp.initialPopulation = sp.population;
		sp.populationCap = 50;
		sp.AddBooster (Species.Vegetacion);
		speciesDataList.Add (sp);

		//RABBIT
		sp = new SpeciesData(Species.Conejos, deathRate + vegetationReproduction * 1.5f);
		sp.population = 366;
		sp.initialPopulation = sp.population;
		sp.AddPrey (Species.Vegetacion);
		speciesDataList.Add (sp);

		//SALMON
		sp = new SpeciesData(Species.Salmones, deathRate + vegetationReproduction * 0.31f);
		sp.population = 100;
		sp.initialPopulation = sp.population;
		sp.AddPrey (Species.Vegetacion);
		speciesDataList.Add (sp);

		//DEER
		sp = new SpeciesData(Species.Ciervos, deathRate + vegetationReproduction * 0.32f);
		sp.population = 100;
		sp.initialPopulation = sp.population;
		sp.AddPrey (Species.Vegetacion);
		speciesDataList.Add (sp);

		//WOLF
		sp = new SpeciesData(Species.Lobos, deathRate + vegetationReproduction * 0.05f);
		sp.population = 62;
		sp.initialPopulation = sp.population;
		sp.AddPrey (Species.Conejos);
		sp.AddPrey (Species.Ciervos);
		speciesDataList.Add (sp);

		//BEAR
		sp = new SpeciesData(Species.Osos, deathRate + vegetationReproduction * 0.01f);
		sp.population = 20;
		sp.initialPopulation = sp.population;
		sp.AddPrey (Species.NidoAbejas);
		sp.AddPrey (Species.Conejos);
		sp.AddPrey (Species.Salmones);
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
	public float initialPopulation = 0f;
	public float population = 100f;
	public float reproductionRate = 0f;
	public int populationCap = -1;

	private List<Species> preys = new List<Species> ();
	private List<Species> boosters = new List<Species> ();

	public SpeciesData(Species auxSpecies, float auxReproductionRate) {

		species = auxSpecies;
		reproductionRate = auxReproductionRate;

	}


	public float GetPercentage() {

		float percentage = population / (initialPopulation * 2f);
		percentage = Mathf.Clamp (percentage, 0f, 1f);
		return percentage;

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

				if (eat > preyData.population * 0.05f) {
					eat = preyData.population * 0.05f;
				}

				preyData.population -= eat;
				foodModifier += (eat/wantToEat)*weight;

			}

			foreach (Species booster in boosters) {

				float weight = 1f / ((float)preys.Count + (float) boosters.Count);
				SpeciesData boosterData = Ecosystem.GetSpeciesData (booster);

				float wantToEat = population * weight * 0.05f;
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
			populationChange += (reproductionRate) * foodModifier;
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
	Vegetacion,
	NidoAbejas,
	Conejos,
	Ciervos,
	Lobos,
	Osos,
	Salmones
}


