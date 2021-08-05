using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[SerializeField]
public class FindMatchData
{
    public int avgWaitTime;
    public int unmatchedUsers;
    public int activeUsers;
    public string[] placeholderImages;
    public Dictionary<string, Sprite> savedPlaceholderImages;

    public FindMatchData()
    {
        savedPlaceholderImages = new Dictionary<string, Sprite>();
    }

   
}

public class BattleFinishData
{
    public string gameEndReason;
    public FinalScore winner;
    public FinalScore[] scores;
    public bool isTie;
    public bool canPlayAgain;
    public bool isFightAgainDisabled;


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
        //return String.Format("winner: {0}, finalScores: {1}, isTie: {2}, canPlayAgain: {3}", "isFightAgainDisabled:{4}",
                             //winner, scores, isTie, canPlayAgain, isFightAgainDisabled);
    }
}

[SerializeField]
public class FinalScore
{
    public int id;
    public int finalScore;
    public int mplUserId;
    public int rank;
    public Reward[] rewards;
    public bool isFollowingOpponent;
    public bool canPlayAgain;
    
    public string nextLobbySuggestedConfig;
    public string extraInfo;
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
        //return String.Format("id: {0}, finalScore: {1}, mplUserId: {2}, rank: {3}, rewards: {4}",
                             //id, finalScore, mplUserId, rank, rewards);
    }
}
[SerializeField]
public class ExtraInfo
{
    public bool isFraudGamePlay;
    public string fraudDetectionMessage;
    public string blockStatus;

    public string fraudInfo;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);

    }
}
[SerializeField]
public class FraudInfo
{
    public int blockDuration;
    public bool isGamePlayCorrected;
    public string blockType;
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
        //return String.Format("winner: {0}, finalScores: {1}, isTie: {2}, canPlayAgain: {3}", "isFightAgainDisabled:{4}",
        //winner, scores, isTie, canPlayAgain, isFightAgainDisabled);
    }
}

/*[SerializeField]
public class DynamicReward
{
    public double cashWinning;
    public int playerCount;

    public override string ToString()
    {
        return String.Format("playerCount: {0}, amount: {1}",
                             playerCount,cashWinning);
    }
}*/

[SerializeField]
public class Reward
{
    public double amount;
    public string currency;
    public bool isCashReward=true;
    public string extReward;

    

    public override string ToString()
    {
        return String.Format("amount: {0}, currency: {1},isCashReward: {2},extReward:{3}",
                             amount, currency, isCashReward, extReward);
    }
}

[SerializeField]
public class AccountBalance
{
    public bool isError;
    public string errorMessage;
    public string errorReason;

    public double tokenBalance;
    public double depositBalance;
    public double bonusBalance;
    public double totalBalance;
    public double withdrawableBalance;

    public override string ToString()
    {
        return String.Format("isError: {0}, errorMessage: {1}, errorReason: {2}, tokenBalance: {3}, depositBalance: {4}, bonusBalance: {5}, totalBalance: {6}, withdrawableBalance: {7}",
                             isError, errorMessage, errorReason, tokenBalance, depositBalance, bonusBalance, totalBalance, withdrawableBalance);
    }

    public AccountBalance(double TokenBalance,double DepositBalance, double BonusBalance,double TotalBalance,double WithdrawableBalance)
    {
        this.tokenBalance = TokenBalance;
        this.depositBalance = DepositBalance;
        this.bonusBalance = BonusBalance;
        this.totalBalance = TotalBalance;
        this.withdrawableBalance = WithdrawableBalance;
    }
}

[SerializeField]
public class FightAgainState
{
    public string fightAgainState;
    public Dictionary<string, string> fightAgainStateDict;

    public void InitData()
    {
        fightAgainStateDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fightAgainState);
    }

    public override string ToString()
    {
        if (fightAgainStateDict == null) 
            return String.Format("fightAgainState: {0}, fightAgainStateDict: {1}",
                             fightAgainState, fightAgainStateDict);
        
        return String.Format("fightAgainState: {0}, fightAgainStateDict: {1}, fightAgainStateDict: {2}",
                             fightAgainState, fightAgainStateDict.Count, fightAgainStateDict);
    }
}

[SerializeField]
public class ServerErrorData
{
    public string title;
    public string description;
    public string isFatal;
}

[SerializeField]
public class MatchFoundData
{
    public MatchFoundUserProfile[] users;
    public int owner;
    public bool gReconnect;
    public int gReconnectionRetries;
    public int gReconnectTimeout;
}

[SerializeField]
public class OpponentFinishedData
{
    public int finishedUser;
}

[SerializeField]
public class MatchFoundUserProfile
{
    public string tier;
    public string displayName;
    public int mplUserId;
    public string avatar;
    public bool isPro;
    public int appVersion;
    public string mobileNumber;
}

[SerializeField]
public class MatchUserInData
{
    public MatchFoundUserProfile user;
    public MatchFoundUserProfile[] currentUsers;
    public int timeLeft;
    public string roundName;
    public int winnerUserId;
    public int minPlayers;
    public MatchFoundUserProfile[] joinedUsers;
    public MatchFoundUserProfile joinedUser;


}

[SerializeField]
public class ServerGameEndData
{
    public string reason;
}