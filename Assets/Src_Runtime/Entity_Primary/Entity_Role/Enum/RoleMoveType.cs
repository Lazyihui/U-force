using UF;

public enum RoleMoveType {
    PlayerUpDown, //玩家移动, 上下运动
    Ghost, //暂定小怪移动，悬浮，向左
    OffensiveUDRL, //进攻战 玩家移动(UDRL上下左右)
    Prohibit, //禁止移动
}