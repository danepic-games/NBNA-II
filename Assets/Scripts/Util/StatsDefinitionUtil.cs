public class StatsDefinitionUtil {

    public static int TotalHP(int resistence) {
        switch (resistence) {
            case 1:
                return 900;
            case 2:
                return 925;
            case 3:
                return 950;
            case 4:
                return 975;
            case 5:
                return 1000;
            case 6:
                return 1025;
            case 7:
                return 1050;
            case 8:
                return 1075;
            case 9:
                return 1100;
            case 10:
                return 1125;
            default:
                return 0;
        }
    }

    public static float Speed(int speed) {
        switch (speed) {
            case 1:
                return 9.000000f;
            case 2:
                return 9.500000f;
            case 3:
                return 10.000000f;
            case 4:
                return 10.500000f;
            case 5:
                return 11.000000f;
            case 6:
                return 11.500000f;
            case 7:
                return 12.000000f;
            case 8:
                return 12.500000f;
            case 9:
                return 13.000000f;
            case 10:
                return 13.500000f;
            default:
                return 0f;
        }
    }

    public static int Stamina(int stamina) {
        switch (stamina) {
            case 1:
                return 800;
            case 2:
                return 850;
            case 3:
                return 900;
            case 4:
                return 950;
            case 5:
                return 1000;
            case 6:
                return 1050;
            case 7:
                return 1100;
            case 8:
                return 1150;
            case 9:
                return 1200;
            case 10:
                return 1250;
            default:
                return 0;
        }
    }

    public static int Agressive(int agressive) {
        switch (agressive) {
            case 1:
                return 30;
            case 2:
                return 40;
            case 3:
                return 50;
            case 4:
                return 60;
            case 5:
                return 70;
            case 6:
                return 80;
            case 7:
                return 90;
            case 8:
                return 100;
            case 9:
                return 110;
            case 10:
                return 120;
            default:
                return 0;
        }
    }
}