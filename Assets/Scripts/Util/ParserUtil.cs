#pragma warning disable 0168

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct Undefined {}

public class ParserUtil : MonoBehaviour {
	public InputField inputField;
	public Text outputField;

	private readonly List<string> parameterTypes = new List<string> {
		typeof(Verb).ToString(),
		typeof(Item).ToString(),
		typeof(Resource).ToString(),
		typeof(Direction).ToString(),
		typeof(Number).ToString()
	};

	public void ParseInput() {
		string input = inputField.text.Trim();

		string[] parameters = input.Split(' ');

		Verb verb;
		if (!IsValueOfType<Verb>(parameters[0], out verb)) {
			PrintResponse("Invalid verb!\n\n" + HelpType(typeof(Verb)));
		} else {
			if (verb == Verb.HELP) {
				if (parameters.Length > 2) {
					PrintResponse("Help command accepts up to 1 parameter, had " + (parameters.Length - 1));
				} else if (parameters.Length == 1) {
					PrintResponse(HelpGeneral());
				} else {
					ParseHelpVerb(parameters[1]);
				}
			} else {
				switch (verb) {
					case Verb.MOVE:
						parameters = AdjustParameterList(parameters);
						SingletonFactory.GetInstance<VerbUtil>().Move(parameters);
						break;
					case Verb.CRAFT:
						parameters = AdjustParameterList(parameters);
						SingletonFactory.GetInstance<VerbUtil>().Craft(parameters);
						break;
					case Verb.USE:
						parameters = AdjustParameterList(parameters);
						SingletonFactory.GetInstance<VerbUtil>().Use(parameters);
						break;
					case Verb.EQUIP:
						parameters = AdjustParameterList(parameters);
						SingletonFactory.GetInstance<VerbUtil>().Equip(parameters);
						break;
					case Verb.INVENTORY:
						SingletonFactory.GetInstance<Player>().DisplayInventory();
						break;
				}
			}
		}

		inputField.text = "";
		inputField.ActivateInputField();
	}

	private void ParseHelpVerb(string request) {
		if (parameterTypes.FindIndex(x => x.Equals(request, System.StringComparison.OrdinalIgnoreCase)) != -1) {
			switch (request.ToLower()) {
				case "verb":
					PrintResponse(HelpType(typeof(Verb)));
					break;
				case "item":
					PrintResponse(HelpType(typeof(Item)));
					break;
				case "resource":
					PrintResponse(HelpType(typeof(Resource)));
					break;
				case "direction":
					PrintResponse(HelpType(typeof(Direction)));
					break;
				case "number":
					PrintResponse("Any positive integer value\n\nDefault: " + NumberUtil.GetDefault());
					break;
				default:
					PrintResponse("Help for '" + request + "' not defined");
					break;
			}
		} else {
			System.Type type = GetValueType(request);
			if (type == typeof(Verb)) {
				Verb verbRequest = GetEnumValue<Verb>(request);
				PrintResponse(SingletonFactory.GetInstance<VerbUtil>().Help(verbRequest));
			} else if (type == typeof(Item)) {
				Item itemRequest = GetEnumValue<Item>(request);
				PrintResponse(SingletonFactory.GetInstance<ItemUtil>().Help(itemRequest));
			} else if (type == typeof(Resource)) {
				Resource resourceRequest = GetEnumValue<Resource>(request);
				PrintResponse(SingletonFactory.GetInstance<ResourceUtil>().Help(resourceRequest));
			} else if (type == typeof(Direction)) {
				Direction directionRequest = GetEnumValue<Direction>(request);
				PrintResponse(SingletonFactory.GetInstance<DirectionUtil>().Help(directionRequest));
			} else if (type == typeof(Number)) {
				uint numberRequest = SingletonFactory.GetInstance<NumberUtil>().GetNumber(request);
				PrintResponse(SingletonFactory.GetInstance<NumberUtil>().Help(numberRequest));
			} else {
				PrintResponse(request + " is " + GetValueType(request).ToString());
			}
		}
	}

	public void PrintResponse(string response) {
		outputField.text = response.ToLower();
	}

	private string[] AdjustParameterList(string[] parameters) {
		if (parameters.Length == 1) {
			parameters = new string[0];
		} else {
			parameters = parameters.Skip(1).ToArray();
		}

		return parameters;
	}

	private bool IsValueOfType<T>(string value) {
		T t;
		return IsValueOfType<T>(value, out t);
	}

	private bool IsValueOfType<T>(string value, out T enumValue) {
		try {
			enumValue = (T)System.Enum.Parse(typeof(T), value, true);
			
			if (System.Enum.IsDefined(typeof(T), enumValue)) {
				return true;
			}
		} catch (System.ArgumentException e) {
			enumValue = default(T);
		}

		return false;
	}

	public T GetEnumValue<T>(string value) {
		T t;
		IsValueOfType<T>(value, out t);

		return t;
	}

	public List<System.Type> GetValueTypes(string[] values) {
		List<System.Type> types = new List<System.Type>();

		foreach(string value in values) {
			types.Add(GetValueType(value));
		}

		return types;
	}

	public System.Type GetValueType(string value) {
		if (SingletonFactory.GetInstance<NumberUtil>().IsNumber(value)) {
			return typeof(Number);
		} else if (IsValueOfType<Verb>(value)) {
			return typeof(Verb);
		} else if (IsValueOfType<Item>(value)) {
			return typeof(Item);
		} else if (IsValueOfType<Resource>(value)) {
			return typeof(Resource);
		} else if (IsValueOfType<Direction>(value)) {
			return typeof(Direction);
		}

		return typeof(Undefined);
	}

	private List<string> GetEnumAsList(System.Type type) {
		return new List<string>(System.Enum.GetNames(type));
	}

	public string HelpGeneral() {
		string values = "";

		int i = 0;
		foreach(string type in parameterTypes) {
			if (i > 0) {
				values += "\n";
			}

			values += type;

			i++;
		}

		values += "\n\nValid command: <Verb> <Space separated parameters>";
		values += "\nExample syntax: " + SingletonFactory.GetInstance<VerbUtil>().Help(Verb.MOVE);

		return values;
	}

	public string HelpType(System.Type type) {
		string values = "";

		List<string> typeList = GetEnumAsList(type);

		int i = 0;
		foreach(string typeValue in typeList) {
			if (i > 0) {
				values += "\n";
			}

			values += typeValue.ToString();

			i++;
		}

		if (type == typeof(Direction)) {
			values += "\n\nDefault: " + DirectionUtil.GetDefault();
		}

		return values;
	}

	void Start() {
		inputField.ActivateInputField();
	}
}
