//Epic Awesome Tank - Enemy Turret Face Player
protected override void RotateTurret()
{
	//Calculate angle between tank and player
	double deltaY = target.Position.Y - turret.Position.Y;
	double deltaX = target.Position.X - turret.Position.X;
	double angleDegrees = Math.Atan2(deltaY, deltaX) * 180 / MathHelper.Pi;
	turretRotation = (float)Math.Atan2(deltaX, -deltaY);
}