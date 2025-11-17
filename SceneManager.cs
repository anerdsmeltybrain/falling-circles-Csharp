using Raylib_cs;
using System.Numerics;

namespace falling_circles;

public enum GameScreen {
	Title = 0,
	Main,
	Game,
	End
}

public enum Transition {
	UP = 0,
	DOWN
}

public class Button {
	string down_button_png { get; set; }
	string up_button_png { get; set; }
	Texture2D up_texture { get; set; }
	Texture2D down_texture { get; set; }
	Texture2D cur_texture { get; set; }
	int x { get; set; }
	int y { get; set; }
	public Rectangle button_rec { get; set; }
	Vector2 position { get; set; }
	Transition trans { get; set; }
	

	public Button(string up, string down, Rectangle bRec) {
		down_button_png = down;
		up_button_png = up;
		up_texture = Raylib.LoadTexture(up_button_png);
		down_texture = Raylib.LoadTexture(down_button_png);
		cur_texture = up_texture;
		button_rec = bRec;
		x = (int)bRec.X;
		y = (int)bRec.Y;
		position = new Vector2(x, y);
		trans = Transition.UP;
	}

	public void MousePress(Player play, GameScreen screen, GameScreen trans_screen) {
		if(Raylib.CheckCollisionCircleRec(play.Position, play.Radius, this.button_rec) == true) {
			this.trans = Transition.UP;
			if (Raylib.IsMouseButtonDown(MouseButton.Left)) 
				screen = trans_screen; 
		} else {
			this.trans = Transition.DOWN;
		}
	}

	public void draw() {
		if ( this.trans == Transition.UP ) {
			Raylib.DrawTextureRec(this.up_texture, this.button_rec, this.position, Color.White); 
		} else {
			Raylib.DrawTextureRec(this.down_texture, this.button_rec, this.position, Color.White); 
		}
	}
}

public class Logo {
	string logo_string {get; set;}
	Texture2D logo_texture {get; set;}
	int x {get; set;}
	int y {get; set;}
	int yUpperBounds {get; set;}
	int yLowerBounds {get; set;}
	int boundSpacing {get; set;}
	Transition trans {get; set;}
	int multi {get; set;}

	public Logo(string logo, int _x, int _y, int bs, int mul) {
		logo_string = logo;
		logo_texture = Raylib.LoadTexture(logo_string);
		x = _x;
		y = _y;
		boundSpacing = bs;
		yUpperBounds = y + boundSpacing;
		yLowerBounds = y - boundSpacing;
		trans = Transition.DOWN;
		multi = mul;
	}

	public void draw() {
		Raylib.DrawTexture(this.logo_texture, this.x, this.y, Color.White);
	}

	public void floaty() {
		if(this.trans == Transition.DOWN) {
			this.y += 1;
			if( this.y == yUpperBounds) 
				this.trans = Transition.UP;
		}

		if(this.trans == Transition.UP) {
			this.y -= 1;
			if( this.y == yLowerBounds) 
				this.trans = Transition.DOWN;
		}
	}
}

public class ScreenManager {

	GameScreen Screen { get; set; }
	int enemyCounter { get; set; }
	int framesCounter { get; set; }
	int framesDividend { get; set; }
	int framesDivisor { get; set; }
	Player player { get; set; }
	Entity[] entity { get; set; }
	Logo[] logoArray {get; set; }
	Button[] mainMenuButtons {get; set;}

	public ScreenManager(GameScreen scree,
	int eCounter, int fCounter,
	int fDividend, int fDivisor,
	Player play, Entity[] ent, Logo[] logos, Button[] buttArr) {
		Screen = scree;
		enemyCounter = eCounter;
		framesCounter = fCounter;
		framesDividend = fDividend;
		framesDivisor = fDivisor;
		player = play;
		entity = ent;
		logoArray = logos;
		mainMenuButtons = buttArr;
	}

	public void setLogos(Logo main) {
		logoArray[0] = main;
	}

