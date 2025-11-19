using Raylib_cs;
using System.Numerics;

namespace falling_circles;

public enum particleType {
	Circle = 1,
	Box = 4,
	Scatter = 3,
}

public class Entity {
	public int Radius { get; set;}
	public int X { get; set;}
	public int Y { get; set;}
	public int Speed { get; set; }
	public bool Active { get; set; }
	public Color InitColor { get; set;}
	public Color CurColor { get; set;}
	
	public particleType particletype {get; set;}
	public int maxSize { get; set; }
	public int minSize { get; set; }
	public int newAlpha { get; set; }
	public bool partActive { get; set; }

	public int r { get; set; }
	public int b { get; set; }
	public int g { get; set; }
	public int a { get; set; }

	public particleType[] partTypes {get; set;}

	public Vector2 Position { get; set; }


	public Entity() {}
	public Entity(int con_x,int con_y, int rad, int spd, bool act, Color init, Color cur) {
		X = con_x;
		Y = con_y;
		Radius = rad;
		Speed = spd;
		Active = act;
		InitColor = init;
		CurColor = cur;
		Position = new Vector2(X,Y);
		r = CurColor.R;
		b = CurColor.B;
		g = CurColor.G;
		a = CurColor.A;

		partTypes = new particleType[] {
			particleType.Circle,
			particleType.Box,
			particleType.Scatter
		};

		var rand = new Random();
		particletype = partTypes[rand.Next(0, partTypes.Length)];
		maxSize = Radius;
		minSize = Radius / 2;
		newAlpha = 255;
		partActive = false;
	}

	public virtual void updatePosition() {
		this.Position = new Vector2(this.X, this.Y);
	}

	public virtual void draw() {
		if(this.Active == true) {
			Raylib.DrawCircle((int)this.X, (int)this.Y, this.Radius, new Color(r, b, g, a));
			// Raylib.DrawText($"{a}", this.X, this.Y - 32, 16, Color.Black);
		}

		if(partActive == true) {
			switch(particletype) {
				case particleType.Circle:
					Raylib.DrawCircleLines(X, Y, minSize, new Color(r, g, b, newAlpha)); 
				break;
				case particleType.Box:
					Raylib.DrawRectangleLines(X, Y, minSize, minSize, new Color(r, g, b, newAlpha));
				break;
				case particleType.Scatter:
					Raylib.DrawPolyLines(new Vector2(X, Y), (int)particleType.Scatter, minSize, 0, new Color(r, g, b, newAlpha));
				break;
			}
		} 
	}

	public void fall() {
		this.Y += Speed;
	}
	public void fall_right() {
		this.X += Speed;
	}
	public void fall_left() {
		this.X -= Speed;
	}

	public void track_point(Player play) {
		if(this.a <= 0) {
			if (X > 0)
				X-=5;
			if (Y > 0)
				Y-=5;
			partActive = true;
			newAlpha -= 5 ;
			if (newAlpha % 25 == 0) {
				if( minSize < maxSize) {
					minSize++;
				}
			}
		}

 		if(newAlpha - 5 < 0) {
			partActive = false;
			this.Speed = 0;
			this.X = 0 - Radius;
			this.Y = 0 + Radius;
			this.Active = false;
			a++;
		 }
	}

	public void lost_health(Player play) {
		if (Y > 480) {
			Y -= Radius * 2;
			Speed = 0;
			this.a = 0;
			play.Health--;
			play.killStreak = 0;
		}
	}
};

public class Player : Entity {
	public int SpecialCounter { get; set; }
	public bool specActive { get; set; }
	public int specActiveCounter { get; set; }
	public int Health { get; set; }
	public int score { get; set; }
	public int[] attackDmg { get; set; }
	public Entity[] lastEntity { get; set; }
	public int killStreak {get; set;}
	public int healthStreak {get; set;}
	public int attackMultiplierIndex { get; set; }
	
	public Player(Entity ent, int special, int heal) {
		this.X = ent.X;
		this.Y = ent.Y;
		this.Radius = ent.Radius;
		this.Active = ent.Active;
		this.Speed = ent.Speed;
		this.InitColor = ent.InitColor;
		this.CurColor = ent.CurColor;
		this.Position = new Vector2(X,Y);
		this.r = ent.r;
		this.b = ent.b;
		this.g = ent.g;
		this.a = ent.a;
		SpecialCounter = special;
		specActive = false;
		specActiveCounter = 0;
		Health = heal;
		attackDmg = new int[] {
			55, 155, 256
		};
		lastEntity = new Entity[1];
		lastEntity[0] = new Entity(X, Y, Radius, 0, false, Color.Purple, Color.DarkPurple);
	}

	public void attack(Entity[] ent) {
		for(int i = 0; i < ent.Length; i++) {
			if(Raylib.CheckCollisionCircles(this.Position, this.Radius, ent[i].Position, ent[i].Radius)  ) {
				if( specActive == true && Raylib.CheckCollisionCircles(this.Position, this.Radius, ent[i].Position, ent[i].Radius) && ent[i].a != 0) {
					specActiveCounter++;
					ent[i].a = 0;
					score++;
					killStreak++;
					if (specActiveCounter == 10)  {
						specActive = false;
						specActiveCounter = 0;
					}
				}
				lastEntity[0] = ent[i];
				ent[i].CurColor = Color.Blue;
				if (ent[i].Active == true && ent[i].partActive == false)
					if(Raylib.IsMouseButtonPressed(MouseButton.Left)) {
						// this.score++;
						if(ent[i].a - attackDmg[attackMultiplierIndex] < 0) {
							int bufferDmg = ent[i].a - attackDmg[attackMultiplierIndex];
							bufferDmg *= -1;
							ent[i].a += bufferDmg;
							ent[i].a -= attackDmg[attackMultiplierIndex];
							ent[i].Speed = 0;
							// ent[i].X = 0 - Radius;
							// ent[i].Y = 0 + Radius;
							this.score += 1;
							killStreak++;
							// ent[i].Active = false;
						} else {
							ent[i].a -= attackDmg[attackMultiplierIndex];
						}
					}
				}
		}

		if(Raylib.IsMouseButtonPressed(MouseButton.Right)) {
			this.SpecialCounter--;
			specActive = true;
			if(this.SpecialCounter < 0)
				this.SpecialCounter = 0;
		}
	}

	public override void updatePosition() {
		this.X = Raylib.GetMouseX();
		this.Y = Raylib.GetMouseY();
		this.Position = new Vector2(this.X, this.Y);
	}

	public override void draw() {
		Raylib.DrawCircle((int)this.X, (int)this.Y, this.Radius, new Color(r, b, g, a));
		Raylib.DrawText($"{specActiveCounter}", X, Y - Radius, 16, Color.Black);
	}

	public void monitorStreak() {
		if(killStreak >= 20 && SpecialCounter <= 3) {
			killStreak = 0;
			if (SpecialCounter != 3) {
				SpecialCounter++;
			}
			if (attackMultiplierIndex != 2  ) {
				attackMultiplierIndex++;
			} 			
		}

		if(healthStreak >= 50 && Health <= 3) {
			Health++;
			healthStreak = 0;
		}
	}
	
}


