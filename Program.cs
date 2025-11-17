// See https://aka.ms/new-console-template for more information
using Raylib_cs;
using System.Numerics;

namespace falling_circles;

internal static class Program {

	public static void Main() {
		Raylib.InitWindow(640, 480, "Falling Circles");

		Entity[] entity = new Entity[10000];

		var rand = new Random();
		int enemyCounter = 0;
		int framesCounter = 0;
		int framesDividend = 120;
		int framesDivisor = 2;

		for(int i = 0; i < entity.Length; i++) {
			entity[i] = new Entity(rand.Next(0, 480), 0, 32, false, Color.Green, Color.Red); 
		}

		Entity player_ent = new Entity(0, 0, 32, true, Color.Blue, Color.Green);
		Player player = new Player(player_ent, 3, 3);

		GameScreen mainScreen = GameScreen.Title;

		Logo mainMenuLogo = new Logo("./falling_circles_game_banner.png", 128, 128, 32, 3);

		Rectangle gameButtonRec = new Rectangle(128, 128, 128, 64);
		Button gameButton = new Button("./gameButtonUp.png", "./gameButtonDown.png", gameButtonRec);

		Rectangle endButtonRec = new Rectangle(128, 256, 128, 64);
		Button endButton = new Button("./endButtonUp.png", "./endButtonDown.png", endButtonRec);

		Rectangle quitButtonRec = new Rectangle(128, 256, 128, 64);
		Button quitButton = new Button("./quitButtonUp.png", "./quitButtonDown.png", quitButtonRec);

		Button[] buttonArray = new Button[3];

		buttonArray[0] = gameButton;
		// buttonArray[1] = endButton;
		buttonArray[1] = quitButton;
		
		ScreenManager screen = new ScreenManager(mainScreen, enemyCounter,
												framesCounter, framesDividend,
												framesDivisor, player, entity,
												mainMenuLogo, buttonArray);

		Raylib.SetTargetFPS(60);

		while (!Raylib.WindowShouldClose()) {

			screen.updateScreen();

			Raylib.BeginDrawing();

			screen.drawScreen();
		
			Raylib.EndDrawing();
		}
	}
}
