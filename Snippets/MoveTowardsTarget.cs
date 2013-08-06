//Epic Awesome Tank - Enemy Tank Move Towards Player Code
public override void Move()
{
	if (moving)
	{
		double deltaY = target.Position.Y - body.Position.Y;
		double deltaX = target.Position.X - body.Position.X;
		rotation = (float)Math.Atan2(deltaX, -deltaY);

		float moveDist = 0;
		moveDist -= 6;
		moveDist *= 0.75f;

		moveDist = CheckCollisions(moveDist);
		
		position.X -= moveDist * (float)(Math.Sin(rotation));
		position.Y += moveDist * (float)(Math.Cos(rotation));
	}
}
	