#pragma warning disable 0168

public enum Number {
}

public class NumberUtil {
	public string Help(uint number) {
		return number + " is a valid number";
	}

	public uint GetNumber(string value) {
		uint number;

		IsNumber(value, out number);

		return number;
	}

	public bool IsNumber(string value) {
		uint number;
		return IsNumber(value, out number);
	}

	private bool IsNumber(string value, out uint number) {
		number = GetDefault();
		try {
			number = uint.Parse(value);
			return true;
		} catch (System.Exception e) {
		}

		return false;
	}

	public static uint GetDefault() {
		return 1;
	}
}