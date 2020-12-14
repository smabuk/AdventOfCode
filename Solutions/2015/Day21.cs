using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2015 {
	/// <summary>
	/// Day 21: RPG Simulator 20XX
	/// https://adventofcode.com/2015/day/21
	/// </summary>
	public class Day21 {

		record Item(string Name, int Cost, int Damage, int Armor);
		record Weapon(string Name, int Cost, int Damage, int Armor) : Item(Name, Cost, Damage, Armor);
		record Armor(string Name, int Cost, int Damage, int Armor) : Item(Name, Cost, Damage, Armor);
		record Ring(string Name, int Cost, int Damage, int Armor) : Item(Name, Cost, Damage, Armor);

		record Inventory(Weapon Weapon, Armor Armor, List<Ring> Rings) {
			public int DamageValue => Weapon.Damage + Armor.Damage + Rings.Sum(r => r.Damage);
			public int ArmorValue => Weapon.Armor + Armor.Armor + Rings.Sum(r => r.Armor);
			public int Cost => Weapon.Cost + Armor.Cost + Rings.Sum(r => r.Cost);
		}

		record Player(string Name, int HitPoints, int Damage, int Armor);

		private static int Solution1(string[] input) {
			int bossHitPoints = int.Parse(input[0].Split(": ")[1]);
			int bossDamage = int.Parse(input[1].Split(": ")[1]);
			int bossArmor = int.Parse(input[2].Split(": ")[1]);
			Player boss = new("Boss", bossHitPoints, bossDamage, bossArmor);
			const int PLAYER_HIT_POINTS = 100;
			List<Item> inventory = new();
			List<Item> shop = new() {
				new Weapon("Dagger", 8, 4, 0) ,
				new Weapon("Shortsword", 10, 5, 0) ,
				new Weapon("Warhammer", 25, 6, 0) ,
				new Weapon("Logsword", 40, 7, 0) ,
				new Weapon("Greataxe", 74, 8, 0) ,

				new Armor("No Armor", 0, 0, 0) ,
				new Armor("Leather", 13, 0, 1) ,
				new Armor("Chainmail", 31, 0, 2) ,
				new Armor("Splintmail", 53, 0, 3) ,
				new Armor("Bandedmail", 75, 0, 4) ,
				new Armor("Platemail ", 102, 0, 5) ,

				new Ring("No Ring 1", 0, 0, 0),
				new Ring("No Ring 2", 0, 0, 0),
				new Ring("Damage +1", 25, 1, 0),
				new Ring("Damage +2", 50, 2, 0),
				new Ring("Damage +3", 100, 3, 0),
				new Ring("Defense +1", 20, 0, 1),
				new Ring("Defense +2", 40, 0, 2),
				new Ring("Defense +3", 80, 0, 3)
			};


			/*
			 * Constraints:
			 * 
			 *   1 Weapon
			 * 0-1 Armor
			 * 0-2 Rings
			 * 
			*/
			int leastGold = int.MaxValue;
			foreach (Weapon weapon in shop.Where(x => x is Weapon)) {
				foreach (Armor armor in shop.Where(x => x is Armor)) {
					foreach (var rings in shop.Where(x => x is Ring)
															.Combinations(2)
															.ToArray()) {
						Inventory inv = new(weapon, armor, rings.Cast<Ring>().ToList());
						Player player = new("Player", PLAYER_HIT_POINTS, inv.DamageValue, inv.ArmorValue);
						if (PlayTheGame(player, boss)) {
							if (leastGold > inv.Cost) {
								leastGold = inv.Cost;
							}
						}

					}
				}
			}

			return leastGold;
		}

		private static bool PlayTheGame(Player player, Player boss) {
			bool playersTurn = false;
			do {
				playersTurn = !playersTurn;
				if (playersTurn) {
					int damageDone = player.Damage - boss.Armor;
					if (damageDone <= 0) {
						damageDone = 1;
					}
					boss = boss with { HitPoints = boss.HitPoints - damageDone };
				} else {
					int damageDone = boss.Damage - player.Armor;
					if (damageDone <= 0) {
						damageDone = 1;
					}
					player = player with { HitPoints = player.HitPoints - damageDone };
				}

			} while (player.HitPoints > 0 && boss.HitPoints > 0);

			return player.HitPoints > 0; // Player wins
		}

		private static int Solution2(string[] input) {
			int bossHitPoints = int.Parse(input[0].Split(": ")[1]);
			int bossDamage = int.Parse(input[1].Split(": ")[1]);
			int bossArmor = int.Parse(input[2].Split(": ")[1]);
			Player boss = new("Boss", bossHitPoints, bossDamage, bossArmor);
			const int PLAYER_HIT_POINTS = 100;
			List<Item> inventory = new();
			List<Item> shop = new() {
				new Weapon("Dagger", 8, 4, 0),
				new Weapon("Shortsword", 10, 5, 0),
				new Weapon("Warhammer", 25, 6, 0),
				new Weapon("Logsword", 40, 7, 0),
				new Weapon("Greataxe", 74, 8, 0),

				new Armor("No Armor", 0, 0, 0),
				new Armor("Leather", 13, 0, 1),
				new Armor("Chainmail", 31, 0, 2),
				new Armor("Splintmail", 53, 0, 3),
				new Armor("Bandedmail", 75, 0, 4),
				new Armor("Platemail ", 102, 0, 5),

				new Ring("No Ring 1", 0, 0, 0),
				new Ring("No Ring 2", 0, 0, 0),
				new Ring("Damage +1", 25, 1, 0),
				new Ring("Damage +2", 50, 2, 0),
				new Ring("Damage +3", 100, 3, 0),
				new Ring("Defense +1", 20, 0, 1),
				new Ring("Defense +2", 40, 0, 2),
				new Ring("Defense +3", 80, 0, 3)
			};


			/*
			 * Constraints:
			 * 
			 *   1 Weapon
			 * 0-1 Armor
			 * 0-2 Rings
			 * 
			*/
			int mostGold = 0;
			foreach (Weapon weapon in shop.Where(x => x is Weapon)) {
				foreach (Armor armor in shop.Where(x => x is Armor)) {
					foreach (var rings in shop.Where(x => x is Ring)
															.Combinations(2)
															.ToArray()) {
						Inventory inv = new(weapon, armor, rings.Cast<Ring>().ToList());
						Player player = new("Player", PLAYER_HIT_POINTS, inv.DamageValue, inv.ArmorValue);
						if (PlayTheGame(player, boss) == false) {
							if (mostGold < inv.Cost) {
								mostGold = inv.Cost;
							}
						}

					}
				}
			}

			return mostGold;
		}




		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
