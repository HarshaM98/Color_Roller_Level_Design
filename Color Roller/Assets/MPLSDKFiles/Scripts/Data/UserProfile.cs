using System;

[Serializable]
public class UserProfile
{
    public int id;
    public string mobileNumber;
    public string displayName;
    public string avatar;
    public string tier;
    public bool pro;
    public double TokenBalance;
    public double TotalBalance;
    public double WithdrawableBalance;
    public double DepositBalance;
    public double BonusBalance;
    public int appVersion;

    public UserProfile()
    {
    }

    public UserProfile(int id, string mobileNum, string displayName, string avatar, string tier, bool pro,int appVersion)
    {
        this.id = id;
        this.mobileNumber = mobileNum;
        this.displayName = displayName;
        this.avatar = avatar;
        this.tier = tier;
        this.pro = pro;
        this.appVersion = appVersion;
    }

	public override string ToString()
	{
        return String.Format("id: {0}, mobileNumber: {1}, displayName: {2}, avatar: {3}, tier: {4}, pro: {5}, TokenBalance: {6}, TotalBalance: {7}, WithdrawableBalance: {8}, DepositBalance: {9}, BonusBalance: {10},AppVersion:{11}",
                             id, mobileNumber, displayName, avatar, tier, pro, TokenBalance, TotalBalance, WithdrawableBalance, DepositBalance, BonusBalance,appVersion);
	}
}
