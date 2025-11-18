using Raylib_cs;
using System.Numerics;

namespace falling_circles;

public class Entity {
	public int Radius { get; set;}
	public int X { get; set;}
	public int Y { get; set;}
	public int Speed { get; set; }
	public bool Active { get; set; }
	public Color InitColor { get; set;}
	public Color CurColor { get; set;}
	
	public int r { get; set; }
	public int b { get; set; }
	public int g { get; set; }
	public int a { get; set; }

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

		}

	public virtual void updatePosition() {
		this.Position = new Vector2(this.X, this.Y);
	}

	public virtual void draw() {
		Raylib.DrawCircle((int)this.X, (int)this.Y, this.Radius, new Color(r, b, g, a));
		Raylib.DrawText($"{a}", this.X, this.Y - 24, 16, Color.Black);
	}

	public void fall() {
		this.Y += Speed;
	}

	public void track_point(Player play) {
		if(this.a <= 0) {
			this.Speed = 0;
			this.X = 0 - Radius;
			this.Y = 0 + Radius;
			play.score += 1;
			this.Active = false;
		}
	}
};

public class Player : Entity {
	public int SpecialCounter { get; set; }
	public int Health { get; set; }
	public int score { get; set; }
	public int attackDmg { get; set; }

	public Player(Entity ent, int special, int heal, int attDmg) {
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
		Health = heal;
		attackDmg = attDmg;
	}

	public void attack(Entity[] ent) {
		for(int i = 0; i < ent.Length; i++) {
			if(Raylib.CheckCollisionCircles(this.Position, this.Radius, ent[i].Position, ent[i].Radius)  ) {
				ent[i].CurColor = Color.Blue;
				if (ent[i].Active == true)
					if(Raylib.IsMouseButtonPressed(MouseButton.Left)) {
						// this.score++;
						if(ent[i].a - attackDmg < 0) {
							int bufferDmg = ent[i].a - attackDmg;
							bufferDmg *= -1;
							ent[i].a += bufferDmg;
							ent[i].a -= attackDmg;
							ent[i].Speed = 0;
							ent[i].X = 0 - Radius;
							ent[i].Y = 0 + Radius;
							this.score += 1;
							ent[i].Active = false;
						} else {
							ent[i].a -= attackDmg;
						}
					}
				}
		}

		if(Raylib.IsMouseButtonPressed(MouseButton.Right)) {
			this.SpecialCounter--;
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
	}
	
}

