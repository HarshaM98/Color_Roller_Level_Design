using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translations
{
    /************************************** Prefabs **************************************/


    /************************************** Constants ************************************/

    public static readonly Dictionary<string, string> INDO_STRINGS = new Dictionary<string, string>
    {

        {"FindingPlayerText","Mencari Pemain..."},
        {"StartingBattleText","Mulai pertandingan"},
        {"PlayerLeftText","Pemain pergi! Cari pemain baru..."},
        {"MatchError","Tidak dapat menemukan lawan untukmu!"},
        {"errorsubtitle","Klik Coba Lagi atau lihat Ruang Pertandingan lain?"},
        {"TryAgainText","COBA LAGI"},
        {"PlayerScoreLabel","SKOR"},
        {"PlayAgainText","Mau Main Lagi?"},
        {"Can'tPlayAgainText","Yah, Gak Bisa Main!"},
        {"WaitingForResults","Menunggu pemain lain selesai bermain"},
        {"AvailableBalanceText","Saldo Tersedia"},
        {"BonusCashCannotText"," Bonus Berlian tidak dapat digunakan dalam pertandingan ini"},
        {"AddMoneyText","AJAK DAN DAPATKAN"},
        {"BattleAgainText","MAIN LAGI"},
        {"NewBattleText","PERTANDINGAN BARU"},
        {"BonusAddMoneyText","TAMBAH BERLIAN"},
        {"BonusCashTitle","Saldo Berlian Top Up dan Berlian Kemenangan Tidak Cukup"},
        {"ExitBattleRoom","KELUAR RUANGAN PERTANDINGAN"},
        {"StartNextBattleText","MULAI PERTANDINGAN SELANJUTNYA"},
        {"ExitText","KELUAR"},
        {"ConnectingText","Ada masalah koneksi! Coba lagi"},
        {"GameEndReasonButtonText","KE RUANG PERTANDINGAN"},
        {"TotalBalancelabel","Saldo Tersedia" }
     


    };
}

[System.Serializable]
public class Localisation
{
    public string key;// { get; set; }
    public string value;// { get; set; }
}
[System.Serializable]
public class LocalisationDetails
{
    public List<Localisation> Localisation;// { get; set; }


    public Dictionary<string, string> translations = new Dictionary<string, string>();

    public void SetDictionary()
    {
        foreach (Localisation translationObj in Localisation)
        {
            var newKey = GenerateKey(translationObj.key);
            if (!translations.ContainsKey(newKey))
                translations.Add(newKey, translationObj.value);
        }
    }

    public string GetLocalizedText(string key)
    {
        if (!MPLController.Instance.IsItIndo())
        {
            return key;
        }

        string valueToReturn = key;
        string newKey = GenerateKey(key);
        //Debug.LogError("newKey " + newKey);
        if (translations.ContainsKey(newKey))
        {
            valueToReturn = translations[newKey];
        }
        return valueToReturn;
    }

    string GenerateKey(string key)
    {
        return key.Replace(" ", "").Replace("\n", "").ToUpper();
    }

}

[System.Serializable]
public class TutorialTexts
{
    public string countryCode;
    public List<string> tutorialStrings;
}

[System.Serializable]
public class TutorialTextDetails
{
    public List<TutorialTexts> tutorialTexts;
}

