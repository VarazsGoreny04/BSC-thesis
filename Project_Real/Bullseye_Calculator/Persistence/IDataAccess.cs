namespace Bullseye_Calculator.Persistence;
using System.Collections.Generic;

public interface IDataAccess
{
	public List<string> Read(string input);
}