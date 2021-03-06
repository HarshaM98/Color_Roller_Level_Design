using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPLGameEndReason
{
    public enum GameEndReasons
    {
        NONE=0,
        PAUSE_TIME_LIMIT_EXCEEDED = 1,
        TOURNAMENT_ENDED = 2,
        OUT_OF_LIVES = 3,
        OUT_OF_TIME = 4,
        ALL_LEVELS_COMPLETED = 5,
        USER_QUIT = 6,
        APP_KILLED = 7,
        SERVER_GAME_END = 8,
        WENT_IN_BACKGROUND = 9,
        OPPONENT_DISCONNECTED = 10,
        CONNECTION_LOST = 11,
        POOR_NETWORK = 12,
        OPPONENT_FINISHED = 13,
        USER_ALL_BALLS_POTTED = 14,
        OPPONENT_POTTED_BLACK = 15,
        OPPONENT_POTTED_ALL_BALLS = 16,
        USER_POTTED_BLACK = 17,
        OPPONENT_QUIT = 18,
        CHECKMATE_WIN = 19,
        CHECKMATE_LOSS = 20,
        STALEMATE = 21,
        FIFTY_MOVE_RULE = 22,
        DRAW_BY_AGREEMENT = 23,
        THREEFOLD_REPETITION = 24,
        INSUFFICIENT_MATING_MATERIAL = 25,
        OPPONENT_WENT_IN_BACKGROUND = 26,
        OPPONENT_OUT_OF_TIME = 27,
        OPPONENT_ALL_PIECES_CAPTURED = 28,
        ALL_PIECES_CAPTURED = 29,
        OPPONENT_NO_VALID_MOVES = 30,
        NO_VALID_MOVES = 31,
        MATCH_NOT_FOUND = 32,
        BATTLE_CREATION_FAILED = 33,
        SUBMIT_SCORE_FAILED = 34,
        FINISH_BATTLE_FAILED = 35,
        OPPONENT_OUT_OF_LIVES = 36,
        USER_DISCONNECTED = 37,
        USER_PUTTED_BALL = 38,
        OPPONENT_PUTTED_BALL = 39,
        GOLF_TIE_BREAKER = 40,
        USER_IDLE = 41,
        OPPONENT_IDLE = 42,
        RUMMY_TOURNAMENT_QUIT = 43,

        ALL_WORDS_FOUND = 44,

        SERVER_ERROR = 45,
        SERVER_DISCONNECTED = 46,
        MONITIRING_TIMEOUT = 47,
        ENCRYPTION_INITIALIZED_FAILED = 48,
        APPLICATION_QUIT = 49,
        CONNECTION_RETRY_TIMEOUT = 50,
        PING_PONG_TIMEOUT = 51,
        LOGIN_ERROR = 52,
        PHOTON_PING_PONG_TIMEOUT = 53,
        DISCONNECTED_DUE_TO_HACKING = 54,
        CONNECTION_TIMEOUT = 55,
        PAUSE_GRECONNECT = 56,
        CONNECTION_ALREADY_LOST = 57,
        BATTLE_ALREADY_FINISHED = 58,
        GRECONNECTION_RETRIES_FINISHED = 59,
        PONG_MISS_GRECONNECT = 60,
        GRECONNECTION_MAX_TIME_FINISHED = 61,
        END_TRAINING = 62
    }
}
