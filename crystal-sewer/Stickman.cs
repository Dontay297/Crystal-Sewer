using Godot;
using System;


public partial class Stickman : CharacterBody3D
{
    private Node3D _head;
    private Camera3D _view;
    //Variables moving the camera with the player
    private float _cameraAngle = 0F;
    private float _mouseSensitivity = 0.1F;
    private float _moveSpeed = 5F;
    private float _originalSpeed = 5F;
    private float _sprintSpeed = 9F;

    private float _crouchSpeed = 3F;

    // variable for keeping the player on the ground
    private const float Gravity = 30.0F;

    // variables for the crouching system
    private bool _isCrouching = false;
    private CollisionShape3D _standingShape;
    private CollisionShape3D _crouchingShape;
    private AnimationPlayer _Anim;

    //Orb collection
    public Orb CurrentOrb { get; set; }

    //Stamina variables
    private float _maxStamina = 100f;
    private float _currentStamina = 100f;
    private float _staminaRegen = 5f;
    private float _staminaDrain = 20f;

    private bool _isExhausted = false;

    //Public method to trigger all of the nodes in the scence
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _head = GetNode<Node3D>("Head");
        _view = GetNode<Camera3D>("Head/View");
        _standingShape = GetNode<CollisionShape3D>("StandingCollisionShape");
        _crouchingShape = GetNode<CollisionShape3D>("CrouchingShape");
        _Anim = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    //method to process what is happening every frame
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (Input.IsActionPressed("interact") && CurrentOrb != null)
        {
            CurrentOrb.Collect();
            CurrentOrb = null;
        }

    }

    //same as process but with object movement 
    public override void _PhysicsProcess(double delta)
    {
        Movement();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion motion) return;

        _head.RotateY(Mathf.DegToRad(-motion.Relative.X * _mouseSensitivity));
        float change = -motion.Relative.Y * _mouseSensitivity;

        if (!((change + _cameraAngle) < 90F) || !((change + _cameraAngle) > -90F)) return;

        _view.RotateX(Mathf.DegToRad(change));
        _cameraAngle += change;
    }

    private void Movement()
    {
        Vector3 direction = new();
        Basis aim = _view.GlobalTransform.Basis;

        if (Input.IsActionPressed("move_up"))
        {
            direction -= aim.Z;
        }
        if (Input.IsActionPressed("move_back"))

        {
            direction += aim.Z;
        }

        if (Input.IsActionPressed("move_left"))
        {
            direction -= aim.X;
        }

        if (Input.IsActionPressed("move_right"))
        {
            direction += aim.X;
        }
        if (Input.IsActionJustPressed("crouch"))
        {
            ToggleCrouch();
        }

        if (Input.IsActionPressed("run") && _currentStamina > 0 && !_isExhausted)
        {
            _moveSpeed = _sprintSpeed;
            _currentStamina -= _staminaDrain * (float)GetPhysicsProcessDeltaTime();

            if (_currentStamina <= 0)
            {
                _currentStamina = 0;
                _isExhausted = true;
            }

        }
        else
        {
            _moveSpeed = _originalSpeed;

            if (_currentStamina < _maxStamina)
            {
                _currentStamina += _staminaRegen * (float)GetPhysicsProcessDeltaTime();
            }

            if (_isExhausted && _currentStamina > 30.0f)
            {
                _isExhausted = false;
            }
        }

        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);

        float currentSpeed = _isCrouching ? _crouchSpeed : _moveSpeed;

        Vector3 horizontalVelocity = direction.Normalized() * currentSpeed;

        float newY = Velocity.Y;

        if (!IsOnFloor())
        {
            newY -= Gravity * (float)GetPhysicsProcessDeltaTime();
        }
        else
        {
            newY = -1f;
        }

        Velocity = new Vector3(horizontalVelocity.X, Velocity.Y - Gravity * (float)GetPhysicsProcessDeltaTime(), horizontalVelocity.Z);


        MoveAndSlide();
    }

    private void ToggleCrouch()
    {
        _isCrouching = !_isCrouching;

        _standingShape.Disabled = _isCrouching;
        _crouchingShape.Disabled = !_isCrouching;

        if (_isCrouching)
        {
            _Anim.Play("StandingToCrouch");
        }
        else
        {
            _Anim.Play("RESET");
        }

    }

    
}