	public void updateScreen() {
		switch(this.Screen) {
			case GameScreen.Title:
				player.updatePosition();
				framesCounter++;

				if (((framesCounter / framesDividend) % framesDivisor) == 1) {
					entity[enemyCounter].Active = true;
					enemyCounter++;
					framesCounter = 0;
				}

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].fall();
						entity[i].updatePosition();
				}

				logoArray[0].floaty();
				if(Raylib.IsMouseButtonDown(MouseButton.Left)) {
					var rand = new Random();
					for(int i = 0; i < entity.Length; i++) {
						entity[i] = new Entity(rand.Next(0, 480), 0, 32, rand.Next(1, 5), false, Color.Green, Color.Red); 
					}
					this.framesDividend = 60;
					this.framesDividend = 15;
					this.Screen = GameScreen.Main;
				}
			break;
			case GameScreen.Main:
				player.updatePosition();
				this.mainMenuButtons[0].MousePress(this.player, this.Screen, GameScreen.Game);
				if(Raylib.CheckCollisionCircleRec(this.player.Position, this.player.Radius, this.mainMenuButtons[0].button_rec)) {
					if(Raylib.IsMouseButtonDown(MouseButton.Left)) {
						this.Screen = GameScreen.Game;
						var rand = new Random();
						for(int i = 0; i < entity.Length; i++) {
							entity[i] = new Entity(rand.Next(0, 480), 0, 32, rand.Next(1, 3), false, Color.Green, Color.Red); 
						}
}				}

				this.mainMenuButtons[1].MousePress(this.player, this.Screen, GameScreen.Game);
				if(Raylib.CheckCollisionCircleRec(this.player.Position, this.player.Radius, this.mainMenuButtons[1].button_rec)) {
					if(Raylib.IsMouseButtonDown(MouseButton.Left))
						this.Screen = GameScreen.End;
				}

				framesCounter++;

				if (((framesCounter / framesDividend) % framesDivisor) == 1) {
					entity[enemyCounter].Active = true;
					enemyCounter++;
					framesCounter = 0;
				}

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].fall();
						entity[i].updatePosition();
				}


			break;
			case GameScreen.Game:
				player.updatePosition();
				player.attack(entity);

				framesCounter++;

				if (((framesCounter / framesDividend) % framesDivisor) == 1) {
					entity[enemyCounter].Active = true;
					enemyCounter++;
					framesCounter = 0;
				}

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].fall();
						entity[i].updatePosition();
				}

			break;
			case GameScreen.End:
				player.updatePosition();
			break;
		}
	}

	public void drawScreen() {
		switch(this.Screen) {
			case GameScreen.Title:
				Raylib.ClearBackground(Color.White);

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].draw();
				}

				// Raylib.DrawTexture(logo, 128, 128, Color.White);
				Raylib.DrawText("Press Any Key to Continue", 640 / 2, 480 / 2, 16, Color.Black);
				player.draw();
				logoArray[0].draw();
			break;
			case GameScreen.Main:
				Raylib.ClearBackground(Color.Purple);

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].draw();
				}
				// mainMenuButtons[2].draw();
				//
				mainMenuButtons[0].draw();
				mainMenuButtons[1].draw();
				player.draw();
			break;
			case GameScreen.Game:
				Raylib.ClearBackground(Color.White);
				Raylib.DrawText($"{player.score}", 0, 0, 32, Color.Black);
				Raylib.DrawText($"{player.SpecialCounter}", 0, 32, 32, Color.Black);

				switch(player.SpecialCounter) {
					case 0:
					break;
					case 1:
					logoArray[1].draw();
					break;
					case 2:
					logoArray[1].draw();
					logoArray[2].draw();
					break;
					case 3:
					logoArray[1].draw();
					logoArray[2].draw();
					logoArray[3].draw();
					break;
				}

				for(int i = 0; i < entity.Length; i++) {
					if(entity[i].Active == true)
						entity[i].draw();
				}

				player.draw();
			break;
			case GameScreen.End:
				Raylib.ClearBackground(Color.Black);
				player.draw();
			break;
		}
	}

}
