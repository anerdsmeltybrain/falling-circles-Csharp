using Raylib_cs;
using System.Numerics;

namespace falling_circles;

struct innerColor {
	int r, g, b, a;
}

public class Entity {
	public int Radius { get; set;}
	public int X { get; set;}
	public int Y { get; set;}
	public int Speed { get; set; }
	public bool Active { get; set; }
	public Color InitColor { get; set;}
	public Color CurColor { get; set;}

	public Vector2 Position { get; set; }
	private innerColor innColor { get; set; }


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
	}

	public virtual void updatePosition() {
		this.Position = new Vector2(this.X, this.Y);
	}

	public virtual void draw() {
		Raylib.DrawCircle((int)this.X, (int)this.Y, this.Radius, this.CurColor);
	}

	public void fall() {
		this.Y += Speed;
	}

	public void track_point(Player play) {
		if(this.CurColor.A <= 0) {
			this.Active = false;
			play.score += 1;
		}
	}
};

public class Player : Entity {
	public int SpecialCounter { get; set; }
	public int Health { get; set; }
	public int score { get; set; }

	public Player(Entity ent, int special, int heal) {
		this.X = ent.X;
		this.Y = ent.Y;
		this.Radius = ent.Radius;
		this.Active = ent.Active;
		this.Speed = ent.Speed;
		this.InitColor = ent.InitColor;
		this.CurColor = ent.CurColor;
		this.Position = new Vector2(X,Y);
		SpecialCounter = special;
		Health = heal;
	}

	public void attack(Entity[] ent) {
		for(int i = 0; i < ent.Length; i++) {
			if(Raylib.CheckCollisionCircles(this.Position, this.Radius, ent[i].Position, ent[i].Radius)  ) {
				ent[i].CurColor = Color.Blue;
				if (ent[i].Active == true)
					if(Raylib.IsMouseButtonPressed(MouseButton.Left)) {
						this.score++;
						ent[i].Active = false;
					}
			} else {
				ent[i].CurColor = ent[i].InitColor;
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
		Raylib.DrawCircle((int)this.X, (int)this.Y, this.Radius, this.CurColor);
	}
	
}

