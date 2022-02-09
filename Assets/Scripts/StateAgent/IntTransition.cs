using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntTransition : Transition
{
	IntRef parameter;
	int condition;
	Predicate predicate;

	public IntTransition(IntRef parameter, Predicate predicate, int condition)
	{
		this.parameter = parameter;
		this.predicate = predicate;
		this.condition = condition;
	}

	public override bool ToTransition()
	{
		bool result = false;

		switch (predicate)
		{
			case Predicate.EQUAL:
				result = (parameter == condition);
				break;
			case Predicate.LESS_EQUAL:
				result = (parameter <= condition);
				break;
			case Predicate.LESS:
				result = (parameter < condition);
				break;
			case Predicate.GREATER:
				result = (parameter > condition);
				break;
			case Predicate.GREATER_EQUAL:
				result = (parameter >= condition);
				break;
			default:
				break;
		}

		return result;
	}
}
