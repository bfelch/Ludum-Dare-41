using System.Collections;
using System.Collections.Generic;

public enum Resource {
	WOOD,
	STONE,
	IRON
}

public class ResourceUtil {

	public string Help(Resource resource) {
		string response = "Resource '" + resource + "' does not have defined help!";

		switch (resource) {
			case Resource.WOOD:
				response = "Gather wood by chopping down trees";
				break;
			case Resource.STONE:
				response = "Gather stone by breaking rocks";
				break;
			case Resource.IRON:
				response = "Gather iron by breaking iron nodes";
				break;
		}

		return response;
	}
}
