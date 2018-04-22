using System.Collections;
using System.Collections.Generic;

public enum Direction {
	NORTH,
	EAST,
	SOUTH,
	WEST
}

public class DirectionUtil {

	public string Help(Direction direction) {
		string response = "Direction '" + direction + "' does not have defined help!";

		switch (direction) {
			case Direction.NORTH:
				response = "North ^";
				break;
			case Direction.EAST:
				response = "East >";
				break;
			case Direction.SOUTH:
				response = "South v";
				break;
			case Direction.WEST:
				response = "West <";
				break;
		}

		return response;
	}

	public static Direction GetDefault() {
		return Direction.NORTH;
	}
}
