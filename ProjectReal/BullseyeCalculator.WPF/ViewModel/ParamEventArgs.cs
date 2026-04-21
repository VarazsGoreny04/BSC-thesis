using System;

namespace BullseyeCalculator.WPF.ViewModel;

public class ParamEventArgs : EventArgs
{
	private readonly object? param;

	public object? Param => param;

	public ParamEventArgs(object? param) => this.param = param;
}