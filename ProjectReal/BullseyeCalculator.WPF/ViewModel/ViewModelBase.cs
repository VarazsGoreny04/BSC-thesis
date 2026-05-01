using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BullseyeCalculator.WPF.ViewModel;

public abstract class ViewModelBase : INotifyPropertyChanged
{
	#region Events

	public event PropertyChangedEventHandler? PropertyChanged;

	#endregion

	#region Constructors

	protected ViewModelBase() { }

	#endregion

	#region Protected methods

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	#endregion
}