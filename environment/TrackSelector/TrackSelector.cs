using Godot;
using System;

public partial class TrackSelector : StaticBody3D
{
	public Directions SelectedDirection { get; set; }

	private Label3D display = null!;
	private AudioStreamPlayer3D beepSound = null!;

    private bool _isActive = false;

    public override void _Ready()
	{
		display = GetNode<Label3D>("Display");
		beepSound = GetNode<AudioStreamPlayer3D>("BeepSound");

        TrainEventBus.Instance.TrainDirectionChanged += OnTrainDirectionChanged;
		TrainEventBus.Instance.TrainLocationTravelled += OnTrainLocationTravelled;
    }

	public void UpdateDirection(Directions newDirection)
	{
		SelectedDirection = newDirection;
		display.Text = SelectedDirection.ToString();
    }

    public void CycleDirectionRight()
	{
		if (!_isActive)
		{
			return;
		}

        var newDirection = SelectedDirection switch
		{
			Directions.North => Directions.East,
			Directions.East => Directions.South,
			Directions.South => Directions.West,
			Directions.West => Directions.North,
			_ => SelectedDirection
		};
		TrainEventBus.Instance.ChangeTrainDirection(newDirection);
    }

	public void CycleDirectionLeft()
	{
        if (!_isActive)
        {
            return;
        }

        var newDirection = SelectedDirection switch
		{
			Directions.North => Directions.West,
			Directions.East => Directions.North,
			Directions.South => Directions.East,
			Directions.West => Directions.South,
			_ => SelectedDirection
		};
        TrainEventBus.Instance.ChangeTrainDirection(newDirection);
    }

    private void OnTrainDirectionChanged(TrainDirectionChangedEventArgs args)
    {
        if (_isActive)
        {
            UpdateDirection(args.NewDirection);
        }
    }

	private void OnTrainLocationTravelled(Location location)
	{
		_isActive = location is Junction;

		if(!_isActive)
		{
			display.Text = "";
        }
		else
		{
			beepSound.Play();

            var train = GetTree().GetFirstNodeInGroup("train") as Train;
			if(train != null)
			{
				UpdateDirection(train.FacingDirection);
            }
        }
    }
}
