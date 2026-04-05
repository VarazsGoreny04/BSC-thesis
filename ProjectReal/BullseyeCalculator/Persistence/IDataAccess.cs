using System.Collections.Generic;

namespace BullseyeCalculator.Persistence;

public interface IDataAccess
{
	public List<string> Read(string input);
}