using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Verb {
	HELP,
	MOVE,
	CRAFT,
	USE,
	EQUIP,
	INVENTORY
}

public class VerbUtil {

	public static readonly Dictionary<Verb, List<System.Type>> verbParameters = new Dictionary<Verb, List<System.Type>> {
		{Verb.MOVE,			new List<System.Type>{typeof(Direction), typeof(Number)}},
		{Verb.CRAFT,		new List<System.Type>{typeof(Item), typeof(Number)}},
		{Verb.USE,			new List<System.Type>{typeof(Direction), typeof(Number)}},
		{Verb.EQUIP,		new List<System.Type>{typeof(Item)}}
	};

	public string Help(Verb verb) {
		string response = "Verb '" + verb.ToString() + "' does not have a defined parameter list!";

		if (verbParameters.ContainsKey(verb)) {
			response = verb.ToString() + ParametersToString(verbParameters[verb]);
		} else if (verb == Verb.HELP) {
			response = "http://www.dictionary.com/browse/help";
		} else if (verb == Verb.INVENTORY) {
			response = "Displays your inventory";
		}

		return response;
	}

	public void Move(string[] parameters) {
		List<object> values;
		if (BuildParameterList(Verb.MOVE, parameters, out values)) {
			SingletonFactory.GetInstance<Player>().Move((Direction)System.Enum.Parse(typeof(Direction), values[0].ToString()), int.Parse(values[1].ToString()));
		} else {
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Parameters invalid for 'move'\nMove" + ParametersToString(verbParameters[Verb.MOVE]));
		}
	}

	public void Craft(string[] parameters) {
		List<object> values;
		if (BuildParameterList(Verb.CRAFT, parameters, out values)) {
			SingletonFactory.GetInstance<Player>().Craft((Item)System.Enum.Parse(typeof(Item), values[0].ToString()), int.Parse(values[1].ToString()));
		} else {
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Parameters invalid for 'craft'\nCraft: " + ParametersToString(verbParameters[Verb.CRAFT]));
		}
	}

	public void Use(string[] parameters) {
		List<object> values;
		if (BuildParameterList(Verb.USE, parameters, out values)) {
			SingletonFactory.GetInstance<Player>().Use((Direction)System.Enum.Parse(typeof(Direction), values[0].ToString()), int.Parse(values[1].ToString()));
		} else {
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Parameters invalid for 'use'\nUse: " + ParametersToString(verbParameters[Verb.USE]));
		}
	}

	public void Equip(string[] parameters) {
		List<object> values;
		if (BuildParameterList(Verb.EQUIP, parameters, out values)) {
			SingletonFactory.GetInstance<Player>().Equip((Item)System.Enum.Parse(typeof(Item), values[0].ToString()));
		} else {
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Parameters invalid for 'equip'\nEquip: " + ParametersToString(verbParameters[Verb.EQUIP]));
		}
	}

	private string ParametersToString(List<System.Type> parameters) {
		string parameterString = "";

		foreach(System.Type parameter in parameters) {
			parameterString += " <" + parameter.ToString() + ">";
		}

		return parameterString;
	}

	private bool BuildParameterList(Verb verb, string[] parameters, out List<object> values) {
		ParserUtil parserUtil = SingletonFactory.GetInstance<ParserUtil>();

		values = new List<object>();

		if (IsValidCommandSyntax(verb, parserUtil.GetValueTypes(parameters))) {
			switch (verb) {
				case Verb.MOVE:
				case Verb.USE:
					if (parameters.Length >= 2) {
						values.Add(parserUtil.GetEnumValue<Direction>(parameters[0]));
						values.Add(int.Parse(parameters[1]));
					} else if (parameters.Length == 1) {
						System.Type parameterType = parserUtil.GetValueType(parameters[0]);
						if (parameterType == typeof(Direction)) {
							values.Add(parserUtil.GetEnumValue<Direction>(parameters[0]));
							values.Add(NumberUtil.GetDefault());
						} else if (parameterType == typeof(Number)) {
							values.Add(DirectionUtil.GetDefault());
							values.Add(int.Parse(parameters[0]));
						}
					} else {
						values.Add(DirectionUtil.GetDefault());
						values.Add(NumberUtil.GetDefault());
					}
					break;
				case Verb.CRAFT:
					if (parameters.Length >= 2) {
						values.Add(parserUtil.GetEnumValue<Item>(parameters[0]));
						values.Add(int.Parse(parameters[1]));
					} else if (parameters.Length == 1) {
						System.Type parameterType = parserUtil.GetValueType(parameters[0]);
						if (parameterType == typeof(Item)) {
							values.Add(parserUtil.GetEnumValue<Item>(parameters[0]));
							values.Add(NumberUtil.GetDefault());
						}
					}
					break;
				case Verb.EQUIP:
					if (parameters.Length >= 1) {
						values.Add(parserUtil.GetEnumValue<Item>(parameters[0]));
					}
					break;
			}
		} else {
			return false;
		}

		return true;
	}

	public bool IsValidCommandSyntax(Verb verb, List<System.Type> parameterTypes) {
		if (parameterTypes.SequenceEqual(verbParameters[verb])) {
			return true;
		} else {
			switch (verb) {
				case Verb.MOVE:
				case Verb.USE:
					if (parameterTypes.Count == 1) {
						return (parameterTypes[0] == typeof(Direction) || parameterTypes[0] == typeof(Number));
					} else if (parameterTypes.Count == 0) {
						return true;
					}
					break;
				case Verb.CRAFT:
					if (parameterTypes.Count == 1) {
						return parameterTypes[0] == typeof(Item);
					}
					break;
			}
		}

		return false;
	}
}
