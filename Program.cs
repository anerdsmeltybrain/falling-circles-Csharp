// See https://aka.ms/new-console-template for more information
using Raylib_cs;
using System.Numerics;

namespace falling_circles;

internal static class Program {

	public static void Main() {
		Raylib.InitWindow(640, 480, "Falling Circles");

		Entity[] entity = new Entity[100000];

		Color[] colors = new Color[] {
			Color.Black,
			Color.Brown,
			Color.Purple,
			Color.Blue,
			Color.SkyBlue,
			Color.Green,
			Color.Orange,
			Color.Red,
			Color.DarkGreen,
			Color.Yellow
		};

		var rand = new Random();
		int enemyCounter = 0;
		int framesCounter = 0;
		int framesDividend = 14;
		int framesDivisor = 7;

		for(int i = 0; i < entity.Length; i++) {
			entity[i] = new Entity(rand.Next(0, 640), 0, 32, 1, false, Color.Green, colors[rand.Next(0, colors.Length)]); 
		}

		Entity player_ent = new Entity(0, 0, 32, 1, true, Color.Blue, Color.Green);
		Player player = new Player(player_ent, 3, 3);

		GameScreen mainScreen = GameScreen.Title;

		Logo mainMenuLogo = new Logo("./falling_circles_game_banner.png", 128, 128, 32, 3);
		Logo specialIcon0 = new Logo("./spMoveIcon.png", 32, 32, 8, 1);
		Logo specialIcon1 = new Logo("./spMoveIcon.png", 64, 32, 8, 1);
		Logo specialIcon2 = new Logo("./spMoveIcon.png", 96, 32, 8, 1);
		Logo healthIcon0 = new Logo("./healthIcon.png", 32, 64, 8, 1);
		Logo healthIcon1 = new Logo("./healthIcon.png", 64, 64, 8, 1);
		Logo healthIcon2 = new Logo("./healthIcon.png", 96, 64, 8, 1);


		Logo[] logoArray = new Logo[7];

		logoArray[0] = mainMenuLogo;
		logoArray[1] = specialIcon0;
		logoArray[2] = specialIcon1;
		logoArray[3] = specialIcon2;
		logoArray[4] = healthIcon0;
		logoArray[5] = healthIcon1;
		logoArray[6] = healthIcon2;


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
												logoArray, buttonArray, colors);

		Raylib.SetTargetFPS(60);

		while (!Raylib.WindowShouldClose()) {

			screen.updateScreen();

			Raylib.BeginDrawing();

			screen.drawScreen();
		
			Raylib.EndDrawing();
		}
	}
}
