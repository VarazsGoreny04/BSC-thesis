using System.Drawing;

namespace ELTE.Snake.Model;
public class SnakeFieldEventArgs : EventArgs
{
	private Point _headField;
	private Point? _tailField;

	public Point HeadField { get { return _headField; } }
	public Point? TailField { get { return _tailField; } }

	public SnakeFieldEventArgs(Point headField, Point? tailField)
	{
		_headField = headField;
		_tailField = tailField;
	}
}